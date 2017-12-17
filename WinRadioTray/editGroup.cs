using System;
using System.IO;
using System.Windows.Forms;

namespace WinRadioTray
{
    public partial class editGroup : Form
    {
        public editGroup(string x, string i)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
            groupName.Text = x;
            image.Text = i;
            openFileDialog1.Filter = "Image Files (*.PNG, *.JPG; *.GIF, *.BMP)| *.PNG; *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)+"\\images";
            if (image.Text.Length > 0)
            {
                removeButton.Visible = true;
                imageSelectButton.Visible = false;
            }
            else
            {
                removeButton.Visible = false;
                imageSelectButton.Visible = true;
            }
        }

        public string ReturnValue { get; set; }
        public string ReturnImage { get; set; }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(groupName.Text))
            {
                MessageBox.Show("You forgot to enter a group name.", "Value Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.ReturnValue = groupName.Text;
                this.ReturnImage = image.Text;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageSelectButton_Click(object sender, EventArgs e)
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
                imageSelectButton.Visible = false;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            image.Text = "";
            removeButton.Visible = false;
            imageSelectButton.Visible = true;
        }
    }
}
