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
using System.Xml.Linq;

namespace WinRadioTray
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinRadioTray");
        Boolean nodeClicked = false;

        private void Form3_Load(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(path + "\\bookmarks.xml");

            //TreeNodeCollection groupNodes = new TreeNodeCollection(;

            var lv1s = from lv1 in xdoc.Descendants("group").OrderBy(lv1 => lv1.Attribute("name").Value)
                       select new
                       {
                           Group = lv1.Attribute("name").Value,
                           Stations = lv1.Descendants("bookmark")
                       };

            foreach (var lv1 in lv1s)
            {
                TreeNode groupNode = new TreeNode(lv1.Group.ToString());
                stationTreeView.Nodes.Add(groupNode);
                foreach (var lv2 in lv1.Stations.OrderBy(lv2 => lv2.Attribute("name").Value))
                {
                    TreeNode station = new TreeNode(lv2.Attribute("name").Value);
                    groupNode.Nodes.Add(station);
                }
            }
        }

        private void stationTreeView_MouseDoubleClick(Object sender, TreeNodeMouseClickEventArgs e)
        {
            nodeClicked = true;
            Console.WriteLine(e.Node.Text.ToUpper());
            Console.WriteLine(e.Node.Level);
        }

        private void stationTreeView_AfterExpand(Object sender, TreeViewCancelEventArgs e)
        {
            nodeClicked = false;
            //Console.WriteLine(e.Node.Text.ToUpper());
            //Console.WriteLine(e.Node.Level);
        }

        private void stationTreeView_BeforeExpand(Object sender, TreeViewCancelEventArgs e)
        {
            if (nodeClicked == true)
            {
                e.Cancel = true;
                nodeClicked = false;
            }
        }

        private void stationTreeView_BeforeCollapse(Object sender, TreeViewCancelEventArgs e)
        {
            if (nodeClicked == true)
            {
                e.Cancel = true;
                nodeClicked = false;
            }
        }
    }
}
