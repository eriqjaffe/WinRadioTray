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
    public partial class editStation : Form
    {
        public editStation(List<String> gps, string cn, string curl, string cg, string img)
        {
            InitializeComponent();
            name.Text = cn;
            url.Text = curl;
            image.Text = img;
            group.DataSource = gps;
            group.SelectedIndex = group.FindString(cg);
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
            openFileDialog1.Filter = "Image Files (*.PNG, *.JPG; *.GIF, *.BMP)| *.PNG; *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)+"\\images";
            if (image.Text.Length > 0)
            {
                removeButton.Visible = true;
                imgSelectButton.Visible = false;
            } else
            {
                removeButton.Visible = false;
                imgSelectButton.Visible = true;
            }
        }

        public string ReturnName { get; set; }
        public string ReturnURL { get; set; }
        public string ReturnGroup { get; set; }
        public string ReturnImage { get; set; }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(name.Text) || String.IsNullOrEmpty(url.Text) || String.IsNullOrEmpty(group.Text))
            {
                MessageBox.Show("You forgot to enter a required value.", "Value Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.None;
            }
            if (!validateURL(url.Text))
            {
                MessageBox.Show("That doesn't appear to be a valid URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.ReturnName = name.Text;
                this.ReturnURL = url.Text;
                this.ReturnGroup = group.Text;
                this.ReturnImage = image.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool validateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void imgSelectButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fullPath = openFileDialog1.FileName;
                string displayName = openFileDialog1.SafeFileName;
                string fallbackDisplayname = image.Text;
                if (fullPath.Substring(0, fullPath.LastIndexOf('\\')) != openFileDialog1.InitialDirectory)
                {
                    string sourceFile = openFileDialog1.FileName;
                    string destFile = openFileDialog1.InitialDirectory + "\\" + openFileDialog1.SafeFileName;
                    if (File.Exists(destFile))
                    {
                        DialogResult confirm = MessageBox.Show("A file already exists with that name.  Overwrite?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirm == DialogResult.Yes)
                        {
                            File.Copy(sourceFile, destFile, true);
                        }
                        else
                        {
                            displayName = fallbackDisplayname;
                        }
                    }
                    else
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                image.Text = displayName;
                removeButton.Visible = true;
                imgSelectButton.Visible = false;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            image.Text = "";
            removeButton.Visible = false;
            imgSelectButton.Visible = true;
        }
    }
}
