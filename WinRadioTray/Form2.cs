using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace WinRadioTray
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinRadioTray");
        AutoCompleteStringCollection groupAutoComplete = new AutoCompleteStringCollection();

        private void Form2_Load(object sender, EventArgs e)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinRadioTray");
            XDocument xdoc = XDocument.Load(path + "\\bookmarks.xml");

            var lv1s = from lv1 in xdoc.Descendants("group").OrderBy(lv1 => lv1.Attribute("name").Value)
                       select new
                       {
                           Group = lv1.Attribute("name").Value,
                           Stations = lv1.Descendants("bookmark")
                       };

            foreach (var lv1 in lv1s)
            {
                foreach (var lv2 in lv1.Stations.OrderBy(lv2 => lv2.Attribute("name").Value))
                {
                    Console.WriteLine(lv1.Group + " " + lv2.Attribute("name").Value);
                    dataGridView1.Rows.Add(lv1.Group, lv2.Attribute("name").Value, lv2.Attribute("url").Value);
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int column = dataGridView1.CurrentCell.ColumnIndex;
            string headerText = dataGridView1.Columns[column].HeaderText;

            if (headerText.Equals("Group"))
            {
                groupAutoComplete.Clear();

                List<string> vv = dataGridView1.Rows.Cast<DataGridViewRow>()
                           .Where(x => !x.IsNewRow)                   // either..
                           .Where(x => x.Cells[0].Value != null) //..or or both
                           .Select(x => x.Cells[0].Value.ToString())
                           .Distinct()
                           .ToList();
                
                foreach(var v in vv)
                {
                    groupAutoComplete.Add(v);
                }

                TextBox tb = e.Control as TextBox;

                if (tb != null)
                {
                    tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    tb.AutoCompleteCustomSource = groupAutoComplete;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            //var datagridview = sender as DataGridView;

            //if (e.ColumnIndex == 1 && e.RowIndex > 0)
            //{
            //    datagridview.BeginEdit(true);
            //    ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            //}
        }
        
        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //Console.WriteLine(e.ColumnIndex);
            if (e.ColumnIndex == 2 && e.RowIndex > 0)
            {
                String enteredText = ((TextBox)sender).Text;
            }
        }

        private void editRawXML_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("If you edit the XML outside of WinRadioTray, you'll have to reload the stations from the menu if you make any changes.", "Heads Up", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (confirm == DialogResult.OK)
            {
                System.Diagnostics.Process.Start(path + "\\bookmarks.xml");
                this.Close();
            }
        }

        private bool validateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("bookmarks");
            Boolean invalidURLs = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                string tmpURL = row.Cells[2].FormattedValue.ToString();
                int rowIndex = -1;
                DataGridViewRow tmpRow = dataGridView1.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value.ToString().Equals(tmpURL))
                    .First();
                rowIndex = row.Index;
                if (!validateURL(tmpURL))
                {
                    invalidURLs = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[2];
                    dataGridView1.CurrentCell.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                }
                else
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[2];
                    dataGridView1.CurrentCell.Style = new DataGridViewCellStyle { ForeColor = Color.Black };
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                string tmpURL = row.Cells[2].FormattedValue.ToString();
                int rowIndex = -1;
                DataGridViewRow tmpRow = dataGridView1.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value.ToString().Equals(tmpURL))
                    .First();
                rowIndex = row.Index;
                if (!validateURL(tmpURL))
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                    break;
                }
            }

            if (invalidURLs == true)
            {
                dataGridView1.ClearSelection();
                MessageBox.Show("I found at least one invalid URL, hold up!", "Invalid URL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var distinctGroups = dataGridView1.Rows.Cast<DataGridViewRow>()
                           .Where(x => !x.IsNewRow)                   // either..
                           .Where(x => x.Cells[0].Value != null) //..or or both
                           .Select(x => x.Cells[0].Value.ToString())
                           .Distinct()
                           .ToList();

            distinctGroups.ForEach(delegate (String groupName)
            {
                XmlElement group = doc.CreateElement("group");
                group.SetAttribute("name", groupName);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        if (row.Cells[0].Value.ToString() == groupName)
                        {
                            XmlElement bookmark = doc.CreateElement("bookmark");
                            bookmark.SetAttribute("name", row.Cells[1].Value.ToString());
                            bookmark.SetAttribute("url", row.Cells[2].Value.ToString());
                            group.AppendChild(bookmark);
                        }
                    }

                }
                root.AppendChild(group);
                doc.AppendChild(root);
            });
            Console.WriteLine(doc.OuterXml);
            DialogResult confirm = MessageBox.Show("Are you sure?  This will permanently overwrite your existing bookmarks.", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes) 
            {
                doc.Save(path + "\\bookmarks.xml");
                SysTrayApp.updateStations();
                this.Close();
            }
        }
    }
}
