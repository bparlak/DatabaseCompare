namespace DBTest
{
    partial class EditRecord
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTarget = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtUpdateTarget = new System.Windows.Forms.TextBox();
            this.txtUpdateName = new System.Windows.Forms.TextBox();
            this.txtUpdateSource = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.lstRecordList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(440, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 48);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(16, 444);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(38, 13);
            this.lblTarget.TabIndex = 15;
            this.lblTarget.Text = "Target";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(15, 393);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "Name";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(14, 418);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(41, 13);
            this.lblSource.TabIndex = 17;
            this.lblSource.Text = "Source";
            // 
            // txtUpdateTarget
            // 
            this.txtUpdateTarget.Location = new System.Drawing.Point(58, 441);
            this.txtUpdateTarget.Name = "txtUpdateTarget";
            this.txtUpdateTarget.Size = new System.Drawing.Size(375, 20);
            this.txtUpdateTarget.TabIndex = 12;
            // 
            // txtUpdateName
            // 
            this.txtUpdateName.Location = new System.Drawing.Point(58, 389);
            this.txtUpdateName.Name = "txtUpdateName";
            this.txtUpdateName.Size = new System.Drawing.Size(187, 20);
            this.txtUpdateName.TabIndex = 13;
            // 
            // txtUpdateSource
            // 
            this.txtUpdateSource.Location = new System.Drawing.Point(58, 415);
            this.txtUpdateSource.Name = "txtUpdateSource";
            this.txtUpdateSource.Size = new System.Drawing.Size(375, 20);
            this.txtUpdateSource.TabIndex = 14;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(369, 41);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(370, 70);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(119, 23);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Location = new System.Drawing.Point(368, 12);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(120, 23);
            this.btnDeleteRecord.TabIndex = 9;
            this.btnDeleteRecord.Text = "Delete";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.Click += new System.EventHandler(this.btnDeleteRecord_Click);
            // 
            // lstRecordList
            // 
            this.lstRecordList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstRecordList.FormattingEnabled = true;
            this.lstRecordList.HorizontalScrollbar = true;
            this.lstRecordList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstRecordList.Location = new System.Drawing.Point(12, 12);
            this.lstRecordList.Name = "lstRecordList";
            this.lstRecordList.ScrollAlwaysVisible = true;
            this.lstRecordList.Size = new System.Drawing.Size(331, 366);
            this.lstRecordList.Sorted = true;
            this.lstRecordList.TabIndex = 8;
            // 
            // EditRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 475);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.txtUpdateTarget);
            this.Controls.Add(this.txtUpdateName);
            this.Controls.Add(this.txtUpdateSource);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.lstRecordList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditRecord";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Record";
            this.Load += new System.EventHandler(this.EditRecord_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtUpdateTarget;
        private System.Windows.Forms.TextBox txtUpdateName;
        private System.Windows.Forms.TextBox txtUpdateSource;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.ListBox lstRecordList;
    }
}