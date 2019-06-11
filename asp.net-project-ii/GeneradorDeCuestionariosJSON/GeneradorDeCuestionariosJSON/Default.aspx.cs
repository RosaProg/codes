using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;

namespace GeneradorDeCuestionariosJSON
{
    public partial class _Default : Page
    {
        public string seccion;
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "C://Users//August//Desktop//JSONFromCS//json_parcial.json";
            string Questionner = "prueba";
            // Crea un modelo JSON estandar a todos los cuestionarios
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\" + Questionner + ".cs", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            //Apertura
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Web;");
            writer.WriteLine("using System.Web.UI;");
            writer.WriteLine("using System.Web.UI.WebControls;");
            writer.WriteLine("using System.IO;");
            writer.WriteLine("using System.Web.Script;");
            writer.WriteLine("namespace FeederDSPRIMA");
            writer.WriteLine("{");
            writer.WriteLine("public class CuestionarioSettings");
            writer.WriteLine("{");
            writer.WriteLine("public string file_to_import = " + '"' + url + '"' + ";");
            writer.WriteLine("}");
            writer.WriteLine("public class Cuestionario");
            writer.WriteLine("{");
            writer.WriteLine("public List<MiCuestionario> data { get; set; }");
            writer.WriteLine("}");
            writer.WriteLine("public class MiCuestionario"); 
            writer.WriteLine("{");
            writer.WriteLine("public string Name { get; set; }");
            writer.WriteLine("public string ExtendedName { get; set; }";
            writer.WriteLine("public string DateFormat { get; set; }");
            writer.WriteLine("public string InitialQuestion { get; set; }
        public List<DataItemHeaderQuestionAnswer> HeaderQuestionAnswer { get; set; }
        public List<DataItemQuestions> Questions { get; set; }
        public List<DataItemScores> Scores { get; set; }
        public string QuestionsTitle { get; set; }
        public string QuestionsInstructions { get; set; } // ESTO IRIA CON QuestionnaireInstructions.cs
        public string Note { get; set; }
        public string QuestionnaireFooter { get; set; }
    }
    public class DataItemQuestions // ESTO LE PASARIA LOS DATOS A QuestionnaireItem.cs
    {
        public List<DataItemQuestion> question1 { get; set; }
        public List<DataItemQuestion> question2 { get; set; }
        public List<DataItemQuestion> question3 { get; set; }
        public List<DataItemQuestion> question4 { get; set; }
        public List<DataItemQuestion> question5 { get; set; }
        public List<DataItemQuestion> question6 { get; set; }
        public List<DataItemQuestion> question7 { get; set; }
        public List<DataItemQuestion> question8 { get; set; }
        public List<DataItemQuestion> question9 { get; set; }
        public List<DataItemQuestion> question10 { get; set; }
        public List<DataItemQuestion> question11 { get; set; }
        public List<DataItemQuestion> question12 { get; set; }
    }
    public class DataItemQuestion // ESTO LE PASARIA LOS DATOS A QuestionnaireItem.cs
    {
        public string QuestionIntroduction { get; set; }
        public string QuestionText { get; set; }
        public List<DataItemAnswer> Answers { get; set; }
    }
    public class DataItemAnswer // ESTO LE PASARIA LOS DATOS A QuestionnaireItemOption.cs
    {
        public string answer4 { get; set; }
        public string answer3 { get; set; }
        public string answer2 { get; set; }
        public string answer1 { get; set; }
        public string answer0 { get; set; }
    }
    public class DataItemHeaderQuestionAnswer // ESTO LE PASARIA LOS DATOS A Questionnaire.cs
    {
        public string optionA { get; set; }
        public string optionB { get; set; }
        public string optionC { get; set; }
    }
    public class DataItemScores // ESTO IRIA CON ProDomain.cs
    {
        public List<DataItemScore> scoreA { get; set; }
        public List<DataItemScore> scoreB { get; set; }
        public List<DataItemScore> scoreC { get; set; }
        public List<DataItemScore> scoreD { get; set; }
        public string MetricScore { get; set; }
        public List<DataItemQuestionDomain> QuestionsDomains { get; set; }
    } 
    public class DataItemScore // ESTO LE PASARIA LOS DATOS A ProDomainResultRange.cs
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string Message { get; set; }
    }
    public class DataItemQuestionDomain // ESTO LE PASARIA LOS DATOS A ProDomain.cs
    {
        public List<DataItemPainDomain> PainDomain { get; set; }
        public List<DataItemElbowFunction> ElbowFunction { get; set; }
        public List<DataItemSocialPsychologicalDomain> SocialPsychologicalDomain { get; set; }
    }
    public class DataItemPainDomain// USARLA COMO CLASE INTERMEDIA O NUEVO MODELO Y DEVOLVER LOS VALORES MEDIANTE LAS OTRAS CLASES
    {
       //public List<DataItemNumberQuestionPainDomain> NumberQuestionPainDomain { get; set; }
       public string NumberQuestionA { get; set; }
       public string NumberQuestionB { get; set; }
       public string NumberQuestionC { get; set; }
       public string NumberQuestionD { get; set; }
       public string MaximumScoreDomain { get; set; }
    }
    public class DataItemElbowFunction // USARLA COMO CLASE INTERMEDIA O NUEVO MODELO Y DEVOLVER LOS VALORES MEDIANTE LAS OTRAS CLASES
    {
        //public  List<DataItemNumberQuestionElbowFunction> NumberQuestionElbowFunction { get; set; }
        public string MaximumScoreDomain { get; set; }
        public string NumberQuestionA { get; set; }
        public string NumberQuestionB { get; set; }
        public string NumberQuestionC { get; set; }
        public string NumberQuestionD { get; set; }

    }
    public class DataItemSocialPsychologicalDomain // USARLA COMO CLASE INTERMEDIA O NUEVO MODELO Y DEVOLVER LOS VALORES MEDIANTE LAS OTRAS CLASES
    {
        //public List<DataItemNumberQuestionSocialPsychologicalDomain> NumberQuestionSocialPsychologicalDomain { get; set; }
        public string NumberQuestionA { get; set; }
        public string NumberQuestionB { get; set; }
        public string NumberQuestionC { get; set; }
        public string NumberQuestionD { get; set; }
        public string MaximumScoreDomain { get; set; }
    }
 
