using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.Reflection;

namespace DBTest
{
    public partial class CompareForm : Form
    {
        XmlDocument xmlDocStoreInfo = new XmlDocument();
        SqlConnection conSource = new SqlConnection();
        SqlConnection conTarget = new SqlConnection();
        DataTable dtSourceDifference = new DataTable();
        DataTable dtTargetDifference = new DataTable();
        DataTable dtBlackListTableSource = new DataTable();//ekranda veri tekrarını önlemek için. Mesela 1 tabloyu bulamazsa tüm kolonlarını view herşeyi getirecek buna engel olmak için tanımlandı.
        DataTable dtBlackListTableTarget = new DataTable();
        DataTable dtBlackListColumnSource = new DataTable();
        DataTable dtBlackListColumnTarget = new DataTable();
        CreateScript createScript = new CreateScript();
        DataGridViewCheckBoxColumn myCheckedColumn = new DataGridViewCheckBoxColumn() { Name = "Check", FalseValue = 0, TrueValue = 1, Visible = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 50 };//ana ekrana sonradan eklenen checkbox
        DataGridViewCheckBoxColumn myCheckedColumn1 = new DataGridViewCheckBoxColumn() { Name = "Check", FalseValue = 0, TrueValue = 1, Visible = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 50 };
        List<string> checkedList = new List<string>();
        List<string> viewColumnList = new List<string>();
        private string xmlDocCurrentPath = Application.StartupPath + "\\QuickComparison.xml";
        ConnectingForm cf = new ConnectingForm();
        private string sourceDBInfo;
        private string targetDBInfo;
        private string strTableName = "Table Name";
        private string strColumnName = "Column Name";
        private string strReason = "Reason";
        object sname, tname;
        bool checkcontrol = false;
        bool buttoncontrol = false;
        bool connectionON = true;
        public CompareForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//bgworker için
        }


