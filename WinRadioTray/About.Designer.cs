namespace WinRadioTray
{
    partial class About
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
            this.headerLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.creditsButton = new System.Windows.Forms.Button();
            this.licenseButton = new System.Windows.Forms.Button();
            this.creditsBox = new System.Windows.Forms.LinkLabel();
            this.licenseBox = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // headerLabel
            // 
            this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.Location = new System.Drawing.Point(15, 6);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(620, 64);
            this.headerLabel.TabIndex = 1;
            this.headerLabel.Text = "WinRadioTray";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(12, 70);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(623, 23);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "Version 1.0";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // creditsButton
            // 
            this.creditsButton.Location = new System.Drawing.Point(245, 462);
            this.creditsButton.Name = "creditsButton";
            this.creditsButton.Size = new System.Drawing.Size(75, 23);
            this.creditsButton.TabIndex = 4;
            this.creditsButton.Text = "&Credits";
            this.creditsButton.UseVisualStyleBackColor = true;
            this.creditsButton.Click += new System.EventHandler(this.creditsButton_Click);
            // 
            // licenseButton
            // 
            this.licenseButton.Location = new System.Drawing.Point(327, 462);
            this.licenseButton.Name = "licenseButton";
            this.licenseButton.Size = new System.Drawing.Size(75, 23);
            this.licenseButton.TabIndex = 5;
            this.licenseButton.Text = "&License";
            this.licenseButton.UseVisualStyleBackColor = true;
            this.licenseButton.Click += new System.EventHandler(this.licenseButton_Click);
            // 
            // creditsBox
            // 
            this.creditsBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.creditsBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.creditsBox.Location = new System.Drawing.Point(15, 100);
            this.creditsBox.Name = "creditsBox";
            this.creditsBox.Padding = new System.Windows.Forms.Padding(10);
            this.creditsBox.Size = new System.Drawing.Size(620, 354);
            this.creditsBox.TabIndex = 7;
            this.creditsBox.TabStop = true;
            this.creditsBox.Text = "credits box here";
            this.creditsBox.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.creditsBox_LinkClicked);
            // 
            // licenseBox
            // 
            this.licenseBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.licenseBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.licenseBox.Location = new System.Drawing.Point(15, 100);
            this.licenseBox.Name = "licenseBox";
            this.licenseBox.Padding = new System.Windows.Forms.Padding(10);
            this.licenseBox.Size = new System.Drawing.Size(620, 354);
            this.licenseBox.TabIndex = 8;
            this.licenseBox.TabStop = true;
            this.licenseBox.Text = "license box here";
            this.licenseBox.Visible = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 492);
            this.Controls.Add(this.licenseBox);
            this.Controls.Add(this.creditsBox);
            this.Controls.Add(this.licenseButton);
            this.Controls.Add(this.creditsButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.headerLabel);
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.Text = "About";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button creditsButton;
        private System.Windows.Forms.Button licenseButton;
        private System.Windows.Forms.LinkLabel creditsBox;
        private System.Windows.Forms.LinkLabel licenseBox;
    }
}