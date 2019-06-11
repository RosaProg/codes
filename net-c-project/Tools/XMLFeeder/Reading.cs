using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProXmlFeeder
{
    class Reading
    { 
        public string readString;
        public string concatenarString;
        public  string ShortName = "";
        public string path = "C:\\Users\\August\\Documents\\OSS.xml";
        public string LongName = "";
        public string DefaultFormatName = "";
        public string MinNumberAnswers;
       public int per=0;
        public string concatenar(string escribir,string de) {
            concatenarString = concatenarString + "\n\n " + de + escribir;
        return concatenarString;

        }

       
        public string read() {
            #region LeerXML
            #region reader
                                 
            XmlDocument reader = new XmlDocument();
            try
            {
                reader.Load(path);
            }catch(Exception exc){
                string exception = exc.ToString();
                readString = concatenar(path,"No se encunetra el archivoXML en :  ");
            }
            XmlNodeList listaNodos = reader.SelectNodes("Questionnaire");
            XmlNode Questionnaire;
            #endregion
            
          


            if (listaNodos != null){ 
                for (int i = 0; i < listaNodos.Count; i++){ 
                    # region cabecera
                    // Questionnaire
                    Questionnaire = listaNodos.Item(i);
                    foreach (XmlElement lista in listaNodos)
                    {
                        string tipoQ = lista.GetAttribute("type");
                        readString = concatenar(tipoQ, "Questionnarie Type:  ");
                    }
                    ShortName = Questionnaire.SelectSingleNode("ShortName").InnerText;

                    LongName = Questionnaire.SelectSingleNode("LongName").InnerText;
                    DefaultFormatName = Questionnaire.SelectSingleNode("DefaultFormatName").InnerText;
                    MinNumberAnswers = Questionnaire.SelectSingleNode("MinNumberAnswers").InnerText;
                    readString = concatenar(ShortName,"Short Name:  ");
                    readString = concatenar(LongName,"Long Name:   ");
readString = concatenar(" Concepts:", "\n\n ");
 XmlNodeList Concept = Questionnaire.SelectSingleNode("Concept").SelectNodes("Name");
                    XmlNode Name;
                    if (Concept != null)
                    {
                        string ConceptName = "";
                        for (int f = 0; f < Concept.Count; f++)
                        {
                            Name = Concept.Item(f);
                            ConceptName = Name.InnerText;
                            readString = concatenar(ConceptName, "ConceptName:   ");
                        }
                    }
                    Concept = Questionnaire.SelectSingleNode("Concept").SelectNodes("Description");
                    XmlNode Description;
                    if (Concept != null)
                    {
                        string ConceptDescription = "";
                        for (int f = 0; f < Concept.Count; f++)
                        {
                            Description = Concept.Item(f);
                            ConceptDescription = Description.InnerText;
                            readString = concatenar(ConceptDescription,"Concept Description: ");

                        }
                    }
                    readString = concatenar("","\n\n");
                    readString = concatenar(DefaultFormatName,"DefaultFormatName:  ");
                    readString = concatenar(MinNumberAnswers,"MinNumberAnswers:   ");
                    
                    // Concepts
                   
                    readString = concatenar("Sections","\n\n");
                    #endregion

                    #region Sections

                    XmlNodeList lsSections = reader.SelectNodes("Questionnaire/Sections");
                    XmlNode itmSections;

                    if (lsSections != null){
                        
                        for (i = 0; i < lsSections.Count; i++){
                            itmSections = lsSections.Item(i);
                          
                            XmlNodeList lsSection = itmSections.SelectNodes("Section");
                            XmlNode itmSection;
                            if (lsSection != null)
                            {
                                string currentActionID;
                                for (int f = 0; f < lsSection.Count; f++)
                                {
                         
                                     readString = concatenar("", "--------------------------");
                                      readString = concatenar((f+1).ToString(),"Seccion Nº");
                                    itmSection = lsSection.Item(f);
                                    currentActionID = itmSection.SelectSingleNode("ActionId").InnerText;
                                   readString = concatenar(currentActionID,"->Action ID:   ");

                                    XmlNodeList lsElement = itmSection.SelectNodes("Elements");
                                    XmlNode itmElement;
                                    if(lsElement!=null){
                                        for (int h = 0; h < lsElement.Count;h++ )
                                        {
                                            
                                            itmElement = lsElement.Item(h);
                                           
                                     
                                            readString = concatenar((h+1).ToString(),"->Elemento Nº");
                                            
                                            XmlNodeList lsElemento = itmElement.SelectNodes("Element");
                                            XmlNode itmElemento;
                                            if (lsElemento != null)
                                            { 
  
                                             foreach (XmlElement lista2 in itmElement)
                                            {

                                                string tipo = lista2.GetAttribute("type");
                                                readString = concatenar(tipo,"->Elemento tipo:  ");
                                                per++;
                                            if(tipo=="Text"){
                                             string currentElementActionID;
                                                for (int h1 = 0; h1 < lsElemento.Count; h1++)
                                                {
                                                    
                                                    itmElemento = lsElemento.Item(h1);
                                                    currentElementActionID = itmElemento.SelectSingleNode("ActionId").InnerText;
                                                    readString = concatenar(currentElementActionID , "*Element Action ID:  ");
                                                    XmlNodeList lsTextVersions = itmElemento.SelectNodes("TextVersions");
                                                    XmlNode itmTextVersions;
                                                    for (int h2 = 0; h2 < lsTextVersions.Count;h2++) {

                                                        
                                                        readString = concatenar("\n", " **TextVersions");
                                                        itmTextVersions = lsTextVersions.Item(h2);
                                                        XmlNodeList lsTextVersion = itmTextVersions.SelectNodes("TextVersion");

                                                        XmlNode itmTextVersion;
                                                        for (int h3 = 0; h3 < lsTextVersion.Count;h3++)
                                                        {
                                                            readString = concatenar("\n", ""); 

                                                            readString = concatenar("", "*** TextVersion"); 
                                                            itmTextVersion = lsTextVersion.Item(h3);
                                                            //SupportedInstances
                                                            string SupportedInstances = itmTextVersion.SelectSingleNode("SupportedInstances").InnerText;
                                                            string SupportedPlatforms = itmTextVersion.SelectSingleNode("SupportedPlatforms").InnerText;
                                                            string Text = itmTextVersion.SelectSingleNode("Text").InnerText;
                                                            readString = concatenar(SupportedInstances,"    SupportedInstances:   ");
                                                            readString = concatenar(SupportedPlatforms, "    SupportedPlatforms:   ");
                                                            readString = concatenar(Text, "    Text:   ");
                                                        }
                                                    }
                                                }
                                            }
                                            if(tipo=="Item"){
                                                string currentElementActionID;
                                                string currentElementDisplayID;
                                                string currentElementAttributeID;
                                                for (int h1 = 0; h1 < lsElemento.Count; h1++)
                                                {

                                                    itmElemento = lsElemento.Item(h1);
                                                    currentElementActionID = itmElemento.SelectSingleNode("ActionId").InnerText;
                                                    currentElementDisplayID = itmElemento.SelectSingleNode("DisplayId").InnerText;
                                                    currentElementAttributeID = itmElemento.SelectSingleNode("Attributes").InnerText;

                                                    readString = concatenar(currentElementActionID, "*Element Action ID:  ");
                                                    readString = concatenar(currentElementDisplayID, "*Element Display ID:  ");
                                                    readString = concatenar(currentElementAttributeID, "*Element Attribute ID:  ");
                                                    XmlNodeList lsTextVersions = itmElemento.SelectNodes("TextVersions");
                                                    XmlNode itmTextVersions;
                                                    for (int h2 = 0; h2 < lsTextVersions.Count; h2++)
                                                    {
                                                        readString = concatenar("\n", " **TextVersions");
                                                        
                                                        readString = concatenar((h2 + 1).ToString(), " **TextVersions Nº");
                                                        itmTextVersions = lsTextVersions.Item(h2);
                                                        XmlNodeList lsTextVersion = itmTextVersions.SelectNodes("TextVersion");
                                                        XmlNode itmTextVersion;
                                                        for (int h3 = 0; h3 < lsTextVersion.Count; h3++)
                                                        {
                                                       
                                                            readString = concatenar("\n", ""); 
                                                            readString = concatenar((h3 + 1).ToString(), "*** TextVersion Nº");
                                                            itmTextVersion = lsTextVersion.Item(h3);
                                                            //SupportedInstances
                                                            string SupportedInstances = itmTextVersion.SelectSingleNode("SupportedInstances").InnerText;
                                                            string SupportedPlatforms = itmTextVersion.SelectSingleNode("SupportedPlatforms").InnerText;
                                                            string Text = itmTextVersion.SelectSingleNode("Text").InnerText;
                                                            readString = concatenar(SupportedInstances, "    SupportedInstances:   ");
                                                            readString = concatenar(SupportedPlatforms, "    SupportedPlatforms:   ");
                                                            readString = concatenar(Text, "    Text:   ");
                                                        }
                                                        XmlNodeList lsInstructionsE = itmElemento.SelectNodes("Instructions");
                                                        XmlNode itmInstructionsE;
                                                        for (int intInstructions = 0; intInstructions < lsInstructionsE.Count; intInstructions++)
                                                        {
                                                            itmInstructionsE = lsInstructionsE.Item(intInstructions);
                                                            XmlNodeList lsIntructionE = itmInstructionsE.SelectNodes("Instruction");
                                                            XmlNode itmInstructionE;
                                                            for (int intI = 0; intI < lsIntructionE.Count;intI++ ) {
                                                                itmInstructionE = lsIntructionE.Item(intI);
                                                                string SupportedInstances = itmInstructionE.SelectSingleNode("SupportedInstances").InnerText;
                                                                string SupportedPlatforms = itmInstructionE.SelectSingleNode("SupportedPlatforms").InnerText;
                                                                string Text = itmInstructionE.SelectSingleNode("Text").InnerText;
                                                                readString = concatenar(SupportedInstances,"Support Instances:   ");
                                                                readString = concatenar(SupportedPlatforms, "Support Plataforms:   ");
                                                                readString = concatenar(Text, "Text:   ");

                                                            }
                                                        }

                                                        XmlNodeList lsOptionsE = itmElemento.SelectNodes("OptionGroups");
                                                        XmlNode itmOptionsE;
                                                        for (int intI = 0; intI < lsOptionsE.Count; intI++)
                                                        {
                                                            itmOptionsE = lsOptionsE.Item(intI);
                                                            XmlNodeList lsOptionE = itmOptionsE.SelectNodes("OptionGroup");
                                                            XmlNode itmOptionE;
                                                            for (int intI2 = 0; intI2 < lsOptionE.Count; intI2++)
                                                            {
                                                                itmOptionE = lsOptionE.Item(intI2);

                                                                string ResponseType = itmOptionE.SelectSingleNode("ResponseType").InnerText;
                                                                 string RangeStep = itmOptionE.SelectSingleNode("RangeStep").InnerText;
                                                                 readString = concatenar(ResponseType, "Response Type:  ");
                                                                 readString = concatenar(RangeStep, "Range Step:  ");
                                                                 XmlNodeList lsTextVersionOption = itmOptionE.SelectNodes("TextVersion");
                                                                 XmlNode itmTextVersionOption;
                                                                 for (int h3 = 0; h3 < lsTextVersionOption.Count; h3++)
                                                                 {

                                                                     itmTextVersionOption = lsTextVersionOption.Item(h3);

                                                                     string SupportedInstancesOP = itmTextVersionOption.SelectSingleNode("SupportedInstances").InnerText;
                                                                     string SupportedPlatformsOP = itmTextVersionOption.SelectSingleNode("SupportedPlatforms").InnerText;
                                                                     string TextOP = itmTextVersionOption.SelectSingleNode("Text").InnerText;
                                                                     readString = concatenar(SupportedInstancesOP, "Supported Instances Option:  ");
                                                                     readString = concatenar(SupportedPlatformsOP, "Supported Plataforms Option:  ");
                                                                     readString = concatenar(TextOP, "Text Option:  ");
                                                                 }
                                                                 XmlNodeList lsOption = itmOptionE.SelectNodes("Options");
                                                                 XmlNode itmOption;
                                                                 for (int h3 = 0; h3 < lsOption.Count; h3++)
                                                                 {

                                                                     itmOption = lsOption.Item(h3);

                                                                     XmlNodeList lsOption2 = itmOptionE.SelectNodes("Option");
                                                                     XmlNode itmOption2;
                                                                     for (int h4 = 0; h4 < lsOption2.Count; h4++)
                                                                     {

                                                                         itmOption2 = lsOption2.Item(h4);

                                                                         string ActionOption = itmOption2.SelectSingleNode("Action").InnerText;
                                                                         string ActionOptionID = itmOption2.SelectSingleNode("ActionID").InnerText;
                                                                         string DisplayIdOption = itmOption2.SelectSingleNode("DisplayId").InnerText;
                                                                         string TextOption = itmOption2.SelectSingleNode("Text").InnerText;
                                                                         string ValueOption = itmOption2.SelectSingleNode("Value").InnerText;
                                                                         readString = concatenar(ActionOptionID, "ActionID Option:  ");
                                                                         readString = concatenar(ActionOption, "Action Option:  ");
                                                                         readString = concatenar(DisplayIdOption, "Display Id Option:  ");
                                                                         readString = concatenar(TextOption, "Text  Option:  ");
                                                                         readString = concatenar(ValueOption, "Value  Option:  ");
                                                                     }
                                                                 }

                                                            }
                                                        }
                                                    }
                                                } 


                                            }
                                            }
                                               
                                            }
                                        }
                                    }
                                    XmlNodeList lsIntructions = reader.SelectNodes("Questionnaire/Sections/Instructions"); ;
                                    XmlNode itmInstructions;
                                    for (int j = 0; j < lsIntructions.Count;j++ )
                                    {
                                        
                                        itmInstructions = lsIntructions.Item(j);
                                        XmlNodeList lsInstruction = itmInstructions.SelectNodes("Instruction");
                                        XmlNode itmInstruction;
                                        for (int j1 = 0; j1 < lsInstruction.Count; j1++)
                                        {readString = concatenar("","Instruction: ");
                                            itmInstruction = lsInstruction.Item(j1);
                                            string currentSupportedInstances = itmInstruction.SelectSingleNode("SupportedInstances").InnerText;
                                            string currentSupportedPlatforms = itmInstruction.SelectSingleNode("SupportedPlatforms").InnerText;
                                            string currentText = itmInstruction.SelectSingleNode("Text").InnerText;
                                            readString = concatenar(currentSupportedInstances, "SupportedInstances:");
                                            readString = concatenar(currentSupportedPlatforms, "currentSupportedPlatforms:");
                                            readString = concatenar(currentText, "currentText:");
                                        }
                                    }
                                }
                                    XmlNodeList lsDomains = reader.SelectNodes("Questionnaire/Domains");
                                    XmlNode itmDomains;
                                    for (int domain = 0; domain < lsDomains.Count;domain++ )
                                    {
                                        itmDomains = lsDomains.Item(domain);
                                        XmlNodeList lsDomain = itmDomains.SelectNodes("Domain");
                                        XmlNode itmDomain;
                                        for (int domain2 = 0; domain2 < lsDomain.Count;domain2++ )
                                        {
                                            itmDomain=lsDomain.Item(domain2);
                                            string DomainName = itmDomain.SelectSingleNode("Name").InnerText;
                                            string DomainDescription = itmDomain.SelectSingleNode("Description").InnerText;
                                            string DomainAudience = itmDomain.SelectSingleNode("Audience").InnerText;
                                            string DomainScoreFormula = itmDomain.SelectSingleNode("ScoreFormula").InnerText;
                                            readString = concatenar(DomainName,"Domain Name:  ");
                                            readString = concatenar(DomainDescription, "Domain Description:  ");
                                            readString = concatenar(DomainAudience, "Domain Audience:  ");
                                            readString = concatenar(DomainScoreFormula, "Domain Score Formula:  ");
                                            XmlNodeList lsDomainRange = itmDomain.SelectNodes("ResultRanges");
                                            XmlNode itmDomainRange;
                                            for (int domain3 = 0; domain3 < lsDomainRange.Count;domain3++ )
                                            {
                                                itmDomainRange = lsDomainRange.Item(domain3);
                                                XmlNodeList lsResultRanges = itmDomainRange.SelectNodes("ResultRange");
                                                XmlNode itmResultRanges;
                                                for (int domain4 = 0; domain4 < lsResultRanges.Count;domain4++ )
                                                {
                                                    itmResultRanges = lsResultRanges.Item(domain4);
                                                    string Start = itmResultRanges.SelectSingleNode("Start").InnerText;
                                                    string End = itmResultRanges.SelectSingleNode("End").InnerText;
                                                    string Meaning = itmResultRanges.SelectSingleNode("Meaning").InnerText;
                                                    readString = concatenar(Start," Result Range Start:  ");
                                                    readString = concatenar(End, " Result Range End:  ");
                                                    readString = concatenar(Meaning, " Result Range Meaning:  ");
                                                }
                                            }
                                        }

                                    }


                                    XmlNodeList lsTags = reader.SelectNodes("Questionnaire/Tags");
                                    XmlNode itmTags;
                                    for (int inttag = 0; inttag < lsTags.Count;inttag++ )
                                    {
                                        itmTags = lsTags.Item(inttag);
                                        XmlNodeList lsTag=itmTags.SelectNodes("Tag");
                                        XmlNode itmTag;
                                        for (int dataTag = 0; dataTag < lsTag.Count;dataTag++ )
                                        { 
                                            itmTag = lsTag.Item(dataTag);
                                            string TagName = itmTag.SelectSingleNode("Name").InnerText;
                                            string TagValue = itmTag.SelectSingleNode("Value").InnerText;
                                            readString = concatenar(TagName," Tag Name:  ");
                                            readString = concatenar(TagValue, " Tag Value:  ");
                                        }

                                    }
                                
                          }
                       
                       } 
                    }
#endregion


                }
            }

            #endregion
            return readString;
        }
    }
}
