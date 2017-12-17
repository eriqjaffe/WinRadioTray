using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRadioTray
{
    public partial class About : Form
    {

        public About()
        {

            InitializeComponent();

            this.Icon = Properties.Resources.icons8_radio_tower_34495e;

            creditsBox.Text = "WinRadioTray is a simple internet radio player that sits in the System Tray, hence the name.  It is heavily influenced by a Linux application called RadioTray," +
                " written by Carlos Ribiero.\r\n\r\nThis software makes use of the following:\r\n\r\n\t\u2022 BASS Audio Library for audio playback and metadata retrieval." +
                "\r\n\r\n\t\u2022 LowKey by Beluki.\r\n\r\n\t\u2022 Parts of the GUI are taken from GaGa, also by Beluki.\r\n\r\n\u2022 Icons are from icons8.\r\n\r\nTo the best of my knowledge, " +
                "the above sources are all free software without any licensing restrictions when used for non-commercial application.";
            creditsBox.Links.Add(149, 9, "http://radiotray.sourceforge.net/");
            creditsBox.Links.Add(238, 18, "http://www.un4seen.com/bass.html");
            creditsBox.Links.Add(306, 6, "https://github.com/Beluki/LowKey");
            creditsBox.Links.Add(362, 4, "https://github.com/Beluki/GaGa");
            creditsBox.Links.Add(404, 6, "https://icons8.com/");

            licenseBox.Text = "WinRadioTray\r\n\r\n\u00A9 2017 by Eriq Jaffe (eriqjaffe@gmail.com)\r\n\r\nWinRadioTray is free software: you can redistribute it and / or modify it under the terms of the " +
                "GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.\r\n\r\n WinRadioTray is " +
                "distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. " +
                "See the GNU General Public License for more details.\r\n\r\nYou should have received a copy of the GNU General Public License along with WinRadioTray." +
                "If not, see http://www.gnu.org/licenses/.";
            licenseBox.Links.Add(38, 19, "mailto:eriqjaffe@gmail.com");
            licenseBox.Links.Add(646, 29, "http://www.gnu.org/licenses/");

            versionLabel.Text = "Version " + Application.ProductVersion;

        }

        private void creditsBox_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }

        private void creditsButton_Click(object sender, EventArgs e)
        {
            licenseBox.Visible = false;
            creditsBox.Visible = true;
        }

        private void licenseButton_Click(object sender, EventArgs e)
        {
            creditsBox.Visible = false;
            licenseBox.Visible = true;
        }
    }
}
