namespace DBTest
{
    partial class ConnectingForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.formControl = new System.Windows.Forms.ErrorProvider(this.components);
            this.bgWorkerTarget = new System.ComponentModel.BackgroundWorker();
            this.bgWorkerSource = new System.ComponentModel.BackgroundWorker();
            this.txtRecordName = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.cmbServerNameTarget = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.btnDatabaseTarget = new System.Windows.Forms.Button();
            this.rBWindowsAuthTarget = new System.Windows.Forms.RadioButton();
            this.rBSQLServerAuthTarget = new System.Windows.Forms.RadioButton();
            this.cmbDatabaseTarget = new System.Windows.Forms.ComboBox();
            this.lblUserNameTarget = new System.Windows.Forms.Label();
            this.lblDatabaseTarget = new System.Windows.Forms.Label();
            this.txtUserNameTarget = new System.Windows.Forms.TextBox();
            this.chkSavePassTarget = new System.Windows.Forms.CheckBox();
            this.lblPasswordTarget = new System.Windows.Forms.Label();
            this.txtPasswordTarget = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.serverLabel = new System.Windows.Forms.Label();
            this.rBWindowsAuthSource = new System.Windows.Forms.RadioButton();
            this.rBSQLServerAuthSource = new System.Windows.Forms.RadioButton();
            this.lblUserNameSource = new System.Windows.Forms.Label();
            this.txtUserNameSource = new System.Windows.Forms.TextBox();
            this.lblPasswordSource = new System.Windows.Forms.Label();
            this.txtPasswordSource = new System.Windows.Forms.TextBox();
            this.lblDatabaseSource = new System.Windows.Forms.Label();
            this.pnlSource = new System.Windows.Forms.Panel();
            this.cmbServerNameSource = new System.Windows.Forms.ComboBox();
            this.btnDatabaseSource = new System.Windows.Forms.Button();
            this.cmbDatabaseSource = new System.Windows.Forms.ComboBox();
            this.chkSavePassSource = new System.Windows.Forms.CheckBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formControl)).BeginInit();
            this.pnlTarget.SuspendLayout();
            this.pnlSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(142, 507);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 29;
            this.label1.Text = "Record Name";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editRecordsToolStripMenuItem
            // 
            this.editRecordsToolStripMenuItem.Name = "editRecordsToolStripMenuItem";
            this.editRecordsToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.editRecordsToolStripMenuItem.Text = "Edit";
            this.editRecordsToolStripMenuItem.Click += new System.EventHandler(this.editRecordsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editRecordsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "File";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(736, 24);
            this.menuStrip1.TabIndex = 27;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // formControl
            // 
            this.formControl.BlinkRate = 0;
            this.formControl.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.formControl.ContainerControl = this;
            // 
            // bgWorkerTarget
            // 
            this.bgWorkerTarget.WorkerReportsProgress = true;
            this.bgWorkerTarget.WorkerSupportsCancellation = true;
            this.bgWorkerTarget.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerTarget_DoWork);
            this.bgWorkerTarget.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerTarget_RunWorkerCompleted);
            // 
            // bgWorkerSource
            // 
            this.bgWorkerSource.WorkerReportsProgress = true;
            this.bgWorkerSource.WorkerSupportsCancellation = true;
            this.bgWorkerSource.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerSource_DoWork);
            this.bgWorkerSource.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerSource_RunWorkerCompleted);
            // 
            // txtRecordName
            // 
            this.txtRecordName.Location = new System.Drawing.Point(230, 504);
            this.txtRecordName.Name = "txtRecordName";
            this.txtRecordName.Size = new System.Drawing.Size(100, 20);
            this.txtRecordName.TabIndex = 28;
            // 
            // btnCompare
            // 
            this.btnCompare.Enabled = false;
            this.btnCompare.Location = new System.Drawing.Point(400, 501);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(133, 23);
            this.btnCompare.TabIndex = 26;
            this.btnCompare.Text = "ADD";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // cmbServerNameTarget
            // 
            this.cmbServerNameTarget.FormattingEnabled = true;
            this.cmbServerNameTarget.Location = new System.Drawing.Point(18, 50);
            this.cmbServerNameTarget.Name = "cmbServerNameTarget";
            this.cmbServerNameTarget.Size = new System.Drawing.Size(260, 21);
            this.cmbServerNameTarget.Sorted = true;
            this.cmbServerNameTarget.TabIndex = 12;
            this.cmbServerNameTarget.SelectedIndexChanged += new System.EventHandler(this.cmbServerNameTarget_SelectedIndexChanged);
            this.cmbServerNameTarget.TextChanged += new System.EventHandler(this.cmbServerNameTarget_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(15, 24);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(54, 16);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Server";
            // 
            // pnlTarget
            // 
            this.pnlTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTarget.Controls.Add(this.cmbServerNameTarget);
            this.pnlTarget.Controls.Add(this.lblServer);
            this.pnlTarget.Controls.Add(this.btnDatabaseTarget);
            this.pnlTarget.Controls.Add(this.rBWindowsAuthTarget);
            this.pnlTarget.Controls.Add(this.rBSQLServerAuthTarget);
            this.pnlTarget.Controls.Add(this.cmbDatabaseTarget);
            this.pnlTarget.Controls.Add(this.lblUserNameTarget);
            this.pnlTarget.Controls.Add(this.lblDatabaseTarget);
            this.pnlTarget.Controls.Add(this.txtUserNameTarget);
            this.pnlTarget.Controls.Add(this.chkSavePassTarget);
            this.pnlTarget.Controls.Add(this.lblPasswordTarget);
            this.pnlTarget.Controls.Add(this.txtPasswordTarget);
            this.pnlTarget.Location = new System.Drawing.Point(400, 126);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(300, 350);
            this.pnlTarget.TabIndex = 25;
            // 
            // btnDatabaseTarget
            // 
            this.btnDatabaseTarget.Image = global::DBTest.Properties.Resources.bluedatabase;
            this.btnDatabaseTarget.Location = new System.Drawing.Point(242, 309);
            this.btnDatabaseTarget.Name = "btnDatabaseTarget";
            this.btnDatabaseTarget.Size = new System.Drawing.Size(36, 23);
            this.btnDatabaseTarget.TabIndex = 13;
            this.btnDatabaseTarget.UseVisualStyleBackColor = true;
            this.btnDatabaseTarget.Click += new System.EventHandler(this.btnDatabaseTarget_Click);
            // 
            // rBWindowsAuthTarget
            // 
            this.rBWindowsAuthTarget.AutoSize = true;
            this.rBWindowsAuthTarget.Location = new System.Drawing.Point(18, 88);
            this.rBWindowsAuthTarget.Name = "rBWindowsAuthTarget";
            this.rBWindowsAuthTarget.Size = new System.Drawing.Size(140, 17);
            this.rBWindowsAuthTarget.TabIndex = 2;
            this.rBWindowsAuthTarget.Text = "Windows Authentication";
            this.rBWindowsAuthTarget.UseVisualStyleBackColor = true;
            this.rBWindowsAuthTarget.CheckedChanged += new System.EventHandler(this.rBWindowsAuthTarget_CheckedChanged);
            // 
            // rBSQLServerAuthTarget
            // 
            this.rBSQLServerAuthTarget.AutoSize = true;
            this.rBSQLServerAuthTarget.Checked = true;
            this.rBSQLServerAuthTarget.Location = new System.Drawing.Point(18, 112);
            this.rBSQLServerAuthTarget.Name = "rBSQLServerAuthTarget";
            this.rBSQLServerAuthTarget.Size = new System.Drawing.Size(151, 17);
            this.rBSQLServerAuthTarget.TabIndex = 3;
            this.rBSQLServerAuthTarget.TabStop = true;
            this.rBSQLServerAuthTarget.Text = "SQL Server Authentication";
            this.rBSQLServerAuthTarget.UseVisualStyleBackColor = true;
            this.rBSQLServerAuthTarget.CheckedChanged += new System.EventHandler(this.rBSQLServerAuthTarget_CheckedChanged);
            // 
            // cmbDatabaseTarget
            // 
            this.cmbDatabaseTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseTarget.FormattingEnabled = true;
            this.cmbDatabaseTarget.Location = new System.Drawing.Point(18, 309);
            this.cmbDatabaseTarget.Name = "cmbDatabaseTarget";
            this.cmbDatabaseTarget.Size = new System.Drawing.Size(220, 21);
            this.cmbDatabaseTarget.Sorted = true;
            this.cmbDatabaseTarget.TabIndex = 11;
            this.cmbDatabaseTarget.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseTarget_SelectedIndexChanged);
            // 
            // lblUserNameTarget
            // 
            this.lblUserNameTarget.AutoSize = true;
            this.lblUserNameTarget.Location = new System.Drawing.Point(18, 171);
            this.lblUserNameTarget.Name = "lblUserNameTarget";
            this.lblUserNameTarget.Size = new System.Drawing.Size(58, 13);
            this.lblUserNameTarget.TabIndex = 4;
            this.lblUserNameTarget.Text = "User name";
            // 
            // lblDatabaseTarget
            // 
            this.lblDatabaseTarget.AutoSize = true;
            this.lblDatabaseTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseTarget.Location = new System.Drawing.Point(18, 288);
            this.lblDatabaseTarget.Name = "lblDatabaseTarget";
            this.lblDatabaseTarget.Size = new System.Drawing.Size(76, 16);
            this.lblDatabaseTarget.TabIndex = 10;
            this.lblDatabaseTarget.Text = "Database";
            // 
            // txtUserNameTarget
            // 
            this.txtUserNameTarget.Location = new System.Drawing.Point(81, 168);
            this.txtUserNameTarget.Name = "txtUserNameTarget";
            this.txtUserNameTarget.Size = new System.Drawing.Size(197, 20);
            this.txtUserNameTarget.TabIndex = 5;
            this.txtUserNameTarget.TextChanged += new System.EventHandler(this.txtUserNameTarget_TextChanged);
            // 
            // chkSavePassTarget
            // 
            this.chkSavePassTarget.AutoSize = true;
            this.chkSavePassTarget.Location = new System.Drawing.Point(84, 229);
            this.chkSavePassTarget.Name = "chkSavePassTarget";
            this.chkSavePassTarget.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassTarget.TabIndex = 8;
            this.chkSavePassTarget.Text = "Save password";
            this.chkSavePassTarget.UseVisualStyleBackColor = true;
            // 
            // lblPasswordTarget
            // 
            this.lblPasswordTarget.AutoSize = true;
            this.lblPasswordTarget.Location = new System.Drawing.Point(19, 204);
            this.lblPasswordTarget.Name = "lblPasswordTarget";
            this.lblPasswordTarget.Size = new System.Drawing.Size(53, 13);
            this.lblPasswordTarget.TabIndex = 6;
            this.lblPasswordTarget.Text = "Password";
            // 
            // txtPasswordTarget
            // 
            this.txtPasswordTarget.Location = new System.Drawing.Point(81, 200);
            this.txtPasswordTarget.Name = "txtPasswordTarget";
            this.txtPasswordTarget.Size = new System.Drawing.Size(197, 20);
            this.txtPasswordTarget.TabIndex = 7;
            this.txtPasswordTarget.UseSystemPasswordChar = true;
            this.txtPasswordTarget.TextChanged += new System.EventHandler(this.txtPasswordTarget_TextChanged);
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTarget.Location = new System.Drawing.Point(503, 70);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(80, 20);
            this.lblTarget.TabIndex = 24;
            this.lblTarget.Text = "TARGET";
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverLabel.Location = new System.Drawing.Point(15, 24);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(54, 16);
            this.serverLabel.TabIndex = 1;
            this.serverLabel.Text = "Server";
            // 
            // rBWindowsAuthSource
            // 
            this.rBWindowsAuthSource.AutoSize = true;
            this.rBWindowsAuthSource.Location = new System.Drawing.Point(18, 88);
            this.rBWindowsAuthSource.Name = "rBWindowsAuthSource";
            this.rBWindowsAuthSource.Size = new System.Drawing.Size(140, 17);
            this.rBWindowsAuthSource.TabIndex = 2;
            this.rBWindowsAuthSource.Text = "Windows Authentication";
            this.rBWindowsAuthSource.UseVisualStyleBackColor = true;
            this.rBWindowsAuthSource.CheckedChanged += new System.EventHandler(this.rBWindowsAuthSource_CheckedChanged);
            // 
            // rBSQLServerAuthSource
            // 
            this.rBSQLServerAuthSource.AutoSize = true;
            this.rBSQLServerAuthSource.Checked = true;
            this.rBSQLServerAuthSource.Location = new System.Drawing.Point(18, 112);
            this.rBSQLServerAuthSource.Name = "rBSQLServerAuthSource";
            this.rBSQLServerAuthSource.Size = new System.Drawing.Size(151, 17);
            this.rBSQLServerAuthSource.TabIndex = 3;
            this.rBSQLServerAuthSource.TabStop = true;
            this.rBSQLServerAuthSource.Text = "SQL Server Authentication";
            this.rBSQLServerAuthSource.UseVisualStyleBackColor = true;
            this.rBSQLServerAuthSource.CheckedChanged += new System.EventHandler(this.rBSQLServerAuthSource_CheckedChanged);
            // 
            // lblUserNameSource
            // 
            this.lblUserNameSource.AutoSize = true;
            this.lblUserNameSource.Location = new System.Drawing.Point(18, 171);
            this.lblUserNameSource.Name = "lblUserNameSource";
            this.lblUserNameSource.Size = new System.Drawing.Size(58, 13);
            this.lblUserNameSource.TabIndex = 4;
            this.lblUserNameSource.Text = "User name";
            // 
            // txtUserNameSource
            // 
            this.txtUserNameSource.Location = new System.Drawing.Point(81, 168);
            this.txtUserNameSource.Name = "txtUserNameSource";
            this.txtUserNameSource.Size = new System.Drawing.Size(197, 20);
            this.txtUserNameSource.TabIndex = 5;
            // 
            // lblPasswordSource
            // 
            this.lblPasswordSource.AutoSize = true;
            this.lblPasswordSource.Location = new System.Drawing.Point(19, 204);
            this.lblPasswordSource.Name = "lblPasswordSource";
            this.lblPasswordSource.Size = new System.Drawing.Size(53, 13);
            this.lblPasswordSource.TabIndex = 6;
            this.lblPasswordSource.Text = "Password";
            // 
            // txtPasswordSource
            // 
            this.txtPasswordSource.Location = new System.Drawing.Point(81, 200);
            this.txtPasswordSource.Name = "txtPasswordSource";
            this.txtPasswordSource.Size = new System.Drawing.Size(197, 20);
            this.txtPasswordSource.TabIndex = 7;
            this.txtPasswordSource.UseSystemPasswordChar = true;
            this.txtPasswordSource.TextChanged += new System.EventHandler(this.txtPasswordSource_TextChanged);
            // 
            // lblDatabaseSource
            // 
            this.lblDatabaseSource.AutoSize = true;
            this.lblDatabaseSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseSource.Location = new System.Drawing.Point(18, 288);
            this.lblDatabaseSource.Name = "lblDatabaseSource";
            this.lblDatabaseSource.Size = new System.Drawing.Size(76, 16);
            this.lblDatabaseSource.TabIndex = 10;
            this.lblDatabaseSource.Text = "Database";
            // 
            // pnlSource
            // 
            this.pnlSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSource.Controls.Add(this.cmbServerNameSource);
            this.pnlSource.Controls.Add(this.serverLabel);
            this.pnlSource.Controls.Add(this.btnDatabaseSource);
            this.pnlSource.Controls.Add(this.rBWindowsAuthSource);
            this.pnlSource.Controls.Add(this.rBSQLServerAuthSource);
            this.pnlSource.Controls.Add(this.cmbDatabaseSource);
            this.pnlSource.Controls.Add(this.lblUserNameSource);
            this.pnlSource.Controls.Add(this.lblDatabaseSource);
            this.pnlSource.Controls.Add(this.txtUserNameSource);
            this.pnlSource.Controls.Add(this.chkSavePassSource);
            this.pnlSource.Controls.Add(this.lblPasswordSource);
            this.pnlSource.Controls.Add(this.txtPasswordSource);
            this.pnlSource.Location = new System.Drawing.Point(30, 126);
            this.pnlSource.Name = "pnlSource";
            this.pnlSource.Size = new System.Drawing.Size(300, 350);
            this.pnlSource.TabIndex = 23;
            // 
            // cmbServerNameSource
            // 
            this.cmbServerNameSource.FormattingEnabled = true;
            this.cmbServerNameSource.Location = new System.Drawing.Point(18, 50);
            this.cmbServerNameSource.Name = "cmbServerNameSource";
            this.cmbServerNameSource.Size = new System.Drawing.Size(260, 21);
            this.cmbServerNameSource.Sorted = true;
            this.cmbServerNameSource.TabIndex = 12;
            this.cmbServerNameSource.SelectedIndexChanged += new System.EventHandler(this.cmbServerNameSource_SelectedIndexChanged);
            this.cmbServerNameSource.TextChanged += new System.EventHandler(this.cmbServerNameSource_TextChanged);
            // 
            // btnDatabaseSource
            // 
            this.btnDatabaseSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDatabaseSource.Image = global::DBTest.Properties.Resources.bluedatabase;
            this.btnDatabaseSource.Location = new System.Drawing.Point(242, 309);
            this.btnDatabaseSource.Name = "btnDatabaseSource";
            this.btnDatabaseSource.Size = new System.Drawing.Size(36, 23);
            this.btnDatabaseSource.TabIndex = 13;
            this.btnDatabaseSource.UseVisualStyleBackColor = true;
            this.btnDatabaseSource.Click += new System.EventHandler(this.btnDatabaseSource_Click);
            // 
            // cmbDatabaseSource
            // 
            this.cmbDatabaseSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseSource.FormattingEnabled = true;
            this.cmbDatabaseSource.Location = new System.Drawing.Point(18, 309);
            this.cmbDatabaseSource.Name = "cmbDatabaseSource";
            this.cmbDatabaseSource.Size = new System.Drawing.Size(220, 21);
            this.cmbDatabaseSource.Sorted = true;
            this.cmbDatabaseSource.TabIndex = 11;
            this.cmbDatabaseSource.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseSource_SelectedIndexChanged);
            // 
            // chkSavePassSource
            // 
            this.chkSavePassSource.AutoSize = true;
            this.chkSavePassSource.Location = new System.Drawing.Point(84, 229);
            this.chkSavePassSource.Name = "chkSavePassSource";
            this.chkSavePassSource.Size = new System.Drawing.Size(99, 17);
            this.chkSavePassSource.TabIndex = 8;
            this.chkSavePassSource.Text = "Save password";
            this.chkSavePassSource.UseVisualStyleBackColor = true;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.BackColor = System.Drawing.Color.Transparent;
            this.lblSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.ForeColor = System.Drawing.Color.Red;
            this.lblSource.Location = new System.Drawing.Point(129, 70);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(84, 20);
            this.lblSource.TabIndex = 22;
            this.lblSource.Text = "SOURCE";
            // 
            // ConnectingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 540);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtRecordName);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.pnlTarget);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.pnlSource);
            this.Controls.Add(this.lblSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ConnectingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Compare";
            this.Load += new System.EventHandler(this.ConnectingForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formControl)).EndInit();
            this.pnlTarget.ResumeLayout(false);
            this.pnlTarget.PerformLayout();
            this.pnlSource.ResumeLayout(false);
            this.pnlSource.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ErrorProvider formControl;
        private System.Windows.Forms.TextBox txtRecordName;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Panel pnlTarget;
        private System.Windows.Forms.ComboBox cmbServerNameTarget;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Button btnDatabaseTarget;
        private System.Windows.Forms.RadioButton rBWindowsAuthTarget;
        private System.Windows.Forms.RadioButton rBSQLServerAuthTarget;
        private System.Windows.Forms.ComboBox cmbDatabaseTarget;
        private System.Windows.Forms.Label lblUserNameTarget;
        private System.Windows.Forms.Label lblDatabaseTarget;
        private System.Windows.Forms.TextBox txtUserNameTarget;
        private System.Windows.Forms.CheckBox chkSavePassTarget;
        private System.Windows.Forms.Label lblPasswordTarget;
        private System.Windows.Forms.TextBox txtPasswordTarget;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Panel pnlSource;
        private System.Windows.Forms.ComboBox cmbServerNameSource;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Button btnDatabaseSource;
        private System.Windows.Forms.RadioButton rBWindowsAuthSource;
        private System.Windows.Forms.RadioButton rBSQLServerAuthSource;
        private System.Windows.Forms.ComboBox cmbDatabaseSource;
        private System.Windows.Forms.Label lblUserNameSource;
        private System.Windows.Forms.Label lblDatabaseSource;
        private System.Windows.Forms.TextBox txtUserNameSource;
        private System.Windows.Forms.CheckBox chkSavePassSource;
        private System.Windows.Forms.Label lblPasswordSource;
        private System.Windows.Forms.TextBox txtPasswordSource;
        private System.Windows.Forms.Label lblSource;
        private System.ComponentModel.BackgroundWorker bgWorkerTarget;
        private System.ComponentModel.BackgroundWorker bgWorkerSource;
    }
}

