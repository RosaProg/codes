using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCHI.DataAccessLibrary.AccessHandelers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Instructions;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using PCHI.Model.Questionnaire.Styling.Presentation;
using System.Diagnostics;
using PCHI.Model.Tag;
using PCHI.Model.Questionnaire.Response;

namespace PCHI.DataAccessLibrary.AccessHandelers.Tests
{
    [TestClass()]
    public class ProAccessHandlerTests
    {

        public struct ValuesOption
        {
            public string Text;
            public string Action;
        }

        public class MyDictionary : Dictionary<int, ValuesOption>
        {
            public void Add(int key, string text, string action)
            {
                ValuesOption val;
                val.Text = text;
                val.Action = action;
                this.Add(key, val);
            }
        }

        private QuestionnaireText AddTextToSection(QuestionnaireSection section, string actionId, string text, Platform platform, Instance instance)
        {
            QuestionnaireText txt = new QuestionnaireText()
            {
                ActionId = actionId,
                OrderInSection = section.Elements.Count + 1,
                Section = section,                
            };
            txt.TextVersions.Add(new QuestionnaireElementTextVersion() { SupportedInstances = instance, SupportedPlatforms = platform, Text = text });

            section.Elements.Add(txt);

            return txt;
        }

        private QuestionnaireItem AddItemToSection(QuestionnaireSection section, string actionId, string displayId, string text, Platform platform, Instance instance)
        {
            QuestionnaireItem item = new QuestionnaireItem()
            {
                ActionId = actionId,
                DisplayId = displayId,
                OrderInSection = section.Elements.Count + 1,
                Section = section,                
            };
            item.TextVersions.Add(new QuestionnaireElementTextVersion() { SupportedInstances = instance, SupportedPlatforms = platform, Text = text });
            section.Elements.Add(item);

            return item;
        }

        private QuestionnaireItemOptionGroup AddOptionGroupToItem(QuestionnaireItem item, MyDictionary options, int rangeStep, QuestionnaireResponseType type)
        {
            QuestionnaireItemOptionGroup qog = new QuestionnaireItemOptionGroup();
            qog.OrderInItem = item.OptionGroups.Count + 1;
            qog.RangeStep = rangeStep;
            qog.ResponseType = type;
            foreach (int v in options.Keys)
            {
                qog.Options.Add(new QuestionnaireItemOption() { Text = options[v].Text, Value = v, Group = qog, Action = options[v].Action });
            }

            qog.Item = item;

            item.OptionGroups.Add(qog);

            return qog;
        }

        [TestMethod()]
        public void AddFullProInstrumentTest()
        {

            ProInstrument pro = new ProInstrument();
            pro.Name = "OES2";
            pro.Status = QuestionnaireStatus.Indevelopment;
            pro.IsActive = true;

            pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Male" });
            pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Female" });

            {
                ProDomain d1 = new ProDomain();
                d1.Name = "Total Domain";
                d1.Audience = PCHI.Model.Users.UserTypes.Patient;
                d1.Description = "Hello";
                d1.ScoreFormula = "{OES.1} + {OES.2}";
                {
                    ProDomainResultRange r1 = new ProDomainResultRange();
                    r1.Start = 0;
                    r1.End = 10;
                    r1.Meaning = "Great";
                    d1.ResultRanges.Add(r1);
                }
                {
                    ProDomainResultRange r2 = new ProDomainResultRange();
                    r2.Start = 11;
                    r2.End = 20;
                    r2.Meaning = "Oops";
                    d1.ResultRanges.Add(r2);
                }
                pro.Domains.Add(d1);
            }

            {
                ProDomain d2 = new ProDomain();
                d2.Name = "Sleep Domain";
                d2.Audience = PCHI.Model.Users.UserTypes.Patient;
                d2.Description = "Hello";
                d2.ScoreFormula = "{OES.1} + {OES.2}";
                {
                    ProDomainResultRange r1 = new ProDomainResultRange();
                    r1.Start = 0;
                    r1.End = 10;
                    r1.Meaning = "Great";
                    d2.ResultRanges.Add(r1);
                }
                {
                    ProDomainResultRange r2 = new ProDomainResultRange();
                    r2.Start = 11;
                    r2.End = 20;
                    r2.Meaning = "Oops";
                    d2.ResultRanges.Add(r2);
                }
                pro.Domains.Add(d2);
            }

            pro.Concept = new QuestionnaireConcept() { Name = "Elbow", Description = "Tests Elbow Condition" };
            {
                QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 1, Questionnaire = pro };
                pro.Sections.Add(section);
                AddTextToSection(section, "Intro1", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                AddTextToSection(section, "Intro2", @"Please make sure you answer all the questions that follow by ticking one option for every question.", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
            }

            {
                //Dictionary<int, string> options = null;
                //var options;// = new MyDictionary();

                QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 2, Questionnaire = pro };
                pro.Sections.Add(section);
                {
                    // same options apply to items 1 to 4
                    MyDictionary options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "No difficulty" } }, { 3, new ValuesOption() { Action = "", Text = "A little bit of difficulty" } }, { 2, new ValuesOption() { Action = "", Text = "Moderate difficulty" } }, { 1, new ValuesOption() { Action = "", Text = "Extreme difficulty" } }, { 0, new ValuesOption() { Action = "", Text = "Impossible to do" } } };

