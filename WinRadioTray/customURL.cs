using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinRadioTray
{
    public partial class customURL : Form
    {
        public customURL()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
        }

        public string ReturnURL { get; set; }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (!validateURL(urlBox.Text))
            {
                MessageBox.Show("That doesn't appear to be a valid URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                this.DialogResult = DialogResult.OK;
                this.ReturnURL = urlBox.Text;
            }
            
        }

        private bool validateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
