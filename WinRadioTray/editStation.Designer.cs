namespace WinRadioTray
{
    partial class editStation
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
            this.name = new System.Windows.Forms.TextBox();
            this.url = new System.Windows.Forms.TextBox();
            this.group = new System.Windows.Forms.ComboBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.groupLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.imageLabel = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.TextBox();
            this.imgSelectButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.removeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(56, 13);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(438, 20);
            this.name.TabIndex = 0;
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(56, 40);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(437, 20);
            this.url.TabIndex = 1;
            // 
            // group
            // 
            this.group.FormattingEnabled = true;
            this.group.Location = new System.Drawing.Point(56, 67);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(436, 21);
            this.group.TabIndex = 2;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 16);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name:";
            this.nameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(12, 43);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(29, 13);
            this.urlLabel.TabIndex = 4;
            this.urlLabel.Text = "URL";
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(12, 70);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(39, 13);
            this.groupLabel.TabIndex = 5;
            this.groupLabel.Text = "Group:";
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(257, 121);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(176, 121);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageLabel
            // 
            this.imageLabel.AutoSize = true;
            this.imageLabel.Location = new System.Drawing.Point(12, 98);
            this.imageLabel.Name = "imageLabel";
            this.imageLabel.Size = new System.Drawing.Size(39, 13);
            this.imageLabel.TabIndex = 8;
            this.imageLabel.Text = "Image:";
            // 
            // image
            // 
            this.image.Location = new System.Drawing.Point(56, 95);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(372, 20);
            this.image.TabIndex = 9;
            // 
            // imgSelectButton
            // 
            this.imgSelectButton.Location = new System.Drawing.Point(434, 94);
            this.imgSelectButton.Name = "imgSelectButton";
            this.imgSelectButton.Size = new System.Drawing.Size(57, 23);
            this.imgSelectButton.TabIndex = 10;
            this.imgSelectButton.Text = "&Add";
            this.imgSelectButton.UseVisualStyleBackColor = true;
            this.imgSelectButton.Visible = false;
            this.imgSelectButton.Click += new System.EventHandler(this.imgSelectButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(434, 93);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(57, 23);
            this.removeButton.TabIndex = 11;
            this.removeButton.Text = "&Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Visible = false;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // editStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(506, 151);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.imgSelectButton);
            this.Controls.Add(this.image);
            this.Controls.Add(this.imageLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupLabel);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.group);
            this.Controls.Add(this.url);
            this.Controls.Add(this.name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "editStation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Station";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.ComboBox group;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label imageLabel;
        private System.Windows.Forms.TextBox image;
        private System.Windows.Forms.Button imgSelectButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button removeButton;
    }
}