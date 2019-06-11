using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.Model.Users;
using PCHI.Model.Tag;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Instructions;
using DSPrima.WcfUserSession.Model;

namespace ProXmlFeeder
{

    public class ProLoader
    {
        public string resultsave;
        private UserClient userClient;
        private ProClient proClient;
        private QuestionnaireClient questionnaireClient;
        private QuestionnaireFormatClient questionnaireFormatClient;
        private PatientEpisodeClient userQuestionnaireClient;
        private PatientClient patientClient;
        public static ProInstrument pro;
        public List<string> OptionalQuestionnaireXMLItems = new List<string>();


        public ProLoader()
        {
            this.userClient = new UserClient();
            this.userClient.Login("admin", "Welc0me!");
            this.userClient.UserHasMultipleRoles("admin");
            this.userClient.SelectRole("admin", "Administrator");

            RequestHeader header = new RequestHeader();
            header.SessionId = this.userClient.GetConfiguration.SessionId;

            this.proClient = new ProClient();
            this.proClient.RequestHeader = header;

            this.questionnaireClient = new QuestionnaireClient();
            this.questionnaireClient.RequestHeader = header;

            this.questionnaireFormatClient = new QuestionnaireFormatClient();
            this.questionnaireFormatClient.RequestHeader = header;

            this.userQuestionnaireClient = new PatientEpisodeClient();
            this.userQuestionnaireClient.RequestHeader = header;

            this.patientClient = new PatientClient();
            this.patientClient.RequestHeader = header;

            OptionalQuestionnaireXMLItems.Add("IntroductionMessage");
        }

        public static string Error;
        public static ProInstrument Load(XmlElement root)
        {

            pro = new ProInstrument();

            LoadProProperties(root, ref pro);
            LoadSections(root, ref pro);
            LoadDomains(root, ref pro);
            LoadTags(root, ref pro);
            LoadIntroductionMessage(root, ref pro);

            return pro;
        }

