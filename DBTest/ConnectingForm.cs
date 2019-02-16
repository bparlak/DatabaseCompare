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
using System.Xml;
using System.IO;

namespace DBTest
{
    public partial class ConnectingForm : Form
    {
        //private bool sourceConnection = true;//ikinci veritabanına bağlantıya geçerken kullanılacak. false ise geçmeyecek
        SqlConnection sourceCon = new SqlConnection();
        SqlConnection targetCon = new SqlConnection();
        SqlCommand sourceCmd = new SqlCommand();
        SqlCommand targetCmd = new SqlCommand();
        XmlDocument xmlDocStoreInfo = new XmlDocument();
        private string xmlDocPath = Application.StartupPath + "\\information.xml";
        private string xmlDocCurrentPath = Application.StartupPath + "\\QuickComparison.xml";
        private bool sourceBtnflag = true;
        private bool targetBtnflag = true;
        private string sourceDBInfo = "";
        private string targetDBInfo = "";


        public ConnectingForm()
        {
            InitializeComponent();
            CallServerNames();
            CheckForIllegalCrossThreadCalls = false;//db bağlantısı sırasında donmayı engellemek için
        }
        

        //--------------------------------------------------------Başlangıçta Servername'i combobox içerisine okuyor.--------------------------------------------------------
        private void CallServerNames()
        {
            if (System.IO.File.Exists(xmlDocPath))
            {
                xmlDocStoreInfo.Load(xmlDocPath);
                XmlNode getServerName = xmlDocStoreInfo.SelectSingleNode("INFO");
                if (getServerName != null)
                {
                    foreach (XmlNode item in getServerName)
                    {
                        cmbServerNameSource.Items.Add(item.ChildNodes[0].InnerText);//combobox item changed action içerisinde verileri forma cekiyor.
                        cmbServerNameTarget.Items.Add(item.ChildNodes[0].InnerText);
                    }
                }
            }
            else
            {
                /* dosya oluşamazsa*/

            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            bool isok = true;
            //--------kayıtlar aynıysa sadece 1 donguyle farklılık varmı kontrol ediyor. Ilk defa eklenmişse bilgileri kaydediyor.
            if (string.IsNullOrEmpty(txtRecordName.Text))
            {
                formControl.SetError(txtRecordName, "Record Name can not be empty");
                isok = false;
            }
            if (isok)
            {
                formControl.Clear();
                RecordInformation(cmbServerNameSource.Text, rBSQLServerAuthSource, cmbDatabaseSource.Text, txtUserNameSource.Text, txtPasswordSource.Text, chkSavePassSource);//source server bilgiler kayıt
                RecordInformation(cmbServerNameTarget.Text, rBSQLServerAuthTarget, cmbDatabaseTarget.Text, txtUserNameTarget.Text, txtPasswordTarget.Text, chkSavePassTarget);//target server bilgiler kayıt
                recordCurrentDatabase();
                this.Close();
            }


        }

        private void recordCurrentDatabase()//Hızlı karşılaştırma 
        {
            xmlDocStoreInfo.Load(xmlDocCurrentPath);
            XmlNode xnodeSelected = xmlDocStoreInfo.SelectSingleNode("INFO");

            XmlNode xDatabases = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "COMPARISON", null);
            xnodeSelected.AppendChild(xDatabases);

            XmlAttribute xattname = xmlDocStoreInfo.CreateAttribute("NAME");
            xattname.Value = txtRecordName.Text;
            xDatabases.Attributes.Append(xattname);

            XmlNode xsourceDatabase = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "sourcedatabase", null);
            xsourceDatabase.InnerText = sourceDBInfo;
            xDatabases.AppendChild(xsourceDatabase);

            XmlNode xtargetDatabase = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "targetdatabase", null);
            xtargetDatabase.InnerText = targetDBInfo;
            xDatabases.AppendChild(xtargetDatabase);