                    QuestionnaireItem qi = AddItemToSection(section, "OES.1", "1.", @"<strong>During the past 4 weeks</strong>, have you had difficulty lifting things in your home, such as putting out the rubbish, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

                    qi = AddItemToSection(section, "OES.2", "2.", @"<strong>During the past 4 weeks</strong>, have you had difficulty lifting things in your home, such as putting out the rubbish, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

                    qi = AddItemToSection(section, "OES.3", "3.", @"<strong>During the past 4 weeks</strong>, have you had difficulty washing yourself all over, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

                    qi = AddItemToSection(section, "OES.4", "4.", @"<strong>During the past 4 weeks</strong>, have you had difficulty dressing yourself, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

                    qi = AddItemToSection(section, "OES.5", "5.", @"How difficult is for you to get up and down off the floor/gound?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Extreme difficulty" } }, { 100, new ValuesOption() { Action = "", Text = "No difficulty at all" } } };
                    AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

                    qi = AddItemToSection(section, "OES.6", "6.", @"How much trouble do you have with sexual activity because of your hip?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "This is not relevant to me" } } };
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem); 
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Several trouble" } }, { 100, new ValuesOption() { Action = "", Text = "No trouble at all" } } };
                    AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

                    qi = AddItemToSection(section, "OES.7", "7.", @"How much trouble do you have pushing, pulling, lifting or carrying heavy objects at work?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "I do not do these actions in my activities" } } };
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Several trouble" } }, { 100, new ValuesOption() { Action = "", Text = "No trouble at all" } } };
                    AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

                    qi = AddItemToSection(section, "OES.8", "8.", @"How concern are you about cutting/changing directions during your sport or recreational activities?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "I do not do this action in my activities" } } };
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Extremly concerned" } }, { 100, new ValuesOption() { Action = "", Text = "Not concerned at all" } } };
                    AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

                    qi = AddItemToSection(section, "OES.9", "9.", @"Please indicate the sport or instrument which is most important to you:", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "" } } };
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);

                    qi = AddItemToSection(section, "OES.10", "10.", @"Enter your comments below:", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);

                    //Part to add the definition for the body control
                    qi = AddItemToSection(section, "OES.11", "11.", @"Select the parts of your body with some kind of mal functioning:", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
                    options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Burning" } }, { 1, new ValuesOption() { Action = "", Text = "Numbness" } }, { 2, new ValuesOption() { Action = "", Text = "Pins-Needles" } }, { 3, new ValuesOption() { Action = "", Text = "Stabbing" } }, { 4, new ValuesOption() { Action = "", Text = "Ache" } } };
                    AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

                    //Optiongroup for each body part

                    /*
                    qiog1.OrderInItem = 1;
                    {
                        qiog1.Options.Add(new QuestionnaireItemOption() { Text = "None", Value = 0, Group = qiog1 });
                        qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Mild", Value = 1, Group = qiog1 });
                        qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Moderate", Value = 2, Group = qiog1 });
                        qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Severe", Value = 3, Group = qiog1 });
                        qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Unbearable", Value = 4, Group = qiog1 });
                        qiog1.Item = q1;
                    }

                    q1.OptionGroups.Add(qiog1);
                    section.Elements.Add(q1);
                    q1.Section = section;
                     * */
                }

            }

