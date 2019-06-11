using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Survey;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Presentation;

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security;
//using System.Web.Security;
namespace ProXmlFeeder
{

    public partial class Form1 : Form
    {
        public string QuestionnaireType = "";

        public Form1()
        {
            InitializeComponent();
            btnStart.Enabled = false;
            txtConsole.Enabled = false;
            txtXmlFileName.Enabled = false;

            Savebtn.Visible = false;
            textBox1.Text = "Please insert the path of Log file before Continue with the Upload of Questionnaires";
        }
       
        public void button1_Click(object sender, EventArgs e)
        {
            //btnStart.BackColor = System.Drawing.Color.Lime;

            try
            {
                XmlParser parser = new XmlParser(this.txtXmlFileName.Text);

                int QuestionnaireFormat = txtXmlFileName.Text.IndexOf("QuestionnaireFormat_");
                if (QuestionnaireFormat != -1)
                {
                    Format qf = parser.LoadQuestionnaireFormat();

                    if (qf != null)
                    {
                        this.txtConsole.Text += parser.ToString();
                        Savebtn.Visible = true;
                    }
                    else {
                        MessageBox.Show("Nulo");
                    }

                }
                else
                {
                    Questionnaire q = parser.LoadQuestionnaire();

                    if (q != null)
                    {
                        this.txtConsole.Text += parser.ToString();
                        Savebtn.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("The path is wrong or the file is missing. Please verify the path and file are correct.\n"+ exp.ToString());
                logReport.returnError("The path is wrong or the file is missing. Please verify the path and file are correct. \n" + exp.ToString());
            }           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int QuestionnaireFormat = txtXmlFileName.Text.IndexOf("QuestionnaireFormat_");
            if (QuestionnaireFormat != -1)
            {
                ProLoaderQuestionnaireFormat saverFormat = new ProLoaderQuestionnaireFormat();
                saverFormat.SaveQuestionnaireFormatFull();
                MessageBox.Show(saverFormat.resultsave);
            }
            else
            {
               ProLoader saver = new ProLoader();
               saver.SaveQuestionnaireFull();
               MessageBox.Show(saver.resultsave);
            }
        }

        public static void Print(string writer){
            MessageBox.Show(writer);
        }

        private void LogPath_Click(object sender, EventArgs e)
        {
            string LogDirectory;

            LogDirectory = textBox1.Text;

            if (System.IO.Directory.Exists(LogDirectory))
            {
                logReport.loadDirectory((textBox1.Text).ToString());
                txtXmlFileName.Enabled = true;
                btnStart.Enabled = true;
                txtConsole.Enabled = true;
                //comboBox1.Enabled = true;
            }
            else
            {
                MessageBox.Show("This path is not correct");
                
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
