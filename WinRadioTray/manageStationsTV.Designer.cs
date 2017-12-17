namespace WinRadioTray
{
    partial class manageStationsTV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(manageStationsTV));
            this.stationTreeView = new System.Windows.Forms.TreeView();
            this.addGroupButton = new System.Windows.Forms.Button();
            this.editNode = new System.Windows.Forms.Button();
            this.addStationButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stationTreeView
            // 
            this.stationTreeView.FullRowSelect = true;
            this.stationTreeView.HideSelection = false;
            this.stationTreeView.Location = new System.Drawing.Point(13, 13);
            this.stationTreeView.Name = "stationTreeView";
            this.stationTreeView.Size = new System.Drawing.Size(381, 449);
            this.stationTreeView.TabIndex = 0;
            this.stationTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.stationTreeView_BeforeCollapse);
            this.stationTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.stationTreeView_BeforeExpand);
            this.stationTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.stationTreeView_MouseDoubleClick);
            // 
            // addGroupButton
            // 
            this.addGroupButton.Location = new System.Drawing.Point(399, 42);
            this.addGroupButton.Name = "addGroupButton";
            this.addGroupButton.Size = new System.Drawing.Size(136, 23);
            this.addGroupButton.TabIndex = 1;
            this.addGroupButton.Text = "Add Group";
            this.addGroupButton.UseVisualStyleBackColor = true;
            this.addGroupButton.Click += new System.EventHandler(this.addGroupButton_Click);
            // 
            // editNode
            // 
            this.editNode.Location = new System.Drawing.Point(399, 71);
            this.editNode.Name = "editNode";
            this.editNode.Size = new System.Drawing.Size(135, 23);
            this.editNode.TabIndex = 2;
            this.editNode.Text = "Edit";
            this.editNode.UseVisualStyleBackColor = true;
            this.editNode.Click += new System.EventHandler(this.button1_Click);
            // 
            // addStationButton
            // 
            this.addStationButton.Location = new System.Drawing.Point(400, 13);
            this.addStationButton.Name = "addStationButton";
            this.addStationButton.Size = new System.Drawing.Size(134, 23);
            this.addStationButton.TabIndex = 3;
            this.addStationButton.Text = "Add Station";
            this.addStationButton.UseVisualStyleBackColor = true;
            this.addStationButton.Click += new System.EventHandler(this.addStationButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(399, 101);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(135, 23);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(401, 438);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(134, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save Changes";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(401, 409);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(134, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // manageStationsTV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(545, 474);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addStationButton);
            this.Controls.Add(this.editNode);
            this.Controls.Add(this.addGroupButton);
            this.Controls.Add(this.stationTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "manageStationsTV";
            this.Text = "Manage Stations";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView stationTreeView;
        private System.Windows.Forms.Button addGroupButton;
        private System.Windows.Forms.Button editNode;
        private System.Windows.Forms.Button addStationButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}