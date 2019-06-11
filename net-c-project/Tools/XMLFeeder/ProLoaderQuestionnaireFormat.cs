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
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Instructions;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using DSPrima.WcfUserSession.Model;

namespace ProXmlFeeder
{
    public class ProLoaderQuestionnaireFormat
    {
        
        public string resultsave;
        private UserClient userClient;
        private ProClient proClient;
        private QuestionnaireClient questionnaireClient;
        private  QuestionnaireFormatClient questionnaireFormatClient;
        public static QuestionnaireFormatClient QuesFormatClient= new QuestionnaireFormatClient();
        private PatientEpisodeClient userQuestionnaireClient;
        private PatientClient patientClient;
        public static Format pro;
        public ContainerFormatDefinition ContainerFormatDefinition;

        public ProLoaderQuestionnaireFormat()
        {
            this.userClient = new UserClient();
            this.userClient.Login("admin", "Welc0me!");
            this.userClient.UserHasMultipleRoles("admin");
            this.userClient.SelectRole("admin", "Administrator");

            RequestHeader header = new RequestHeader();
            header.SessionId = this.userClient.GetConfiguration.SessionId;

            this.proClient = new ProClient();
            this.questionnaireClient = new QuestionnaireClient();
            this.questionnaireFormatClient = new QuestionnaireFormatClient();
            this.userQuestionnaireClient = new PatientEpisodeClient();
            this.patientClient = new PatientClient();

            this.questionnaireClient = new QuestionnaireClient();
            this.questionnaireClient.RequestHeader = header;

            this.questionnaireFormatClient = new QuestionnaireFormatClient();
            this.questionnaireFormatClient.RequestHeader = header;

            this.userQuestionnaireClient = new PatientEpisodeClient();
            this.userQuestionnaireClient.RequestHeader = header;

        }

        public static string Error;

        public static Format Load(XmlElement root)
        {

           pro = new Format();

           LoadProFormat(root, ref pro);
           LoadContainer(root, ref pro);
            return pro;
        }

        public void SaveQuestionnaireFormatFull()
        {

            try
            {
                questionnaireFormatClient.AddOrUpdateFullFormat(pro);
                resultsave = "Saved";
            }
            catch (Exception exception)
            {
                resultsave = "Error";
                Form1.Print("Cann't save: \n " + exception.ToString());
                logReport.returnError("Cann't save: \n " + exception.ToString());

            }

        }
        private static string GetNodeValue(XmlElement node, string PropName)
        {
            try
            {
                return node.GetElementsByTagName(PropName)[0].InnerText;
            }
            catch(Exception exc){
                Form1.Print("The node "+PropName+" isn't in the File:\n "+ exc);
                logReport.returnError("The node " + PropName + " isn't in the File:\n " + exc.ToString());
                return "0";
            }
        }
        private static void LoadProFormat(XmlElement root, ref Format pro)
        {
            pro.Name = GetNodeValue(root, "FormatName");
            switch (GetNodeValue(root, "PlatformName"))
            {
                case "Classic":
                    pro.SupportedPlatform = Platform.Classic;
                    break;

                case "Chat":
                    pro.SupportedPlatform = Platform.Chat;
                    break;

                case "Mobile":
                    pro.SupportedPlatform = Platform.Mobile;
                    break;
            }

        }
        private static void LoadContainer(XmlElement root, ref Format pro)
        {
            FormatContainer proContainer = new FormatContainer();
            foreach (XmlElement e in root.GetElementsByTagName("ContainerFormatDefinition")){
                pro.Containers.Add(proContainer);
                    proContainer.ContainerFormatDefinition = proContainer.ContainerFormatDefinition = new ContainerFormatDefinition(){ 
                        ContainerDefinitionName = e.Attributes["ContainerDefinitionName"].Value
                    };
                LoadTextFormatDefinition(e, ref proContainer);
                LoadItemsFormatDefinition(e, ref proContainer);
            } 
                
        }

        public static void LoadTextFormatDefinition(XmlElement root, ref  FormatContainer proContainer)
        {
            TextFormatContainer intro = new TextFormatContainer();
            foreach (XmlElement TextFormatDefinition in root.GetElementsByTagName("TextFormatDefinition"))
            {

                
                intro.TextFormatDefinition = new TextFormatDefinition() { ElementFormatDefinitionName = TextFormatDefinition.Attributes["ElementFormatDefinitionName"].Value };
                foreach (XmlElement QuestionnaireElementFormatDefinition in TextFormatDefinition.GetElementsByTagName("QuestionnaireElementFormatDefinition"))
                {
                    string OrderInSectionString = QuestionnaireElementFormatDefinition.Attributes["OrderInSection"].Value;

                    int num1;
                    bool res = int.TryParse(OrderInSectionString, out num1);
                    if (res == false)
                    {
                        Form1.Print("The OrderInSection: " + OrderInSectionString + " need to be a number");
                    }
                    else
                    {
                        int OrderInSectionInt = int.Parse(OrderInSectionString);

                        intro.Elements.Add(new FormatContainerElement()
                        {
                            OrderInSection = OrderInSectionInt,
                            QuestionnaireElementActionId = (QuestionnaireElementFormatDefinition.Attributes["QuestionnaireElementActionId"]).Value
                        });
                    }
                }
            proContainer.Children.Add(intro);

            }
        }     
        public static void LoadItemsFormatDefinition(XmlElement root, ref FormatContainer proContainer)
        {
            ItemFormatContainer items = new ItemFormatContainer();
            foreach(XmlElement ItemsFormatDefinition in root.GetElementsByTagName("ItemsFormatDefinition")){
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = ItemsFormatDefinition.Attributes["ElementFormatDefinitionName"].Value };
                foreach(XmlElement ItemGroupFormat in ItemsFormatDefinition.GetElementsByTagName("ItemGroupFormat")){

                    switch (ItemGroupFormat.Attributes["GroupOptionDefinitionName"].Value)
                            {
                                case "LikertHorizontalRadio":
                                    items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
                                    break;
                                case "BodyControl":
                                    items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "BodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
                                    break;
                                default: break;

                            }
                     
                     foreach (XmlElement QuestionnaireElementFormatDefinition in ItemGroupFormat.GetElementsByTagName("QuestionnaireElementFormatDefinition")){
                    string OrderInSectionString = QuestionnaireElementFormatDefinition.Attributes["OrderInSection"].Value;

                    int num1;
                    bool res = int.TryParse(OrderInSectionString, out num1);
                    if (res == false)
                    {
                        Form1.Print("The OrderInSection: " + OrderInSectionString + " need to be a number");
                    }
                    else
                    {
                        int OrderInSectionInt = int.Parse(OrderInSectionString);

                        items.Elements.Add(new FormatContainerElement()
                        {

                            OrderInSection = OrderInSectionInt,
                            QuestionnaireElementActionId = (QuestionnaireElementFormatDefinition.Attributes["QuestionnaireElementActionId"]).Value
                        });

                    }
                }
                

                }
            }
            
            proContainer.Children.Add(items);
        }
    }
}
