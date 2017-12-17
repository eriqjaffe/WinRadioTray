namespace WinRadioTray
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.stationTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // stationTreeView
            // 
            this.stationTreeView.Location = new System.Drawing.Point(13, 13);
            this.stationTreeView.Name = "stationTreeView";
            this.stationTreeView.Size = new System.Drawing.Size(381, 449);
            this.stationTreeView.TabIndex = 0;
            this.stationTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.stationTreeView_BeforeCollapse);
            this.stationTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.stationTreeView_BeforeExpand);
            this.stationTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.stationTreeView_MouseDoubleClick);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 474);
            this.Controls.Add(this.stationTreeView);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView stationTreeView;
    }
}