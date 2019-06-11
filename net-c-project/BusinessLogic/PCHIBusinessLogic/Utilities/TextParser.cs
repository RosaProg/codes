using DSPrima.TextReplaceable;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using PCHI.Model.Questionnaire;
using System.Collections.Generic;
using System.Text;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// Supports the sending of bespokse messages. Uses the TextReplaceable library to replace parts of message with specified objects
    /// </summary>
    public class TextParser
    {
        /// <summary>
        /// The AccessHandlerManager instance to use
        /// </summary>
        private AccessHandlerManager ahm;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParser"/> class
        /// </summary>
        /// <param name="ahm">The AccessHandlerManager instance to use</param>
        public TextParser(AccessHandlerManager ahm)
        {
            this.ahm = ahm;
        }

        /// <summary>
        /// Retrieves the message of the given Message code and replaces all ReplacementCodes with the proper value
        /// </summary>
        /// <param name="textDefinitionCode">The code of the text definition to retrieve</param>
        /// <param name="objectsForParsing">Any objects that are available should be passed on as they may be used as part of the parsing process.</param>
        /// <returns>The text definition with both it's HTML and Text element parsed</returns>
        public TextDefinition ParseMessage(string textDefinitionCode, Dictionary<ReplaceableObjectKeys, object> objectsForParsing)
        {
            StringBuilder message = new StringBuilder();
            TextDefinition td = this.ahm.MessageHandler.GetTextDefinitionByCode(textDefinitionCode);

            if (td == null) return new TextDefinition();

            ReplaceableRetriever retriever = new ReplaceableRetriever(this.ahm, objectsForParsing);
            TextParser<ReplaceableObjectKeys> parser = new TextParser<ReplaceableObjectKeys>();
            string text = parser.ParseMessage(td.Text, retriever);
            string html = parser.ParseMessage(td.Html, retriever);

            TextDefinition result = new TextDefinition();
            result.DefinitionCode = td.DefinitionCode;
            result.Text = text;
            result.Html = html;

            return result;
        }

        /// <summary>
        /// Parses the given text and replaces all ReplacementCodes with the proper value
        /// </summary>
        /// <param name="text">The text to parse</param>
        /// <param name="objectsForParsing">Any objects that are available should be passed on as they may be used as part of the parsing process.</param>
        /// <returns>The parsed text</returns>
        public string ParseText(string text, Dictionary<ReplaceableObjectKeys, object> objectsForParsing)
        {
            ReplaceableRetriever retriever = new ReplaceableRetriever(this.ahm, objectsForParsing);
            TextParser<ReplaceableObjectKeys> parser = new TextParser<ReplaceableObjectKeys>();
            return parser.ParseMessage(text, retriever);
        }

        /// <summary>
        /// Updates all the text inside the Questionnaire with the proper replacement values.
        /// Includes, Instructions, TextVersions, DefaultValues, etc.
        /// </summary>
        /// <param name="q">The questionnaire to update</param>
        /// <param name="objectsForParsing">Any objects that are available should be passed on as they may be used as part of the parsing process.</param>
        public void UpdateQuestionnaireTexts(Questionnaire q, Dictionary<ReplaceableObjectKeys, object> objectsForParsing)
        {
            ReplaceableRetriever retriever = new ReplaceableRetriever(this.ahm, objectsForParsing);
            TextParser<ReplaceableObjectKeys> parser = new TextParser<ReplaceableObjectKeys>();
            foreach (QuestionnaireSection section in q.Sections)
            {
                foreach (TextVersion v in section.Instructions)
                {
                    v.Text = parser.ParseMessage(v.Text, retriever);
                }

                foreach (QuestionnaireElement element in section.Elements)
                {
                    foreach (TextVersion v in element.TextVersions)
                    {
                        v.Text = parser.ParseMessage(v.Text, retriever);
                    }

                    if (element.GetType() == typeof(QuestionnaireItem))
                    {
                        QuestionnaireItem item = (QuestionnaireItem)element;
                        foreach (TextVersion v in item.TextVersions)
                        {
                            v.Text = parser.ParseMessage(v.Text, retriever);
                        }

                        item.SummaryText = parser.ParseMessage(item.SummaryText, retriever);
                        foreach (QuestionnaireItemOptionGroup group in item.OptionGroups)
                        {
                            foreach (TextVersion v in group.TextVersions)
                            {
                                v.Text = parser.ParseMessage(v.Text, retriever);
                            }

                            group.DefaultValue = parser.ParseMessage(group.DefaultValue, retriever);
                            foreach (QuestionnaireItemOption option in group.Options)
                            {
                                option.DefaultValue = parser.ParseMessage(option.DefaultValue, retriever);
                                option.Text = parser.ParseMessage(option.Text, retriever);
                            }
                        }
                    }
                }
            }
        }
    }
}