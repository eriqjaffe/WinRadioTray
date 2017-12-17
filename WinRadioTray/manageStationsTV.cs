using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WinRadioTray
{
    public partial class manageStationsTV : Form
    {

        public manageStationsTV()
        {
            InitializeComponent();
        }

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinRadioTray");

        private void Form3_Load(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(path + "\\bookmarks.xml");

            var lv1s = from lv1 in xdoc.Descendants("group").OrderBy(lv1 => lv1.Attribute("name").Value)
                       select new
                       {
                           Group = lv1.Attribute("name").Value,
                           Stations = lv1.Descendants("bookmark"),
                       };

            foreach (var lv1 in lv1s)
            {
                TreeNode groupNode = new TreeNode(lv1.Group.ToString());
                stationTreeView.Nodes.Add(groupNode);
                foreach (var lv2 in lv1.Stations.OrderBy(lv2 => lv2.Attribute("name").Value))
                {
                    TreeNode station = new TreeNode(lv2.Attribute("name").Value);
                    station.Tag = lv2.Attribute("url").Value;
                    groupNode.Nodes.Add(station);
                }
            }
        }

        private void stationTreeView_MouseDoubleClick(Object sender, TreeNodeMouseClickEventArgs e)
        {
            //nodeClicked = true;
            //Console.WriteLine(e.Node.Text.ToUpper());
            //Console.WriteLine(e.Node.Level);
        }

        private void stationTreeView_AfterExpand(Object sender, TreeViewCancelEventArgs e)
        {
            //nodeClicked = false;
            //Console.WriteLine(e.Node.Text.ToUpper());
            //Console.WriteLine(e.Node.Level);
        }

        private void stationTreeView_BeforeExpand(Object sender, TreeViewCancelEventArgs e)
        {
            //if (nodeClicked == true)
            //{
            //    e.Cancel = true;
            //    nodeClicked = false;
            //}
        }

        private void stationTreeView_BeforeCollapse(Object sender, TreeViewCancelEventArgs e)
        {
            //if (nodeClicked == true)
            //{
            //    e.Cancel = true;
            //    nodeClicked = false;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stationTreeView.SelectedNode.Level == 0)
            {
                editGroup editGroup = new editGroup(stationTreeView.SelectedNode.Text);
                editGroup.StartPosition = FormStartPosition.CenterParent;
                if (editGroup.ShowDialog(this) == DialogResult.OK)
                {
                    Console.WriteLine(editGroup.ReturnValue);
                    stationTreeView.SelectedNode.Text = editGroup.ReturnValue;
                    stationTreeView.Sort();
                }
            } else if (stationTreeView.SelectedNode.Level == 1)
            {
                List<string> groupNames = new List<string>();
                groupNames.Clear();
                TreeNodeCollection groupNodes = stationTreeView.Nodes;
                foreach (TreeNode n in groupNodes)
                {
                    groupNames.Add(n.Text);
                }
                editStation editStation = new editStation(groupNames, stationTreeView.SelectedNode.Text, stationTreeView.SelectedNode.Tag.ToString(), stationTreeView.SelectedNode.Parent.Text);
                editStation.StartPosition = FormStartPosition.CenterParent;
                if (editStation.ShowDialog(this) == DialogResult.OK)
                {
                    if (stationTreeView.SelectedNode.Parent.Text == editStation.ReturnGroup)
                    {
                        stationTreeView.SelectedNode.Text = editStation.ReturnName;
                        stationTreeView.SelectedNode.Tag = editStation.ReturnURL;
                        stationTreeView.Sort();
                    }
                    else
                    {
                        TreeNode newParent = GetNode(editStation.ReturnGroup);
                        if (newParent != null)
                        {
                            stationTreeView.SelectedNode.Remove();
                            TreeNode newStation = new TreeNode(editStation.ReturnName);
                            newStation.Tag = editStation.ReturnURL;
                            newParent.Nodes.Add(newStation);
                            stationTreeView.Sort();
                            stationTreeView.SelectedNode = newStation;
                            newStation.Parent.Expand();
                        }
                        else
                        {
                            newParent = new TreeNode(editStation.ReturnGroup);
                            stationTreeView.SelectedNode.Remove();
                            TreeNode newStation = new TreeNode(editStation.ReturnName);
                            newStation.Tag = editStation.ReturnURL;
                            newParent.Nodes.Add(newStation);
                            stationTreeView.Nodes.Add(newParent);
                            stationTreeView.Sort();
                            stationTreeView.SelectedNode = newStation;
                            newStation.Parent.Expand();
                        }
                    }
                    TrimTree();
                }
            }           
        }

        private void TrimTree()
        {
            for (int i = stationTreeView.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode n = stationTreeView.Nodes[i];
                if (n.Nodes.Count == 0)
                {
                    n.Remove();
                }
            }
        }

        public TreeNode GetNode(string text, TreeNode rootNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if (node.Text.Equals(text)) return node;
                TreeNode next = GetNode(text, node);
                if (next != null) return next;
            }
            return null;
        }

        public TreeNode GetNode(string text)
        {
            TreeNode itemNode = null;
            foreach (TreeNode node in stationTreeView.Nodes)
            {
                if (node.Text.Equals(text)) return node;
                itemNode = GetNode(text, node);
                if (itemNode != null) break;
            }
            return itemNode;
        }

        private void updateStation(string group, string name, string url)
        {
            if (stationTreeView.SelectedNode.Parent.Text == group)
            {
                stationTreeView.SelectedNode.Text = name;
                stationTreeView.SelectedNode.Tag = url;
                stationTreeView.Sort();
            }
                else
                {
                TreeNode newParent = GetNode(group);
                if (newParent != null)
                {
                    stationTreeView.SelectedNode.Remove();
                    TreeNode newStation = new TreeNode(name);
                    newStation.Tag = url;
                    newParent.Nodes.Add(newStation);
                    stationTreeView.Sort();
                    stationTreeView.SelectedNode = newStation;
                    newStation.Parent.Expand();
                }
                else
                {
                    newParent = new TreeNode(group);
                    stationTreeView.SelectedNode.Remove();
                    TreeNode newStation = new TreeNode(name);
                    newStation.Tag = url;
                    newParent.Nodes.Add(newStation);
                    stationTreeView.Nodes.Add(newParent);
                    stationTreeView.Sort();
                    stationTreeView.SelectedNode = newStation;
                    newStation.Parent.Expand();
                }
            }
            TrimTree();
        }

        private void addStationButton_Click(object sender, EventArgs e)
        {
            List<string> groupNames = new List<string>();
            groupNames.Clear();
            XDocument xdoc = XDocument.Load(path + "\\bookmarks.xml");
            var lv1s = from lv1 in xdoc.Descendants("group").OrderBy(lv1 => lv1.Attribute("name").Value)
                       select new
                       {
                           Group = lv1.Attribute("name").Value
                       };
            foreach (var lv1 in lv1s)
            {
                groupNames.Add(lv1.Group);
            }
            editStation editStation = new editStation(groupNames, "", "", "");
            editStation.StartPosition = FormStartPosition.CenterParent;
            editStation.Text = "Add Station";
            if (editStation.ShowDialog(this) == DialogResult.OK)
            {
                Console.WriteLine("Add here...");
                Console.WriteLine(editStation.ReturnGroup);
                TreeNode newParent = GetNode(editStation.ReturnGroup);
                if (newParent != null)
                {
                    TreeNode newStation = new TreeNode(editStation.ReturnName);
                    newStation.Tag = editStation.ReturnURL;
                    newParent.Nodes.Add(newStation);
                    stationTreeView.Sort();
                    stationTreeView.SelectedNode = newStation;
                    newStation.Parent.Expand();
                }
                else
                {
                    newParent = new TreeNode(editStation.ReturnGroup);
                    TreeNode newStation = new TreeNode(editStation.ReturnName);
                    newStation.Tag = editStation.ReturnURL;
                    newParent.Nodes.Add(newStation);
                    stationTreeView.Nodes.Add(newParent);
                    stationTreeView.Sort();
                    stationTreeView.SelectedNode = newStation;
                    newStation.Parent.Expand();
                }
            }
            TrimTree();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (stationTreeView.SelectedNode.Level == 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to remove the selected group?  All stations in that group will also be deleted!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    stationTreeView.SelectedNode.Remove();
                }
            }
            else if (stationTreeView.SelectedNode.Level == 1)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to remove the selected station?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    stationTreeView.SelectedNode.Remove();
                }
            }
        }

        public void addGroupButton_Click(object sender, EventArgs e)
        {
            addGroup addGroup = new addGroup();
            addGroup.StartPosition = FormStartPosition.CenterParent;
            if (addGroup.ShowDialog(this) == DialogResult.OK)
            {
                Console.WriteLine(addGroup.ReturnValue);
                TreeNode groupNode = new TreeNode(addGroup.ReturnValue);
                stationTreeView.Nodes.Add(groupNode);
                stationTreeView.Sort();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to save your changes?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                TrimTree();
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("bookmarks");
                foreach (TreeNode n in stationTreeView.Nodes)
                {
                    XmlElement group = doc.CreateElement("group");
                    group.SetAttribute("name", n.Text);
                    foreach (TreeNode r in n.Nodes)
                    {
                        XmlElement bookmark = doc.CreateElement("bookmark");
                        bookmark.SetAttribute("name", r.Text);
                        bookmark.SetAttribute("url", r.Tag.ToString());
                        group.AppendChild(bookmark);
                    }
                    root.AppendChild(group);
                }
                doc.AppendChild(root);
                doc.Save(path + "\\bookmarks.xml");
                SysTrayApp.updateStations();
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
