using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;
using LowKey;
using WinRadioTray.Controls;
using System.Drawing;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using System.Resources;
using System.Threading;

namespace WinRadioTray
{
    public class SysTrayApp : Form
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Application.Run(new SysTrayApp());
                }
                else
                {
                    return;
                }
            }
        }

        private BASSActive _status;
        private int newStationIndex;
        private KeyboardHook hooker = KeyboardHook.Hooker;
        private List<String> bookmarkList = new List<String>();
        private List<String> urlList = new List<String>();
        private List<String> groupList = new List<String>();
        private logForm lf;
        private NotifyIcon trayIcon;
        private readonly ToolStripAeroRenderer toolStripRenderer;
        private readonly ToolStripLabeledTrackBar balanceTrackBar;
        private readonly ToolStripLabeledTrackBar volumeTrackBar;
        private ResourceManager resMan;
        private static string appGuid = "F4D81F8E-4D74-455A-B16C-1E825EE549A9";
        private string lastStation;
        private string logFile;
        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private string stationTitle;
        private string currentURL;
        private string tooltipText;
        private SYNCPROC _mySync;
        private System.Windows.Forms.Timer timeX = new System.Windows.Forms.Timer();
        private TAG_INFO _tagInfo;
        private ToolStripLabeledNumber sleepTimerCtl;
        private ToolStripMenuItem about;
        private ToolStripMenuItem addCustom;
        private ToolStripMenuItem audioSettings;
        private ToolStripMenuItem autoPlay;
        private ToolStripMenuItem darkIcon;
        private ToolStripMenuItem custom;
        private ToolStripMenuItem enableAutoRun;
        private ToolStripMenuItem enableLogging;
        private ToolStripMenuItem enableMMKeys;
        private ToolStripMenuItem enableSleepTimer;
        private ToolStripMenuItem exit;
        private ToolStripMenuItem logging;
        private ToolStripMenuItem manage;
        private ToolStripMenuItem next;
        private ToolStripMenuItem pause;
        private ToolStripMenuItem play;
        private ToolStripMenuItem preferences;
        private ToolStripMenuItem previous;
        private ToolStripMenuItem reload;
        private ToolStripMenuItem resetBalance;
        private ToolStripMenuItem resetVolume;
        private ToolStripMenuItem stationSwitcher;
        private ToolStripMenuItem stop;
        private ToolStripMenuItem outputs;
        private ToolStripMenuItem gbhost;
        public Boolean stationSwitcherActive = false;
        public int sleepTimerDuration;
        public int stream;
        public int _aacPlugIn;
        public int _alacPlugin;
        public int _dsdPlugin;
        public int _flacPlugin;
        public int _hlsPlugin;
        public int _opusPlugin;
        public int _wmaPlugin;
        public int _wvPlugin;
        public IntPtr _myUserAgentPtr;
        public static ContextMenuStrip trayMenu;
        public ToolStripMenuItem stations;
        public GroupBox gb;
        private Boolean testbool;
        public Boolean darkTheme;
        public Icon playingImage;
        public Icon stoppedImage;
        
        

        public SysTrayApp()
        {       
            toolStripRenderer = new ToolStripAeroRenderer();

            Directory.CreateDirectory(path + "\\images");
            
            if (!File.Exists(path + "\\bookmarks.xml"))
            {
                File.WriteAllText(path + "\\bookmarks.xml", WinRadioTray.Properties.Resources.bookmarks);
            }

            if (!File.Exists(path + "\\preferences.xml"))
            {
                File.WriteAllText(path + "\\preferences.xml", WinRadioTray.Properties.Resources.preferences);
            }

            if (!File.Exists(path + "\\WinRadioTray.log"))
            {
                File.WriteAllText(path + "\\WinRadioTray.log", WinRadioTray.Properties.Resources.logfile);
            }

            lf = new logForm();

            stations = null;
            stations = new ToolStripMenuItem("Stations");
            stations.Image = Properties.Resources.icons8_radio_2;
            XPathNavigator nav;
            XPathDocument doc;

            doc = new XPathDocument(path + "\\bookmarks.xml");
            nav = doc.CreateNavigator();
            bookmarkList.Clear();
            urlList.Clear();
            groupList.Clear();
            resMan = new ResourceManager("WinRadioTray.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            XPathExpression exp = nav.Compile("bookmarks/group");
            exp.AddSort("@name", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text);
            XPathNodeIterator nodeIterator = nav.Select(exp);
            while (nodeIterator.MoveNext())
            {
                ToolStripMenuItem tmp = new ToolStripMenuItem(nodeIterator.Current.GetAttribute("name", ""));
                XPathExpression exp2 = nav.Compile("bookmarks/group[@name='" + nodeIterator.Current.GetAttribute("name", "") + "']/bookmark");
                exp2.AddSort("@name", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text);
                nav = doc.CreateNavigator();
                XPathNodeIterator nodeIterator2 = nav.Select(exp2);
                while (nodeIterator2.MoveNext())
                {
                    ToolStripMenuItem tmpStation = new ToolStripMenuItem(nodeIterator2.Current.GetAttribute("name", ""), null, new EventHandler(station_click));
                    tmpStation.Tag = nodeIterator2.Current.GetAttribute("url", "");
                    tmpStation.Image = (nodeIterator2.Current.SelectSingleNode("@img") != null) ? thumb(nodeIterator2.Current.GetAttribute("name", ""), nodeIterator2.Current.GetAttribute("img", ""), "station") : Properties.Resources.icons8_speaker;
                    tmp.DropDownItems.Add(tmpStation);
                    tmp.DropDownDirection = ToolStripDropDownDirection.Left;
                    bookmarkList.Add(nodeIterator2.Current.GetAttribute("name", ""));
                    urlList.Add(nodeIterator2.Current.GetAttribute("url", ""));
                }
                tmp.Image = (nodeIterator.Current.SelectSingleNode("@img") != null) ? thumb(nodeIterator.Current.GetAttribute("name", ""), nodeIterator.Current.GetAttribute("img", ""), "group") : Properties.Resources.icons8_radio_station;
                stations.DropDownItems.Add(tmp);
                groupList.Add(nodeIterator.Current.GetAttribute("name", ""));
            }

            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            lastStation = prefsRoot["lastStation"].InnerText;
            preferences = new ToolStripMenuItem("Preferences");
            preferences.Image = Properties.Resources.icons8_settings;

            audioSettings = new ToolStripMenuItem("Audio Settings");
            audioSettings.Image = Properties.Resources.icons8_audio_wave;

            outputs = new ToolStripMenuItem("Audio Output");
            BASS_DEVICEINFO info = new BASS_DEVICEINFO();
            for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
            {
                if (Bass.BASS_Init(n, 44100, BASSInit.BASS_DEVICE_DEFAULT, this.Handle))
                {
                    _aacPlugIn = Bass.BASS_PluginLoad("bass_aac.dll");
                    _alacPlugin = Bass.BASS_PluginLoad("bassalac.dll");
                    _dsdPlugin = Bass.BASS_PluginLoad("bassdsd.dll");
                    _flacPlugin = Bass.BASS_PluginLoad("bassflac.dll");
                    _hlsPlugin = Bass.BASS_PluginLoad("basshls.dll");
                    _opusPlugin = Bass.BASS_PluginLoad("bassopus.dll");
                    _wmaPlugin = Bass.BASS_PluginLoad("basswma.dll");
                    _wvPlugin = Bass.BASS_PluginLoad("basswv.dll");
                }
                else
                {
                    MessageBox.Show(this, "Bass_Init error!");
                }
                if (info.ToString() != "No sound")
                {
                    ToolStripMenuItem tmpOutput = new ToolStripMenuItem(info.ToString(), null, new EventHandler(OnChangeOutput));
                    tmpOutput.Name = info.ToString();
                    tmpOutput.Tag = n;
                    if (info.IsDefault) {
                        tmpOutput.Checked = true;
                    } else { 
                        tmpOutput.Checked = false;
                    }
                    switch (info.type.ToString())
                    {
                        case "BASS_DEVICE_TYPE_SPEAKERS":
                            tmpOutput.Image = Properties.Resources.icons8_speaker;
                            break;
                        case "BASS_DEVICE_TYPE_HEADPHONES":
                            tmpOutput.Image = Properties.Resources.icons8_headphones;
                            break;
                        default:
                            tmpOutput.Image = Properties.Resources.icons8_speaker;
                            break;
                    }
                    outputs.DropDownItems.Add(tmpOutput);
                    
                }
            }
            outputs.Image = Properties.Resources.icons8_speaker;

            logging = new ToolStripMenuItem("Log Actions");
            logging.Image = Properties.Resources.icons8_log;
            logging.DropDownItems.Add("Show Log", Properties.Resources.icons8_log_2, OnShowLog);
            logging.DropDownItems.Add("Clear Log", Properties.Resources.icons8_delete_document, OnClearLog);

            balanceTrackBar = new ToolStripLabeledTrackBar();
            balanceTrackBar.Label.Text = prefsRoot.SelectSingleNode("balance") != null ? "Balance: " + prefsRoot["balance"].InnerText : "Balance: 0";
            balanceTrackBar.TrackBar.Minimum = -100;
            balanceTrackBar.TrackBar.Maximum = 100;
            balanceTrackBar.TrackBar.TickStyle = TickStyle.BottomRight;
            balanceTrackBar.TrackBar.TickFrequency = 10;

            volumeTrackBar = new ToolStripLabeledTrackBar();
            volumeTrackBar.Label.Text = prefsRoot.SelectSingleNode("volume") != null ? "Volume: " + prefsRoot["volume"].InnerText : "Volume: 50";
            volumeTrackBar.TrackBar.Minimum = 0;
            volumeTrackBar.TrackBar.Maximum = 100;
            volumeTrackBar.TrackBar.TickStyle = TickStyle.BottomRight;
            volumeTrackBar.TrackBar.TickFrequency = 5;

            Color back = toolStripRenderer.ColorTable.ToolStripDropDownBackground;

            balanceTrackBar.BackColor = back;
            balanceTrackBar.Label.BackColor = back;
            balanceTrackBar.TrackBar.BackColor = back;
            balanceTrackBar.TrackBar.Value = (prefsRoot["balance"].InnerText.Length > 0 || prefsRoot["balance"].InnerText == null) ? Int32.Parse(prefsRoot["balance"].InnerText) : 0;
            volumeTrackBar.BackColor = back;
            volumeTrackBar.Label.BackColor = back;
            volumeTrackBar.TrackBar.BackColor = back;
            volumeTrackBar.TrackBar.Value = (prefsRoot["volume"].InnerText.Length > 0 || prefsRoot["volume"].InnerText == null) ? Int32.Parse(prefsRoot["volume"].InnerText) : 50;

            resetBalance = new ToolStripMenuItem("Center Balance", null, recenterBalance);
            resetVolume = new ToolStripMenuItem("Reset Volume", null, recenterVolume);

            audioSettings.DropDownItems.Add(volumeTrackBar);
            audioSettings.DropDownItems.Add(resetVolume);
            audioSettings.DropDownItems.Add("-");
            audioSettings.DropDownItems.Add(balanceTrackBar);
            audioSettings.DropDownItems.Add(resetBalance);

            trayMenu = new ContextMenuStrip();

            darkIcon = new ToolStripMenuItem("Dark system tray icon", null, toggleDarkIcon);
            if (prefsRoot.SelectSingleNode("darkicon") != null)
            {
                darkIcon.Checked = prefsRoot["darkicon"].InnerText == "true" ? true : false;
                darkTheme = prefsRoot["darkicon"].InnerText == "true" ? true : false;
                if (darkTheme)
                {
                    playingImage = Properties.Resources.icons8_radio_tower_dark;
                    stoppedImage = Properties.Resources.icons8_radio_tower_idle_dark;
                } else
                {
                    playingImage = Properties.Resources.icons8_radio_tower1;
                    stoppedImage = Properties.Resources.icons8_radio_tower_idle;
                }
            }
            else
            {
                darkIcon.Checked = false;
                darkTheme = false;
                playingImage = Properties.Resources.icons8_radio_tower1;
                stoppedImage = Properties.Resources.icons8_radio_tower_idle;
            }

            autoPlay = new ToolStripMenuItem("Auto play last station on startup", null, toggleAutoPlay);
            if (prefsRoot.SelectSingleNode("autoplay") != null)
            {
                autoPlay.Checked = prefsRoot["autoplay"].InnerText == "true" ? true : false;
            }
            else
            {
                autoPlay.Checked = false;
            }

            enableLogging = new ToolStripMenuItem("Enable activity logging", null, toggleLogging);
            if (prefsRoot.SelectSingleNode("logging") != null)
            {
                enableLogging.Checked = prefsRoot["logging"].InnerText == "true" ? true : false;
            }
            else
            {
                enableLogging.Checked = false;
            }

            enableMMKeys = new ToolStripMenuItem("Use multimedia keys", null, toggleMMKeys);
            if (prefsRoot.SelectSingleNode("mmkeys") != null)
            {
                enableMMKeys.Checked = prefsRoot["mmkeys"].InnerText == "true" ? true : false;
            }
            else
            {
                enableMMKeys.Checked = false;
            }

            stationSwitcher = new ToolStripMenuItem("Back/Foward Keys Switch Stations", null, toggleStationSwitcher);
            if (prefsRoot.SelectSingleNode("stationswitcher") != null)
            {
                stationSwitcher.Checked = prefsRoot["stationswitcher"].InnerText == "true" ? true : false;
            }
            else
            {
                stationSwitcher.Checked = false;
            }

            enableSleepTimer = new ToolStripMenuItem("Enable Sleep Timer", null, toggleSleepTimer);
            if (prefsRoot.SelectSingleNode("sleeptimer") != null)
            {
                enableSleepTimer.Checked = prefsRoot["sleeptimer"].InnerText == "true" ? true : false;
            }
            else
            {
                enableSleepTimer.Checked = false;
            }

            if (prefsRoot.SelectSingleNode("sleeptimerduration") != null)
            {
                sleepTimerDuration = Int32.Parse(prefsRoot["sleeptimerduration"].InnerText);
            }
            else
            {
                sleepTimerDuration = 60;
            }

            enableAutoRun = new ToolStripMenuItem("Start with Windows", null, toggleAutoRun);
            enableAutoRun.Checked = registryValueExists("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", "WinRadioTray");

            sleepTimerCtl = new ToolStripLabeledNumber();
            sleepTimerCtl.Label.Text = "Timer Duration: ";
            sleepTimerCtl.NumericUpDown.Value = sleepTimerDuration;
            sleepTimerCtl.BackColor = back;
            sleepTimerCtl.Label.BackColor = back;
            sleepTimerCtl.Label2.BackColor = back;
            sleepTimerCtl.NumericUpDown.BackColor = back;

            preferences.DropDownItems.Add(darkIcon);
            preferences.DropDownItems.Add(autoPlay);
            preferences.DropDownItems.Add(enableLogging);
            preferences.DropDownItems.Add(enableMMKeys);
            preferences.DropDownItems.Add(stationSwitcher);
            preferences.DropDownItems.Add(enableAutoRun);
            preferences.DropDownItems.Add(enableSleepTimer);
            preferences.DropDownItems.Add(sleepTimerCtl);

            gb = new GroupBox();
            RadioButton rb1 = new RadioButton();
            rb1.Text = "Radio Button 1";
            RadioButton rb2 = new RadioButton();
            rb2.Text = "Radio Button 2";
            gb.Controls.Add(rb1);
            gb.Controls.Add(rb2);
            ToolStripControlHost tsHost = new ToolStripControlHost(rb1);
            tsHost.Text = "Group Box";

            gbhost = new ToolStripMenuItem("group box host");
            gbhost.Image = Properties.Resources.icons8_speaker;
            gbhost.DropDownItems.Add(tsHost);
            

            trayMenu.Items.Add(stations);
            trayMenu.Items.Add("-", null, null);
            trayMenu.Items.Add(preferences);
            trayMenu.Items.Add(audioSettings);
            trayMenu.Items.Add(outputs);
            trayMenu.Items.Add(logging);
            //trayMenu.Items.Add(gbhost);

            custom = new ToolStripMenuItem();
            custom.Text = "Play Custom URL";
            custom.Image = Properties.Resources.icons8_add_link;
            custom.Click += OnCustom;
            trayMenu.Items.Add(custom);

            addCustom = new ToolStripMenuItem();
            addCustom.Text = "Add Custom URL to Station List";
            addCustom.Image = Properties.Resources.icons8_plus_16;
            addCustom.Click += AddCustomToStations;
            trayMenu.Items.Add(addCustom);
            addCustom.Visible = false;

            manage = new ToolStripMenuItem();
            manage.Text = "Manage Stations";
            manage.Image = Properties.Resources.icons8_maintenance;
            manage.Click += OnManage;
            trayMenu.Items.Add(manage);

            reload = new ToolStripMenuItem();
            reload.Text = "Reload Stations";
            reload.Image = Properties.Resources.icons8_synchronize;
            reload.Click += OnReload;
            trayMenu.Items.Add(reload);

            trayMenu.Items.Add("-", null, null);

            play = new ToolStripMenuItem();
            play.Text = "Play";
            play.Image = Properties.Resources.icons8_Play;
            play.Click += OnPlay;
            play.Enabled = prefsRoot["lastURL"].InnerText.Length > 0 ? true : false;

            pause = new ToolStripMenuItem();
            pause.Text = "Pause";
            pause.Image = Properties.Resources.icons8_Pause;
            pause.Click += OnPause;
            pause.Visible = false;

            stop = new ToolStripMenuItem();
            stop.Text = "Stop";
            stop.Image = Properties.Resources.icons8_Stop;
            stop.Click += OnStop;
            stop.Enabled = false;

            previous = new ToolStripMenuItem();
            previous.Text = "Previous Station";
            previous.Image = Properties.Resources.icons8_Rewind;
            previous.Click += OnPrevious;
            previous.Enabled = false;
           
            next = new ToolStripMenuItem();
            next.Text = "Next Station";
            next.Image = Properties.Resources.icons8_Fast_Forward;
            next.Click += OnNext;
            next.Enabled = false;
            
            trayMenu.Items.Add(play);
            trayMenu.Items.Add(pause);
            trayMenu.Items.Add(stop);
            trayMenu.Items.Add(previous);
            trayMenu.Items.Add(next);
            trayMenu.Items.Add("-");

            about = new ToolStripMenuItem();
            about.Text = "About";
            about.Image = Properties.Resources.icons8_info_point;
            about.Click += OnAbout;
            trayMenu.Items.Add(about);

            exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Image = Properties.Resources.icons8_cancel;
            exit.Click += OnExit;
            trayMenu.Items.Add(exit);

            trayIcon = new NotifyIcon();
            Fixes.Fixes.SetNotifyIconText(trayIcon, GetTooltipText("WinRadioTray"));
            //trayIcon.Icon = stoppedImage;
            trayIcon.Icon = stoppedImage;
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseUp += trayIcon_MouseUp;

            volumeTrackBar.TrackBar.ValueChanged += OnVolumeChange;
            balanceTrackBar.TrackBar.ValueChanged += OnBalanceChange;
            sleepTimerCtl.NumericUpDown.ValueChanged += ChangeSleepTimerDuration;
            volumeTrackBar.TrackBar.ValueChanged += OnVolumeChange;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)volumeTrackBar.TrackBar.Value / 100);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_PAN, (float)balanceTrackBar.TrackBar.Value / 100);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            hooker.Add("Toggle Play", Keys.MediaPlayPause);
            hooker.Add("Stop", Keys.MediaStop);

            if (stationSwitcher.Checked == true)
            {
                hooker.Add("Next Station", Keys.MediaNextTrack);
                hooker.Add("Previous Station", Keys.MediaPreviousTrack);
            }

            hooker.HotkeyDown += (sender, q) => {
                switch (q.Name)
                {
                    case "Toggle Play":
                        _status = Bass.BASS_ChannelIsActive(stream);
                        Console.WriteLine(_status);
                        if (_status == BASSActive.BASS_ACTIVE_STOPPED || _status == BASSActive.BASS_ACTIVE_PAUSED)
                        {
                            play.PerformClick();
                        } else if (_status == BASSActive.BASS_ACTIVE_PLAYING)
                        {
                            pause.PerformClick();
                        }
                        break;
                    case "Stop":
                        stop.PerformClick();
                        break;
                    case "Previous Station":
                        previous.PerformClick();
                        break;
                    case "Next Station":
                        next.PerformClick();
                        break;
                }
            };

            if (enableMMKeys.Checked == true)
            {
                hooker.Hook();
            }
            else
            {
                if (hooker.IsHooked) 
                {
                    hooker.Unhook();
                }
            }

            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;

            if (prefsRoot["autoplay"].InnerText == "true")
            {
                foreach (ToolStripMenuItem item in stations.DropDownItems)
                {
                   foreach (ToolStripMenuItem tmp in item.DropDownItems)
                    {
                        if (tmp.Text == prefsRoot["lastStation"].InnerText)
                        {
                            tmp.PerformClick();
                            break;
                        }
                    }
                }
            }

            base.OnLoad(e);
        }

        private void trayIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Reflection.MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                mi.Invoke(trayIcon, null);
            }
        }

        private void OnPlay(object sender, EventArgs e)
        {
            _status = Bass.BASS_ChannelIsActive(stream);
            if (_status == BASSActive.BASS_ACTIVE_STOPPED)
            {
                foreach (ToolStripMenuItem item in stations.DropDownItems)
                {
                    foreach (ToolStripMenuItem tmp in item.DropDownItems)
                    {
                        if (tmp.Text == lastStation)
                        {
                            tmp.PerformClick();
                            break;
                        }
                    }
                }
                if (enableSleepTimer.Checked == true)
                {
                    timeX.Start();
                }
            }
            else if (_status == BASSActive.BASS_ACTIVE_PAUSED)
            {
                if (enableSleepTimer.Checked == true)
                {
                    timeX.Start();
                }
                Bass.BASS_ChannelPlay(stream, true);
                Fixes.Fixes.SetNotifyIconText(trayIcon, GetTooltipText(_tagInfo.artist + "\r\n" + _tagInfo.title + "\r\n" + stationTitle));
                trayIcon.Icon = playingImage;
            }
            play.Visible = false;
            pause.Visible = true;
            stop.Enabled = true;
            next.Enabled = true;
            previous.Enabled = true;
        }

        private void OnPause(object sender, EventArgs e)
        {
            if (enableSleepTimer.Checked == true)
            {
                timeX.Stop();
            }
            Bass.BASS_ChannelPause(stream);
            Fixes.Fixes.SetNotifyIconText(trayIcon, stationTitle + " - PAUSED");
            trayIcon.Icon = stoppedImage;
            play.Visible = true;
            pause.Visible = false;
            stop.Enabled = false;
            next.Enabled = false;
            previous.Enabled = false;
        }

        private void OnStop(object sender, EventArgs e)
        {
            Bass.BASS_ChannelStop(stream);
            //Bass.BASS_ChannelPause(stream);
            Fixes.Fixes.SetNotifyIconText(trayIcon, "WinRadioTray");
            trayIcon.Icon = stoppedImage;
            if (timeX.Enabled == true)
            {
                timeX.Stop();
                timeX.Enabled = false;
            }
            sleepTimerDuration = Int32.Parse(sleepTimerCtl.NumericUpDown.Value.ToString());
            play.Visible = true;
            pause.Visible = false;
            stop.Enabled = false;
            next.Enabled = false;
            previous.Enabled = false;
        }

        private void OnPrevious(object sender, EventArgs e)
        {
            if (bookmarkList.IndexOf(stationTitle) > -1)
            {
                newStationIndex = bookmarkList.IndexOf(stationTitle) - 1;
                if (bookmarkList.IndexOf(stationTitle) > 0)
                {
                    stationTitle = bookmarkList[newStationIndex];
                    stationURL(urlList[newStationIndex]);
                }
                else
                {
                    stationTitle = bookmarkList[bookmarkList.Count - 1];
                    stationURL(urlList[bookmarkList.Count - 1]);
                }
            }
        }

        private void OnNext(object sender, EventArgs e)
        {
            if (bookmarkList.IndexOf(stationTitle) > -1)
            {
                newStationIndex = bookmarkList.IndexOf(stationTitle) + 1;
                if (bookmarkList.IndexOf(stationTitle) < bookmarkList.Count - 1)
                {
                    stationTitle = bookmarkList[newStationIndex];
                    stationURL(urlList[newStationIndex]);
                }
                else
                {
                    stationTitle = bookmarkList[0];
                    stationURL(urlList[0]);
                }
            }
        }

        private void OnManage(object sender, EventArgs e)
        {
            manageStations xmlForm = new manageStations();
            xmlForm.Show();
        }

        private void OnVolumeChange(object sender, EventArgs e)
        {
            //Bass.BASS_SetVolume((float)volumeTrackBar.TrackBar.Value / 100);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)volumeTrackBar.TrackBar.Value / 100);
            volumeTrackBar.Label.Text = "Volume: " + ((float)volumeTrackBar.TrackBar.Value).ToString();      
        }

        private void recenterVolume(object sender, EventArgs e)
        {
            //Bass.BASS_SetVolume(50);
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)0.5);
            volumeTrackBar.TrackBar.Value = 50;
            volumeTrackBar.Label.Text = "Volume: 50";
        }

        private void OnBalanceChange(object sender, EventArgs e)
        {
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_PAN, (float)balanceTrackBar.TrackBar.Value / 100);
            balanceTrackBar.Label.Text = "Balance: " + balanceTrackBar.TrackBar.Value.ToString();
        }

        private void recenterBalance(object sender, EventArgs e)
        {
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_PAN, 0);
            balanceTrackBar.TrackBar.Value = 0;
            volumeTrackBar.Label.Text = "Balance: 0";
        }

        private void ChangeSleepTimerDuration(object sender, EventArgs e)
        {
            sleepTimerDuration = Int32.Parse(sleepTimerCtl.NumericUpDown.Value.ToString());
        }

        private void OnExit(object sender, EventArgs e)
        {
            Bass.BASS_StreamFree(stream);
            Bass.BASS_Free();
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            prefsRoot["volume"].InnerText = ((float)volumeTrackBar.TrackBar.Value).ToString();
            prefsRoot["balance"].InnerText = ((float)balanceTrackBar.TrackBar.Value).ToString();
            prefsRoot["sleeptimerduration"].InnerText = sleepTimerCtl.NumericUpDown.Value.ToString();
            prefs.Save(path + "\\preferences.xml");
            Application.Exit();
            Environment.Exit(0);
        }

        private void OnShowLog(object sender, EventArgs e)
        {
            if (!File.Exists(path + "\\WinRadioTray.log"))
            {
                trayIcon.BalloonTipTitle = "File Not Found";
                trayIcon.BalloonTipText = "There is no log file to be displayed";
                trayIcon.ShowBalloonTip(5);
            } else
            {
                lf.Show();
                //Process.Start("notepad.exe", path + "\\WinRadioTray.log");

            }
        }

        private void OnClearLog(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to clear the playback log?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                if (File.Exists(path + "\\WinRadiotray.log"))
                {
                    File.WriteAllText(path + "\\WinRadiotray.log", string.Empty);
                }
            }
        }

        private void OnCustom(object sender, EventArgs e)
        {
            customURL customURL = new customURL();
            if (customURL.ShowDialog(this) == DialogResult.OK)
            {
                stationTitle = "Custom URL";
                stationURL(customURL.ReturnURL);
            }
        }

        private void AddCustomToStations(object sender, EventArgs e)
        {
            editStation editStation = new editStation(groupList, "", currentURL, "", "");
            editStation.StartPosition = FormStartPosition.CenterParent;
            editStation.Icon = Properties.Resources.icons8_radio_tower_34495e;
            editStation.Text = "Add Station";
            if (editStation.ShowDialog(this) == DialogResult.OK)
            {
                if (!groupList.Contains(editStation.ReturnGroup)) {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "\\bookmarks.xml");
                    XmlElement group = doc.CreateElement("group");
                    group.SetAttribute("name", editStation.ReturnGroup);
                    group.SetAttribute("img", "");
                    XmlElement bookmark = doc.CreateElement("bookmark");
                    bookmark.SetAttribute("name", editStation.ReturnName);
                    bookmark.SetAttribute("url", editStation.ReturnURL);
                    bookmark.SetAttribute("img", editStation.ReturnImage);
                    XmlNode root = doc.SelectSingleNode("bookmarks");
                    group.AppendChild(bookmark);
                    root.AppendChild(group);
                    doc.Save(path + "\\bookmarks.xml");
                    Console.WriteLine("it's a new group");
                } else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path + "\\bookmarks.xml");
                    XmlNode root = doc.SelectSingleNode("/bookmarks/group[@name='"+editStation.ReturnGroup+"']");
                    XmlElement bookmark = doc.CreateElement("bookmark");
                    bookmark.SetAttribute("name", editStation.ReturnName);
                    bookmark.SetAttribute("url", editStation.ReturnURL);
                    bookmark.SetAttribute("img", editStation.ReturnImage);
                    root.AppendChild(bookmark);
                    doc.Save(path + "\\bookmarks.xml");
                }
                updateStations();
            }
        }

        private void OnAbout(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private bool validateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void toggleDarkIcon(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (darkIcon.Checked)
            {
                darkIcon.Checked = false;
                prefsRoot["darkicon"].InnerText = "false";
                darkTheme = false;
                playingImage = Properties.Resources.icons8_radio_tower1;
                stoppedImage = Properties.Resources.icons8_radio_tower_idle;
            }
            else
            {
                darkIcon.Checked = true;
                prefsRoot["darkicon"].InnerText = "true";
                darkTheme = true;
                playingImage = Properties.Resources.icons8_radio_tower_dark;
                stoppedImage = Properties.Resources.icons8_radio_tower_idle_dark;
            }
            prefs.Save(path + "\\preferences.xml");
            _status = Bass.BASS_ChannelIsActive(stream);
            if (_status == BASSActive.BASS_ACTIVE_PLAYING)
            {
                trayIcon.Icon = playingImage;
            } else
            {
                trayIcon.Icon = stoppedImage;
            }
        }

        private void toggleAutoPlay(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (autoPlay.Checked)
            {
                autoPlay.Checked = false;
                prefsRoot["autoplay"].InnerText = "false";
            }
            else
            {
                autoPlay.Checked = true;
                prefsRoot["autoplay"].InnerText = "true";
            }
            prefs.Save(path + "\\preferences.xml");
        }

        private void toggleAutoRun(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (enableAutoRun.Checked)
            {
                rk.DeleteValue("WinRadioTray", false);
                enableAutoRun.Checked = false;
            }
            else
            {
                rk.SetValue("WinRadioTray", Application.ExecutablePath.ToString());
                enableAutoRun.Checked = true;
            }
        }

        private void toggleLogging(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (enableLogging.Checked)
            {
                enableLogging.Checked = false;
                prefsRoot["logging"].InnerText = "false";
            }
            else
            {
                enableLogging.Checked = true;
                prefsRoot["logging"].InnerText = "true";
            }
            prefs.Save(path + "\\preferences.xml");
        }

        private void toggleMMKeys(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (enableMMKeys.Checked)
            {
                enableMMKeys.Checked = false;
                prefsRoot["mmkeys"].InnerText = "false";
                hooker.Unhook();
            }
            else
            {
                enableMMKeys.Checked = true;
                prefsRoot["mmkeys"].InnerText = "true";
                hooker.Hook();
            }
            prefs.Save(path + "\\preferences.xml");
        }

        private void toggleStationSwitcher(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (stationSwitcher.Checked)
            {
                stationSwitcher.Checked = false;
                prefsRoot["stationswitcher"].InnerText = "false";
                if (hooker.KeyExists("Next Station"))
                {
                    hooker.Remove("Next Station");
                }
                if (hooker.KeyExists("Previous Station"))
                {
                    hooker.Remove("Previous Station");
                }
            }
            else
            {
                stationSwitcher.Checked = true;
                prefsRoot["stationswitcher"].InnerText = "true";
                hooker.Add("Next Station", Keys.MediaNextTrack);
                hooker.Add("Previous Station", Keys.MediaPreviousTrack);
            }
            prefs.Save(path + "\\preferences.xml");
        }

        private void toggleSleepTimer(object sender, EventArgs e)
        {
            XmlDocument prefs = new XmlDocument();
            prefs.Load(path + "\\preferences.xml");
            XmlNode prefsRoot = prefs.FirstChild;
            if (enableSleepTimer.Checked)
            {
                enableSleepTimer.Checked = false;
                prefsRoot["sleeptimer"].InnerText = "false";
                stationSwitcherActive = false;
            }
            else
            {
                enableSleepTimer.Checked = true;
                prefsRoot["sleeptimer"].InnerText = "true";
                stationSwitcherActive = true;
            }
            prefs.Save(path + "\\preferences.xml");
        }

        private void OnReload(object sender, EventArgs e)
        {
            trayMenu.Items.Remove(stations);
            stations = null;
            stations = new ToolStripMenuItem("Stations");
            stations.Image = Properties.Resources.icons8_radio_2;
            XPathNavigator nav;
            XPathDocument doc;
            bookmarkList.Clear();
            urlList.Clear();
            groupList.Clear();
            doc = new XPathDocument(path + "\\bookmarks.xml");
            nav = doc.CreateNavigator();
            XPathExpression exp = nav.Compile("bookmarks/group");
            exp.AddSort("@name", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text);
            XPathNodeIterator nodeIterator = nav.Select(exp);
            while (nodeIterator.MoveNext())
            {
                ToolStripMenuItem tmp = new ToolStripMenuItem(nodeIterator.Current.GetAttribute("name", ""));
                XPathExpression exp2 = nav.Compile("bookmarks/group[@name='" + nodeIterator.Current.GetAttribute("name", "") + "']/bookmark");
                exp2.AddSort("@name", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Text);
                nav = doc.CreateNavigator();
                XPathNodeIterator nodeIterator2 = nav.Select(exp2);
                while (nodeIterator2.MoveNext())
                {
                    
                    ToolStripMenuItem tmpStation = new ToolStripMenuItem(nodeIterator2.Current.GetAttribute("name", ""), null, new EventHandler(station_click));
                    tmpStation.Tag = nodeIterator2.Current.GetAttribute("url", "");
                    tmpStation.Image = (nodeIterator2.Current.SelectSingleNode("@img") != null) ? thumb(nodeIterator2.Current.GetAttribute("name", ""), nodeIterator2.Current.GetAttribute("img", ""), "station") : Properties.Resources.icons8_speaker;
                    tmp.DropDownItems.Add(tmpStation);
                    tmp.DropDownDirection = ToolStripDropDownDirection.Left;
                    bookmarkList.Add(nodeIterator2.Current.GetAttribute("name", ""));
                    urlList.Add(nodeIterator2.Current.GetAttribute("url", ""));
                }
                tmp.Image = (nodeIterator.Current.SelectSingleNode("@img") != null) ? thumb(nodeIterator.Current.GetAttribute("name", ""), nodeIterator.Current.GetAttribute("img", ""), "group") : Properties.Resources.icons8_radio_station;
                stations.DropDownItems.Add(tmp);
                groupList.Add(nodeIterator.Current.GetAttribute("name", ""));
            }
            trayMenu.Items.Insert(0, stations);
            addCustom.Visible = (urlList.Contains(currentURL)) ? false : true;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Bass.BASS_StreamFree(stream);
                Bass.BASS_Free();
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }


        protected void station_click(System.Object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            stationTitle = mi.Text;
            stationURL(mi.Tag.ToString());
        }

        protected void OnChangeOutput(object sender, EventArgs e)
        {
            String clickedOption = (((ToolStripMenuItem)sender).Name);
            foreach (ToolStripMenuItem item in outputs.DropDownItems)
            {
                if (item.Name == clickedOption)
                {
                    item.Checked = true;
                } else
                {
                    item.Checked = false;
                }
            }
            ((ToolStripMenuItem)sender).Checked = true;
            int foo = Int32.Parse(((System.Windows.Forms.ToolStripItem)sender).Tag.ToString());
            Console.WriteLine(stream);
            Console.WriteLine(foo);
            Bass.BASS_ChannelSetDevice(stream, foo); 
        }

        protected void stationURL(string url)
        {
            if (Utils.HighWord(Bass.BASS_GetVersion()) != Bass.BASSVERSION)
            {
                MessageBox.Show(this, "Wrong Bass Version!");
            }

            Bass.BASS_Free();

            Bass.BASS_SetConfigPtr(BASSConfig.BASS_CONFIG_NET_AGENT, _myUserAgentPtr);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 0);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1);

            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, this.Handle))
            {
                _aacPlugIn = Bass.BASS_PluginLoad("bass_aac.dll");
                _alacPlugin = Bass.BASS_PluginLoad("bassalac.dll");
                _dsdPlugin = Bass.BASS_PluginLoad("bassdsd.dll");
                _flacPlugin = Bass.BASS_PluginLoad("bassflac.dll");
                _hlsPlugin = Bass.BASS_PluginLoad("basshls.dll");
                _opusPlugin = Bass.BASS_PluginLoad("bassopus.dll");
                _wmaPlugin = Bass.BASS_PluginLoad("basswma.dll");
                _wvPlugin = Bass.BASS_PluginLoad("basswv.dll");
            }
            else
            {
                MessageBox.Show(this, "Bass_Init error!");
            }
            stream = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_STATUS, null, IntPtr.Zero);
            _tagInfo = new TAG_INFO(url);
            _mySync = new SYNCPROC(MetaSync);
            Bass.BASS_ChannelSetSync(stream, BASSSync.BASS_SYNC_META, 0, _mySync, IntPtr.Zero);
            if (Bass.BASS_ChannelPlay(stream, false) != true)
            {
                trayIcon.BalloonTipTitle = "Playback Error";
                trayIcon.BalloonTipText = "There was a problem playing back " + stationTitle + "\r\n"+ Bass.BASS_ErrorGetCode();
                trayIcon.ShowBalloonTip(5);
                //trayIcon.Icon = stoppedImage;
                trayIcon.Icon = stoppedImage;
                currentURL = null;
            } else
            {
                if (enableSleepTimer.Checked == true)
                {
                    timeX.Interval = 60000;
                    timeX.Tick += new EventHandler(timeX_Tick);
                    timeX.Enabled = true;
                }
                else
                {
                    timeX.Enabled = false;
                }
                //trayIcon.Icon = playingImage;
                trayIcon.Icon = playingImage;
                XmlDocument prefs = new XmlDocument();
                prefs.Load(path + "\\preferences.xml");
                XmlNode prefsRoot = prefs.FirstChild;
                if (prefsRoot["lastURL"] == null)
                {
                    XmlElement e = prefsRoot.OwnerDocument.CreateElement("lastURL");
                    e.InnerText = url;
                    prefsRoot.AppendChild(e);
                }
                else
                {
                    prefsRoot["lastURL"].InnerText = url;
                }
                if (prefsRoot["lastStation"] == null)
                {
                    XmlElement e = prefsRoot.OwnerDocument.CreateElement("lastStation");
                    e.InnerText = stationTitle;
                    prefsRoot.AppendChild(e);
                }
                else
                {
                    prefsRoot["lastStation"].InnerText = stationTitle;
                }
                prefs.Save(path + "\\preferences.xml");
                lastStation = stationTitle;
                currentURL = url;
                play.Enabled = true;
                play.Visible = false;
                pause.Visible = true;
                next.Enabled = true;
                previous.Enabled = true;
                stop.Enabled = true;
                
                addCustom.Visible = (urlList.Contains(url)) ? false : true;
            }
        }

        private void timeX_Tick(object sender, EventArgs e)
        {
            if (sleepTimerDuration-- <= 0)
            {
                timeX.Stop();
                Bass.BASS_ChannelStop(stream);
                Bass.BASS_Free();
                Fixes.Fixes.SetNotifyIconText(trayIcon, "WinRadioTray");
                //trayIcon.Icon = stoppedImage;
                trayIcon.Icon = stoppedImage;
                timeX.Enabled = false;
                sleepTimerDuration = Int32.Parse(sleepTimerCtl.NumericUpDown.Value.ToString());
                stop.Enabled = false;
                next.Enabled = false;
                previous.Enabled = false;
                pause.Visible = false;
                play.Visible = true;
            }
        }

        private void MetaSync(int handle, int channel, int data, IntPtr user)
        {
            if (_tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META), false, true))
            {
                tooltipText = _tagInfo.artist + "\r\n" + _tagInfo.title + "\r\n" + stationTitle;
                string logText = GetTimestamp(DateTime.Now) + ": " + _tagInfo.artist + " - " + _tagInfo.title + " - " + stationTitle;
                if (enableLogging.Checked == true)
                {
                    logFile = path + "\\WinRadioTray.log";
                    TextWriter t = new StreamWriter(logFile, true);
                    t.Close();
                    if (new FileInfo(logFile).Length == 0)
                    {
                        t = new StreamWriter(logFile, true);
                        t.WriteLine(logText);
                        t.Close();
                        t.Dispose();
                    } else
                    {
                        String last = File.ReadLines(@logFile).Last();
                        if (last == null || logText != last)
                        {
                            t = new StreamWriter(logFile, true);
                            t.WriteLine(logText);
                            t.Close();
                            t.Dispose();
                        }
                    }
                }
            }
            else
            {
                tooltipText = stationTitle;
            }
            Fixes.Fixes.SetNotifyIconText(trayIcon, GetTooltipText(tooltipText));
        }

        protected String GetTimestamp(DateTime value)
        {
            return value.ToString("MM/dd/yyyy HH:mm:ss");
        }

        protected String GetTooltipText(String text)
        {
            return text.Substring(0, Math.Min(text.Length, 127));
        }

        public static void updateStations()
        {
            foreach (object item in trayMenu.Items)
            {
                if (item.GetType().Equals(typeof(ToolStripMenuItem)))
                {
                    ToolStripMenuItem tmp = (ToolStripMenuItem)item;
                    if (tmp.Text == "Reload Stations")
                    {
                        tmp.PerformClick();
                        break;
                    }
                }     
            }
        }

        private bool registryValueExists(string registryRoot, string valueName)
        {
            if (Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "WinRadioTray", null) == null)
            {
                return false;
            } else
            {
               return true;
            }
        }

        protected Bitmap thumb(string displayname, string file, string type)
        {
            Bitmap b;
            Bitmap fallback = type == "group" ? WinRadioTray.Properties.Resources.icons8_radio_station : WinRadioTray.Properties.Resources.icons8_speaker;
            try
            {
                if (File.Exists(path + "\\images\\" + file))
                {
                    b = (Bitmap)Image.FromFile(path + "\\images\\" + file);
                } else
                {
                    b = fallback;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                b = fallback;
            }
            return b;
        }
    }
}