        private static void LoadIntroductionMessage(XmlElement root, ref ProInstrument pro)
        {
            XmlElement elementIntroductionMessages = (XmlElement)root.GetElementsByTagName("IntroductionMessages")[0];
            foreach (XmlElement elementTextVersion in elementIntroductionMessages.GetElementsByTagName("TextVersion"))
            {
                QuestionnaireIntroductionMessage intro = new QuestionnaireIntroductionMessage();
                intro.Text = GetNodeValue(elementTextVersion,"Text");

                switch (GetNodeValue(elementTextVersion, "Audience"))
                {
                    case "Patient":
                        intro.Audience = UserTypes.Patient;
                        break;
                    case "Patient,Physician":
                        intro.Audience = UserTypes.Patient | UserTypes.Physician;
                        break;
                    case "Patient,Researcher":
                        intro.Audience = UserTypes.Patient | UserTypes.Researcher;
                        break;
                    case "Patient,Physician,Researcher":
                        intro.Audience = UserTypes.Patient | UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Physician":
                        intro.Audience = UserTypes.Physician;
                        break;
                    case "Physician,Researcher":
                        intro.Audience = UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Researcher":
                        intro.Audience = UserTypes.Researcher;
                        break;
                    default:
                        break;
                }
                switch (GetNodeValue(elementTextVersion, "SupportedInstances"))
                {
                    case "Baseline":
                        intro.SetSupportedInstances(Instance.Baseline);
                        break;
                    case "Baseline,FollowUp":
                        intro.SetSupportedInstances(Instance.Baseline|Instance.Followup);
                        break;
                    case "FollowUp":
                        intro.SetSupportedInstances(Instance.Followup);
                        break;
                    default:
                        break;
                }

                switch (GetNodeValue(elementTextVersion, "SupportedPlatforms"))
                {
                    case "Chat":
                        intro.SetSupportedPlatforms(Platform.Chat);
                        break;
                    case "Chat,Classic":
                        intro.SetSupportedPlatforms(Platform.Chat|Platform.Classic);
                        break;
                    case "Chat,Mobile":
                        intro.SetSupportedPlatforms(Platform.Chat | Platform.Mobile);
                        break;
                    case "Chat,Classic,Mobile":
                        intro.SetSupportedPlatforms(Platform.Chat | Platform.Classic|Platform.Mobile);
                        break;
                    case "Classic":
                        intro.SetSupportedPlatforms(Platform.Classic);
                        break;
                    case "Classic,Mobile":
                        intro.SetSupportedPlatforms(Platform.Classic | Platform.Mobile);
                        break;
                    case "Mobile":
                        intro.SetSupportedPlatforms(Platform.Mobile);
                        break;
                    default:
                        break;
                }
                pro.IntroductionMessages.Add(intro);
            }

        }
        public bool GetOptionalElement(string element)
        {
            try
            {
                string resultFind = OptionalQuestionnaireXMLItems.Find(
                    delegate(string current)
                    {
                        return current.Contains(element);
                    }
                );

                if (resultFind != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void SaveQuestionnaireFull()
        {
            try
            {
                questionnaireClient.SaveFullQuestionnaire(pro);
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

        private static string GetNodeValue(XmlElement node, string PropName)
        {
            try
            {
                /*if (GetOptionalElement(PropName) == false)
                {*/
                return node.GetElementsByTagName(PropName)[0].InnerText;
                /*}
                return "";*/
            }
            catch (Exception exc)
            {
                Form1.Print("The node " + PropName + " isn't in the File:\n " + exc.ToString());
                logReport.returnError("The node " + PropName + " isn't in the File:\n " + exc.ToString());
                return "0";
            }
        }

        private static void LoadProProperties(XmlElement root, ref ProInstrument pro)
        {
            pro.Name = GetNodeValue(root, "ShortName");
            pro.DisplayName = GetNodeValue(root, "DisplayName");
            pro.DefaultFormatName = GetNodeValue(root, "DefaultFormatName");
            pro.Concept = new QuestionnaireConcept();

            XmlElement con = (XmlElement)root.GetElementsByTagName("Concept")[0];
            pro.Concept.Name = GetNodeValue(con, "Name");
            pro.Concept.Description = GetNodeValue(con, "Description");
            pro.IsActive = true;

            XmlElement descon = (XmlElement)root.GetElementsByTagName("Descriptions")[0];
            foreach (XmlElement descriptionsTextVersion in descon.GetElementsByTagName("TextVersion"))
            {
                QuestionnaireDescription des = new QuestionnaireDescription();
                des.Text = GetNodeValue(descriptionsTextVersion,"Text");
                switch (GetNodeValue(descriptionsTextVersion, "Audience"))
                {
                    case "Patient":
                        des.Audience = UserTypes.Patient;
                        break;
                    case "Patient,Physician":
                        des.Audience = UserTypes.Patient | UserTypes.Physician;
                        break;
                    case "Patient,Researcher":
                        des.Audience = UserTypes.Patient | UserTypes.Researcher;
                        break;
                    case "Patient,Physician,Researcher":
                        des.Audience = UserTypes.Patient | UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Physician":
                        des.Audience = UserTypes.Physician;
                        break;
                    case "Physician,Researcher":
                        des.Audience = UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Researcher":
                        des.Audience = UserTypes.Researcher;
                        break;
                    default:
                        break;
                }
                switch (GetNodeValue(descriptionsTextVersion, "SupportedInstances"))
                {
                    case "Baseline":
                        des.SetSupportedInstances(Instance.Baseline);
                        break;
                    case "Baseline,FollowUp":
                        des.SetSupportedInstances(Instance.Baseline | Instance.Followup);
                        break;
                    case "FollowUp":
                        des.SetSupportedInstances(Instance.Followup);
                        break;
                    default:
                        break;
                }

                switch (GetNodeValue(descriptionsTextVersion, "SupportedPlatforms"))
                {
                    case "Chat":
                        des.SetSupportedPlatforms(Platform.Chat);
                        break;
                    case "Chat,Classic":
                        des.SetSupportedPlatforms(Platform.Chat | Platform.Classic);
                        break;
                    case "Chat,Mobile":
                        des.SetSupportedPlatforms(Platform.Chat | Platform.Mobile);
                        break;
                    case "Chat,Classic,Mobile":
                        des.SetSupportedPlatforms(Platform.Chat | Platform.Classic | Platform.Mobile);
                        break;
                    case "Classic":
                        des.SetSupportedPlatforms(Platform.Classic);
                        break;
                    case "Classic,Mobile":
                        des.SetSupportedPlatforms(Platform.Classic | Platform.Mobile);
                        break;
                    case "Mobile":
                        des.SetSupportedPlatforms(Platform.Mobile);
                        break;
                    default:
                        break;
                }
              pro.Descriptions.Add(des);
            }
        }

        private static void LoadSections(XmlElement root, ref ProInstrument pro)
        {
            foreach (XmlElement s in root.GetElementsByTagName("Section"))
            {
                QuestionnaireSection sec = new QuestionnaireSection();
                sec.ActionId = GetNodeValue(s, "ActionId");

                LoadSectionElements(s, ref sec);
                LoadSectionInstructions(s, ref sec);

                pro.Sections.Add(sec);
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
            item.ScoringNote = GetNodeValue(e, "ScoringNote");
            if (GetNodeValue(e, "BestScore") == "Highest") {
                item.HigherIsBetter = true;
            } else {
                item.HigherIsBetter = false;
            }
            try
            {
                item.SummaryText = GetNodeValue(e, "Summary");

            }
            catch (Exception exp)
            {

                Error = "The summary: \n doesn't exist";
                Form1.Print("The summary:  doesn't exist" + exp.ToString());
                logReport.returnError("The summary:  doesn't exist" + exp.ToString());

            }
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
                opt.Action = GetNodeValue(o, "Action");
                opt.DisplayId = GetNodeValue(o, "DisplayId");
                opt.ActionId = GetNodeValue(o, "ActionID");
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

        private static void LoadDomains(XmlElement root, ref ProInstrument pro)
        {
            foreach (XmlElement d in root.GetElementsByTagName("Domain"))
            {
                ProDomain dom = new ProDomain();
                dom.Name = GetNodeValue(d,"Name");
                dom.Description = GetNodeValue(d, "Description");
                dom.ScoreFormula = GetNodeValue(d, "ScoreFormula");
                dom.ScoringNote = GetNodeValue(d, "ScoringNote");
                if (GetNodeValue(d, "BestScore") == "Highest")
                {
                    dom.HigherIsBetter = true;
                } else {
                    dom.HigherIsBetter = false;
                }
                switch (GetNodeValue(d, "Audience"))
                {
                    case "Patient":
                        dom.Audience = UserTypes.Patient;
                        break;
                    case "Patient,Physician":
                        dom.Audience = UserTypes.Patient | UserTypes.Physician;
                        break;
                    case "Patient,Researcher":
                        dom.Audience = UserTypes.Patient | UserTypes.Researcher;
                        break;
                    case "Patient,Physician,Researcher":
                        dom.Audience = UserTypes.Patient | UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Physician":
                        dom.Audience = UserTypes.Physician;
                        break;
                    case "Physician,Researcher":
                        dom.Audience = UserTypes.Physician | UserTypes.Researcher;
                        break;
                    case "Researcher":
                        dom.Audience = UserTypes.Researcher;
                        break;
                    default:
                        break;
                }
                ProDomainResultRange result = new ProDomainResultRange();
                result.Start = double.Parse(GetNodeValue(d,"Start"));
                result.End = double.Parse(GetNodeValue(d, "End"));
                result.Meaning = GetNodeValue(d, "Meaning");
                dom.ResultRanges.Add(result);

                pro.Domains.Add(dom);
            }
        }

        private static void LoadTags(XmlElement root, ref ProInstrument pro)
        {
            foreach (XmlElement t in root.GetElementsByTagName("Tag"))
            {
                Tag tag = new Tag();
                tag.TagName = GetNodeValue(t, "Name");
                tag.Value = GetNodeValue(t, "Value");
                pro.Tags.Add(tag);
            }
        }

    }
}
