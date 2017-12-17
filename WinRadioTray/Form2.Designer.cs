using System;
using System.Data;
using System.IO;
using System.Xml.XPath;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace WinRadioTray
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stationURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookmarkDataSet = new System.Data.DataSet();
            this.saveChangesButton = new System.Windows.Forms.Button();
            this.editXML_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupName,
            this.stationName,
            this.stationURL});
            this.dataGridView1.Location = new System.Drawing.Point(13, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1192, 451);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // groupName
            // 
            this.groupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.groupName.DataPropertyName = "groupName";
            this.groupName.HeaderText = "Group";
            this.groupName.Name = "groupName";
            this.groupName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.groupName.Width = 21;
            // 
            // stationName
            // 
            this.stationName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.stationName.DataPropertyName = "stationName";
            this.stationName.HeaderText = "Name";
            this.stationName.Name = "stationName";
            this.stationName.Width = 21;
            // 
            // stationURL
            // 
            this.stationURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stationURL.DataPropertyName = "stationURL";
            this.stationURL.HeaderText = "URL";
            this.stationURL.Name = "stationURL";
            // 
            // bookmarkDataSet
            // 
            this.bookmarkDataSet.DataSetName = "bookmarkDataSet";
            // 
            // saveChangesButton
            // 
            this.saveChangesButton.Location = new System.Drawing.Point(1115, 470);
            this.saveChangesButton.Name = "saveChangesButton";
            this.saveChangesButton.Size = new System.Drawing.Size(90, 23);
            this.saveChangesButton.TabIndex = 1;
            this.saveChangesButton.Text = "Save Changes";
            this.saveChangesButton.UseVisualStyleBackColor = true;
            this.saveChangesButton.Click += new System.EventHandler(this.saveChanges_Click);
            // 
            // editXML_button
            // 
            this.editXML_button.Location = new System.Drawing.Point(1015, 470);
            this.editXML_button.Name = "editXML_button";
            this.editXML_button.Size = new System.Drawing.Size(94, 23);
            this.editXML_button.TabIndex = 2;
            this.editXML_button.Text = "Edit Raw XML";
            this.editXML_button.UseVisualStyleBackColor = true;
            this.editXML_button.Click += new System.EventHandler(this.editRawXML_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 500);
            this.Controls.Add(this.editXML_button);
            this.Controls.Add(this.saveChangesButton);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Manage Stations";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;

        private DataSet bookmarkDataSet;
        private Button saveChangesButton;
        private DataGridViewTextBoxColumn groupName;
        private DataGridViewTextBoxColumn stationName;
        private DataGridViewTextBoxColumn stationURL;
        private Button editXML_button;
    }
}