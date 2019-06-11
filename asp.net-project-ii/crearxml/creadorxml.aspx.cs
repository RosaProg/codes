using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace crearxml
{
    public partial class creadorxml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           #region XMLWriter
            
            using (XmlTextWriter Writer = new XmlTextWriter("C:\\Users\\August\\Documents\\MIXML.xml", Encoding.Unicode))
            {
                Writer.WriteStartDocument();
                Writer.Formatting = Formatting.Indented;
                Writer.Indentation = 12;

                Writer.WriteStartElement("data");

                    Writer.WriteStartElement("Questionnaire");
                            Writer.WriteElementString("Name", "OES");
                            Writer.WriteElementString("ExtendedName", "Oxford Elbow Score");
                            Writer.WriteElementString("DateFormat", "DDMMYYYY");
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("Header");

                            Writer.WriteElementString("InitialQuestion", "On which side of your body is the affected joint, for which you are receiving treatment");

                            Writer.WriteStartElement("InitialQuestionAnswers");
                                    Writer.WriteElementString("Answer", "Left");
                                    Writer.WriteElementString("Answer", "Right");
                                    Writer.WriteElementString("Answer", "Both");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("QuestionsHeader");
                                    Writer.WriteElementString("QuestionTitle", "Problems with your Elbow");
                                    Writer.WriteElementString("Instruction", "If you said 'both', please complete the first questionnaire thinking about the right side. A second questionnaire, for the left side, will follow.");
                                    Writer.WriteElementString("PerQuestionInstruction", "During the past 4 weeks...");
                            Writer.WriteEndElement();

                    Writer.WriteEndElement();

                    Writer.WriteStartElement("Questions");

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you had difficulty lifting things in your home, such as putting the rubbish, because of your elbow problem?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No difficulty");
                                                Writer.WriteElementString("Answer", "A little bit of difficulty");
                                                Writer.WriteElementString("Answer", "Moderate Difficulty");
                                                Writer.WriteElementString("Answer", "Extreme Difficulty");
                                                Writer.WriteElementString("Answer", "Impossible to do");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you had difficulty carrying bags of shopping,  because of your elbow problem?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No difficulty");
                                                Writer.WriteElementString("Answer", "A little bit of difficulty");
                                                Writer.WriteElementString("Answer", "Moderate Difficulty");
                                                Writer.WriteElementString("Answer", "Extreme Difficulty");
                                                Writer.WriteElementString("Answer", "Impossible to do");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you had any difficulty washing yourself all over,  because of your elbow problem?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No difficulty");
                                                Writer.WriteElementString("Answer", "A little bit of difficulty");
                                                Writer.WriteElementString("Answer", "Moderate Difficulty");
                                                Writer.WriteElementString("Answer", "Extreme Difficulty");
                                                Writer.WriteElementString("Answer", "Impossible to do");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you had any difficulty dressing yourself,  because of your elbow problem?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No difficulty");
                                                Writer.WriteElementString("Answer", "A little bit of difficulty");
                                                Writer.WriteElementString("Answer", "Moderate Difficulty");
                                                Writer.WriteElementString("Answer", "Extreme Difficulty");
                                                Writer.WriteElementString("Answer", "Impossible to do");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you felt your elbow problem is 'controlling your life'?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No, not at all");
                                                Writer.WriteElementString("Answer", "Ocassionally");
                                                Writer.WriteElementString("Answer", "Some Days");
                                                Writer.WriteElementString("Answer", "Most Days");
                                                Writer.WriteElementString("Answer", "Every Day");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "How much has your elbow problem been 'on your mind'?");
                                        Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No, no at all");
                                                Writer.WriteElementString("Answer", "A little bit of the time");
                                                Writer.WriteElementString("Answer", "Some of the time");
                                                Writer.WriteElementString("Answer", "Most of the time");
                                                Writer.WriteElementString("Answer", "All of the time");
                                        Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Have you been troubled by pain from your elbow problem in bed at night?");
                                       Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "Not at all");
                                                Writer.WriteElementString("Answer", "1 or 2 nights");
                                                Writer.WriteElementString("Answer", "Some nights");
                                                Writer.WriteElementString("Answer", "Most nights");
                                                Writer.WriteElementString("Answer", "Every night");
                                      Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "How often has your elbow pain interfered with your sleeping?");
                                Writer.WriteStartElement("Answers");
                                                Writer.WriteElementString("Answer", "No, not at all");
                                                Writer.WriteElementString("Answer", "Ocassionally");
                                                Writer.WriteElementString("Answer", "Some of the time");
                                                Writer.WriteElementString("Answer", "Most of the time");
                                                Writer.WriteElementString("Answer", "All of the time");
                               Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "How much has your elbow problem interfered with your usual work or every day activities?");
                                Writer.WriteStartElement("Answers");
                                            Writer.WriteElementString("Answer", "Not at all");
                                            Writer.WriteElementString("Answer", "A little bit");
                                            Writer.WriteElementString("Answer", "Moderately");
                                            Writer.WriteElementString("Answer", "Greatly");
                                            Writer.WriteElementString("Answer", "Totally");
                                Writer.WriteEndElement();
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                            Writer.WriteElementString("Text", "Has your elbow problem limited your ability to take part in leisure activities that you enjoy doing?");
                                Writer.WriteStartElement("Answers");
                                        Writer.WriteElementString("Answer", "No, not at all");
                                        Writer.WriteElementString("Answer", "Ocassionally");
                                        Writer.WriteElementString("Answer", "Some of the time");
                                        Writer.WriteElementString("Answer", "Most of the time");
                                        Writer.WriteElementString("Answer", "All of the time");
                                Writer.WriteEndElement();
                                Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                                Writer.WriteElementString("Text", "How would you describe the worst pain you have from your elbow?");

                                Writer.WriteStartElement("Answers");
                                    Writer.WriteElementString("Answer", "No Pain");
                                    Writer.WriteElementString("Answer", "Mild Pain");
                                    Writer.WriteElementString("Answer", "Moderate Pain");
                                    Writer.WriteElementString("Answer", "Severe Pain");
                                    Writer.WriteElementString("Answer", "Unbearable");
                                Writer.WriteEndElement();

                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Question");
                                Writer.WriteElementString("Text", "How would you describe the pain you usually have from your elbow");

                                Writer.WriteStartElement("Answers");
                                    Writer.WriteElementString("Answer", "No Pain");
                                    Writer.WriteElementString("Answer", "Mild Pain");
                                    Writer.WriteElementString("Answer", "Moderate Pain");
                                    Writer.WriteElementString("Answer", "Severe Pain");
                                    Writer.WriteElementString("Answer", "Unbearable");
                                Writer.WriteEndElement();

                            Writer.WriteEndElement();

                Writer.WriteEndElement();

                Writer.WriteStartElement("Scores");

                            Writer.WriteElementString("MetricScore", "(100 / Maximum Possible Domain Score) * Actual Score");

                            Writer.WriteStartElement("Score");
                            Writer.WriteElementString("Min", "0");
                            Writer.WriteElementString("Max", "19");
                            Writer.WriteElementString("Message", "May indicate severe elbow arthritis. It is highly likely that you may well require some form of surgical intervention, contact your family physician for a consult with an Orthopaedic Surgeon");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Score");
                            Writer.WriteElementString("Min", "20");
                            Writer.WriteElementString("Max", "29");
                            Writer.WriteElementString("Message", "May indicate moderate to severe elbow arthritis. See your family physician for an assessment and x-ray. Consider a consult with an Orthopaedic Surgeon");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Score");
                            Writer.WriteElementString("Min", "30");
                            Writer.WriteElementString("Max", "39");
                            Writer.WriteElementString("Message", "May indicate mild to moderate elbow arthritis. Consider seeing your family physician for an assessment and possible x-ray. You may benefit from non surgical treatment, such as excercise, weight loss, and/or anti-inflammatory medication.");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Score");
                            Writer.WriteElementString("Min", "40");
                            Writer.WriteElementString("Max", "49");
                            Writer.WriteElementString("Message", "May indicate satisfactory joint function. May not require any formal treatment.");
                            Writer.WriteEndElement();

                Writer.WriteEndElement();

                Writer.WriteStartElement("Domains");

                            Writer.WriteStartElement("Domain");
                            Writer.WriteElementString("Name", "Pain Domain");
                            Writer.WriteElementString("Question", "7");
                            Writer.WriteElementString("Question", "8");
                            Writer.WriteElementString("Question", "12");
                            Writer.WriteElementString("Question", "11");
                            Writer.WriteElementString("MaximumScoreDomain", "16");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Domain");
                            Writer.WriteElementString("Name", "Elbow Function");
                            Writer.WriteElementString("Question", "4");
                            Writer.WriteElementString("Question", "3");
                            Writer.WriteElementString("Question", "1");
                            Writer.WriteElementString("Question", "2");
                            Writer.WriteElementString("MaximumScoreDomain", "16");
                            Writer.WriteEndElement();

                            Writer.WriteStartElement("Domain");
                            Writer.WriteElementString("Name", "Social Psychological");
                            Writer.WriteElementString("Question", "10");
                            Writer.WriteElementString("Question", "6");
                            Writer.WriteElementString("Question", "5");
                            Writer.WriteElementString("Question", "9");
                            Writer.WriteElementString("MaximumScoreDomain", "16");
                            Writer.WriteEndElement();

                Writer.WriteEndElement();

                Writer.WriteStartElement("Footer");
                            Writer.WriteElementString("Note", "Have you had difficulty lifting things in your home, such as putting the rubbish, because of your elbow problem?");
                Writer.WriteEndElement();

                Writer.WriteEndDocument();
                Writer.Flush();
            }
            #endregion
            
            // Leer XML
            #region Leer XML

                #region Questionnaire
                XmlDocument reader = new XmlDocument();
                reader.Load("C:\\Users\\August\\Documents\\csharp\\MIXML.xml");

                XmlNodeList listaNodos = reader.SelectNodes("data/Questionnaire");
                XmlNode Questionnaire;

                for (int i = 0; i < listaNodos.Count; i++)
                {
                    Questionnaire = listaNodos.Item(i);

                    string Name = Questionnaire.SelectSingleNode("Name").InnerText;
                    
                    string ExtendedName = Questionnaire.SelectSingleNode("ExtendedName").InnerText;
                    string DateFormat = Questionnaire.SelectSingleNode("DateFormat").InnerText;
                    Label1.Text = Label1.Text + "<br/>" + Name + "<br/>" + ExtendedName + "<br/>" + DateFormat + "<br/>";
                }
                #endregion
                #region Header

                listaNodos = reader.SelectNodes("data/Header");
                XmlNode Header;

                for (int i = 0; i < listaNodos.Count; i++)
                {
                    Header = listaNodos.Item(i);

                    string InitialQuestion = Header.SelectSingleNode("InitialQuestion").InnerText;
                    Label1.Text = Label1.Text + InitialQuestion + "<br/>";
                    XmlNodeList InitialQuestionAnswers = Header.SelectSingleNode("InitialQuestionAnswers").SelectNodes("Answer");
                    XmlNode Answer;

                    for (int f = 0; f < InitialQuestionAnswers.Count; f++)
                    {
                        Answer = InitialQuestionAnswers.Item(f);

                        string CurrentAnswer = Answer.InnerText;
                        Label1.Text =Label1.Text + CurrentAnswer + "<br/>";
                    }

                }
                #endregion 
                #region QuestionsHeader
                XmlNodeList listaQuestionsHeader = reader.SelectNodes("data/QuestionsHeader");
                XmlNode QuestionsHeader;

                for (int i = 0; i < listaQuestionsHeader.Count; i++)
                {
                    QuestionsHeader = listaQuestionsHeader.Item(i);

                    string QuestionTitle = QuestionsHeader.SelectSingleNode("QuestionTitle").InnerText;

                    string Instruction = QuestionsHeader.SelectSingleNode("Instruction").InnerText;
                    string PerQuestionInstruction = QuestionsHeader.SelectSingleNode("PerQuestionInstruction").InnerText;
                    Label1.Text = Label1.Text + QuestionTitle + "<br/>" + Instruction + "<br/>" + PerQuestionInstruction + "<br/>";

                }
                #endregion
                #region Questions

                listaNodos = reader.SelectNodes("data/Questions");
                XmlNode QQuestion;

               // Label1.Text = listaNodos.Count.ToString();
                for (int i = 0; i < listaNodos.Count; i++)
                {
                    QQuestion = listaNodos.Item(i);

                    XmlNodeList Questions = QQuestion.SelectNodes("Question");

                    XmlNode Question;
                    XmlNode Answer;

                    for (int f = 0; f < Questions.Count; f++)
                    {
                        Question = Questions.Item(f);

                        string CurrentQuestion = Question.SelectSingleNode("Text").InnerText;
                        Label1.Text = Label1.Text + CurrentQuestion + "<br/>";
                        XmlNodeList CurrentQuestionAnswers = Question.SelectSingleNode("Answers").SelectNodes("Answer");

                        for (int g = 0; g < CurrentQuestionAnswers.Count; g++)
                        {
                            Answer = CurrentQuestionAnswers.Item(g);
                            string CurrentAnswer = Answer.InnerText;
                            Label1.Text = Label1.Text + CurrentAnswer + "<br/>";
                        }
                    }

                }
                #endregion
                #region Scores
                listaNodos = reader.SelectNodes("data/Scores");
                XmlNode Scores;

                for (int i = 0; i < listaNodos.Count; i++)
                {
                    Scores = listaNodos.Item(i);

                    string MetricScore = Scores.SelectSingleNode("MetricScore").InnerText;
                    Label1.Text = Label1.Text + MetricScore + "<br/>";
                    XmlNodeList SScores = Scores.SelectNodes("Score");
                    XmlNode Score;

                    for (int f = 0; f < SScores.Count; f++)
                    {
                        Score = SScores.Item(f);

                        string Min = Score.SelectSingleNode("Min").InnerText;
                        string Max = Score.SelectSingleNode("Max").InnerText;
                        string Message = Score.SelectSingleNode("Message").InnerText;
                        Label1.Text = Label1.Text + Min + "<br/>" + Max + "<br/>" + Message + "<br/>";
                    }

                }
                #endregion

                #region Domains
                listaNodos = reader.SelectNodes("data/Domains");
                XmlNode Domain;

                for (int i = 0; i < listaNodos.Count; i++)
                {
                    Domain = listaNodos.Item(i);

                    XmlNodeList DomainName = Domain.SelectSingleNode("Domain").SelectNodes("Name");
                    XmlNode DName;
                    for (int f = 0; f < DomainName.Count; f++)
                    {
                        DName = DomainName.Item(f);
                        string CurrentDomain = DName.InnerText;
                        Label1.Text = Label1.Text + CurrentDomain + "<br/>";
                    }

                    XmlNodeList DQuestion = Domain.SelectSingleNode("Domain").SelectNodes("Question");
                    XmlNode DomainQuestion;

                    for (int f = 0; f < DQuestion.Count; f++)
                    {
                        DomainQuestion = DQuestion.Item(f);

                      string CurrentDomainQuestion = DomainQuestion.InnerText;
                      Label1.Text = Label1.Text + CurrentDomainQuestion + "<br/>";
                    }

                    XmlNodeList MaximumScoreDomain = Domain.SelectSingleNode("Domain").SelectNodes("MaximumScoreDomain");
                    XmlNode MaxScoreDomain;

                    for (int f = 0; f < MaximumScoreDomain.Count; f++)
                    {
                        MaxScoreDomain = MaximumScoreDomain.Item(f);

                       string CurrentMaxScoreDomain = MaxScoreDomain.InnerText;
                       Label1.Text = Label1.Text + CurrentMaxScoreDomain + "<br/>";

                    }
                }
                #endregion

                #region Footer
                listaNodos = reader.SelectNodes("data/Footer");
                XmlNode Footer;

                for (int i = 0; i < listaNodos.Count; i++)
                {
                    Footer = listaNodos.Item(i);

                    string CurrentFooterNode = Footer.SelectSingleNode("Note").InnerText;
                    Label1.Text = Label1.Text + CurrentFooterNode + "<br/>";
                }
                #endregion

            #endregion
        }
        protected void enviar_Click1(object send, EventArgs e)
        {
        }

        /*protected void enviar_Click1(object sender, EventArgs e)
        {
            
            string nombreArchivos = nombreArchivo.Value.ToString();
            string name = Text1.Value.ToString();
            string age = Text2.Value.ToString();
            string last_name = Text3.Value;
            using (XmlTextWriter Writer = new XmlTextWriter("C://Users/august/documents/csharp/"+nombreArchivos+".xml", Encoding.Unicode))
            {
                Writer.WriteStartDocument();
                Writer.Formatting = Formatting.Indented;
                Writer.Indentation = 12;

                Writer.WriteStartElement("Questionnaire");

                //Escribimos un nodo empleado.
                Writer.WriteStartElement("name");
                                    Writer.WriteElementString("First Name", name);
                                    Writer.WriteElementString("Last Name",last_name);
                                    Writer.WriteElementString("Age", age);
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question 1",pregunta1.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta11.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta12.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta13.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta14.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta15.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question2",pregunta2.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta21.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta22.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta23.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta24.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta25.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question3",pregunta3.Value.ToString());
                                   Writer.WriteElementString("Answer0", Respuesta31.Value.ToString());
                                   Writer.WriteElementString("Answer1", Respuesta32.Value.ToString());
                                   Writer.WriteElementString("Answer2", Respuesta33.Value.ToString());
                                   Writer.WriteElementString("Answer3", Respuesta34.Value.ToString());
                                   Writer.WriteElementString("Answer4", Respuesta35.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question4",pregunta4.Value.ToString());
                                   Writer.WriteElementString("Answer0", Respuesta41.Value.ToString());
                                   Writer.WriteElementString("Answer1", Respuesta42.Value.ToString());
                                   Writer.WriteElementString("Answer2", Respuesta43.Value.ToString());
                                   Writer.WriteElementString("Answer3", Respuesta44.Value.ToString());
                                   Writer.WriteElementString("Answer4", Respuesta45.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question5", pregunta5.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta51.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta52.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta53.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta54.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta55.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question6", pregunta6.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta61.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta62.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta63.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta64.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta65.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question7", pregunta7.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta71.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta72.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta73.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta74.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta75.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question8", pregunta8.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta81.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta82.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta83.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta84.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta85.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question9", pregunta9.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta91.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta92.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta93.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta94.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta95.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question10", pregunta10.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta101.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta102.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta103.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta104.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta105.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question11", pregunta11.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta111.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta112.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta113.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta114.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta115.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteStartElement("Question12", pregunta12.Value.ToString());
                                    Writer.WriteElementString("Answer0", Respuesta121.Value.ToString());
                                    Writer.WriteElementString("Answer1", Respuesta122.Value.ToString());
                                    Writer.WriteElementString("Answer2", Respuesta123.Value.ToString());
                                    Writer.WriteElementString("Answer3", Respuesta124.Value.ToString());
                                    Writer.WriteElementString("Answer4", Respuesta125.Value.ToString());
                Writer.WriteEndElement();

                Writer.WriteEndDocument();
                Writer.Flush();
            }
           
      }*/

    }
}