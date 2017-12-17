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
    public partial class addGroup : Form
    {
        public addGroup()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
        }

        public string ReturnValue { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(newGroupName.Text))
            {
                MessageBox.Show("You forgot to enter a group name.", "Value Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.ReturnValue = newGroupName.Text;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageSelectButton_Click(object sender, EventArgs e)
        {

        }
    }
}