        private void CompareForm_Load(object sender, EventArgs e)
        {
            GetQuickComparisonInfo();
        }
        public void GetQuickComparisonInfo()//xmlden kayıtlı karşılaştırmalar combobox içerisine çekiliyor
        {
            cmbQuickList.Items.Clear();
            xmlDocStoreInfo.Load(xmlDocCurrentPath);
            XmlNode xnodeSelected = xmlDocStoreInfo.SelectSingleNode("INFO");
            foreach (XmlNode item in xnodeSelected)
            {
                cmbQuickList.Items.Add(item.Attributes[0].Value);
            }
        }
        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cmbQuickList.Text))//listbox boş ise herhangi birşey yapmasın
            {
                try
                {
                    xmlDocStoreInfo.Load(xmlDocCurrentPath);
                    XmlNode xnodeSelected = xmlDocStoreInfo.SelectSingleNode("INFO");
                    foreach (XmlNode item in xnodeSelected)
                    {
                        if (cmbQuickList.Text.ToString() == item.Attributes[0].Value)
                        {
                            sourceDBInfo = item.ChildNodes[0].InnerText;//connection bilgileri değişkenlere aktarılıyor.
                            targetDBInfo = item.ChildNodes[1].InnerText;
                            break;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Didn't reach xml file to get database information");
                }
                bgWorker.RunWorkerAsync();
            }
        }


        #region StripMenu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)//new
        {
            ConnectingForm cf = new ConnectingForm();
            this.Enabled = false;
            cf.ShowDialog();
            this.Enabled = true;
            this.BringToFront();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)//exit
        {
            Close();
        }
        #endregion
        private void FirstConnection()//Serverlara bağlantı kontrolü
        {
            try//bağlantı kontrol
            {
                btnCompare.Text = "COMPARING";
                btnCompare.Enabled = false;
                btnAddNewComparison.Enabled = false;
                menuStrip1.Enabled = false;
                conSource.ConnectionString = sourceDBInfo;
                conTarget.ConnectionString = targetDBInfo;
                SqlCommand dbNameSource = new SqlCommand("SELECT DB_NAME() AS [Database]", conSource);
                SqlCommand dbNameTarget = new SqlCommand("SELECT DB_NAME() AS [Database]", conTarget);
                // Application.DoEvents();
                conSource.Open();
                //Application.DoEvents();
                sname = dbNameSource.ExecuteScalar();//connecting stringten password siliniyor?!!?!!?!
                conSource.Close();
                conTarget.Open();
                tname = dbNameTarget.ExecuteScalar();
                conTarget.Close();
                connectionON = true;
            }
            catch
            {
                connectionON = false;
                MessageBox.Show("Check your server informations.", "CONNECTION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCompare.Text = "COMPARE";
                btnCompare.Enabled = true;
                btnAddNewComparison.Enabled = true;
                menuStrip1.Enabled = true;
            }
        }

        private string GetFKCreate(string name, bool source)
        {
            string retval = string.Empty;
            #region SQL
            string sql = @"SELECT N'
                              ALTER TABLE '
                                 + QUOTENAME(cs.name) + '.' + QUOTENAME(ct.name)
                                 + ' WITH CHECK ADD CONSTRAINT ' + QUOTENAME(fk.name)
                                 + ' FOREIGN KEY (' + STUFF((SELECT ',' + QUOTENAME(c.name)
                                 -- get all the columns in the constraint table
                                  FROM sys.columns AS c
                                  INNER JOIN sys.foreign_key_columns AS fkc
                                  ON fkc.parent_column_id = c.column_id
                                  AND fkc.parent_object_id = c.[object_id]
                                  WHERE fkc.constraint_object_id = fk.[object_id]
                                  ORDER BY fkc.constraint_column_id
                                  FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)'), 1, 1, N'')
                                + ')
                              REFERENCES ' + QUOTENAME(rs.name) + '.' + QUOTENAME(rt.name)
                                + '(' + STUFF((SELECT ',' + QUOTENAME(c.name)
                                 -- get all the referenced columns
                                  FROM sys.columns AS c
                                  INNER JOIN sys.foreign_key_columns AS fkc
                                  ON fkc.referenced_column_id = c.column_id
                                  AND fkc.referenced_object_id = c.[object_id]
                                  WHERE fkc.constraint_object_id = fk.[object_id]
                                  ORDER BY fkc.constraint_column_id
                                  FOR XML PATH(N''), TYPE).value(N'.[1]', N'nvarchar(max)'), 1, 1, N'') + ')

                              GO

                              ALTER TABLE '+QUOTENAME(cs.name) + '.' + QUOTENAME(ct.name)+' CHECK CONSTRAINT ' + QUOTENAME(fk.name)

                              FROM sys.foreign_keys AS fk
                              INNER JOIN sys.tables AS rt -- referenced table
                                ON fk.referenced_object_id = rt.[object_id]
                              INNER JOIN sys.schemas AS rs
                                ON rt.[schema_id] = rs.[schema_id]
                              INNER JOIN sys.tables AS ct -- constraint table
                                ON fk.parent_object_id = ct.[object_id]
                              INNER JOIN sys.schemas AS cs
                                ON ct.[schema_id] = cs.[schema_id]
                              WHERE rt.is_ms_shipped = 0 AND ct.is_ms_shipped = 0
                              AND fk.name = N'" + name + "'";
            #endregion
            SqlCommand cmd = new SqlCommand(sql, (source ? conSource : conTarget));
            try
            {
                cmd.Connection.Open();
                object tmp = cmd.ExecuteScalar();
                if (tmp != null && tmp != DBNull.Value)
                {
                    retval = tmp.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return retval;
        }
        #region FONKSIYONLAR
        private void Functions()//database baglantısında sıkıntı cıkarsa donmayı engellemek için bgworker bitişinde calısıyor
        {
            if (connectionON)
            {
                CreateDataTables();
                CompareTables();
                CompareViews();
                CompareColumns();
                CompareColumnOrder();
                CompareDataType();
                CompareDataLength();
                CompareDataAllows();
                CompareCollation();
                CompareIndex();
                CompareConstraintsAndKey();
                CompareRoutines();

                DisplayResult();
                lblDatabaseSource.Visible = true;
                lblDatabaseSource.Text = sname.ToString();
                lblDatabaseTarget.Visible = true;
                lblDatabaseTarget.Text = tname.ToString();
                lblSourceResult.Visible = true;
                lblTargetResult.Visible = true;
                lblSourceResult.Text = "Number of Result: " + dtSourceDifference.Rows.Count.ToString();
                lblTargetResult.Text = "Number of Result: " + dtTargetDifference.Rows.Count.ToString();
                btnCompare.Text = "COMPARE";
                btnCompare.Enabled = true;
                btnAddNewComparison.Enabled = true;
                menuStrip1.Enabled = true;
                checkcontrol = false;
                buttoncontrol = true;

            }
        }
        #endregion
        #region Tablo Colon Tekrar Kontrolleri
        private bool ControlTable(DataTable bl, string tablename)
        {
            for (int i = 0; i < bl.Rows.Count; i++)
            {
                if (bl.Rows[i][strTableName].ToString() == tablename)
                {
                    return false;
                }
            }
            return true;
        }
        private bool ControlColumn(DataTable bl, string tablename, string columnname)
        {
            for (int i = 0; i < bl.Rows.Count; i++)
            {
                if (bl.Rows[i][strTableName].ToString() == tablename && bl.Rows[i][strColumnName].ToString() == columnname)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Create Data Tables
        private void CreateDataTables()
        {
            dtSourceDifference = new DataTable();
            dtSourceDifference.Columns.Add("Type", typeof(string));
            dtSourceDifference.Columns.Add(strTableName, typeof(string));
            dtSourceDifference.Columns.Add(strColumnName, typeof(string));
            dtSourceDifference.Columns.Add(strReason, typeof(string));
            dtSourceDifference.Columns.Add("Definition", typeof(string));
            dtSourceDifference.Columns["Definition"].ColumnMapping = MappingType.Hidden;//defination gizlemek için

            dtTargetDifference = new DataTable();
            dtTargetDifference.Columns.Add("Type", typeof(string));
            dtTargetDifference.Columns.Add(strTableName, typeof(string));
            dtTargetDifference.Columns.Add(strColumnName, typeof(string));
            dtTargetDifference.Columns.Add(strReason, typeof(string));
            dtTargetDifference.Columns.Add("Definition", typeof(string));
            dtTargetDifference.Columns["Definition"].ColumnMapping = MappingType.Hidden;
            dtBlackListTableSource = new DataTable();
            dtBlackListTableSource.Columns.Add("Type", typeof(string));
            dtBlackListTableSource.Columns.Add(strTableName, typeof(string));
            dtBlackListTableSource.Columns.Add(strColumnName, typeof(string));
            dtBlackListTableSource.Columns.Add(strReason, typeof(string));
            dtBlackListTableTarget = new DataTable();
            dtBlackListTableTarget.Columns.Add("Type", typeof(string));
            dtBlackListTableTarget.Columns.Add(strTableName, typeof(string));
            dtBlackListTableTarget.Columns.Add(strColumnName, typeof(string));
            dtBlackListTableTarget.Columns.Add(strReason, typeof(string));

            dtBlackListColumnSource = new DataTable();
            dtBlackListColumnSource.Columns.Add("Type", typeof(string));
            dtBlackListColumnSource.Columns.Add(strTableName, typeof(string));
            dtBlackListColumnSource.Columns.Add(strColumnName, typeof(string));
            dtBlackListColumnSource.Columns.Add(strReason, typeof(string));

            dtBlackListColumnTarget = new DataTable();
            dtBlackListColumnTarget.Columns.Add("Type", typeof(string));
            dtBlackListColumnTarget.Columns.Add(strTableName, typeof(string));
            dtBlackListColumnTarget.Columns.Add(strColumnName, typeof(string));
            dtBlackListColumnTarget.Columns.Add(strReason, typeof(string));
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.dgvSource, new object[] { true });//buffering-form ekranında boyut değişiminde kasmayı önlemek için
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, this.dgvTarget, new object[] { true });
            dgvSource.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgvSource.RowHeadersVisible = false;
            dgvTarget.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgvTarget.RowHeadersVisible = false;

        }

        #endregion
        #region Sonuçların ekrana yazılması ve gridview colon renklendirme
        private void DisplayResult()
        {

            dgvSource.Columns.Clear();
            dgvTarget.Columns.Clear();
            string type = "TABLE";//table ile basladığı için
            int i = 0;

            dgvSource.DataSource = dtSourceDifference;
            dgvTarget.DataSource = dtTargetDifference;
            //datagridview satırları boyama 

            foreach (DataGridViewRow dgrow in dgvSource.Rows)
            {

                if (Convert.ToString(dgrow.Cells["Type"].Value) != type)
                {
                    i++;
                }
                if (i % 2 != 0)
                {
                    dgrow.DefaultCellStyle.BackColor = Color.LightGray;
                }
                type = Convert.ToString(dgrow.Cells["Type"].Value);
            }
            i = 0;
            foreach (DataGridViewRow dgrow in dgvTarget.Rows)
            {

                if (Convert.ToString(dgrow.Cells["Type"].Value) != type)
                {
                    i++;
                }
                if (i % 2 != 0)
                {
                    dgrow.DefaultCellStyle.BackColor = Color.LightGray;
                }
                type = Convert.ToString(dgrow.Cells["Type"].Value);
            }

            dgvSource.Columns.Add(myCheckedColumn);//check kolonunun eklenmesi
            dgvTarget.Columns.Add(myCheckedColumn1);
            dgvSource.Columns["Check"].Visible = false;
            dgvTarget.Columns["Check"].Visible = false;
            if (dtSourceDifference.Rows.Count == 0 && dtTargetDifference.Rows.Count == 0)
            {
                MessageBox.Show("There is no differences between two databases.");
            }
        }
        #endregion
        #region Table
        private void CompareTables()
        {
            SqlCommand cmdTable = new SqlCommand("SELECT TABLE_NAME,TABLE_TYPE FROM [INFORMATION_SCHEMA].[TABLES] order by TABLE_TYPE");//Source farklılıklar
            SqlDataAdapter daTableSource = new SqlDataAdapter(cmdTable.CommandText, conSource);
            SqlDataAdapter daTableTarget = new SqlDataAdapter(cmdTable.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daTableSource.Fill(dts);
            daTableTarget.Fill(dtt);
            //BULMA
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                for (int j = 0; j < dtt.Rows.Count; j++)
                {
                    if (dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString())
                    {
                        dts.Rows.Remove(dts.Rows[i]);
                        i--;
                        dtt.Rows.Remove(dtt.Rows[j]);
                        break;
                    }

                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows[i]["TABLE_TYPE"].ToString() == "BASE TABLE")
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = "-----";
                    dr[strReason] = "Not Found";
                    dr["Type"] = "TABLE";
                    // dgvSource.Rows[intColorCount].DefaultCellStyle.BackColor = Color.Gray;
                    dtSourceDifference.Rows.Add(dr);
                }
                // olmayan tablo ve view isimlerini kara listeye al
                DataRow bl = dtBlackListTableSource.NewRow();
                bl[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                dtBlackListTableSource.Rows.Add(bl);
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (dtt.Rows[i]["TABLE_TYPE"].ToString() == "BASE TABLE")
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = "-----";
                    dr[strReason] = "Not Found";
                    dr["Type"] = "TABLE";
                    dtTargetDifference.Rows.Add(dr);
                }
                // olmayan tablo ve view isimlerini kara listeye al 
                DataRow bl = dtBlackListTableTarget.NewRow();
                bl[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                dtBlackListTableTarget.Rows.Add(bl);
            }
        }
        #endregion
        #region Views
        private void CompareViews()
        {
            List<string> viewList = new List<string>();
            int count = 0;
            SqlCommand cmdTable = new SqlCommand("select VIEW_NAME=v.name,VIEW_DEFINITION=m.definition from sys.views v inner join sys.sql_modules m on v.object_id=m.object_id");//Source farklılıklar
            SqlCommand cmdViewColumn = new SqlCommand();
            cmdViewColumn.CommandText = "select VIEW_NAME=v.name, VIEW_COLUMNS=c.name,VIEW_DEFINITION=m.definition from sys.views v inner join sys.columns c on v.object_id=c.object_id inner join sys.sql_modules m on v.object_id=m.object_id";
            SqlDataAdapter daTableSource = new SqlDataAdapter(cmdTable.CommandText, conSource);
            SqlDataAdapter daTableTarget = new SqlDataAdapter(cmdTable.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daTableSource.Fill(dts);
            daTableTarget.Fill(dtt);
            string strSourceDef, strTargetDef;
            //BULMA
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                for (int j = 0; j < dtt.Rows.Count; j++)
                {
                    if (dts.Rows[i]["VIEW_NAME"].ToString() == dtt.Rows[j]["VIEW_NAME"].ToString())
                    {
                        strSourceDef = dts.Rows[i]["VIEW_DEFINITION"].ToString();//içerik kontrolü için
                        strTargetDef = dtt.Rows[j]["VIEW_DEFINITION"].ToString();
                        if (string.Compare(strSourceDef, strTargetDef) != 0)// içeriği farklı olan viewnameleri alıyor (column kontrolü için)
                        {
                            break;
                        }
                        else
                        {
                            viewList.Add(dts.Rows[i]["VIEW_NAME"].ToString());//
                            count++;
                        }
                        dts.Rows.Remove(dts.Rows[i]);
                        i--;
                        dtt.Rows.Remove(dtt.Rows[j]);
                        break;
                    }

                }
            }
            /*
            SqlDataAdapter daViewSource = new SqlDataAdapter(cmdViewColumn.CommandText, conSource);
            SqlDataAdapter daViewTarget = new SqlDataAdapter(cmdViewColumn.CommandText, conTarget);
            DataTable dtsw = new DataTable();
            DataTable dttw = new DataTable();
            daViewSource.Fill(dtsw);
            daViewTarget.Fill(dttw);
            //içerik farklı ise columnları karşılaştıracak
            for (int i = 0; i < viewList.Count; i++)// listedeki her bir isim için sorgu yapıp colonlarını kontrol edecek
            {
                for (int s = 0; s < dtsw.Rows.Count; s++)//source view column
                {

                    for (int t = 0; t < dttw.Rows.Count; t++)//target view column
                    {
                        if (viewList[i] == dtsw.Rows[s]["VIEW_NAME"].ToString() && viewList[i] == dttw.Rows[t]["VIEWNAME"].ToString())
                        {
                            if (dtsw.Rows[s]["VIEW_COLUMNS"].ToString() == dttw.Rows[t]["VIEW_COLUMNS"].ToString())
                            {
                                dtsw.Rows.Remove(dtsw.Rows[s]);
                                s--;
                                dttw.Rows.Remove(dttw.Rows[t]);
                                break;
                            }
                        }
                    }
                }

                for (int s = 0; s < dtsw.Rows.Count; s++)
                {
                    DataRow drs = dtSourceDifference.NewRow();
                    drs[strTableName] = dtsw.Rows[s]["VIEW_NAME"].ToString();
                    drs[strColumnName] = dtsw.Rows[s]["VIEW_COLUMNS"].ToString();
                    drs[strReason] = "Not Found. ";
                    drs["Type"] = "VIEW COLUMN";
                    drs["Definition"] = dtsw.Rows[s]["VIEW_DEFINITION"].ToString();
                    dtSourceDifference.Rows.Add(drs);

                }

                for (int t = 0; t < dttw.Rows.Count; t++)
                {
                    DataRow drt = dtTargetDifference.NewRow();
                    drt[strTableName] = dttw.Rows[t]["VIEW_NAME"].ToString();
                    drt[strColumnName] = dttw.Rows[t]["VIEW_COLUMNS"].ToString();
                    drt[strReason] = "Not Found. ";
                    drt["Type"] = "VIEW COLUMN";
                    drt["Definition"] = dttw.Rows[t]["VIEW_DEFINITION"].ToString();
                    dtTargetDifference.Rows.Add(drt);
                }
            }
            */

            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                DataRow dr = dtSourceDifference.NewRow();
                dr[strTableName] = dts.Rows[i]["VIEW_NAME"].ToString();
                dr[strColumnName] = "-----";
                dr[strReason] = "Not Found. ";
                dr["Type"] = "VIEW";
                dr["Definition"] = dts.Rows[i]["VIEW_DEFINITION"].ToString();
                for (int j = 0; j < dtt.Rows.Count; j++)
                {
                    if (dts.Rows[i]["VIEW_NAME"].ToString() == dtt.Rows[j]["VIEW_NAME"].ToString())//aynı routine name varsa içerik farklıdır. varsa üzerine yazacak
                    {
                        dr[strReason] = "Content is different. ";
                        break;
                    }
                }
                dtSourceDifference.Rows.Add(dr);
                // olmayan tablo ve view isimlerini kara listeye al
                DataRow bl = dtBlackListTableSource.NewRow();
                bl[strTableName] = dts.Rows[i]["VIEW_NAME"].ToString();
                dtBlackListTableSource.Rows.Add(bl);
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                DataRow dr = dtTargetDifference.NewRow();
                dr[strTableName] = dtt.Rows[i]["VIEW_NAME"].ToString();
                dr[strColumnName] = "-----";
                dr[strReason] = "Not Found. ";
                dr["Type"] = "VIEW";
                dr["Definition"] = dtt.Rows[i]["VIEW_DEFINITION"].ToString();
                for (int j = 0; j < dts.Rows.Count; j++)
                {
                    if (dts.Rows[j]["VIEW_NAME"].ToString() == dtt.Rows[i]["VIEW_NAME"].ToString())//aynı routine name varsa içerik farklıdır. varsa üzerine yazacak
                    {
                        dr[strReason] = "Content is different. ";
                        break;
                    }
                }
                dtTargetDifference.Rows.Add(dr);
                // olmayan tablo ve view isimlerini kara listeye al
                DataRow bl = dtBlackListTableTarget.NewRow();
                bl[strTableName] = dtt.Rows[i]["VIEW_NAME"].ToString();
                dtBlackListTableTarget.Rows.Add(bl);
            }
        }
        #endregion
        #region Comapre Columns
        private void CompareColumns()
        {
            SqlCommand cmdColumn = new SqlCommand("SELECT TABLE_NAME,COLUMN_NAME FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdColumn.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdColumn.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //FARKI BUL
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            // SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Not Found";
                    dr["Type"] = "COLUMN NAME";
                    dtSourceDifference.Rows.Add(dr);
                    // olmayan tablo ve view isimlerini kara listeye al
                    DataRow bl = dtBlackListColumnSource.NewRow();
                    bl[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    bl[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dtBlackListColumnSource.Rows.Add(bl);
                }
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Not Found";
                    dr["Type"] = "COLUMN NAME";
                    dtTargetDifference.Rows.Add(dr);
                    // olmayan tablo ve view isimlerini kara listeye al
                    DataRow bl = dtBlackListColumnTarget.NewRow();
                    bl[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    bl[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dtBlackListColumnTarget.Rows.Add(bl);
                }
            }

        }
        #endregion
        #region Compare Column Order
        private void CompareColumnOrder()
        {
            SqlCommand cmdColumnOrder = new SqlCommand("SELECT TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdColumnOrder.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdColumnOrder.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //FARKI BUL
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["ORDINAL_POSITION"].ToString() == dtt.Rows[j]["ORDINAL_POSITION"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Position error at " + dts.Rows[i]["ORDINAL_POSITION"].ToString() + ".";
                    dr["Type"] = "COLUMN ORDER";
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Position error at " + dtt.Rows[i]["ORDINAL_POSITION"].ToString() + ".";
                    dr["Type"] = "COLUMN ORDER";
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Compare Data Type
        private void CompareDataType()
        {
            SqlCommand cmdData = new SqlCommand("SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdData.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdData.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["DATA_TYPE"].ToString() == dtt.Rows[j]["DATA_TYPE"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {

                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Type error. Current type " + dts.Rows[i]["DATA_TYPE"].ToString();
                    dr["Type"] = "DATA TYPE";
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Type error. Current type " + dtt.Rows[i]["DATA_TYPE"].ToString();
                    dr["Type"] = "DATA TYPE";
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Comare Data Length
        private void CompareDataLength()
        {
            SqlCommand cmdDataLenght = new SqlCommand("SELECT TABLE_NAME,COLUMN_NAME,CHARACTER_MAXIMUM_LENGTH FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdDataLenght.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdDataLenght.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString() == dtt.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Lenght error.Current lenght " + dts.Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    dr["Type"] = "DATA LENGTH";
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Lenght error.Current lenght " + dtt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    dr["Type"] = "DATA LENGTG";
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Compare Data Allows
        private void CompareDataAllows()
        {
            SqlCommand cmdDataAllows = new SqlCommand("SELECT TABLE_NAME,COLUMN_NAME,IS_NULLABLE FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdDataAllows.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdDataAllows.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["IS_NULLABLE"].ToString() == dtt.Rows[j]["IS_NULLABLE"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Nullable error. Current - " + dts.Rows[i]["IS_NULLABLE"].ToString();
                    dr["Type"] = "DATA NULLABLE";
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET TABLOYA DOLDUR
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Nullable error. Current - " + dtt.Rows[i]["IS_NULLABLE"].ToString();
                    dr["Type"] = "DATA NULLABLE";
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Compare Collation
        private void CompareCollation()
        {
            SqlCommand cmdCollation = new SqlCommand("SELECT TABLE_NAME,COLUMN_NAME,COLLATION_NAME FROM [INFORMATION_SCHEMA].[COLUMNS]");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdCollation.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdCollation.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["COLLATION_NAME"].ToString() == dtt.Rows[j]["COLLATION_NAME"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = dts.Rows[i]["COLLATION_NAME"].ToString() + " Not Found. ";
                    dr["Type"] = "COLLATION NAME";
                    dtSourceDifference.Rows.Add(dr);
                    // olmayan tablo ve view isimlerini kara listeye al
                }
            }

            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = dtt.Rows[i]["COLLATION_NAME"].ToString() + " Not Found. ";
                    dr["Type"] = "COLLATION NAME";
                    dtTargetDifference.Rows.Add(dr);
                    // olmayan tablo ve view isimlerini kara listeye al
                }
            }
        }
        #endregion
        #region Compare INDEXES
        private void CompareIndex()
        {
            SqlCommand cmdIndex = new SqlCommand("select TABLE_NAME=t.name,index_name=i.name, index_type=i.type_desc  from sys.tables t inner join sys.indexes i on t.object_id=i.object_id");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdIndex.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdIndex.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["index_name"].ToString() == dtt.Rows[j]["index_name"].ToString())
                        {
                            if (dts.Rows[i]["index_type"].ToString() != dtt.Rows[j]["index_type"].ToString()) break;
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && dts.Rows[i]["index_type"].ToString() != "HEAP")
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strReason] = "Not Found - " + dts.Rows[i]["index_name"].ToString() + " - " + dts.Rows[i]["index_type"].ToString();
                    dr[strColumnName] = dts.Rows[i]["index_name"].ToString();
                    dr["Type"] = "INDEX";
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["index_name"].ToString() == dtt.Rows[j]["index_name"].ToString())
                        {
                            dr[strReason] = "Different index." + dts.Rows[i]["index_name"].ToString() + " - " + dts.Rows[i]["index_type"].ToString();
                        }
                    }
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && dtt.Rows[i]["index_type"].ToString() != "HEAP")
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strReason] = "Not Found - " + dtt.Rows[i]["index_name"].ToString() + " - " + dtt.Rows[i]["index_type"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["index_name"].ToString();
                    dr["Type"] = "INDEX";
                    for (int j = 0; j < dts.Rows.Count; j++)
                    {
                        if (dtt.Rows[i]["index_name"].ToString() == dts.Rows[j]["index_name"].ToString())
                        {
                            dr[strReason] = "Different index. " + dtt.Rows[i]["index_name"].ToString() + " " + dtt.Rows[i]["index_type"].ToString(); ;
                        }
                    }
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Compare Constraints and Key
        private void CompareConstraintsAndKey()
        {
            SqlCommand cmdConstraints = new SqlCommand(@"SELECT TABLE_NAME = c.TABLE_NAME, COLUMN_NAME = k.COLUMN_NAME, CONSTRAINT_NAME = c.CONSTRAINT_NAME, CONSTRAINT_TYPE = c.CONSTRAINT_TYPE FROM [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS] c inner join [INFORMATION_SCHEMA].[CONSTRAINT_COLUMN_USAGE] k on c.CONSTRAINT_NAME = k.CONSTRAINT_NAME");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdConstraints.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdConstraints.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["COLUMN_NAME"].ToString() == dtt.Rows[j]["COLUMN_NAME"].ToString() && dts.Rows[i]["TABLE_NAME"].ToString() == dtt.Rows[j]["TABLE_NAME"].ToString() && dts.Rows[i]["CONSTRAINT_NAME"].ToString() == dtt.Rows[j]["CONSTRAINT_NAME"].ToString())
                        {
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableSource, dts.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnSource, dts.Rows[i]["TABLE_NAME"].ToString(), dts.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtSourceDifference.NewRow();
                    dr[strTableName] = dts.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dts.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Not Found " + dts.Rows[i]["CONSTRAINT_NAME"].ToString();
                    dr["Type"] = dts.Rows[i]["CONSTRAINT_TYPE"].ToString();
                    dtSourceDifference.Rows.Add(dr);
                }
            }
            //TARGET
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                if (ControlTable(dtBlackListTableTarget, dtt.Rows[i]["TABLE_NAME"].ToString()) && ControlColumn(dtBlackListColumnTarget, dtt.Rows[i]["TABLE_NAME"].ToString(), dtt.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    DataRow dr = dtTargetDifference.NewRow();
                    dr[strTableName] = dtt.Rows[i]["TABLE_NAME"].ToString();
                    dr[strColumnName] = dtt.Rows[i]["COLUMN_NAME"].ToString();
                    dr[strReason] = "Not Found " + dtt.Rows[i]["CONSTRAINT_NAME"].ToString();
                    dr["Type"] = dtt.Rows[i]["CONSTRAINT_TYPE"].ToString();
                    dtTargetDifference.Rows.Add(dr);
                }
            }
        }
        #endregion
        #region Compare Routines(Procedure-Functions)
        private void CompareRoutines()
        {
            SqlCommand cmdRoutines = new SqlCommand("select ROUTINE_NAME,ROUTINE_TYPE,ROUTINE_DEFINITION from [INFORMATION_SCHEMA].[ROUTINES] order by ROUTINE_DEFINITION");
            SqlDataAdapter daColumnSource = new SqlDataAdapter(cmdRoutines.CommandText, conSource);
            SqlDataAdapter daColumnTarget = new SqlDataAdapter(cmdRoutines.CommandText, conTarget);
            DataTable dts = new DataTable();
            DataTable dtt = new DataTable();
            daColumnSource.Fill(dts);
            daColumnTarget.Fill(dtt);
            string strSourceDef = "", strTargetDef = "";
            //farkları bul
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                if (dts.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dts.Rows[i]["ROUTINE_NAME"].ToString() == dtt.Rows[j]["ROUTINE_NAME"].ToString() && dts.Rows[i]["ROUTINE_TYPE"].ToString() == dtt.Rows[j]["ROUTINE_TYPE"].ToString())
                        {
                            strSourceDef = dts.Rows[i]["ROUTINE_DEFINITION"].ToString();
                            strTargetDef = dtt.Rows[j]["ROUTINE_DEFINITION"].ToString();
                            if (string.Compare(strSourceDef, strTargetDef) != 0) break;
                            dts.Rows.Remove(dts.Rows[i]);
                            i--;
                            dtt.Rows.Remove(dtt.Rows[j]);
                            break;
                        }
                    }
                }
            }
            //SOURCE TABLOYA DOLDUR
            for (int i = 0; i < dts.Rows.Count; i++)
            {
                DataRow dr = dtSourceDifference.NewRow();
                dr[strTableName] = dts.Rows[i]["ROUTINE_NAME"].ToString();
                dr[strReason] = "Not Found. ";
                dr["Type"] = dts.Rows[i]["ROUTINE_TYPE"].ToString();
                dr["Definition"] = dts.Rows[i]["ROUTINE_DEFINITION"].ToString();
                for (int j = 0; j < dtt.Rows.Count; j++)
                {
                    if (dts.Rows[i]["ROUTINE_NAME"].ToString() == dtt.Rows[j]["ROUTINE_NAME"].ToString())//aynı routine name varsa içerik farklıdır. varsa üzerine yazacak
                    {
                        dr[strReason] = "Content is different. ";
                        break;
                    }
                }
                dtSourceDifference.Rows.Add(dr);
            }
            //TARGET
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                DataRow dr = dtTargetDifference.NewRow();
                dr[strTableName] = dtt.Rows[i]["ROUTINE_NAME"].ToString();
                dr[strReason] = "Not Found. ";
                dr["Type"] = dtt.Rows[i]["ROUTINE_TYPE"].ToString();
                dr["Definition"] = dtt.Rows[i]["ROUTINE_DEFINITION"].ToString();
                for (int j = 0; j < dts.Rows.Count; j++)
                {
                    if (dtt.Rows[i]["ROUTINE_NAME"].ToString() == dts.Rows[j]["ROUTINE_NAME"].ToString())//aynı routine name varsa içerik farklıdır. varsa üzerine yazacak
                    {
                        dr[strReason] = "Content is different. ";
                        break;
                    }
                }
                dtTargetDifference.Rows.Add(dr);
            }
        }
        #endregion
        #region + butonu
        private void btnAddNewComparison_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            ConnectingForm cf = new ConnectingForm();
            cf.ShowDialog();
            this.Enabled = true;
            this.BringToFront();
            cmbQuickList.Items.Clear();
            GetQuickComparisonInfo();
        }
        #endregion
        #region Background Workers
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            FirstConnection();
        }
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Functions();

        }
        #endregion

        private void dgvSource_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void dgvTarget_ColumnAdded(object sender, DataGridViewColumnEventArgs e)// Ana ekranda sıralama engelleme. Sıralama yapıldığında renklendirme gidiyor diye kapadım.
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvSource.Columns.Clear();
            dgvTarget.Columns.Clear();
            lblDatabaseSource.Visible = false;
            lblDatabaseTarget.Visible = false;
            lblSourceResult.Visible = false;
            lblTargetResult.Visible = false;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditRecord ed = new EditRecord();
            this.Enabled = false;
            ed.ShowDialog();
            cmbQuickList.Items.Clear();
            GetQuickComparisonInfo();
            this.Enabled = true;
            this.BringToFront();
        }

        private void btnChkList_Click(object sender, EventArgs e)
        {
            //if (buttoncontrol)
            //{
            //    dgvSource.Columns["Check"].Visible = true;
            //    dgvTarget.Columns["Check"].Visible = true;
            //    checkcontrol = true;
            //    buttoncontrol = false;
            //}
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (checkcontrol)
            {
                checkedList.Clear();
                for (int i = 0; i < dgvSource.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dgvSource.Rows[i].Cells["Check"].Value) == 1)
                    {
                        //txtdene.Text = dgvSource.Rows[i].Cells["Table Name"].Value.ToString();
                        //listBox1.Items.Add(dgvSource.Rows[i].Cells["Table Name"].Value.ToString());
                        checkedList.Add(dgvSource.Rows[i].Cells["Table Name"].Value.ToString());
                    }
                }
                //txtdene.Text = dgvSource.CurrentRow.Cells[0].Value.ToString() + " " + dgvSource.CurrentRow.Cells[1].Value.ToString() + " " + dgvSource.CurrentRow.Cells[2].Value.ToString();
                dgvSource.Columns["Check"].Visible = false;
                dgvTarget.Columns["Check"].Visible = false;
                checkcontrol = false;
                buttoncontrol = true;
                dgvSource.DataSource = dtSourceDifference;
                dgvTarget.DataSource = dtTargetDifference;
                for (int i = 0; i < checkedList.Count; i++)
                {
                    MessageBox.Show(createScript.CreateTableScript(sourceDBInfo, checkedList[i]));
                }

            }
        }

        private void dgvSource_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Clipboard.SetText("Deneme");
            string status = "Source";
            int rowIndex = dgvSource.CurrentRow.Index;
            if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "TABLE")
            {
                string script = createScript.CreateTableScript(sourceDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString());
                Creating createTable = new Creating(script, targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);//status targetmı yoksa sourcemı onu gönderiyor form texti için-dbinfolar script çalıştırılınca karşılaştırmaların son hali için function çalıştıracak
                createTable.ShowDialog();
                if (createTable.isClickOn) { Functions(); }
            }
            else if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "VIEW")//procedure view function farkları topladığım table içerisinde definition columndan geliyor
            {
                Creating createView = new Creating(dtSourceDifference.Rows[rowIndex]["Definition"].ToString(), targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createView.ShowDialog();
                if (createView.isClickOn) { Functions(); }
            }
            else if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "PROCEDURE")
            {
                Creating createProcedure = new Creating(dtSourceDifference.Rows[rowIndex]["Definition"].ToString(), targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createProcedure.ShowDialog();
                if (createProcedure.isClickOn) { Functions(); }
            }
            else if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "FUNCTION")
            {
                Creating createFunction = new Creating(dtSourceDifference.Rows[rowIndex]["Definition"].ToString(), targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createFunction.ShowDialog();
                if (createFunction.isClickOn) { Functions(); }
            }
            else if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "COLUMN NAME")
            {
                string script = createScript.CreateColumnScript(sourceDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), dgvSource.Rows[rowIndex].Cells[strColumnName].Value.ToString());
                Creating createColumn = new Creating(script, targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createColumn.ShowDialog();
                if (createColumn.isClickOn) { Functions(); }
            }
            //index kısmında hata veriyor çözüm için araştırma yapılması gerekiyor.
          /*  else if (dgvSource.Rows[rowIndex].Cells["Type"].Value.ToString() == "INDEX")
            {
                string script = createScript.CreateIndexScript(sourceDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), dgvSource.Rows[rowIndex].Cells[strColumnName].Value.ToString());
                Creating createIndex = new Creating(script, targetDBInfo, dgvSource.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createIndex.ShowDialog();
                if (createIndex.isClickOn) { Functions(); }
            }*/




        }
        private void dgvTarget_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Clipboard.SetText("Deneme");
            string status = "Target";
            int rowIndex = dgvTarget.CurrentRow.Index;
            if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "TABLE")
            {
                string script = createScript.CreateTableScript(targetDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString());
                Creating createTable = new Creating(script, sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createTable.ShowDialog();
                if (createTable.isClickOn) { Functions(); }
            }
            else if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "VIEW")//procedure view function farkları topladığım table içerisinde definition columndan geliyor
            {
                Creating createView = new Creating(dtTargetDifference.Rows[rowIndex]["Definition"].ToString(), sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createView.ShowDialog();
                if (createView.isClickOn) { Functions(); }

            }
            else if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "PROCEDURE")
            {
                Creating createProcedure = new Creating(dtTargetDifference.Rows[rowIndex]["Definition"].ToString(), sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createProcedure.ShowDialog();
                if (createProcedure.isClickOn) { Functions(); }
            }
            else if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "FUNCTION")
            {
                Creating createFunction = new Creating(dtTargetDifference.Rows[rowIndex]["Definition"].ToString(), sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createFunction.ShowDialog();
                if (createFunction.isClickOn) { Functions(); }
            }
            else if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "COLUMN NAME")
            {
                string script = createScript.CreateColumnScript(targetDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), dgvTarget.Rows[rowIndex].Cells[strColumnName].Value.ToString());
                Creating createColumn = new Creating(script, sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createColumn.ShowDialog();
                if (createColumn.isClickOn) { Functions(); }
            }
          /*  else if (dgvTarget.Rows[rowIndex].Cells["Type"].Value.ToString() == "INDEX")
            {
                string script = createScript.CreateIndexScript(targetDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), dgvTarget.Rows[rowIndex].Cells[strColumnName].Value.ToString());
                Creating createIndex = new Creating(script, sourceDBInfo, dgvTarget.Rows[rowIndex].Cells[strTableName].Value.ToString(), status);
                createIndex.ShowDialog();
                if (createIndex.isClickOn) { Functions(); }
            }*/

        }

        #region Bulunamayan tablo kolon ve diğerlerinin yeniden oluşturulması için çalıştığım kodlar
        private void dgvSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (checkcontrol)
            {
                int rowIndex;
                int columnIndex = dgvSource.CurrentCell.ColumnIndex;
                rowIndex = dgvSource.CurrentRow.Index;
                if (Convert.ToInt32(dgvSource.Rows[rowIndex].Cells["Check"].Value) == 0 && columnIndex == 4)
                {
                    dgvSource.Rows[rowIndex].Cells["Check"].Value = 1;
                }
                else if (Convert.ToInt32(dgvSource.Rows[rowIndex].Cells["Check"].Value) == 1 && columnIndex == 4)
                {
                    dgvSource.Rows[rowIndex].Cells["Check"].Value = 0;
                }
            }
        }
        //target
        private void dgvTarget_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (checkcontrol)
            {
                int rowIndex;
                int columnIndex = dgvTarget.CurrentCell.ColumnIndex;
                rowIndex = dgvTarget.CurrentRow.Index;
                if (Convert.ToInt32(dgvTarget.Rows[rowIndex].Cells["Check"].Value) == 0 && columnIndex == 4)
                {
                    dgvTarget.Rows[rowIndex].Cells["Check"].Value = 1;
                }
                else if (Convert.ToInt32(dgvTarget.Rows[rowIndex].Cells["Check"].Value) == 1 && columnIndex == 4)
                {
                    dgvTarget.Rows[rowIndex].Cells["Check"].Value = 0;
                }
            }
        }
        #endregion

        private void cmbQuickList_SelectedIndexChanged(object sender, EventArgs e)
        {
            xmlDocStoreInfo.Load(xmlDocCurrentPath);//bağlanılacak server ve database bilgilerini labelde göstermek için yapılacak
        }
    }
}