            {
                QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 3, Questionnaire = pro };
                pro.Sections.Add(section);
                AddTextToSection(section, "Footer1", @"Thank you for answering. You are done now and the results will be reported to your physician.", Platform.Classic , Instance.Baseline | Instance.Followup);
                AddTextToSection(section, "Footer1", @"Thank you for answering. You are done now and I will evaluate the results as soon as possible.", Platform.Chat, Instance.Baseline | Instance.Followup);
            }

            new AccessHandlerManager().QuestionnaireAccessHandler.AddFullQuestionnaire(pro);
        }

        [TestMethod]
        public void AddQuestionnaireFormatDefinition()
        {
            {
                ContainerFormatDefinition canvasFormatDef = new ContainerFormatDefinition();
                canvasFormatDef.ContainerDefinitionName = "GenericQuestionnaireCanvas";
                canvasFormatDef.StartHtml = "<table>";
                canvasFormatDef.EndHtml = "</table>";
                canvasFormatDef.StartEndRepeat = 0;
                
                ItemFormatDefinition rowFormatDefn = new ItemFormatDefinition();
                rowFormatDefn.ElementFormatDefinitionName = "GenericQuestionnaireItem";
                rowFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                rowFormatDefn.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(rowFormatDefn);
                
                ItemGroupOptionsFormatDefinition horizontalRadioFormatDef = new ItemGroupOptionsFormatDefinition();
                rowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(horizontalRadioFormatDef);
                horizontalRadioFormatDef.GroupOptionDefinitionName = "LikertHorizontalRadio";
                horizontalRadioFormatDef.StartHtml = "<tr><td></td><td><table>";
                horizontalRadioFormatDef.EndHtml = "</table></td></tr>";
                horizontalRadioFormatDef.ForEachOptionStart = "<tr>";
                horizontalRadioFormatDef.ForEachOptionEnd = "</tr>";
                horizontalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td>" });
                horizontalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType= ItemOptionDisplayType.RadioButton, EndText = "</td>" });
                
                ItemGroupOptionsFormatDefinition verticalRadioFormatDef = new ItemGroupOptionsFormatDefinition();
                rowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(verticalRadioFormatDef);
                verticalRadioFormatDef.GroupOptionDefinitionName = "LikertVerticalRadio";
                verticalRadioFormatDef.StartHtml = "<tr><td></td><td><table>";
                verticalRadioFormatDef.EndHtml = "</table></td></tr>";
                verticalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<tr><td>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "</td><td><%OptionText%/></td></tr>" });

                
                ItemFormatDefinition rowSliderFormatDefn = new ItemFormatDefinition();
                rowSliderFormatDefn.ElementFormatDefinitionName = "SliderQuestionnaireItem";
                rowSliderFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                rowSliderFormatDefn.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(rowSliderFormatDefn);
                
                ItemGroupOptionsFormatDefinition sliderDef = new ItemGroupOptionsFormatDefinition();
                rowSliderFormatDefn.ItemGroupOptionsFormatDefinitions.Add(sliderDef);
                sliderDef.GroupOptionDefinitionName = "LikertSlider";
                sliderDef.StartHtml = "<tr><td></td><td><table>";
                sliderDef.EndHtml = "</table></td></tr>";
                sliderDef.ForEachOptionStart = "<tr>";
                sliderDef.ForEachOptionEnd = "</tr>";
                sliderDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td><td>", ItemOptionDisplayType = ItemOptionDisplayType.Slider, EndText = "</td><td><%OptionText%/></td>" });

                ItemFormatDefinition conditionalRowFormatDefn = new ItemFormatDefinition();
                conditionalRowFormatDefn.ElementFormatDefinitionName = "ConditionalQuestionnaireItem";
                conditionalRowFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                conditionalRowFormatDefn.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(conditionalRowFormatDefn);
                
                ItemGroupOptionsFormatDefinition conditionalItemDef = new ItemGroupOptionsFormatDefinition();
                conditionalRowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(conditionalItemDef);
                //conditionalRowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(sliderDef);
                conditionalItemDef.GroupOptionDefinitionName = "ConditionalItem";
                conditionalItemDef.StartHtml = "<tr><td></td><td><table>";
                conditionalItemDef.EndHtml = "</table></td></tr>";
                conditionalItemDef.ForEachOptionStart = "<tr>";
                conditionalItemDef.ForEachOptionEnd = "</tr>";
                conditionalItemDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", EndText = "</td><td><%OptionText%/></td>" });
                //conditionalItemDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td><td>", ItemOptionDisplayType = ItemOptionDisplayType.Slider, EndText = "</td><td><%OptionText%/></td>" });


                ItemFormatDefinition rowTextBoxFormatDefn = new ItemFormatDefinition();
                rowTextBoxFormatDefn.ElementFormatDefinitionName = "TextBoxQuestionnaireItem";
                rowTextBoxFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                rowTextBoxFormatDefn.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(rowTextBoxFormatDefn);

                ItemGroupOptionsFormatDefinition textBoxDef = new ItemGroupOptionsFormatDefinition();
                rowTextBoxFormatDefn.ItemGroupOptionsFormatDefinitions.Add(textBoxDef);
                textBoxDef.GroupOptionDefinitionName = "TextBox";
                textBoxDef.StartHtml = "<tr><td></td><td><table>";
                textBoxDef.EndHtml = "</table></td></tr>";
                textBoxDef.ForEachOptionStart = "<tr>";
                textBoxDef.ForEachOptionEnd = "</tr>";
                textBoxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextBox, EndText = "</td>" });

                ItemFormatDefinition rowTextAreaFormatDefn = new ItemFormatDefinition();
                rowTextAreaFormatDefn.ElementFormatDefinitionName = "TextAreaQuestionnaireItem";
                rowTextAreaFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                rowTextAreaFormatDefn.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(rowTextAreaFormatDefn);

                ItemGroupOptionsFormatDefinition textAreaDef = new ItemGroupOptionsFormatDefinition();
                rowTextAreaFormatDefn.ItemGroupOptionsFormatDefinitions.Add(textAreaDef);
                textAreaDef.GroupOptionDefinitionName = "TextArea";
                textAreaDef.StartHtml = "<tr><td></td><td><table>";
                textAreaDef.EndHtml = "</table></td></tr>";
                textAreaDef.ForEachOptionStart = "<tr>";
                textAreaDef.ForEachOptionEnd = "</tr>";
                textAreaDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextArea, EndText = "</td>" });

                ItemFormatDefinition rowBodyControlFormatDefn = new ItemFormatDefinition();
                rowBodyControlFormatDefn.ElementFormatDefinitionName = "BodyControlQuestionnaireItem";
                rowBodyControlFormatDefn.Html = "<tr><td><%DisplayId%/></td><td><%Text%/></td></tr>";
                rowBodyControlFormatDefn.ContainerFormatDefinition = canvasFormatDef;

                string bodyControlStartTag = "<img src=\"~/Content/images/BodyControl/bodyFront.png\" alt=\"\" usemap=\"#bodyFront\" id=\"imgBodyFront\" /><map name=\"bodyFront\">";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"headFront\" shape=\"poly\" coords=\"83,1,75,3,71,4,67,8,64,13,64,16,64,20,64,23,65,25,63,26,61,26,61,29,61,34,64,36,65,39,66,42,68,46,71,50,76,54,81,57,86,58,91,55,94,52,97,48,99,44,99,41,102,39,104,34,105,31,105,27,104,25,104,19,103,15,101,10,98,7,94,4,89,1\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"leftHandFront\" shape=\"poly\" coords=\"143,201,147,200,151,199,155,198,158,199,161,202,163,204,165,206,166,210,168,214,170,217,172,220,171,222,168,219,166,217,164,215,165,219,165,223,167,225,167,229,169,232,168,235,165,232,163,229,162,225,161,222,159,224,160,228,161,230,161,233,162,235,162,237,159,236,158,230,157,227,156,224,155,222,154,225,155,228,155,230,155,232,155,235,152,236,151,233,150,229,150,227,151,224,151,222,149,222,148,225,148,230,148,232,146,233,146,222,146,220,145,217,144,215,143,212,143,207,143,205\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"leftForearmFront\" shape=\"poly\" coords=\"127,149,131,148,135,146,139,145,142,144,145,147,146,153,150,158,150,161,151,167,152,172,152,178,153,182,155,188,156,191,156,194,153,197,149,198,146,198,143,199,142,196,140,193,138,188,137,183,134,178,132,174,131,169,129,164,127,156,127,152\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"rightFootFront\" shape=\"poly\" coords=\"64,368,67,368,72,368,73,370,74,375,75,378,76,383,76,387,76,392,78,397,79,401,80,405,78,407,75,407,71,406,68,405,64,405,62,404,57,404,55,403,55,399,57,394,59,390,59,383,59,380,59,375,58,371,58,369\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"leftFootFront\" shape=\"poly\" coords=\"94,370,98,370,100,370,103,370,105,370,108,371,108,376,107,382,108,387,108,389,109,393,110,397,111,401,111,403,108,404,105,405,100,406,98,405,95,405,92,406,89,406,87,404,86,401,88,395,91,392,90,385,93,373\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"rightHandFront\" shape=\"poly\" coords=\"11,198,14,199,17,200,21,201,23,201,23,204,23,210,22,214,21,217,21,221,22,225,21,228,21,231,20,233,18,232,17,227,17,223,17,221,15,224,15,227,15,231,15,234,15,236,12,235,12,232,12,228,12,224,10,227,9,230,9,233,8,235,7,238,5,239,3,239,3,235,6,229,7,226,7,223,4,225,3,227,1,229,1,220,2,217,2,214,1,206,5,202,9,199\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"rightForearmFront\" shape=\"poly\" coords=\"25,141,29,142,31,143,37,144,41,146,42,149,42,152,42,157,41,161,39,166,37,172,35,177,33,180,31,184,28,189,27,192,25,197,19,197,16,196,13,194,13,190,13,186,14,181,14,175,15,170,16,164,18,158,20,152,21,147,23,142\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"rightLegFront\" shape=\"poly\" coords=\"42,225,49,226,56,226,64,226,69,226,75,226,80,228,79,236,77,254,76,265,76,275,74,284,74,289,74,298,72,303,70,312,71,318,74,325,74,330,74,335,73,337,72,340,71,345,71,350,71,353,71,357,71,361,71,365,67,366,64,365,60,365,55,361,54,354,51,349,50,345,49,340,47,336,47,332,48,326,48,319,50,314,50,311,51,307,51,296,50,292,49,289,47,279,46,274,44,268,43,254,41,248,42,243,41,236,41,232,41,228\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"leftLegFront\" shape=\"poly\" coords=\"86,227,91,227,95,226,101,226,107,226,110,227,116,227,124,238,124,241,124,245,124,248,124,253,123,256,123,260,122,263,121,267,121,272,119,275,118,279,118,283,117,286,117,291,116,297,115,303,116,306,115,310,116,315,117,320,118,324,119,328,120,332,119,337,118,342,116,344,114,348,112,352,111,357,110,360,108,364,107,367,104,368,100,368,96,368,95,366,94,359,95,353,95,347,93,338,93,328,95,322,94,320,95,313,94,306,94,301,94,297,93,292,93,286,92,283,91,276,91,271,90,267,90,263,89,257,87,253,87,248,87,244,88,238,85,235,86,230\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"rightArmFront\" shape=\"poly\" coords=\"25,140,30,141,35,142,38,143,40,143,43,143,44,139,46,132,48,123,50,114,51,111,50,107,49,99,48,94,48,91,48,87,49,84,50,78,50,76,51,72,52,69,48,67,43,67,39,70,36,72,33,76,32,79,30,84,29,88,28,93,27,99,28,103,27,108,27,113,27,119,28,124,27,129,26,135,25,137\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"leftArmFront\" shape=\"poly\" coords=\"114,68,117,72,118,78,118,80,118,85,119,89,119,96,118,101,118,104,117,107,117,110,117,115,118,121,119,129,120,134,122,139,125,144,126,149,131,146,135,146,139,144,142,144,143,142,141,135,140,126,140,120,139,112,138,108,139,100,139,95,138,90,136,85,135,79,132,74,129,70,126,68,121,67\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"neckFront\" shape=\"poly\" coords=\"70,53,69,59,66,61,62,64,57,66,54,68,54,71,62,71,72,71,77,72,80,74,86,74,89,72,94,71,101,71,107,72,115,72,110,67,104,65,99,60,98,58,97,56,97,53,94,54,92,56,88,58,81,59,75,56\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"chestFront\" shape=\"poly\" coords=\"53,76,52,80,51,89,51,102,53,108,53,116,59,118,64,118,71,118,78,119,86,119,92,120,100,120,109,119,115,119,117,118,118,92,118,75,113,74,102,74,96,73,93,73,85,76,78,74,75,73,70,73,65,73,60,73\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"stomachFront\" shape=\"poly\" coords=\"51,117,51,122,49,128,49,133,49,139,50,143,51,149,52,157,53,162,52,169,52,176,52,178,60,179,69,179,78,180,84,180,92,180,97,180,102,181,105,179,111,178,114,177,118,176,117,169,118,165,118,159,118,155,117,149,118,143,119,140,120,137,119,131,117,121,112,121,107,122,98,122,93,122,88,121,85,121,81,121,75,121,65,120,59,120,53,118\" />";
                bodyControlStartTag += "<area alt=\"\" title=\"\" href=\"#\" name=\"hipFront\" shape=\"poly\" coords=\"49,180,46,187,45,191,43,196,42,201,41,207,41,213,41,217,41,222,47,224,67,225,73,224,77,226,84,229,86,226,91,226,97,225,101,225,106,225,108,224,114,224,117,227,119,229,124,230,125,227,125,223,126,219,125,212,125,207,124,201,122,197,121,191,120,187,120,183,119,179,117,177,113,178,108,179,105,181,100,181,94,181,89,181,78,180,70,180,64,180,59,180,53,179,49,179\" />";
                bodyControlStartTag += "</map>";

                string bodyControlEndTag = "<div style=\"clear: both; width: 500px; height: 170px; border: 1px solid black;\" id=\"selections\"></div>";

                rowBodyControlFormatDefn.StartHtml = bodyControlStartTag;
                rowBodyControlFormatDefn.EndHtml = bodyControlEndTag;
                canvasFormatDef.ElementFormatDefinitions.Add(rowBodyControlFormatDefn);

                ItemGroupOptionsFormatDefinition bodyControlDef = new ItemGroupOptionsFormatDefinition();
                rowBodyControlFormatDefn.ItemGroupOptionsFormatDefinitions.Add(bodyControlDef);
                bodyControlDef.GroupOptionDefinitionName = "BodyControl";
                bodyControlDef.StartHtml = "<tr><td></td><td><table>";
                bodyControlDef.EndHtml = "</table></td></tr>";
                bodyControlDef.ForEachOptionStart = "<tr>";
                bodyControlDef.ForEachOptionEnd = "</tr>";
                bodyControlDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%Image%></td><td>", ItemOptionDisplayType = ItemOptionDisplayType.CheckBox, EndText = "</td>" });


                TextFormatDefinition textFormatDef = new TextFormatDefinition();
                textFormatDef.ElementFormatDefinitionName = "GenericQuestionnaireText";
                textFormatDef.Html = "<tr><td colspan=\"2\"><%Text%/></td></tr>";
                textFormatDef.ContainerFormatDefinition = canvasFormatDef;
                canvasFormatDef.ElementFormatDefinitions.Add(textFormatDef);
                
                new AccessHandlerManager().QuestionnaireFormatAccessHandler.AddOrUpdateFullDefinitionContainer(canvasFormatDef);
            }
        }

        [TestMethod]
        public void AddQuestionnaireFormat()
        {
            Format format = new Format();
            format.Name = "OES";
            format.SupportedPlatform = Platform.Classic;
            FormatContainer pro = new FormatContainer();
            format.Containers.Add(pro);
            pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireCanvas" };
            {
                TextFormatContainer intro = new TextFormatContainer();
                //container.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "Defines start and end of Table" };                
                intro.TextFormatDefinition = new TextFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireText" };
                intro.Elements.Add(new FormatContainerElement() { OrderInSection = 1, QuestionnaireElementActionId = "Intro1" });
                intro.Elements.Add(new FormatContainerElement() { OrderInSection = 2, QuestionnaireElementActionId = "Intro2" });
                pro.Children.Add(intro);
            }
            
            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.1", OrderInSection = 1 });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.2", OrderInSection = 2 });
                //items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.3", OrderInSection = 3 });
                //items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.4", OrderInSection = 4 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertVerticalRadio" }, ResponseType = QuestionnaireResponseType.List });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.3", OrderInSection = 3 });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.4", OrderInSection = 4 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "SliderQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertSlider"}, ResponseType = QuestionnaireResponseType.Range });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.5", OrderInSection = 5 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "ConditionalQuestionnaireItem" };
                //items.ItemFormatDefinition.ItemGroupOptionsFormatDefinitions = new ItemGroupOptionsFormatDefinition() { };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.6", OrderInSection = 6 });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.7", OrderInSection = 7 });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.8", OrderInSection = 8 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "TextBoxQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.9", OrderInSection = 9 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "TextAreaQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextArea" }, ResponseType = QuestionnaireResponseType.Text });
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.10", OrderInSection = 10 });
                pro.Children.Add(items);
            }

            {
                ItemFormatContainer items = new ItemFormatContainer();
                //container.ContainerFormatDefinition = ""
                items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "BodyControlQuestionnaireItem" };
                items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "BodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect});
                items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.11", OrderInSection = 11 });
                pro.Children.Add(items);
            }
            
            {
                TextFormatContainer footer = new TextFormatContainer();
                //container.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "Defines start and end of Table" };                
                footer.TextFormatDefinition = new TextFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireText" };
                footer.Elements.Add(new FormatContainerElement() { OrderInSection = 1, QuestionnaireElementActionId = "Footer1" });
                pro.Children.Add(footer);
            }

            new AccessHandlerManager().QuestionnaireFormatAccessHandler.AddOrUpdateFullFormat(format);
        }

        [TestMethod]
        public void GetQuestionnaireFormatByName()
        {
            Format f = new AccessHandlerManager().QuestionnaireFormatAccessHandler.GetFullFormatByName("OES", Platform.Classic);
        }

        [TestMethod]
        public void GetFullProInstrumenTest()
        {
            Questionnaire instrument = new AccessHandlerManager().QuestionnaireAccessHandler.GetFullQuestionnaireById(1);
        }

        [TestMethod]
        public void AddTagToQuestionnaireByName()
        {
            AccessHandlerManager accessHandlerManager = new AccessHandlerManager();
            Tag tag = new Tag() { TagName = "Gender", Value = "Male" };
            accessHandlerManager.QuestionnaireAccessHandler.AddTagToQuestionnaireByName(tag, "OES2");
        }

        [TestMethod]
        public void GetTags()
        {
            new AccessHandlerManager().TagAccessHandler.GetTags("Gender");
        }
    }
}
