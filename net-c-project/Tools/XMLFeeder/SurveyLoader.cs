using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;

using PCHI.Model.Tag;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Survey;
using PCHI.Model.Questionnaire.Instructions;
using DSPrima.WcfUserSession.Model;

namespace ProXmlFeeder
{
    class SurveyLoader
    {
        
        public string resultsave;
        private UserClient userClient;
        private QuestionnaireClient questionnaireClient;
        private QuestionnaireFormatClient questionnaireFormatClient;
        private PatientEpisodeClient userQuestionnaireClient;
        private PatientClient patientClient;
        public static Survey surv;

        public SurveyLoader()
        {
            this.userClient = new UserClient();
            this.userClient.Login("admin", "Welc0me!");
            this.userClient.UserHasMultipleRoles("admin");
            this.userClient.SelectRole("admin", "Administrator");

            RequestHeader header = new RequestHeader();
            header.SessionId = this.userClient.GetConfiguration.SessionId;

            this.questionnaireClient = new QuestionnaireClient();
            this.questionnaireClient.RequestHeader = header;

            this.questionnaireFormatClient = new QuestionnaireFormatClient();
            this.questionnaireFormatClient.RequestHeader = header;

            this.userQuestionnaireClient = new PatientEpisodeClient();
            this.userQuestionnaireClient.RequestHeader = header;

            this.patientClient = new PatientClient();
            this.patientClient.RequestHeader = header;
        }

        private static string GetNodeValue(XmlElement node, string PropName)
        {
            string returnString = "  ";
            try
            {
                returnString = node.GetElementsByTagName(PropName)[0].InnerText;

            }
            catch (Exception exp)
            {
                Form1.Print("The node can't be read:" + PropName + " \n");
                logReport.returnError("The node can't be read:"+PropName+" \n");
            }
            return returnString;
        }

        public static Survey Load(XmlElement root)
        {

            surv = new Survey();
           
                LoadSurveyProperties(root, ref surv);       
           
               LoadSections(root, ref surv);

                LoadTags(root, ref surv);
            

            return surv;
        }

        public void SaveQuestionnaireFull()
        {

            try
            {
                questionnaireClient.SaveFullQuestionnaire(surv);
                resultsave = "Saved";
            }
            catch (Exception exception)
            {
                //OperationResult result = new OperationResult(exception);
                resultsave = "Error";
                Form1.Print("Cann't save: \n " + exception.ToString());
                logReport.returnError("Cann't save: \n " + exception.ToString());

            }

            //OperationResult result =  questionnaireClient.SaveFullQuestionnaire(q);
        }

        private static void LoadSurveyProperties(XmlElement root, ref Survey surv)
        {
            surv.Name = GetNodeValue(root, "ShortName");
            surv.DefaultFormatName = GetNodeValue(root, "DefaultFormatName");
            surv.Concept = new QuestionnaireConcept();

            XmlElement con = (XmlElement)root.GetElementsByTagName("Concept")[0];
            surv.Concept.Name = GetNodeValue(con, "Name");
            surv.Concept.Description = GetNodeValue(con, "Description");

            surv.IsActive = true;
        }

        private static void LoadSections(XmlElement root, ref Survey surv)
        {
            foreach (XmlElement s in root.GetElementsByTagName("Section"))
            {
                QuestionnaireSection sec = new QuestionnaireSection();
                sec.ActionId = GetNodeValue(s, "ActionId");

                
                    LoadSectionElements(s, ref sec);
                    LoadSectionInstructions(s, ref sec);
                    surv.Sections.Add(sec);
            }
        }

        private static void LoadSectionElements(XmlElement s, ref QuestionnaireSection section)
        {
            foreach (XmlElement e in s.GetElementsByTagName("Element"))
            {
                if (e.Attributes["type"].Value == "Item")
                {
                      LoadItemIntoSection(e, ref section);
                }
                else
                {
                        LoadTextIntoSection(e, ref section);
                }
            }
        }

