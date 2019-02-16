using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DBTest
{
    public partial class EditRecord : Form
    {
        XmlDocument xmlDoc = new XmlDocument();
        public EditRecord()
        {
            InitializeComponent();
        }


        private string xmlDocPath = Application.StartupPath + "\\QuickComparison.xml";
        private void EditRecord_Load(object sender, EventArgs e)
        {
            CallAllRecord();
        }

        private void CallAllRecord()
        {
            xmlDoc.Load(xmlDocPath);
            XmlNode xNode = xmlDoc.SelectSingleNode("INFO");
            foreach (XmlNode item in xNode)
            {
                lstRecordList.Items.Add(item.Attributes[0].Value);
            }

        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            if (lstRecordList.SelectedItem != null)
            {
                string xserverName = lstRecordList.SelectedItem.ToString();
                xmlDoc.Load(xmlDocPath);
                XmlNode xfindRecord = xmlDoc.SelectSingleNode("INFO");

                foreach (XmlNode item in xfindRecord)
                {
                    if (item.Attributes[0].Value == xserverName)
                    {
                        item.ParentNode.RemoveChild(item);
                        xmlDoc.Save(xmlDocPath);
                    }
                }
                lstRecordList.Items.Clear();
                CallAllRecord();
            }
            else { MessageBox.Show("Select a record."); }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string updateInfo = "";
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstRecordList.SelectedItem != null)
            {
                xmlDoc.Load(xmlDocPath);
                XmlNode xUpdate = xmlDoc.SelectSingleNode("INFO");
                foreach (XmlNode item in xUpdate)
                {
                    if (item.Attributes[0].Value == lstRecordList.SelectedItem.ToString())
                    {
                        updateInfo = item.Attributes[0].Value;
                        txtUpdateName.Text = item.Attributes[0].Value;
                        txtUpdateSource.Text = item.ChildNodes[0].InnerText;
                        txtUpdateTarget.Text = item.ChildNodes[1].InnerText;
                    }
                }

            }
            else
            {
                MessageBox.Show("Choose a record.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            xmlDoc.Load(xmlDocPath);
            XmlNode xUpdate = xmlDoc.SelectSingleNode("INFO");
            foreach (XmlNode item in xUpdate)
            {
                if (item.Attributes[0].Value == updateInfo)
                {
                    item.Attributes[0].Value = txtUpdateName.Text;
                    item.ChildNodes[0].InnerText = txtUpdateSource.Text;
                    item.ChildNodes[1].InnerText = txtUpdateTarget.Text;
                    xmlDoc.Save(xmlDocPath);
                    MessageBox.Show("Record updated");
                    lstRecordList.Items.Clear();
                    CallAllRecord();
                    txtUpdateName.Text = "";
                    txtUpdateSource.Text = "";
                    txtUpdateTarget.Text = "";

                    break;
                }
            }
        }
    }
}
