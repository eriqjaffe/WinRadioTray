using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRadioTray
{
    public partial class logForm : Form
    {

        private string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        public void Log(string str)
        {
            logBox.Text += str + Environment.NewLine;
        }

        public logForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
            if (File.Exists(path + "\\WinRadioTray.log"))
            {
                using (StreamReader reader = new StreamReader(path + "\\WinRadioTray.log"))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        logBox.Text += line + Environment.NewLine;
                        line = reader.ReadLine();
                    }
                    reader.Close();
                    reader.Dispose();
                }
            }
        }
    }
}
