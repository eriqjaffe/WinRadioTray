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

    public partial class manageStations : Form
    {

        public manageStations()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icons8_radio_tower_34495e;
        }

        string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        List<string> initialState = new List<string>();
        List<string> closingState = new List<string>();

        private void Form3_Load(object sender, EventArgs e)
        {
            initialState.Clear();
            closingState.Clear();
            XDocument xdoc = XDocument.Load(path + "\\bookmarks.xml");

            var lv1s = from lv1 in xdoc.Descendants("group").OrderBy(lv1 => lv1.Attribute("name").Value)
                       select new
                       {
                           Group = lv1.Attribute("name").Value,
                           Image = lv1.Attribute("img").Value,
                           Stations = lv1.Descendants("bookmark"),
                       };

            foreach (var lv1 in lv1s)
            {
                TreeNode groupNode = new TreeNode(lv1.Group.ToString());
                groupNode.Tag = lv1.Image.ToString();
                stationTreeView.Nodes.Add(groupNode);
                foreach (var lv2 in lv1.Stations.OrderBy(lv2 => lv2.Attribute("name").Value))
                {
                    TreeNode station = new TreeNode(lv2.Attribute("name").Value);
                    station.Tag = new StationInfo { url = lv2.Attribute("url").Value, img = lv2.Attribute("img").Value };
                    groupNode.Nodes.Add(station);
                }
            }

            treeToList(stationTreeView, initialState);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stationTreeView.SelectedNode.Level == 0)
            {
                editGroup editGroup = new editGroup(stationTreeView.SelectedNode.Text, stationTreeView.SelectedNode.Tag.ToString());
                editGroup.StartPosition = FormStartPosition.CenterParent;
                if (editGroup.ShowDialog(this) == DialogResult.OK)
                {
                    stationTreeView.SelectedNode.Text = editGroup.ReturnValue;
                    stationTreeView.SelectedNode.Tag = editGroup.ReturnImage;
                    stationTreeView.SelectedNode = GetNode(editGroup.ReturnValue);
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
                var metainfo = (StationInfo)stationTreeView.SelectedNode.Tag;
                editStation editStation = new editStation(groupNames, stationTreeView.SelectedNode.Text, metainfo.url, stationTreeView.SelectedNode.Parent.Text, metainfo.img);
                editStation.StartPosition = FormStartPosition.CenterParent;
                if (editStation.ShowDialog(this) == DialogResult.OK)
                {
                    if (stationTreeView.SelectedNode.Parent.Text == editStation.ReturnGroup)
                    {
                        stationTreeView.SelectedNode.Text = editStation.ReturnName;
                        stationTreeView.SelectedNode.Tag = new StationInfo { url = editStation.ReturnURL, img = editStation.ReturnImage };
                        stationTreeView.Sort();
                        stationTreeView.SelectedNode = GetNode(editStation.ReturnName);
                    }
                    else
                    {
                        TreeNode newParent = GetNode(editStation.ReturnGroup);
                        if (newParent != null)
                        {
                            stationTreeView.SelectedNode.Remove();
                            TreeNode newStation = new TreeNode(editStation.ReturnName);
                            newStation.Tag = new StationInfo { url = editStation.ReturnURL, img = editStation.ReturnImage };
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
                            newStation.Tag = new StationInfo { url = editStation.ReturnURL, img = editStation.ReturnImage };
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
            editStation editStation = new editStation(groupNames, "", "", "", "");
            editStation.StartPosition = FormStartPosition.CenterParent;
            editStation.Text = "Add Station";
            if (editStation.ShowDialog(this) == DialogResult.OK)
            {
                TreeNode newParent = GetNode(editStation.ReturnGroup);
                if (newParent != null)
                {
                    TreeNode newStation = new TreeNode(editStation.ReturnName);
                    newStation.Tag = new StationInfo { url = editStation.ReturnURL, img = editStation.ReturnImage };
                    newParent.Nodes.Add(newStation);
                    stationTreeView.Sort();
                    stationTreeView.SelectedNode = newStation;
                    newStation.Parent.Expand();
                }
                else
                {
                    newParent = new TreeNode(editStation.ReturnGroup);
                    TreeNode newStation = new TreeNode(editStation.ReturnName);
                    newStation.Tag = new StationInfo { url = editStation.ReturnURL, img = editStation.ReturnImage };
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
            editGroup editGroup = new editGroup("", "");
            editGroup.Text = "Add Group";
            editGroup.StartPosition = FormStartPosition.CenterParent;
            if (editGroup.ShowDialog(this) == DialogResult.OK)
            {
                TreeNode groupNode = new TreeNode(editGroup.ReturnValue);
                groupNode.Tag = editGroup.ReturnImage;
                stationTreeView.Nodes.Add(groupNode);
                stationTreeView.Sort();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to save your changes?  This will overwrite your current bookmarks file.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                TrimTree();
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("bookmarks");
                foreach (TreeNode n in stationTreeView.Nodes)
                {
                    XmlElement group = doc.CreateElement("group");
                    group.SetAttribute("name", n.Text);
                    group.SetAttribute("img", n.Tag.ToString());
                    foreach (TreeNode r in n.Nodes)
                    {
                        XmlElement bookmark = doc.CreateElement("bookmark");
                        bookmark.SetAttribute("name", r.Text);
                        var metainfo = (StationInfo)r.Tag;
                        bookmark.SetAttribute("url", metainfo.url);
                        bookmark.SetAttribute("img", metainfo.img);
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
            closingState.Clear();
            treeToList(stationTreeView, closingState);
            if (!initialState.SequenceEqual(closingState))
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to cancel?  There appear to be unsaved changes that will be lost.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void stationTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (stationTreeView.SelectedNode.Level == 0)
            {
                editNode.Text = "&Edit Group";
            }
            if (stationTreeView.SelectedNode.Level == 1)
            {
                editNode.Text = "&Edit Station";
            }
        }

        private void treeToList(TreeView treeView, List<string> list)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                list.Add(n.Text);
                TreeNodeCollection stations = n.Nodes;
                foreach (TreeNode s in stations)
                {
                    var metainfo = (StationInfo)s.Tag;
                    list.Add(s.Text);
                    list.Add(metainfo.url);
                    list.Add(metainfo.img);
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

        private void stationTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            //stationTreeView.SelectedNode = e.Node;
        }

        private void stationTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            stationTreeView.SelectedNode = e.Node;
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            editNode.PerformClick();
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            removeButton.PerformClick();
        }
    }

    public class StationInfo
    {
        public string url { get; set; }
        public string img { get; set; }
    }
}
