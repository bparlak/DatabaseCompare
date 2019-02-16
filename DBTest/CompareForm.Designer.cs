namespace DBTest
{
    partial class CompareForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblDatabaseSource = new System.Windows.Forms.Label();
            this.lblDatabaseTarget = new System.Windows.Forms.Label();
            this.lblSourceResult = new System.Windows.Forms.Label();
            this.lblTargetResult = new System.Windows.Forms.Label();
            this.dgvSource = new System.Windows.Forms.DataGridView();
            this.dgvTarget = new System.Windows.Forms.DataGridView();
            this.tblLytPnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMiddleRight = new System.Windows.Forms.Panel();
            this.pnlMiddleLeft = new System.Windows.Forms.Panel();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.btnRepair = new System.Windows.Forms.Button();
            this.btnChkList = new System.Windows.Forms.Button();
            this.pnlTopleft = new System.Windows.Forms.Panel();
            this.tblLytPnlLeft = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddNewComparison = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.pnlcmbedit = new System.Windows.Forms.Panel();
            this.cmbQuickList = new System.Windows.Forms.ComboBox();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).BeginInit();
            this.tblLytPnlMain.SuspendLayout();
            this.pnlMiddleRight.SuspendLayout();
            this.pnlMiddleLeft.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            this.pnlTopleft.SuspendLayout();
            this.tblLytPnlLeft.SuspendLayout();
            this.pnlcmbedit.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1020, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.newToolStripMenuItem.Text = "New Connection";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.openToolStripMenuItem.Text = "Edit Comparison List";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // lblDatabaseSource
            // 
            this.lblDatabaseSource.AutoSize = true;
            this.lblDatabaseSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseSource.Location = new System.Drawing.Point(3, 6);
            this.lblDatabaseSource.Name = "lblDatabaseSource";
            this.lblDatabaseSource.Size = new System.Drawing.Size(11, 13);
            this.lblDatabaseSource.TabIndex = 11;
            this.lblDatabaseSource.Text = " ";
            this.lblDatabaseSource.Visible = false;
            // 
            // lblDatabaseTarget
            // 
            this.lblDatabaseTarget.AutoSize = true;
            this.lblDatabaseTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseTarget.Location = new System.Drawing.Point(3, 6);
            this.lblDatabaseTarget.Name = "lblDatabaseTarget";
            this.lblDatabaseTarget.Size = new System.Drawing.Size(11, 13);
            this.lblDatabaseTarget.TabIndex = 11;
            this.lblDatabaseTarget.Text = " ";
            this.lblDatabaseTarget.Visible = false;
            // 
            // lblSourceResult
            // 
            this.lblSourceResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSourceResult.AutoSize = true;
            this.lblSourceResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceResult.Location = new System.Drawing.Point(361, 6);
            this.lblSourceResult.Name = "lblSourceResult";
            this.lblSourceResult.Size = new System.Drawing.Size(110, 13);
            this.lblSourceResult.TabIndex = 11;
            this.lblSourceResult.Text = " number of result :";
            this.lblSourceResult.Visible = false;
            // 
            // lblTargetResult
            // 
            this.lblTargetResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTargetResult.AutoSize = true;
            this.lblTargetResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetResult.Location = new System.Drawing.Point(354, 6);
            this.lblTargetResult.Name = "lblTargetResult";
            this.lblTargetResult.Size = new System.Drawing.Size(106, 13);
            this.lblTargetResult.TabIndex = 11;
            this.lblTargetResult.Text = "number of result :";
            this.lblTargetResult.Visible = false;
            // 
            // dgvSource
            // 
            this.dgvSource.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSource.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSource.Location = new System.Drawing.Point(3, 82);
            this.dgvSource.Name = "dgvSource";
            this.dgvSource.ReadOnly = true;
            this.dgvSource.RowHeadersVisible = false;
            this.dgvSource.Size = new System.Drawing.Size(504, 538);
            this.dgvSource.TabIndex = 6;
            this.dgvSource.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSource_CellClick);
            this.dgvSource.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSource_CellDoubleClick);
            this.dgvSource.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvSource_ColumnAdded);
            // 
            // dgvTarget
            // 
            this.dgvTarget.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTarget.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTarget.Location = new System.Drawing.Point(513, 82);
            this.dgvTarget.Name = "dgvTarget";
            this.dgvTarget.ReadOnly = true;
            this.dgvTarget.RowHeadersVisible = false;
            this.dgvTarget.Size = new System.Drawing.Size(504, 538);
            this.dgvTarget.TabIndex = 6;
            this.dgvTarget.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTarget_CellClick);
            this.dgvTarget.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTarget_CellDoubleClick);
            this.dgvTarget.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvTarget_ColumnAdded);
            // 
            // tblLytPnlMain
            // 
            this.tblLytPnlMain.ColumnCount = 2;
            this.tblLytPnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLytPnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLytPnlMain.Controls.Add(this.dgvSource, 0, 2);
            this.tblLytPnlMain.Controls.Add(this.dgvTarget, 1, 2);
            this.tblLytPnlMain.Controls.Add(this.pnlMiddleRight, 1, 1);
            this.tblLytPnlMain.Controls.Add(this.pnlMiddleLeft, 0, 1);
            this.tblLytPnlMain.Controls.Add(this.pnlTopRight, 1, 0);
            this.tblLytPnlMain.Controls.Add(this.pnlTopleft, 0, 0);
            this.tblLytPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLytPnlMain.Location = new System.Drawing.Point(0, 24);
            this.tblLytPnlMain.Name = "tblLytPnlMain";
            this.tblLytPnlMain.RowCount = 3;
            this.tblLytPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.186195F));
            this.tblLytPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.815409F));
            this.tblLytPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.15891F));
            this.tblLytPnlMain.Size = new System.Drawing.Size(1020, 623);
            this.tblLytPnlMain.TabIndex = 12;
            // 
            // pnlMiddleRight
            // 
            this.pnlMiddleRight.Controls.Add(this.lblDatabaseTarget);
            this.pnlMiddleRight.Controls.Add(this.lblTargetResult);
            this.pnlMiddleRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddleRight.Location = new System.Drawing.Point(513, 53);
            this.pnlMiddleRight.Name = "pnlMiddleRight";
            this.pnlMiddleRight.Size = new System.Drawing.Size(504, 23);
            this.pnlMiddleRight.TabIndex = 7;
            // 
            // pnlMiddleLeft
            // 
            this.pnlMiddleLeft.Controls.Add(this.lblDatabaseSource);
            this.pnlMiddleLeft.Controls.Add(this.lblSourceResult);
            this.pnlMiddleLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddleLeft.Location = new System.Drawing.Point(3, 53);
            this.pnlMiddleLeft.Name = "pnlMiddleLeft";
            this.pnlMiddleLeft.Size = new System.Drawing.Size(504, 23);
            this.pnlMiddleLeft.TabIndex = 7;
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.btnRepair);
            this.pnlTopRight.Controls.Add(this.btnChkList);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopRight.Location = new System.Drawing.Point(513, 3);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(504, 44);
            this.pnlTopRight.TabIndex = 7;
            // 
            // btnRepair
            // 
            this.btnRepair.Image = global::DBTest.Properties.Resources.create32;
            this.btnRepair.Location = new System.Drawing.Point(87, 0);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.Size = new System.Drawing.Size(42, 41);
            this.btnRepair.TabIndex = 3;
            this.btnRepair.UseVisualStyleBackColor = true;
            this.btnRepair.Visible = false;
            this.btnRepair.Click += new System.EventHandler(this.btnRepair_Click);
            // 
            // btnChkList
            // 
            this.btnChkList.Location = new System.Drawing.Point(3, 11);
            this.btnChkList.Name = "btnChkList";
            this.btnChkList.Size = new System.Drawing.Size(75, 22);
            this.btnChkList.TabIndex = 1;
            this.btnChkList.Text = "Check List";
            this.btnChkList.UseVisualStyleBackColor = true;
            this.btnChkList.Visible = false;
            this.btnChkList.Click += new System.EventHandler(this.btnChkList_Click);
            // 
            // pnlTopleft
            // 
            this.pnlTopleft.Controls.Add(this.tblLytPnlLeft);
            this.pnlTopleft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopleft.Location = new System.Drawing.Point(3, 3);
            this.pnlTopleft.Name = "pnlTopleft";
            this.pnlTopleft.Size = new System.Drawing.Size(504, 44);
            this.pnlTopleft.TabIndex = 7;
            // 
            // tblLytPnlLeft
            // 
            this.tblLytPnlLeft.ColumnCount = 3;
            this.tblLytPnlLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.32478F));
            this.tblLytPnlLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.67521F));
            this.tblLytPnlLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tblLytPnlLeft.Controls.Add(this.btnAddNewComparison, 1, 1);
            this.tblLytPnlLeft.Controls.Add(this.btnCompare, 2, 1);
            this.tblLytPnlLeft.Controls.Add(this.pnlcmbedit, 0, 1);
            this.tblLytPnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLytPnlLeft.Location = new System.Drawing.Point(0, 0);
            this.tblLytPnlLeft.Name = "tblLytPnlLeft";
            this.tblLytPnlLeft.RowCount = 2;
            this.tblLytPnlLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLytPnlLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tblLytPnlLeft.Size = new System.Drawing.Size(504, 44);
            this.tblLytPnlLeft.TabIndex = 11;
            // 
            // btnAddNewComparison
            // 
            this.btnAddNewComparison.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewComparison.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewComparison.Image = global::DBTest.Properties.Resources._24plus;
            this.btnAddNewComparison.Location = new System.Drawing.Point(313, 11);
            this.btnAddNewComparison.Name = "btnAddNewComparison";
            this.btnAddNewComparison.Size = new System.Drawing.Size(32, 30);
            this.btnAddNewComparison.TabIndex = 9;
            this.btnAddNewComparison.UseVisualStyleBackColor = true;
            this.btnAddNewComparison.Click += new System.EventHandler(this.btnAddNewComparison_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(364, 11);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(137, 30);
            this.btnCompare.TabIndex = 10;
            this.btnCompare.Text = "COMPARE";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // pnlcmbedit
            // 
            this.pnlcmbedit.Controls.Add(this.cmbQuickList);
            this.pnlcmbedit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlcmbedit.Location = new System.Drawing.Point(3, 11);
            this.pnlcmbedit.Name = "pnlcmbedit";
            this.pnlcmbedit.Size = new System.Drawing.Size(295, 30);
            this.pnlcmbedit.TabIndex = 11;
            // 
            // cmbQuickList
            // 
            this.cmbQuickList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbQuickList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbQuickList.FormattingEnabled = true;
            this.cmbQuickList.Location = new System.Drawing.Point(0, 4);
            this.cmbQuickList.Name = "cmbQuickList";
            this.cmbQuickList.Size = new System.Drawing.Size(270, 21);
            this.cmbQuickList.Sorted = true;
            this.cmbQuickList.TabIndex = 9;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 647);
            this.Controls.Add(this.tblLytPnlMain);
            this.Controls.Add(this.menuStrip1);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "CompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompareForm";
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarget)).EndInit();
            this.tblLytPnlMain.ResumeLayout(false);
            this.pnlMiddleRight.ResumeLayout(false);
            this.pnlMiddleRight.PerformLayout();
            this.pnlMiddleLeft.ResumeLayout(false);
            this.pnlMiddleLeft.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopleft.ResumeLayout(false);
            this.tblLytPnlLeft.ResumeLayout(false);
            this.pnlcmbedit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Button btnAddNewComparison;
        private System.Windows.Forms.Label lblDatabaseSource;
        private System.Windows.Forms.Label lblDatabaseTarget;
        private System.Windows.Forms.Label lblSourceResult;
        private System.Windows.Forms.Label lblTargetResult;
        private System.Windows.Forms.DataGridView dgvSource;
        private System.Windows.Forms.DataGridView dgvTarget;
        private System.Windows.Forms.TableLayoutPanel tblLytPnlMain;
        private System.Windows.Forms.Panel pnlMiddleRight;
        private System.Windows.Forms.Panel pnlMiddleLeft;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.Panel pnlTopleft;
        private System.Windows.Forms.TableLayoutPanel tblLytPnlLeft;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Panel pnlcmbedit;
        private System.Windows.Forms.ComboBox cmbQuickList;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button btnRepair;
        private System.Windows.Forms.Button btnChkList;
    }
}