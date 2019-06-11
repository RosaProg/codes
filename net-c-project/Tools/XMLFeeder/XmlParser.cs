using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Survey;

using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.Model.Questionnaire.Styling.Presentation;
using DSPrima.WcfUserSession.Model;

namespace ProXmlFeeder
{
    
    public class XmlParser
    {
        public static string LogDir;
        public string resultsave;
        private ProClient proClient;
        private QuestionnaireClient questionnaireClient;
        private Format questionnaireFormat;
        private QuestionnaireFormatClient questionnaireFormatClient;
        public string Error;
        private XmlDocument _xml = null;
        private Questionnaire _questionnaire = null;
        private Format _questionnaireFormat = null;

        public string FileName { get; set; }

        public Questionnaire Questionnaire
        {
            get { return _questionnaire; }
        }

        public XmlParser(string fileName)
        {

            this.FileName = fileName;

            _xml = new XmlDocument();

            _xml.Load(fileName);
           
        }

        public Format LoadQuestionnaireFormat()
        {
            XmlElement root = (XmlElement)_xml.GetElementsByTagName("QuestionnaireFormat")[0];

            _questionnaireFormat = ProLoaderQuestionnaireFormat.Load(root);

            return _questionnaireFormat;
        }

        public Questionnaire LoadQuestionnaire()
        {
            Survey pro = new Survey();
       
            _questionnaire = null;
           
            XmlElement root = (XmlElement) _xml.GetElementsByTagName("Questionnaire")[0];
            switch(root.Attributes["type"].Value.ToLower())
            {
                case "proinstrument":                
                _questionnaire = ProLoader.Load(root);
                break;

                case "survey":          
                _questionnaire = SurveyLoader.Load(root);
                break;

                default:
                break;
            }
          
            return _questionnaire;
        }

        private Survey LoadSurvey(XmlNode root)
        {
            Survey survey = new Survey();

            return survey;
        }

        public override string  ToString()
        {
            try
            {
                if (_questionnaire != null)
                {
                    Type T = _questionnaire.GetType();
                    XmlObjectSerializer serializer = new DataContractSerializer(T, T.ToString(), "", null, 0x7FFF, false, true, null);
                    StringWriter sww = new StringWriter();
                    XmlWriter writer = XmlWriter.Create(sww);
                    serializer.WriteStartObject(writer, _questionnaire);
                    serializer.WriteObjectContent(writer, _questionnaire);
                    serializer.WriteEndObject(writer);
                    writer.Flush();
                    return sww.ToString();
                    // Your xml
                }
                else
                {

                    Type T = _questionnaireFormat.GetType();
                    XmlObjectSerializer serializer = new DataContractSerializer(T, T.ToString(), "", null, 0x7FFF, false, true, null);
                    StringWriter sww = new StringWriter();
                    XmlWriter writer = XmlWriter.Create(sww);
                    serializer.WriteStartObject(writer, _questionnaireFormat);
                    serializer.WriteObjectContent(writer, _questionnaireFormat);
                    serializer.WriteEndObject(writer);
                    writer.Flush();
                    return sww.ToString();
                }
            }catch(Exception exc){

                Form1.Print("This file is not supported");
                return "0";
            }
        }
    }
}