            xmlDocStoreInfo.Save(xmlDocCurrentPath);
        }


        #region RECORDING INFORMATION TO XML FILE
        //--------------------------------------------------------BİLGİLERİN XML DOSYASINA KAYDI--------------------------------------------------------
        public void RecordInformation(string cmbServerName, RadioButton rbAuthentication, string cmbDatabase, string txtUserName, string txtPassword, CheckBox chkSavePass)
        {
            string rbcheck;
            bool controlFlag = true;//--------------------------------------------------------kontrol flag: aynı kayıt varsa aynı kaydın tekrar kayıt edilmesini engelliyor.
            if (rbAuthentication.Checked == true)
            {
                rbcheck = "sql"; // fonksiyona gelen radiobuton true ise sql false ise windows.
            }
            else
            {
                rbcheck = "windows";
            }

            xmlDocStoreInfo.Load(xmlDocPath);
            XmlNode checkServerName = xmlDocStoreInfo.SelectSingleNode("INFO");

            foreach (XmlNode item in checkServerName)
            {
                if (cmbServerName == item.ChildNodes[0].InnerText)//------------------------------------aynı kayıt var!!
                {
                    controlFlag = false;
                    //-----------------
                    if (chkSavePass.Checked.ToString().ToLower() != item.ChildNodes[2].Attributes[0].Value)//save password özelliğinde değişiklik varsa dosyaya kaydedecek-------------True-true sorunu (büyük kücük harf sourunu)
                    {
                        item.ChildNodes[2].Attributes[0].Value = chkSavePass.Checked.ToString().ToLower();//xml içerisinde savepasword false yapılıyor.
                        item.ChildNodes[2].InnerText = txtPassword;//check özelliğini şifresini değiştirdiği için kaldırıyorsa şifreyi yaz
                    }
                    //----------------
                    if (rbcheck != item.ChildNodes[0].Attributes[0].Value) // radio butonda değişiklik olursa username ve passwordun yeniden ayarlanması. eğer sql iken windows seçerse silecek. windows iken sql seçerse ekleme yapacak
                    {
                        if (rbcheck == "windows")//radiobuton windows olarak değiştiyse
                        {
                            item.ChildNodes[0].Attributes[0].Value = rbcheck;
                            item.ChildNodes[1].InnerText = "";
                            item.ChildNodes[2].InnerText = "";
                        }

                        if (rbcheck == "sql")//radiobuton sql olarak değiştiyse
                        {
                            item.ChildNodes[0].Attributes[0].Value = rbcheck;
                            item.ChildNodes[1].InnerText = txtUserName;
                            item.ChildNodes[2].InnerText = txtPassword;
                        }
                    }

                    xmlDocStoreInfo.Save(xmlDocPath);
                    break;
                }
            }
            if (controlFlag)//ilk kayıt
            {
                XmlElement recordElement = xmlDocStoreInfo.CreateElement("SERVERS");

                XmlNode xserverName = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "servername", null);
                xserverName.InnerText = cmbServerName;
                recordElement.AppendChild(xserverName);

                XmlAttribute xauthentication = xmlDocStoreInfo.CreateAttribute("authentication");
                if (rbAuthentication.Checked == true)
                {
                    xauthentication.Value = "sql";
                }
                else
                {
                    xauthentication.Value = "windows";
                }
                xserverName.Attributes.Append(xauthentication);
                if (rbAuthentication.Checked == true)
                {
                    XmlNode xuserName = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "username", null);
                    xuserName.InnerText = txtUserName;
                    recordElement.AppendChild(xuserName);
                    XmlNode xpassword = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "password", null);
                    xpassword.InnerText = txtPassword;
                    recordElement.AppendChild(xpassword);
                    XmlAttribute attSavePass = xmlDocStoreInfo.CreateAttribute("savepassword");
                    attSavePass.Value = chkSavePass.Checked.ToString().ToLower();
                    xpassword.Attributes.Append(attSavePass);
                }
                else//else durumunda windows authentication seçilmiştir xmlde username ve password boş girecek
                {
                    XmlNode xuserName = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "username", null);
                    xuserName.InnerText = "";
                    recordElement.AppendChild(xuserName);
                    XmlNode xpassword = xmlDocStoreInfo.CreateNode(XmlNodeType.Element, "password", null);
                    xpassword.InnerText = "";
                    recordElement.AppendChild(xpassword);
                    XmlAttribute attSavePass = xmlDocStoreInfo.CreateAttribute("savepassword");
                    attSavePass.Value = "false";
                    xpassword.Attributes.Append(attSavePass);
                }


                xmlDocStoreInfo.DocumentElement.AppendChild(recordElement);
                xmlDocStoreInfo.Save(xmlDocPath);
                MessageBox.Show("New server added.");
            }
        }
        #endregion
        #region SOURCE AND TARGET DATABASEBUTTON
        //---------------------------------------------------------SOURCEDATABASE BTN---------------------------------------------------------
        private void btnDatabaseSource_Click(object sender, EventArgs e)
        {
            if (sourceBtnflag)//database bağlantı sırasında butona yeniden basılırsa hata vermesini önlemek için. flag=false ise buton pasif olacak
            {


                if (rBWindowsAuthSource.Checked == true)
                {
                    sourceDBInfo = "Data source=" + cmbServerNameSource.Text + "; Integrated Security=SSPI;";
                }
                else if (rBSQLServerAuthSource.Checked == true)
                {
                    sourceDBInfo = "Data source=" + cmbServerNameSource.Text + "; User Id=" + txtUserNameSource.Text + "; Password=" + txtPasswordSource.Text + ";";
                }

                //databse baglantı
                sourceCon.ConnectionString = sourceDBInfo;
                sourceCmd.Connection = sourceCon;
                sourceCmd.CommandText = "select name from sys.databases";

                //form kotroller
                if (rBSQLServerAuthSource.Checked == true)
                {
                    //eğer server radiobutton seçilirse
                    if (string.IsNullOrEmpty(cmbServerNameSource.Text)) { formControl.SetError(cmbServerNameSource, "Server Name can not be empty"); }
                    if (string.IsNullOrEmpty(txtUserNameSource.Text)) { formControl.SetError(txtUserNameSource, "Username cannot be empty"); }
                    if (string.IsNullOrEmpty(txtPasswordSource.Text)) { formControl.SetError(txtPasswordSource, "Password cannot be empty"); }
                    if (cmbServerNameSource.Text != "" && txtUserNameSource.Text != "" && txtPasswordSource.Text != "")//3üde boş degilse
                        bgWorkerSource.RunWorkerAsync();//donma durumu için
                }
                //eğer windows radiobuton seçilirse 
                if (rBWindowsAuthSource.Checked == true)
                {
                    if (cmbServerNameSource.Text == "")
                    {
                        formControl.SetError(cmbServerNameSource, "Server Name can not be empty");
                    }
                    else bgWorkerSource.RunWorkerAsync(); //Database bağlantı sırasında donmayı önlemek için kullandım
                }

            }


        }
        //---------------------------------------------------------TARGETDATABSE BTN---------------------------------------------------------
        private void btnDatabaseTarget_Click(object sender, EventArgs e)
        {
            if (targetBtnflag)//database bağlantı sırasında butona yeniden basılırsa hata vermesini önlemek için. flag=false ise buton pasif olacak
            {

                if (rBWindowsAuthTarget.Checked == true)
                {
                    targetDBInfo = "Data source=" + cmbServerNameTarget.Text + ";Integrated Security=SSPI;";
                }
                else if (rBSQLServerAuthTarget.Checked == true)
                {
                    targetDBInfo = "Data source=" + cmbServerNameTarget.Text + ";User Id=" + txtUserNameTarget.Text + ";Password=" + txtPasswordTarget.Text + ";";
                }
                //databse baglantı
                targetCon.ConnectionString = targetDBInfo;
                targetCmd.Connection = targetCon;
                targetCmd.CommandText = "select name from sys.databases";

                if (rBSQLServerAuthTarget.Checked == true)
                {
                    //eğer server radiobutton seçilirse
                    if (string.IsNullOrEmpty(cmbServerNameTarget.Text)) { formControl.SetError(cmbServerNameTarget, "Server Name can not be empty"); }
                    if (string.IsNullOrEmpty(txtUserNameTarget.Text)) { formControl.SetError(txtUserNameTarget, "Username cannot be empty"); }
                    if (string.IsNullOrEmpty(txtPasswordTarget.Text)) { formControl.SetError(txtPasswordTarget, "Password cannot be empty"); }
                    if (cmbServerNameTarget.Text != "" && txtUserNameTarget.Text != "" && txtPasswordTarget.Text != "")//3üde boş degilse
                        bgWorkerTarget.RunWorkerAsync();//donma durumu için
                }
                //eğer windows radiobuton seçilirse 
                if (rBWindowsAuthTarget.Checked == true)
                {
                    if (cmbServerNameTarget.Text == "")
                    {
                        formControl.SetError(cmbServerNameTarget, "Server Name can not be empty");
                    }
                    else bgWorkerTarget.RunWorkerAsync(); //Database bağlantı sırasında donmayı önlemek için kullandım
                }


            }
        }
        #endregion
        #region DATABASE CONNECTIONS
        //---------------------------------------------------------SOURCE DATABASE CONNECTION FUNCTION---------------------------------------------------------
        private void ConnectionDatabaseSource()//Source buttonda donma durumuna önlem için fonksiyon taşıdım.
        {
            try
            {

                btnDatabaseSource.Image = Properties.Resources.reloadgif;//sourceCon.open() da takılırsa reloadgif ekranda görülecek takıldıgı sure boyunca
                sourceCon.Open();
                SqlDataReader dr = sourceCmd.ExecuteReader();
                cmbDatabaseSource.Items.Clear();
                while (dr.Read())//read methodu veri getirdikçe true döndüğü için ve while da true yu döndüğü için bu sekilde kullanılıyor.
                    cmbDatabaseSource.Items.Add(dr["NAME"]);

                dr.Close();
                sourceCon.Close();
                btnDatabaseSource.Image = Properties.Resources.greendatabase;//-------------kesinlikle bağlantı sağlandığından emin olduğum yer-------------
                btnDatabaseTarget.Enabled = true;
            }
            catch
            {
                btnDatabaseSource.Image = Properties.Resources.bluedatabase;//hata bulunduğu için mavi renk simge.
                cmbDatabaseSource.Items.Clear();//
                MessageBox.Show("Check your information or network connection.");

            }

        }

        //---------------------------------------------------------TARGET DATABASE CONNECTION FUNCTION---------------------------------------------------------
        private void ConnectionDatabaseTarget()
        {
            try
            {

                btnDatabaseTarget.Image = Properties.Resources.reloadgif;//sourceCon.open() da takılırsa reloadgif ekranda görülecek takıldıgı sure boyunca
                targetCon.Open();
                SqlDataReader dr = targetCmd.ExecuteReader();
                cmbDatabaseTarget.Items.Clear();
                while (dr.Read())//read methodu veri getirdikçe true döndüğü için ve while da true yu döndüğü için bu sekilde kullanılıyor.
                    cmbDatabaseTarget.Items.Add(dr["NAME"]);

                dr.Close();
                targetCon.Close();
                btnDatabaseTarget.Image = Properties.Resources.greendatabase;//-------------kesinlikle bağlantı sağlandığından emin olduğum yer-------------

            }
            catch
            {
                btnDatabaseTarget.Image = Properties.Resources.bluedatabase;//hata bulunduğu için mavi renk simge.
                cmbDatabaseTarget.Items.Clear();//
                MessageBox.Show("Check your information or network.");

            }

        }
        #endregion

        #region BACKROUNDWORKERS
        //------------------------------------------------------------------------BACKGROUNDWORKER-------------------------------------------------------------
        //-------------------Source-------------------
        private void bgWorkerSource_DoWork(object sender, DoWorkEventArgs e)
        {
            sourceBtnflag = false;
            targetBtnflag = false;
            rBSQLServerAuthSource.Enabled = false;
            rBWindowsAuthSource.Enabled = false;
            cmbServerNameSource.Enabled = false;
            txtUserNameSource.Enabled = false;
            txtPasswordSource.Enabled = false;
            ConnectionDatabaseSource();
        }
        private void bgWorkerSource_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sourceBtnflag = true;
            targetBtnflag = true;
            rBSQLServerAuthSource.Enabled = true;
            rBWindowsAuthSource.Enabled = true;
            cmbServerNameSource.Enabled = true;
            txtUserNameSource.Enabled = true;
            txtPasswordSource.Enabled = true;
        }
        //-------------------Target-------------------
        private void bgWorkerTarget_DoWork(object sender, DoWorkEventArgs e)
        {
            targetBtnflag = false;
            sourceBtnflag = false;
            rBSQLServerAuthTarget.Enabled = false;
            rBWindowsAuthTarget.Enabled = false;
            cmbServerNameTarget.Enabled = false;
            txtUserNameTarget.Enabled = false;
            txtPasswordTarget.Enabled = false;
            ConnectionDatabaseTarget();
        }
        private void bgWorkerTarget_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            targetBtnflag = true;
            sourceBtnflag = true;
            rBSQLServerAuthTarget.Enabled = true;
            rBWindowsAuthTarget.Enabled = true;
            cmbServerNameTarget.Enabled = true;
            txtUserNameTarget.Enabled = true;
            txtPasswordTarget.Enabled = true;


        }
        //BACKGROUNDWORKER END-----------------------------------------------------------------------------------------------------------------
        #endregion
        #region SOURCE COMPONENTS
        //---------------------------------------------------------SOURCE COMPONENTLER---------------------------------------------------------
        private void cmbServerNameSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
            formControl.Clear();
            //--------------------------------------------------------Servername seçildiğinde username ve password bilgilerini getir.--------------------------------------------------------
            xmlDocStoreInfo.Load(xmlDocPath);
            XmlNode selectedItem = xmlDocStoreInfo.SelectSingleNode("INFO");

            foreach (XmlNode item in selectedItem)
            {
                if (item.ChildNodes[0].InnerText == cmbServerNameSource.Text)
                {
                    txtUserNameSource.Text = item.ChildNodes[1].InnerText;
                    if (item.ChildNodes[2].Attributes[0].Value == "true")
                    {
                        txtPasswordSource.Text = item.ChildNodes[2].InnerText;
                        chkSavePassSource.Checked = true;
                    }
                    else
                    {
                        item.ChildNodes[2].Attributes[0].Value = "false";
                        xmlDocStoreInfo.Save(xmlDocPath);//eğer save password kullanıcı tarafından false seçildiyse dosyaya da false olarak kaydet. Check işaretini kaldır. Password alanını temizle.
                        chkSavePassSource.Checked = false;
                        txtPasswordSource.Text = "";
                    }

                    if (item.ChildNodes[0].Attributes[0].Value == "sql")//sql seçili değilse windows true olacak
                    {
                        rBSQLServerAuthSource.Checked = true;
                    }
                    else { rBWindowsAuthSource.Checked = true; }
                }
            }

            btnCompare.Enabled = false;
        }
        private void cmbServerNameSource_TextChanged(object sender, EventArgs e)
        {

            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
            chkSavePassSource.Checked = false;
            txtUserNameSource.Text = "";
            txtPasswordSource.Text = "";
            formControl.Clear();
            btnCompare.Enabled = false;


        }
        private void rBWindowsAuthSource_CheckedChanged(object sender, EventArgs e)
        {
            txtUserNameSource.Text = "";
            txtPasswordSource.Text = "";
            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
            txtUserNameSource.ReadOnly = true;
            txtPasswordSource.ReadOnly = true;
            lblUserNameSource.ForeColor = Color.Gray;
            lblPasswordSource.ForeColor = Color.Gray;
            formControl.Clear();
            btnCompare.Enabled = false;
            chkSavePassSource.Enabled = false;
        }
        private void rBSQLServerAuthSource_CheckedChanged(object sender, EventArgs e)
        {
            txtUserNameSource.ReadOnly = false;
            txtPasswordSource.ReadOnly = false;
            lblUserNameSource.ForeColor = Color.Black;
            lblPasswordSource.ForeColor = Color.Black;
            formControl.Clear();
            btnCompare.Enabled = false;
            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
            chkSavePassSource.Enabled = true;
        }
        private void txtUserNameSource_TextChanged(object sender, EventArgs e)
        {
            formControl.Clear();
            btnCompare.Enabled = false;
            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
            txtPasswordSource.Text = "";
        }
        private void txtPasswordSource_TextChanged(object sender, EventArgs e)
        {
            formControl.Clear();
            btnCompare.Enabled = false;
            cmbDatabaseSource.Items.Clear();
            btnDatabaseSource.Image = Properties.Resources.bluedatabase;
        }
        private void cmbDatabaseSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabaseSource.SelectedItem != null && cmbDatabaseTarget.SelectedItem != null) { btnCompare.Enabled = true; } else { btnCompare.Enabled = false; }
            if (rBWindowsAuthSource.Checked == true)
            {
                sourceDBInfo = "Data source=" + cmbServerNameSource.Text + "; initial catalog=" + cmbDatabaseSource.Text.ToString() + "; Integrated Security=SSPI;";
            }
            else if (rBSQLServerAuthSource.Checked == true)
            {
                sourceDBInfo = "Data source=" + cmbServerNameSource.Text + "; initial catalog=" + cmbDatabaseSource.Text.ToString() + "; User Id=" + txtUserNameSource.Text + "; Password=" + txtPasswordSource.Text + ";";
            }

        }
        //---------------------------------------------------------END OF SOURCE COMPONENT---------------------------------------------------------
        #endregion
        #region TARGET COMPONENTS
        //---------------------------------------------------------TARGET COMPONENTLER-------------------------------------------------------------
        private void cmbServerNameTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            formControl.Clear();
            //--------------------------------------------------------Servername seçildiğinde username ve password bilgilerini getir.--------------------------------------------------------
            xmlDocStoreInfo.Load(xmlDocPath);
            XmlNode selectedItem = xmlDocStoreInfo.SelectSingleNode("INFO");

            foreach (XmlNode item in selectedItem)
            {
                if (item.ChildNodes[0].InnerText == cmbServerNameTarget.Text)
                {
                    txtUserNameTarget.Text = item.ChildNodes[1].InnerText;
                    if (item.ChildNodes[2].Attributes[0].Value == "true")
                    {
                        txtPasswordTarget.Text = item.ChildNodes[2].InnerText;
                        chkSavePassTarget.Checked = true;
                    }
                    else
                    {
                        item.ChildNodes[2].Attributes[0].Value = "false";
                        xmlDocStoreInfo.Save(xmlDocPath);//eğer save password kullanıcı tarafından false seçildiyse dosyaya da false olarak kaydet. Check işaretini kaldır. Password alanını temizle.
                        chkSavePassTarget.Checked = false;
                        txtPasswordTarget.Text = "";
                    }
                    if (item.ChildNodes[0].Attributes[0].Value == "sql")
                    {
                        rBSQLServerAuthTarget.Checked = true;
                    }
                    else { rBWindowsAuthTarget.Checked = true; }
                }
            }
            btnCompare.Enabled = false;

        }
        private void cmbServerNameTarget_TextChanged(object sender, EventArgs e)
        {

            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            txtUserNameTarget.Text = "";
            txtPasswordTarget.Text = "";
            chkSavePassTarget.Checked = false;
            formControl.Clear();
            btnCompare.Enabled = false;


        }

        private void rBSQLServerAuthTarget_CheckedChanged(object sender, EventArgs e)
        {
            txtUserNameTarget.ReadOnly = false;
            txtPasswordTarget.ReadOnly = false;
            lblUserNameTarget.ForeColor = Color.Black;
            lblPasswordTarget.ForeColor = Color.Black;
            formControl.Clear();
            btnCompare.Enabled = false;
            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            chkSavePassTarget.Enabled = true;
        }
        private void rBWindowsAuthTarget_CheckedChanged(object sender, EventArgs e)
        {
            txtUserNameTarget.Text = "";
            txtPasswordTarget.Text = "";
            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            txtUserNameTarget.ReadOnly = true;
            txtPasswordTarget.ReadOnly = true;
            lblUserNameTarget.ForeColor = Color.Gray;
            lblPasswordTarget.ForeColor = Color.Gray;
            formControl.Clear();
            btnCompare.Enabled = false;
            chkSavePassTarget.Enabled = false;

        }
        private void txtUserNameTarget_TextChanged(object sender, EventArgs e)
        {
            formControl.Clear();
            btnCompare.Enabled = false;
            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            txtPasswordTarget.Text = "";
        }
        private void txtPasswordTarget_TextChanged(object sender, EventArgs e)
        {
            formControl.Clear();
            cmbDatabaseTarget.Items.Clear();
            btnDatabaseTarget.Image = Properties.Resources.bluedatabase;
            btnCompare.Enabled = false;// tüm komponentleri sonradan değiştirilmesi durumumda enable yapıldı. Çünkü compare buton aktif edildikten sonra text change edilirse database yanlış veri kaydedilecek
        }

        private void cmbDatabaseTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabaseSource.SelectedItem != null && cmbDatabaseTarget.SelectedItem != null) { btnCompare.Enabled = true; } else { btnCompare.Enabled = false; }
            if (rBWindowsAuthTarget.Checked == true)
            {
                targetDBInfo = "Data source=" + cmbServerNameTarget.Text + "; initial catalog=" + cmbDatabaseTarget.Text.ToString() + "; Integrated Security=SSPI;";
            }
            else if (rBSQLServerAuthTarget.Checked == true)
            {
                targetDBInfo = "Data source=" + cmbServerNameTarget.Text + "; initial catalog=" + cmbDatabaseTarget.Text.ToString() + "; User Id=" + txtUserNameTarget.Text + "; Password=" + txtPasswordTarget.Text + ";";
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        #endregion
        private void ConnectingForm_Load(object sender, EventArgs e)
        {

        }


        private void editRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditRecord ed = new EditRecord();
            this.Enabled = false;
            ed.ShowDialog();
            this.Enabled = true;
            this.BringToFront();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}