        private static void LoadItemIntoSection(XmlElement e, ref QuestionnaireSection section)
        {
            QuestionnaireItem item = new QuestionnaireItem();
           item.ActionId = GetNodeValue(e, "ActionId");
           item.DisplayId = GetNodeValue(e, "DisplayId");
           item.Attributes = QuestionnaireItemAttributes.CanSkip; //TODO

           item.SummaryText = GetNodeValue(e, "Summary");

            QuestionnaireElement x = item;
           LoadTextVersionsIntoElement(e, ref x);

            LoadOptionGroupsIntoItem(e, ref item);

            section.Elements.Add(item);
        }

        private static void LoadTextIntoSection(XmlElement e, ref QuestionnaireSection section)
        {
            QuestionnaireText text = new QuestionnaireText();
            text.ActionId = GetNodeValue(e, "ActionId");

            QuestionnaireElement x = text;
            LoadTextVersionsIntoElement(e, ref x);

            section.Elements.Add(text);
        }

        private static void LoadTextVersionsIntoElement(XmlElement e, ref QuestionnaireElement elem)
        {
            foreach (XmlElement t in e.GetElementsByTagName("TextVersion"))
            {
                QuestionnaireElementTextVersion txt = new QuestionnaireElementTextVersion();
                txt.SetSupportedInstances(Instance.Baseline, Instance.Followup); //TODO
                txt.SetSupportedPlatforms(Platform.Chat, Platform.Classic, Platform.Mobile);  //TODO
                txt.Text = GetNodeValue(t, "Text");

                elem.TextVersions.Add(txt);
            }
        }

        private static void LoadOptionGroupsIntoItem(XmlElement i, ref QuestionnaireItem item)
        {
         foreach (XmlElement og in i.GetElementsByTagName("OptionGroup"))
            {
                QuestionnaireItemOptionGroup opgrp = new QuestionnaireItemOptionGroup();
               LoadOptionGroup(og, ref opgrp);
            }
        }

        private static void LoadOptionGroup(XmlElement og, ref QuestionnaireItemOptionGroup optgrp)
        {
            string rtype = GetNodeValue(og, "ResponseType");
            switch (rtype)
            {
                case "ConditionalItem":
                  optgrp.ResponseType = QuestionnaireResponseType.ConditionalItem;
                    break;
                case "List":
                 optgrp.ResponseType = QuestionnaireResponseType.List;
                    break;
                case "MultiSelect":
                    optgrp.ResponseType = QuestionnaireResponseType.MultiSelect;
                    break;
                case "Range":
                    optgrp.ResponseType = QuestionnaireResponseType.Range;
                    break;
                case "Text":
                    optgrp.ResponseType = QuestionnaireResponseType.Text;
                    break;
            }

          optgrp.RangeStep = Convert.ToDouble(GetNodeValue(og, "RangeStep"));

          foreach (XmlElement o in og.GetElementsByTagName("Option"))
            {
                  QuestionnaireItemOption opt = new QuestionnaireItemOption();

                opt.Text = GetNodeValue(o, "Text");

                opt.Text = GetNodeValue(o, "Value");
                optgrp.Options.Add(opt);
            }

        }

        private static void LoadSectionInstructions(XmlElement s, ref QuestionnaireSection section)
        {
            foreach (XmlElement i in s.GetElementsByTagName("Instruction"))
            {
                QuestionnaireSectionInstruction inst = new QuestionnaireSectionInstruction();
                inst.SetSupportedInstances(Instance.Baseline, Instance.Followup); //TODO
                inst.SetSupportedPlatforms(Platform.Chat, Platform.Classic, Platform.Mobile);  //TODO
                inst.Text = GetNodeValue(i, "Text");

                section.Instructions.Add(inst);
            }
        }

        private static void LoadTags(XmlElement root, ref Survey surv)
        {
            foreach (XmlElement t in root.GetElementsByTagName("Tag"))
            {
                Tag tag = new Tag();

                surv.Tags.Add(tag);
            }
        }
        
    }
}
