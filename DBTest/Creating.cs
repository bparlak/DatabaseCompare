using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBTest
{
    public partial class Creating : Form
    {
        CreateScript dbRepair = new CreateScript();
        private string sqlScript;
        private string dBInfo;
        private string name;
        public bool isClickOn = false;
        public Creating(string sql, string connectionSource, string name,string status)
        {
            sqlScript = sql;
            dBInfo = connectionSource;
            this.name = name;
            InitializeComponent();
            GetScript();
            this.Text = status;
            if (status == "Source")
            {
                lblNote.Text = "From: " + status + " ----->  To: Target";
            }else
            {
                lblNote.Text = "From: " + status + " ----->  To: Source";
            }
        }
        public void GetScript()
        {
            rtxtScript.Text = sqlScript;
        }

        private void btnRunScript_Click(object sender, EventArgs e)
        {
            dbRepair.ExecuteQuery(rtxtScript.Text, dBInfo);
            isClickOn = true;
            this.Close();
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtxtScript.Text);
            MessageBox.Show("Script copied.");
           // this.Close();
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void rtxtScript_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblNote_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