    public ClaseCuestionario
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ESTO VA EN BUSINESS LOGIC
            #region JSONDeserializer
            CuestionarioSettings newCuestionario = new CuestionarioSettings();
            using (StreamReader sr = new StreamReader(newCuestionario.file_to_import))
            {
                string line = sr.ReadToEnd();
                Label1.Text = line.Replace("\"", "\"\"");
                string json = @"" + line + "";
                //Label1.Text = json;

                Cuestionario cuestionarioactual = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Cuestionario>(json);

                foreach (var item in cuestionarioactual.data)
                {
                    Label1.Text = item.Name.ToString();
                    Label1.Text = item.ExtendedName.ToString();
                    Label1.Text = item.InitialQuestion.ToString();
                    Label1.Text = item.QuestionsTitle.ToString();
                    Label1.Text = item.QuestionsInstructions.ToString();
                    Label1.Text = item.Note.ToString();
                    Label1.Text = item.QuestionnaireFooter.ToString();

                    foreach (var itemHeaderQuestionAnswer in item.HeaderQuestionAnswer)
                    {
                        Label1.Text = itemHeaderQuestionAnswer.optionA.ToString();
                        Label1.Text = itemHeaderQuestionAnswer.optionB.ToString();
                        Label1.Text = itemHeaderQuestionAnswer.optionC.ToString();
                    }

                    foreach (var itemQuestions in item.Questions)
                    {
                        #region Question1
                        foreach (var itemQuestion1 in itemQuestions.question1)
                        {
                            Label1.Text = itemQuestion1.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion1.QuestionText.ToString();

                            foreach (var itemQuestion1Answers in itemQuestion1.Answers)
                            {
                                Label1.Text = itemQuestion1Answers.answer4.ToString();
                                Label1.Text = itemQuestion1Answers.answer3.ToString();
                                Label1.Text = itemQuestion1Answers.answer2.ToString();
                                Label1.Text = itemQuestion1Answers.answer1.ToString();
                                Label1.Text = itemQuestion1Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question2
                        foreach (var itemQuestion2 in itemQuestions.question2)
                        {
                            Label1.Text = itemQuestion2.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion2.QuestionText.ToString();

                            foreach (var itemQuestion2Answers in itemQuestion2.Answers)
                            {
                                Label1.Text = itemQuestion2Answers.answer4.ToString();
                                Label1.Text = itemQuestion2Answers.answer3.ToString();
                                Label1.Text = itemQuestion2Answers.answer2.ToString();
                                Label1.Text = itemQuestion2Answers.answer1.ToString();
                                Label1.Text = itemQuestion2Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question3
                        foreach (var itemQuestion3 in itemQuestions.question3)
                        {
                            Label1.Text = itemQuestion3.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion3.QuestionText.ToString();

                            foreach (var itemQuestion3Answers in itemQuestion3.Answers)
                            {
                                Label1.Text = itemQuestion3Answers.answer4.ToString();
                                Label1.Text = itemQuestion3Answers.answer3.ToString();
                                Label1.Text = itemQuestion3Answers.answer2.ToString();
                                Label1.Text = itemQuestion3Answers.answer1.ToString();
                                Label1.Text = itemQuestion3Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question4
                        foreach (var itemQuestion4 in itemQuestions.question4)
                        {
                            Label1.Text = itemQuestion4.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion4.QuestionText.ToString();

                            foreach (var itemQuestion4Answers in itemQuestion4.Answers)
                            {
                                Label1.Text = itemQuestion4Answers.answer4.ToString();
                                Label1.Text = itemQuestion4Answers.answer3.ToString();
                                Label1.Text = itemQuestion4Answers.answer2.ToString();
                                Label1.Text = itemQuestion4Answers.answer1.ToString();
                                Label1.Text = itemQuestion4Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question5
                        foreach (var itemQuestion5 in itemQuestions.question5)
                        {
                            Label1.Text = itemQuestion5.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion5.QuestionText.ToString();

                            foreach (var itemQuestion5Answers in itemQuestion5.Answers)
                            {
                                Label1.Text = itemQuestion5Answers.answer4.ToString();
                                Label1.Text = itemQuestion5Answers.answer3.ToString();
                                Label1.Text = itemQuestion5Answers.answer2.ToString();
                                Label1.Text = itemQuestion5Answers.answer1.ToString();
                                Label1.Text = itemQuestion5Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question6
                        foreach (var itemQuestion6 in itemQuestions.question6)
                        {
                            Label1.Text = itemQuestion6.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion6.QuestionText.ToString();

                            foreach (var itemQuestion6Answers in itemQuestion6.Answers)
                            {
                                Label1.Text = itemQuestion6Answers.answer4.ToString();
                                Label1.Text = itemQuestion6Answers.answer3.ToString();
                                Label1.Text = itemQuestion6Answers.answer2.ToString();
                                Label1.Text = itemQuestion6Answers.answer1.ToString();
                                Label1.Text = itemQuestion6Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question7
                        foreach (var itemQuestion7 in itemQuestions.question7)
                        {
                            Label1.Text = itemQuestion7.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion7.QuestionText.ToString();

                            foreach (var itemQuestion7Answers in itemQuestion7.Answers)
                            {
                                Label1.Text = itemQuestion7Answers.answer4.ToString();
                                Label1.Text = itemQuestion7Answers.answer3.ToString();
                                Label1.Text = itemQuestion7Answers.answer2.ToString();
                                Label1.Text = itemQuestion7Answers.answer1.ToString();
                                Label1.Text = itemQuestion7Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question8
                        foreach (var itemQuestion8 in itemQuestions.question8)
                        {
                            Label1.Text = itemQuestion8.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion8.QuestionText.ToString();

                            foreach (var itemQuestion8Answers in itemQuestion8.Answers)
                            {
                                Label1.Text = itemQuestion8Answers.answer4.ToString();
                                Label1.Text = itemQuestion8Answers.answer3.ToString();
                                Label1.Text = itemQuestion8Answers.answer2.ToString();
                                Label1.Text = itemQuestion8Answers.answer1.ToString();
                                Label1.Text = itemQuestion8Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question9
                        foreach (var itemQuestion9 in itemQuestions.question9)
                        {
                            Label1.Text = itemQuestion9.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion9.QuestionText.ToString();

                            foreach (var itemQuestion9Answers in itemQuestion9.Answers)
                            {
                                Label1.Text = itemQuestion9Answers.answer4.ToString();
                                Label1.Text = itemQuestion9Answers.answer3.ToString();
                                Label1.Text = itemQuestion9Answers.answer2.ToString();
                                Label1.Text = itemQuestion9Answers.answer1.ToString();
                                Label1.Text = itemQuestion9Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question10
                        foreach (var itemQuestion10 in itemQuestions.question10)
                        {
                            Label1.Text = itemQuestion10.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion10.QuestionText.ToString();

                            foreach (var itemQuestion10Answers in itemQuestion10.Answers)
                            {
                                Label1.Text = itemQuestion10Answers.answer4.ToString();
                                Label1.Text = itemQuestion10Answers.answer3.ToString();
                                Label1.Text = itemQuestion10Answers.answer2.ToString();
                                Label1.Text = itemQuestion10Answers.answer1.ToString();
                                Label1.Text = itemQuestion10Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question11
                        foreach (var itemQuestion11 in itemQuestions.question11)
                        {
                            Label1.Text = itemQuestion11.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion11.QuestionText.ToString();

                            foreach (var itemQuestion11Answers in itemQuestion11.Answers)
                            {
                                Label1.Text = itemQuestion11Answers.answer4.ToString();
                                Label1.Text = itemQuestion11Answers.answer3.ToString();
                                Label1.Text = itemQuestion11Answers.answer2.ToString();
                                Label1.Text = itemQuestion11Answers.answer1.ToString();
                                Label1.Text = itemQuestion11Answers.answer0.ToString();
                            }
                        }
                        #endregion
                        #region Question12
                        foreach (var itemQuestion12 in itemQuestions.question12)
                        {
                            Label1.Text = itemQuestion12.QuestionIntroduction.ToString();
                            Label1.Text = itemQuestion12.QuestionText.ToString();

                            foreach (var itemQuestion12Answers in itemQuestion12.Answers)
                            {
                                Label1.Text = itemQuestion12Answers.answer4.ToString();
                                Label1.Text = itemQuestion12Answers.answer3.ToString();
                                Label1.Text = itemQuestion12Answers.answer2.ToString();
                                Label1.Text = itemQuestion12Answers.answer1.ToString();
                                Label1.Text = itemQuestion12Answers.answer0.ToString();
                            }
                        }
                        #endregion
                    }

                    foreach (var itemScores in item.Scores)
                    {
                        Label1.Text = itemScores.MetricScore.ToString();

                        #region ScoreA
                        foreach (var itemScoreA in itemScores.scoreA)
                        {
                            Label1.Text = itemScoreA.Min.ToString();
                            Label1.Text = itemScoreA.Max.ToString();
                            Label1.Text = itemScoreA.Message.ToString();
                        }
                        #endregion
                        #region ScoreB
                        foreach (var itemScoreB in itemScores.scoreB)
                        {
                            Label1.Text = itemScoreB.Min.ToString();
                            Label1.Text = itemScoreB.Max.ToString();
                            Label1.Text = itemScoreB.Message.ToString();
                        }
                        #endregion
                        #region ScoreC
                        foreach (var itemScoreC in itemScores.scoreC)
                        {
                            Label1.Text = itemScoreC.Min.ToString();
                            Label1.Text = itemScoreC.Max.ToString();
                            Label1.Text = itemScoreC.Message.ToString();
                        }
                        #endregion
                        #region ScoreD
                        foreach (var itemScoreD in itemScores.scoreD)
                        {
                            Label1.Text = itemScoreD.Min.ToString();
                            Label1.Text = itemScoreD.Max.ToString();
                            Label1.Text = itemScoreD.Message.ToString();
                        }
                        #endregion

                        foreach (var itemQuestionsDomains in itemScores.QuestionsDomains)
                        {
                            #region PainDomain
                            foreach (var itemPainDomain in itemQuestionsDomains.PainDomain)
                            {
                                Label1.Text = itemPainDomain.MaximumScoreDomain.ToString();
                                Label1.Text = itemPainDomain.NumberQuestionA.ToString();
                                Label1.Text = itemPainDomain.NumberQuestionB.ToString();
                                Label1.Text = itemPainDomain.NumberQuestionC.ToString();
                                Label1.Text = itemPainDomain.NumberQuestionD.ToString();
                            }
                            #endregion
                            #region ElbowFunction
                            foreach (var itemElbowFunction in itemQuestionsDomains.ElbowFunction)
                            {
                                Label1.Text = itemElbowFunction.MaximumScoreDomain.ToString();
                                Label1.Text = itemElbowFunction.NumberQuestionA.ToString();
                                Label1.Text = itemElbowFunction.NumberQuestionB.ToString();
                                Label1.Text = itemElbowFunction.NumberQuestionC.ToString();
                                Label1.Text = itemElbowFunction.NumberQuestionD.ToString();
                            }
                            #endregion
                            #region SocialPsychologicalDomain
                            foreach (var itemSocialPsychological in itemQuestionsDomains.SocialPsychologicalDomain)
                            {
                                Label1.Text = itemSocialPsychological.MaximumScoreDomain.ToString();
                                Label1.Text = itemSocialPsychological.NumberQuestionA.ToString();
                                Label1.Text = itemSocialPsychological.NumberQuestionB.ToString();
                                Label1.Text = itemSocialPsychological.NumberQuestionC.ToString();
                                Label1.Text = itemSocialPsychological.NumberQuestionD.ToString();
                            }
                            #endregion
                        }

                    }
                }
 
                // ********************** DATA OUTPUT ****************************
                // Label1.Text is the replacement of the final output of the data

            }
            #endregion // ESTO VA EN BUSINESS LOGIC
        }
    }
}");

            // Cerramos Escritura
            writer.Close();
        }
        
        /*
        protected void Button2_Click(object sender, EventArgs e)
        {
            // Creamos el Archivos y escribimos cabecera y datos del Cuestionario
            if (TextBox1.Text != "" && TextBox2.Text != "" && TextBox3.Text != "")
            {
                //Guardamos
                FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_"+TextBox1.Text+".json", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);

                //Apertura
                writer.WriteLine("{ \"data\":[{");

                //Escribimos los datos
                writer.WriteLine(" \"Name\": \" " + TextBox1.Text + " \", ",true);
                writer.WriteLine(" \"ExtendedName\": \" " + TextBox2.Text + " \", ", true);
                writer.WriteLine(" \"Concept\": \" " + TextBox3.Text + " \", ", true);

                // Cerramos Escritura
                writer.Close();

                //Guardamos los valores para su posterior utilización
                Session["Name"] = TextBox1.Text;
                Session["ExtendedName"] = TextBox2.Text;

                // Mostramos el siguiente paso
                Panel2.Visible = true;
                Panel1.Visible = false;
            }
            
        }
        public int ints;
        protected void Button3_Click(object sender, EventArgs e)
        {

            string[] ListItems = new string[ListBox3.Items.Count];
            if (TextBox4.Text != "" && TextBox5.Text != "")
            {
                // Guardamos
                FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);

                    if (ListBox3.Items.Count > 0)
                    {
                        writer.WriteLine(" \"Sections \" : [ { ");

                        string[] items = new string[ListBox3.Items.Count];

                        for (int i = 0; i < ListBox3.Items.Count; i++)
                        {
                            ints++;
                            items[i] = ListBox3.Items[i].ToString();

                        }

                        int count_items = items.Count();
                        int afg = 0;

                        foreach (string a in items)
                        {
                            afg++;   
                            string[] b = a.Split(',');


                            writer.WriteLine(" \"Section" + afg.ToString() + " \": [ ", true);
                            writer.WriteLine("{");
                            writer.WriteLine(" \"NameSection \": \" " + b[0] + " \",", true);
                            writer.WriteLine(" \"InstructionsSection \": \" " + b[1] + " \" ", true);
                            if (afg <= items.Count() - 2)
                            {
                                writer.WriteLine("}");
                                writer.WriteLine("],");
                            }

                            if (afg >= items.Count())
                            {
                                writer.WriteLine("}");
                                writer.WriteLine("]");
                            }

                            

                        }
                   
                        // Guardo el array en una sesion
                        Session["Sections"] = ListItems;

                        writer.WriteLine("}],");
                    }
                    else
                    {
                        //Escribimos los datos
                        writer.WriteLine(" \"Section1\": \" " + TextBox4.Text + " \", ", true);
                        writer.WriteLine(" \"InstructionsSection1\": \" " + TextBox5.Text + " \", ", true);

                        // Grabo los items en un array
                        ListItems[0] = TextBox4.Text;
                        ListItems[1] = TextBox5.Text;

                        // Guardo los datos del array en una sesion
                        Session["Sections"] = ListItems;
                    }
                    seccion = TextBox4.Text;
                    DropDownList2.Items.Add(seccion);
                    // Cerramos Escritura
                    writer.Close();

                    // Mostramos el siguiente paso
                    Panel3.Visible = true;
                    Panel2.Visible = false;
                    Panel1.Visible = false;
                }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            //DropDownList4.Items.Add(TextBox6.Text);

            // Guardamos
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            if (ListBox3.Items.Count > 0)
            {
                writer.WriteLine(" \"Sections \" : [ { ");

                string[] items = new string[ListBox3.Items.Count];

                for (int i = 0; i < ListBox3.Items.Count; i++)
                {
                    ints++;
                    items[i] = ListBox3.Items[i].ToString();

                }

                int count_items = items.Count();
                int afg = 0;

                foreach (string a in items)
                {
                    afg++;
                    string[] b = a.Split(',');


                    writer.WriteLine(" \"Section" + afg.ToString() + " \": [ ", true);
                    writer.WriteLine("{");
                    writer.WriteLine(" \"NameSection \": \" " + b[0] + " \",", true);
                    writer.WriteLine(" \"InstructionsSection \": \" " + b[1] + " \" ", true);
                    if (afg <= items.Count() - 2)
                    {
                        writer.WriteLine("}");
                        writer.WriteLine("],");
                    }

                    if (afg >= items.Count())
                    {
                        writer.WriteLine("}");
                        writer.WriteLine("]");
                    }



                }

                // Guardo el array en una sesion
                Session["Sections"] = ListItems;

                writer.WriteLine("}],");
            }
            else
            {
                //Escribimos los datos
                writer.WriteLine(" \"Section1\": \" " + TextBox4.Text + " \", ", true);
                writer.WriteLine(" \"InstructionsSection1\": \" " + TextBox5.Text + " \", ", true);

                // Grabo los items en un array
                ListItems[0] = TextBox4.Text;
                ListItems[1] = TextBox5.Text;

                // Guardo los datos del array en una sesion
                Session["Sections"] = ListItems;
            }
            writer.Close();

            Panel4.Visible = true;
            Panel3.Visible = false;
            Panel2.Visible = false;
            Panel1.Visible = false;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(" \"Metric Score\": \" " + TextBox8.Text + " \",",true);
            writer.Close();

            Panel5.Visible = true;
            Panel4.Visible = false;
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(" \"Minimum Score\": \" " + TextBox9.Text + " \",", true);
            writer.WriteLine(" \"Maximum Score\": \" " + TextBox10.Text + " \",", true);
            writer.WriteLine(" \"Message \": \" " + TextBox11.Text + " \",", true);
            writer.Close();

            Panel6.Visible = true;
            Panel5.Visible = false;
        }

        protected void Button9_Click(object sender, EventArgs e)
        {

            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(" \"Domain Name\": \" " + TextBox12.Text + " \",", true);
            writer.WriteLine(" \"Maximum Score Domain\": \" " + TextBox13.Text + " \",", true);
            writer.Close();
            Panel7.Visible = true;
            Panel6.Visible = false;
            DropDownList3.Items.Add(TextBox12.Text);

        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(" \"Domain Name\": \" " + TextBox12.Text + " \",", true);
            writer.WriteLine(" \"Maximum Score Domain\": \" " + TextBox13.Text + " \",", true);
            writer.Close();

            Panel8.Visible = true;
            Panel7.Visible = false;
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            FileStream stream = new FileStream("c:\\Users\\August\\Documents\\CUESTIONARIO_" + Session["Name"] + ".json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(" \"Footer\": \" " + TextBox14.Text + " \",", true);
            
            // Save finish
            //Guardamos

            // cerrar corchetes finales

            writer.WriteLine("}");
            writer.WriteLine("]");
            writer.WriteLine("}");
            writer.Close();            //writer.WriteLine("}] }");
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            Panel8.Visible = false;
            if (TextBox4.Text != "" && TextBox5.Text != "")
            {
                ListBox3.Items.Add(TextBox4.Text+","+TextBox5.Text);
                DropDownList2.Items.Add(TextBox4.Text);
            }

            
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            //Save & add More sections
            Panel2.Visible = true;
            Panel3.Visible = false;
        }

        void ListBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            //e.ToString();
            //Remueve item sobre el que se hace doble click
            int[] id_item = ListBox1.GetSelectedIndices();
            ListBox1.Items.RemoveAt(id_item[0]);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedItem.Value == "text")
            {
                if (TextBox6.Text != "")
                    ListBox1.Items.Add(TextBox7.Text); // Pregunta o Enunciado
            }
            else
            {
                if (TextBox6.Text != "" && TextBox7.Text != "")
                    ListBox1.Items.Add(TextBox7.Text + "," + TextBox8.Text); // Respuesta (si es pregunta..)
            }
            
            
        }

       */
    }
}