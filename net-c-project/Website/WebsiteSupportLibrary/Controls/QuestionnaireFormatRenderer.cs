using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using WebsiteSupportLibrary.Models;

namespace WebsiteSupportLibrary.Models
{
    /// <summary>
    /// Renders a questionnaire with a specified format onto the screen
    /// </summary>
    public class QuestionnaireFormatRenderer
    {
        /// <summary>
        /// Contains per Item Id the response given to that item
        /// </summary>
        private Dictionary<string, QuestionnaireResponse> responses;

        /// <summary>
        /// The platform we are displaying and for which to load the text
        /// </summary>
        private Platform currentDisplayPlatform;

        /// <summary>
        /// Contains the instance for the current run
        /// </summary>
        private Instance instance;

        /// <summary>
        /// Holds the list of ActionId's of items that have already been answered
        /// </summary>
        public List<string> ActionIdsOfAnsweredItems = new List<string>();

        /// <summary>
        /// Gets or sets the QuestionnaireModel
        /// </summary>
        public QuestionnaireModel Model;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireFormatRenderer"/> class
        /// </summary>
        /// <param name="displayPlatform">The display platform that is wanted</param>
        public QuestionnaireFormatRenderer(Platform displayPlatform)
        {
            this.currentDisplayPlatform = displayPlatform;
        }

        /// <summary>
        /// Generates a UI for the given Questionnaire and Format
        /// </summary>
        /// <param name="q">The Questionnaire to load the UI for</param>
        /// <param name="format">The format to use</param>
        /// <param name="responses">The list of responses already given</param>
        /// <returns>a Html script for displaying the questionnaire</returns>
        public string GenerateUi(Questionnaire q, Format format, ICollection<QuestionnaireResponse> responses)
        {
            this.Model = new QuestionnaireModel();
            this.Model.Title = q.DisplayName;
            this.Model.QuestionnaireId = q.Id.ToString();
            if (q.GetType() == typeof(ProInstrument)) this.Model.IsPro = true;

            this.instance = q.CurrentInstance;
            List<QuestionnaireElement> elements = q.Sections.SelectMany(s => s.Elements).ToList();
            this.responses = new Dictionary<string, QuestionnaireResponse>();
            foreach (QuestionnaireResponse response in responses)
            {
                try
                {
                    this.responses.Add(response.Item.ActionId + "." + response.Option.ActionId, response);
                }
                catch(Exception)
                {

                }
            }
            string result = this.GenerateUi(elements, format.Containers, ref this.Model);

            bool hasCurrent = false;

            if (!this.Model.Items.Any(i => i.ActionId == "<end>"))
            {
                this.Model.Items.Add(new WebsiteSupportLibrary.Models.QuestionnaireItem()
                {
                    ActionId = "<end>",
                    Question = new Question() { HtmlContent = "Thank you for completing the questionnaire. In order to submit press the button below." },
                    Status = ItemStatus.Future.ToString(),
                    Answer = new Answer() { HtmlTemplate = string.Empty },
                    AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                    ResponsePanel = new ResponsePanel()
                    {
                        HtmlContent = string.Empty
                    }
                });
            }

            // Update Model, set all the statuses properly
            for (int i = 0; i < this.Model.Items.Count; i++)
            {
                if (this.Model.Items[i].AnsweredStatus == ItemAnsweredStatus.Answered.ToString())
                {
                    this.Model.Items[i].Status = ItemStatus.Historical.ToString();
                }
                else
                {
                    this.Model.Items[i].AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString();
                    if (hasCurrent)
                    {
                        this.Model.Items[i].Status = ItemStatus.Future.ToString();
                    }
                    else
                    {
                        this.Model.Items[i].Status = ItemStatus.Current.ToString();
                        hasCurrent = true;
                        this.Model.CurrentItem = i;
                    }
                    
                    // this.Model.Items[i].Status = hasCurrent ? ItemStatus.Future.ToString() : ItemStatus.Current.ToString();
                    // hasCurrent = true;
                    for (int j = i; j < this.Model.Items.Count; j++)
                    {
                        if (this.Model.Items[j].AnsweredStatus == ItemAnsweredStatus.Answered.ToString())
                        {
                            this.Model.Items[i].AnsweredStatus = ItemAnsweredStatus.Skipped.ToString();
                            this.Model.Items[i].Status = ItemStatus.Historical.ToString();
                            hasCurrent = false;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the UI for the list of containers and elements
        /// Used Recursion to load child containers as well.
        /// </summary>
        /// <param name="elements">The elemtents to display</param>
        /// <param name="containers">The format containers to display the elements with</param>
        /// <param name="model">The questionnaire Model to populate</param>
        /// <returns>a Html script for displaying the questionnaire</returns>
        private string GenerateUi(List<QuestionnaireElement> elements, ICollection<FormatContainer> containers, ref QuestionnaireModel model)
        {
            StringBuilder html = new StringBuilder();
            foreach (FormatContainer container in containers)
            {
                if (container.ContainerFormatDefinition != null) html.Append(container.ContainerFormatDefinition.StartHtml);

                for (int i = 0; i < container.Elements.Count; i++)
                {
                    if (container.ContainerFormatDefinition != null && container.ContainerFormatDefinition.StartEndRepeat > 0 && i > 0 && i % container.ContainerFormatDefinition.StartEndRepeat == 0)
                    {
                        html.Append(container.ContainerFormatDefinition.EndHtml);
                        html.Append(container.ContainerFormatDefinition.StartHtml);
                    }

                    QuestionnaireElement element = elements.Where(e => e.ActionId == container.Elements.ElementAt(i).QuestionnaireElementActionId).SingleOrDefault();
                    if (element != null)
                    {
                        WebsiteSupportLibrary.Models.QuestionnaireItem item = new WebsiteSupportLibrary.Models.QuestionnaireItem();
                        item.ActionId = element.ActionId;
                        if (element.GetType() == typeof(QuestionnaireText))
                        {
                            html.Append(this.BuildTextElementDisplay((QuestionnaireText)element, (TextFormatContainer)container));
                        }
                        else if (element.GetType() == typeof(PCHI.Model.Questionnaire.QuestionnaireItem))
                        {
                            html.Append(this.BuildItemElementDisplay((PCHI.Model.Questionnaire.QuestionnaireItem)element, (ItemFormatContainer)container, ref item));
                        }

                        model.Items.Add(item);
                    }
                }

                if (container.Children != null)
                {
                    html.Append(this.GenerateUi(elements, container.Children, ref model));
                }

                if (container.ContainerFormatDefinition != null) html.Append(container.ContainerFormatDefinition.EndHtml);
            }

            return html.ToString();
        }

        /// <summary>
        /// Builds a display for the Text Element in the specified format
        /// </summary>
        /// <param name="element">The QuestionnaireText element to display</param>
        /// <param name="format">The Text Format container containing the format to display the Text element in</param>
        /// <returns>a Html script for displaying the Text Element</returns>
        private string BuildTextElementDisplay(QuestionnaireText element, TextFormatContainer format)
        {
            StringBuilder html = new StringBuilder();

            html.Append(this.FormatElementHtml(element, format.TextFormatDefinition.StartHtml));
            html.Append(this.FormatElementHtml(element, format.TextFormatDefinition.Html));
            html.Append(this.FormatElementHtml(element, format.TextFormatDefinition.EndHtml));
            return html.ToString();
        }

        /// <summary>
        /// Builds a display for the Item Element and options in the specified format
        /// </summary>
        /// <param name="element">The QuestionnaireItem element to display</param>
        /// <param name="format">The Item Format container containing the format to display the Item element in</param>
        /// <param name="item">The QuestionnaireItem to pupolate</param>
        /// <returns>a Html script for displaying the Item Element</returns>
        private string BuildItemElementDisplay(PCHI.Model.Questionnaire.QuestionnaireItem element, ItemFormatContainer format, ref WebsiteSupportLibrary.Models.QuestionnaireItem item)
        {
            StringBuilder html = new StringBuilder();
            string groupOptions = "";
            if (format.ItemFormatDefinition_Name.IndexOf("BodyControl") >= 0)
            {
                item.Question = new Question() { HtmlContent = this.FormatElementHtml(element, format.ItemFormatDefinition.Html.Substring(0, format.ItemFormatDefinition.Html.IndexOf("<%BodyControl%/>"))) };
                groupOptions = this.FormatElementHtml(element, format.ItemFormatDefinition.Html.Substring(format.ItemFormatDefinition.Html.IndexOf("<%BodyControl%/>")));
                groupOptions = groupOptions.Replace("<%BodyControl%/>", "");
            }
            else
            {
                item.Question = new Question() { HtmlContent = this.FormatElementHtml(element, format.ItemFormatDefinition.Html) };
            }
            item.Answer = new Answer() { HtmlTemplate = this.FormatAnswer(element) };
            html.Append(this.FormatElementHtml(element, format.ItemFormatDefinition.StartHtml));
            if (!(format.ItemFormatDefinition_Name.IndexOf("BodyControl") >= 0))
            {
                html.Append(this.FormatElementHtml(element, format.ItemFormatDefinition.Html));
            }
            groupOptions += this.BuildGroupOptionDisplay(element.OptionGroups.ToList(), format.ItemGroupFormats.ToList(), ref item);
            html.Append(groupOptions);
            item.ResponsePanel = new ResponsePanel() { HtmlContent = groupOptions };
            item.AnsweredStatus = this.ActionIdsOfAnsweredItems.Contains(element.ActionId) ? ItemAnsweredStatus.Answered.ToString() : ItemAnsweredStatus.NotAnswered.ToString();
            item.Status = ItemStatus.Historical.ToString();

            html.Append(this.FormatElementHtml(element, format.ItemFormatDefinition.EndHtml));
            return html.ToString();
        }

        /// <summary>
        /// Build the html for the options in the list of Option Groups
        /// </summary>
        /// <param name="groups">The groups to format</param>
        /// <param name="formatDefinitions">The list of format definition to format the option groups with</param>
        /// <param name="item">The QuestionnaireItem instance to populate</param>
        /// <returns>The Formatte Html to display the options in</returns>
        private string BuildGroupOptionDisplay(List<QuestionnaireItemOptionGroup> groups, List<ItemGroupFormat> formatDefinitions, ref WebsiteSupportLibrary.Models.QuestionnaireItem item)
        {
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < groups.Count; i++)
            {
                QuestionnaireItemOptionGroup group = groups[i];
                ItemGroupFormat def = formatDefinitions.Where(f => f.ResponseType == group.ResponseType).Single();

                html.Append(this.FormatElementHtml(group.Item, def.ItemGroupOptionsFormatDefinition.StartHtml, group));
                switch (group.ResponseType)
                {
                    case QuestionnaireResponseType.MultiSelect:
                    case QuestionnaireResponseType.List:
                        foreach (ItemGroupOptionsForEachOptionDefinition format in def.ItemGroupOptionsFormatDefinition.ForEachOption)
                        {
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionStart);
                            html.Append(this.BuildOptionStartTag(group, format.ItemOptionDisplayType, ref item));
                            foreach (QuestionnaireItemOption option in group.Options)
                            {
                                List<QuestionnaireItemOption> options = new List<QuestionnaireItemOption>() { option };
                                html.Append(this.FormatOptionHtml(option, format.StartText));
                                html.Append(this.BuildOption(options, format.ItemOptionDisplayType, ref item));
                                html.Append(this.FormatOptionHtml(option, format.EndText));
                            }

                            html.Append(this.BuildOptionEndTag(group, format.ItemOptionDisplayType));
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionEnd);
                        }
                        break;
                    case QuestionnaireResponseType.Range:
                        foreach (ItemGroupOptionsForEachOptionDefinition format in def.ItemGroupOptionsFormatDefinition.ForEachOption)
                        {
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionStart);
                            html.Append(this.BuildOptionStartTag(group, format.ItemOptionDisplayType, ref item));

                            QuestionnaireItemOption option1 = group.Options.ElementAt(0);
                            QuestionnaireItemOption option2 = group.Options.ElementAt(group.Options.Count - 1);
                            html.Append(this.FormatOptionHtml(option1, format.StartText));
                            html.Append(this.BuildOption(group.Options.ToList(), format.ItemOptionDisplayType, ref item));
                            html.Append(this.FormatOptionHtml(option2, format.EndText));

                            html.Append(this.BuildOptionEndTag(group, format.ItemOptionDisplayType));
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionEnd);
                        }

                        break;
                    case QuestionnaireResponseType.Text:
                        foreach (ItemGroupOptionsForEachOptionDefinition format in def.ItemGroupOptionsFormatDefinition.ForEachOption)
                        {
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionStart);
                            html.Append(this.BuildOptionStartTag(group, format.ItemOptionDisplayType, ref item));
                            foreach (QuestionnaireItemOption option in group.Options)
                            {
                                List<QuestionnaireItemOption> options = new List<QuestionnaireItemOption>() { option };
                                html.Append(this.FormatOptionHtml(option, format.StartText));
                                html.Append(this.BuildOption(options, format.ItemOptionDisplayType, ref item));
                                html.Append(this.FormatOptionHtml(option, format.EndText));
                            }

                            html.Append(this.BuildOptionEndTag(group, format.ItemOptionDisplayType));
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionEnd);
                        }

                        break;
                    case QuestionnaireResponseType.ConditionalItem:
                        foreach (ItemGroupOptionsForEachOptionDefinition format in def.ItemGroupOptionsFormatDefinition.ForEachOption)
                        {
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionStart);
                            html.Append(this.BuildOptionStartTag(group, format.ItemOptionDisplayType, ref item));
                            if (group.Options.Count > 1)
                            {
                                format.ItemOptionDisplayType = ItemOptionDisplayType.RadioButton;
                            }
                            else
                            {
                                format.ItemOptionDisplayType = ItemOptionDisplayType.CheckBox;
                            }

                            for (int q = 0; q < group.Options.Count; q++)
                            {
                                QuestionnaireItemOption option = group.Options.ElementAt(q);
                                List<QuestionnaireItemOption> options = new List<QuestionnaireItemOption>() { option };
                                html.Append(this.FormatOptionHtml(option, format.StartText));
                                html.Append(this.BuildOption(options, format.ItemOptionDisplayType, ref item));
                                html.Append(this.FormatOptionHtml(option, format.EndText));
                            }

                            html.Append(this.BuildOptionEndTag(group, format.ItemOptionDisplayType));
                            html.Append(def.ItemGroupOptionsFormatDefinition.ForEachOptionEnd);
                        }

                        break;
                }

                html.Append(this.FormatElementHtml(group.Item, def.ItemGroupOptionsFormatDefinition.EndHtml));
            }

            return html.ToString();
        }

        /// <summary>
        /// Builds the start tag for all options in the given group based upon the display type
        /// </summary>
        /// <param name="group">The group that holds all the options</param>
        /// <param name="itemOptionDisplayType">The ItemOptionDisplayType that is used for the options</param>
        /// <param name="item">The QuestionnaireItem instance to populate</param>
        /// <returns>A string with the start of the Options</returns>
        private string BuildOptionStartTag(QuestionnaireItemOptionGroup group, ItemOptionDisplayType itemOptionDisplayType, ref WebsiteSupportLibrary.Models.QuestionnaireItem item)
        {
            switch (itemOptionDisplayType)
            {
                case ItemOptionDisplayType.DropDown:
                    return "<select  ActionId=\"" + this.FormatText(group.Item.ActionId) + "\" name=\"DropDown." + group.Item.Id + "." + group.Id + "\">";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Builds an entry for an QuestionnaireItemOption based upon the Display Type for that option
        /// </summary>
        /// <param name="options">A list that should hold all the options</param>        
        /// <param name="itemOptionDisplayType">The ItemOptionDisplayType that is used for the options</param>
        /// <param name="item">The QuestionnaireItem instance to fill</param>
        /// <returns>A QuestionnaireItemOption in html format</returns>
        private string BuildOption(List<QuestionnaireItemOption> options, ItemOptionDisplayType itemOptionDisplayType, ref WebsiteSupportLibrary.Models.QuestionnaireItem item)
        {
            QuestionnaireItemOption option1 = options.ElementAt(0);
            QuestionnaireItemOption option2 = new QuestionnaireItemOption();
            if (options.Count > 1)
            {
                option2 = options.ElementAt(1);
            }

            StringBuilder response = new StringBuilder();

            // Used to set something as selected. 
            // Also used to indicate the current item has already been responded to. Therefor must be set even if not needed for marking the object as selected.
            string selected = string.Empty;
            string disabled = string.Empty;
            double value = 0.0;

            string textValue = string.Empty;
            string displayValue = string.Empty;

            string actionId = "ActionId=\"" + this.FormatText(option1.Group.Item.ActionId) + "\"";
            string inputName = string.Empty;
            string inputValue = string.Empty;
            string dataTag = string.Empty;
            string callingFunction = string.Empty;
            string action = string.Empty;
            string placeholder = string.Empty;
            string html5Helper = string.Empty;
            string OptionResponseId = option1.Group.Item.ActionId + "." + option1.ActionId;

            switch (itemOptionDisplayType)
            {
                case ItemOptionDisplayType.None:
                    break;
                case ItemOptionDisplayType.HiddenCheckBox:
                    inputName = " name=\"HiddenCheckBox." + option1.Group.Item.Id + "." + option1.Group.TextVersions.ElementAt(0).Text + "." + option1.Id + "\" ";
                    inputValue = " value=\"" + option1.Text + "\" ";
                    dataTag = " data-" + option1.Group.TextVersions.ElementAt(0).Text + "=\"" + option1.Text + "\" ";
                    if (this.responses.ContainsKey(OptionResponseId) && this.responses[OptionResponseId].ResponseText != null)
                    {
                        string[] values = this.responses[OptionResponseId].ResponseText.Split('.');
                        if (values[0] == option1.Group.TextVersions.ElementAt(0).Text && values[1] == option1.Text)
                            selected = " checked ";
                    }
                    response.Append("<input hidden type=\"checkbox\" " + actionId + " ").Append(dataTag).Append(inputName).Append(selected).Append(inputValue).Append("/>");
                    break;
                case ItemOptionDisplayType.CheckBox:
                    inputName = " name=\"CheckBox." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";
                    inputValue = " value=\"" + option1.Id + "." + option1.Value + "\" ";

                    // Note: Checkboxes have no "Select one only" option, no grouping of checkboxes.
                    if (!string.IsNullOrWhiteSpace(option1.Action))
                    {
                        dataTag = "data-applicable=\"";

                        if (this.responses.ContainsKey(OptionResponseId) && this.responses[OptionResponseId].ResponseText != null)
                        {
                            action = this.responses[OptionResponseId].ResponseText;
                            if (this.responses[OptionResponseId].ResponseText == "MakeItemApplicable")
                            {
                                selected = " checked ";
                                displayValue = option1.Text;
                            }
                        }
                        else
                        {
                            action = option1.Action;
                        }

                        action += "\"";
                        callingFunction = " onclick=\"ConditionalItemBehaviour(" + option1.Group.Item.Id + ", \'CheckBox." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\', \'" + option1.Id + "\')\" ";
                    }

                    response.Append("<input type=\"checkbox\" " + actionId + " ").Append(callingFunction).Append(inputName).Append(selected).Append(dataTag).Append(action).Append(inputValue).Append("/>");

                    break;
                case ItemOptionDisplayType.DropDown:
                    inputName = " name=\"DropDown." + option1.Group.Item.Id + "." + option1.Group.Id + "\" ";
                    inputValue = " value=\"" + option1.Id + "." + option1.Value + "\" ";
                    if (this.responses.ContainsKey(OptionResponseId) && this.responses[OptionResponseId].Option.Value == option1.Value)
                    {
                        selected = " selected ";
                        displayValue = option1.Text;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            double.TryParse(option1.Group.DefaultValue, out value);
                            if (value == option1.Value)
                            {
                                selected = " checked ";
                                displayValue = option1.Text;
                            }
                        }
                    }

                    response.Append("<option").Append(inputValue).Append(inputName).Append(">").Append(option1.Text).Append("</option>");
                    break;
                case ItemOptionDisplayType.RadioButton:
                    inputName = " name=\"Radio." + option1.Group.Item.Id + "." + option1.Group.Id + "\" ";
                    inputValue = " value=\"" + option1.Id + "." + option1.Value + "\" ";

                    if (this.responses.ContainsKey(OptionResponseId) && this.responses[OptionResponseId].Option.Value == option1.Value)
                    {
                        selected = " checked ";
                        displayValue = option1.Text;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            double.TryParse(option1.Group.DefaultValue, out value);
                            if (value == option1.Value)
                            {
                                selected = " checked ";
                                displayValue = option1.Text;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(option1.Action))
                    {
                        dataTag = "data-applicable=\"";
                        action = option1.Action + "\"";
                        callingFunction = " onclick=\"ConditionalItemBehaviour(" + option1.Group.Item.Id + ", \'Radio." + option1.Group.Item.Id + "." + option1.Group.Id + "\', \'" + option1.Id + "\')\" ";
                    }

                    response.Append("<input type=\"radio\" " + actionId + " ").Append(callingFunction).Append(inputName).Append(selected).Append(dataTag).Append(action).Append(inputValue).Append(">");
                    break;
                case ItemOptionDisplayType.Slider:
                    inputName = " name=\"Slider." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";

                    double max = options.Max(op => op.Value);
                    double min = options.Min(op => op.Value);
                    string disabledHTML5 = "false";
                    if (this.responses.ContainsKey(OptionResponseId))
                    {
                        if (this.responses[OptionResponseId].ResponseValue != null)
                        {
                            value = (double)this.responses[OptionResponseId].ResponseValue;
                            selected = "selected";
                            displayValue = value.ToString();
                        }

                        if (this.responses[OptionResponseId].ResponseText == "MakeItemApplicable")
                        {
                            disabled = " disabled = disabled ";
                            disabledHTML5 = "true";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.DefaultValue))
                        {
                            double.TryParse(option1.DefaultValue, out value);
                        }
                        else if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            double.TryParse(option1.Group.DefaultValue, out value);
                        }
                    }

                    inputValue = " value=\"" + value + "\" ";
                    callingFunction = " onchange=\"updateSliderValue('Slider." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "')\" ";
                    string annotation;
                    annotation = "<div><span class=\"SpanSliderValue\" name=\"Span.Slider." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\">" + displayValue + "</span>";
                    // response.Append("@Html.EJ().Slider(\"Slider.").Append(option1.Group.Item.Id).Append("\").SliderType(SlideType.MinRange)").Append(".Value(\"50\")");
                    //element using HTML5, attach space to locate proper element with javascript if HTML5 not supported
                    html5Helper = "<script>checkCompatibility();</script><div title=\"disabled|" + disabledHTML5 + "|max|" + max.ToString() + "|min|" + min.ToString() + "|step|10|value|" + displayValue + "|onchange|" + "updateSliderValue('Slider." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "', true);" + "\" id=\"Support.Slider." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ></div>";
                    response.Append(annotation).Append("<input type=\"range\" ").Append(actionId).Append(callingFunction).Append(inputName).Append(" step=\"10\" min=\"").Append(min.ToString()).Append("\" max=\"").Append(max.ToString()).Append("\"").Append(inputValue).Append(disabled).Append("/>").Append(html5Helper).Append("</div>");
                    break;
                case ItemOptionDisplayType.TextArea:
                    inputName = " name=\"TextArea." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";

                    if (this.responses.ContainsKey(OptionResponseId))
                    {
                        textValue = this.responses[OptionResponseId].ResponseText;
                        selected = "selected";
                        displayValue = textValue;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.DefaultValue))
                        {
                            textValue = option1.DefaultValue;
                        }
                        else if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            textValue = option1.Group.DefaultValue;
                        }
                    }

                    inputValue = "value=\"" + textValue + "\"";
                    placeholder = " placeholder=\"" + option1.Text + "\" ";
                    response.Append("<textarea " + actionId).Append(inputName).Append(placeholder).Append(">").Append(textValue).Append("</textarea>");
                    break;
                case ItemOptionDisplayType.TextBox:
                    inputName = " name=\"TextBox." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";

                    if (this.responses.ContainsKey(OptionResponseId))
                    {
                        textValue = this.responses[OptionResponseId].ResponseText;
                        selected = "selected";
                        displayValue = textValue;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.DefaultValue))
                        {
                            textValue = option1.DefaultValue;
                        }
                        else if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            textValue = option1.Group.DefaultValue;
                        }
                    }

                    inputValue = " value=\"" + textValue + "\" ";
                    placeholder = " placeholder=\"" + option1.Text + "\" ";
                    response.Append("<input type=\"text\" " + actionId).Append(inputName).Append(inputValue).Append(placeholder).Append(" >");
                    break;
                case ItemOptionDisplayType.DatePicker:
                    inputName = " name=\"DatePicker." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";
                    if (this.responses.ContainsKey(OptionResponseId))
                    {
                        textValue = this.responses[OptionResponseId].ResponseText;
                        selected = "selected";
                        displayValue = textValue;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(option1.DefaultValue))
                        {
                            textValue = option1.DefaultValue;
                        }
                        else if (!string.IsNullOrWhiteSpace(option1.Group.DefaultValue))
                        {
                            textValue = option1.Group.DefaultValue;
                        }
                    }

                    inputValue = " value=\"" + textValue + "\" ";
                    response.Append("<script>checkCompatibility();</script><input type=\"date\" " + actionId).Append(inputName).Append(inputValue).Append(" >");
                    break;
                case ItemOptionDisplayType.Password:
                    inputName = " name=\"Password." + option1.Group.Item.Id + "." + option1.Group.Id + "." + option1.Id + "\" ";
                    //Retrieve password from database??
                    displayValue = "*****";
                    response.Append("<input type=\"password\" " + actionId).Append(inputName).Append(">");
                    break;
            }

            if (!string.IsNullOrWhiteSpace(selected))
            {
                this.ActionIdsOfAnsweredItems.Add(option1.Group.Item.ActionId);
                item.Answer.Value = displayValue;
                if (item.AnswerNames == null) item.AnswerNames = new List<string>();
                if (item.AnswerValues == null) item.AnswerValues = new List<string>();
                if (itemOptionDisplayType == ItemOptionDisplayType.DropDown)
                {
                    item.AnswerNames.Add("DropDown." + option1.Group.Item.Id + "." + option1.Group.Id);
                }
                else
                {
                    item.AnswerNames.Add(inputName.Substring(inputName.IndexOf("name=\"") + "name=\"".Length));
                    item.AnswerNames[item.AnswerNames.Count - 1] = item.AnswerNames[item.AnswerNames.Count - 1].Remove(item.AnswerNames[item.AnswerNames.Count - 1].LastIndexOf("\""));

                }

                item.AnswerValues.Add(inputValue.Substring(inputValue.IndexOf("value=\"") + "value=\"".Length));
                item.AnswerValues[item.AnswerValues.Count - 1] = item.AnswerValues[item.AnswerValues.Count - 1].Remove(item.AnswerValues[item.AnswerValues.Count - 1].LastIndexOf("\""));

            }

            string name = string.Empty;
            if (!string.IsNullOrWhiteSpace(inputName))
            {
                name = inputName.Substring(inputName.IndexOf("=")).Trim();
                name = name.Remove(0, 2).Trim();
                name = name.Remove(name.Length - 1, 1).Trim();
                if (!item.ItemNames.Contains(name)) item.ItemNames.Add(name);
            }

            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                string shortInputValue = inputValue.Substring(inputValue.IndexOf("=")).Trim();
                shortInputValue = shortInputValue.Remove(0, 2).Trim();
                shortInputValue = shortInputValue.Remove(shortInputValue.Length - 1, 1).Trim();
                item.PossibleAnswers.Add(new PossibleAnswers() { Name = name, Value = shortInputValue, AnswerText = option1.Text, Action = option1.Action });
            }
            else if (!string.IsNullOrWhiteSpace(displayValue))
            {
                item.PossibleAnswers.Add(new PossibleAnswers() { Name = name, Value = displayValue, AnswerText = option1.Text, Action = option1.Action });
            }

            return response.ToString();
        }

        /// <summary>
        /// Builds the start end tag for all options in the given group based upon the display type
        /// </summary>
        /// <param name="group">The group that holds all the options</param>
        /// <param name="itemOptionDisplayType">The ItemOptionDisplayType that is used for the options</param>
        /// <returns>A string with the start of the Options</returns>
        private string BuildOptionEndTag(QuestionnaireItemOptionGroup group, ItemOptionDisplayType itemOptionDisplayType)
        {
            switch (itemOptionDisplayType)
            {
                case ItemOptionDisplayType.DropDown:
                    return "</select>";
                default:
                    return string.Empty;
            }
        }

        #region Format text modification

        /// <summary>
        /// Formats the answer of every question depending of the type of response
        /// </summary>
        /// <param name="element">The element to get the values from</param>
        /// <returns>A formatted html text</returns>
        private string FormatAnswer(PCHI.Model.Questionnaire.QuestionnaireItem element)
        {
            StringBuilder html = new StringBuilder();
            html.Append("me: ");
            foreach (QuestionnaireItemOptionGroup optionGroup in element.OptionGroups)
            {
                if (optionGroup.ResponseType != QuestionnaireResponseType.MultiSelect)
                {
                    html.Append(optionGroup.TextVersions.ElementAt(0).Text + " %Value% ");
                }
                else
                {
                    foreach (QuestionnaireItemOption option in optionGroup.Options)
                    {
                        html.Append(" %Value% ");
                    }
                    html.Append(" " + optionGroup.TextVersions.ElementAt(0).Text + " ");
                    html.Append("</br>");
                }
            }
            return html.ToString();
        }

        /// <summary>
        /// Formats the Element Html text, replaces certain text elements with the appropriate text from an Element Variable
        /// </summary>
        /// <param name="element">The element to get the values from</param>
        /// <param name="format">The Format to modify</param>
        /// <returns>A formatted html text</returns>
        private string FormatElementHtml(PCHI.Model.Questionnaire.QuestionnaireElement element, string format, QuestionnaireItemOptionGroup group = null)
        {
            string html5Helper = "<div title=\"max|2000|min|20|step|10|value|1000|onchange|resizeBody('" + element.Id + "', true);\" id=\"Support.Slider." + element.Id + "\" ></div>";
            string bodyControlStartTag = "<table><tr><td>" + html5Helper + "<input id=\"bodySize." + element.Id + "\" name=\"SliderZoomBodyControl\" type=\"range\" min=\"20\" max=\"2000\" onchange=\"resizeBody('" + element.Id + "')\"></td></tr><tr><td id=\"selectedBodyPartsFront\" width=\"150px\"></td><td>";
            bodyControlStartTag += "<img src=\"/Content/images/BodyControl/bodyFront-Back.png\" alt=\"" + element.Id + "\" usemap=\"#body\" id=\"imgBody." + element.Id + "\" /><map name=\"body\">";
            bodyControlStartTag += "<area alt=\"\" title=\"headFront\" href=\"#\" shape=\"poly\" coords=\"94,40,93,33,93,25,96,17,102,12,110,8,118,8,125,9,131,11,137,16,141,21,142,27,142,32,141,36,140,39,142,41,143,39,143,43,143,47,142,49,141,53,139,56,137,56,136,59,135,63,132,68,128,73,125,76,121,78,115,78,108,76,103,70,100,65,98,57,97,55,95,55,93,51,92,44,91,40,94,42\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"neckFront\" href=\"#\" shape=\"poly\" coords=\"100,70,100,75,99,79,97,82,94,85,91,87,90,89,90,95,139,95,139,91,139,84,135,80,134,78,133,75,133,71,131,73,128,76,125,79,121,81,118,81,113,80,111,79,106,77,105,75\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightShoulderFront\" href=\"#\" shape=\"poly\" coords=\"90,88,90,119,75,139,48,140,48,129,50,120,52,111,55,103,59,98,64,95,68,93,75,92,82,91,87,90\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftShoulderFront\" href=\"#\" shape=\"poly\" coords=\"144,87,144,119,160,140,186,140,186,130,185,122,184,115,182,108,180,104,176,97,172,94,167,92,160,91,154,91,149,90,146,89\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"chestFront\" href=\"#\" shape=\"poly\" coords=\"90,95,143,95,142,120,158,141,158,144,158,148,159,153,159,159,161,165,162,170,161,175,159,181,158,187,75,188,75,182,73,177,71,173,73,168,75,164,76,158,77,151,76,145,76,140,92,120,92,95\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightArmFront\" href=\"#\" shape=\"poly\" coords=\"48,142,74,142,74,147,74,151,74,155,73,161,73,165,71,169,70,171,69,174,69,178,46,175,47,168,48,161,49,154,49,147\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftArmFront\" href=\"#\" shape=\"poly\" coords=\"160,142,186,142,187,147,187,153,187,160,188,166,188,172,190,177,165,179,164,174,163,168,162,162,162,156,161,150\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightElbowFront\" href=\"#\" shape=\"poly\" coords=\"45,177,68,180,66,185,65,188,64,192,64,196,63,200,63,203,38,198,40,193,42,190,44,186,44,181\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftElbowFront\" href=\"#\" shape=\"poly\" coords=\"166,181,190,180,191,184,192,187,193,191,195,194,196,198,198,201,172,203,171,197,171,192,169,188,168,184\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightForearmFront\" href=\"#\" shape=\"poly\" coords=\"37,200,63,206,61,212,61,216,58,221,56,227,54,231,52,236,50,240,48,243,46,246,45,250,28,246,30,238,32,231,33,224,33,218,33,212,33,210,36,205\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftForearmFront\" href=\"#\" shape=\"poly\" coords=\"174,216,201,210,201,217,201,221,201,225,202,229,203,233,204,238,205,243,206,247,207,250,191,254,189,249,186,242,184,237,182,233,179,229,177,224,175,220,171,204,196,197,198,202,199,206,201,209,201,214\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightWristFront\" href=\"#\" shape=\"poly\" coords=\"28,248,44,252,43,255,42,257,42,261,42,263,41,266,23,258,26,255,27,252\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftWristFront\" href=\"#\" shape=\"poly\" coords=\"191,256,208,252,209,255,211,257,213,258,215,260,192,266,192,262,192,259\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"abdomenFront\" href=\"#\" shape=\"poly\" coords=\"76,195,157,195,157,201,156,208,156,216,157,222,159,231,163,238,165,244,132,261,132,267,105,268,105,261,69,246,72,240,74,235,76,230,77,226,78,218,78,211,78,205,77,200\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightHipFront\" href=\"#\" shape=\"poly\" coords=\"68,247,103,261,104,277,65,289,65,281,65,275,65,269,65,262,66,256,67,251\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftHipFront\" href=\"#\" shape=\"poly\" coords=\"133,262,165,245,167,249,168,254,168,258,169,262,169,268,169,272,169,277,169,282,169,285,133,276\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"groinFront\" href=\"#\" shape=\"poly\" coords=\"105,270,131,269,131,277,141,280,121,294,121,290,120,288,117,288,115,288,114,290,114,292,114,295,92,282,106,279\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightThighFront\" href=\"#\" shape=\"poly\" coords=\"90,282,65,291,65,298,65,303,66,311,66,317,68,324,69,334,71,341,72,347,75,356,75,363,77,370,78,375,104,376,106,371,106,363,106,356,107,349,107,343,109,335,110,326,111,320,112,312,112,308,113,302,113,296\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftThighFront\" href=\"#\" shape=\"poly\" coords=\"121,296,143,281,169,288,169,293,169,297,169,302,168,309,168,316,168,319,167,324,167,332,164,341,162,348,161,353,159,358,158,364,158,368,157,373,157,376,130,377,129,370,128,361,128,353,127,344,126,337,125,330,124,321,123,313,122,306,121,300\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightKneeFront\" href=\"#\" shape=\"poly\" coords=\"77,379,104,379,103,382,103,386,102,390,101,394,101,397,101,401,102,405,103,408,74,410,76,404,77,399,77,394,78,388,78,383\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftKneeFront\" href=\"#\" shape=\"poly\" coords=\"130,379,157,378,157,382,156,385,156,388,156,391,156,395,157,399,158,402,160,406,160,409,131,409,132,404,133,402,133,397,133,392,132,387,130,383\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightLegFront\" href=\"#\" shape=\"poly\" coords=\"73,412,103,410,104,413,104,417,104,423,104,428,103,432,102,437,102,442,102,446,102,450,102,455,103,459,103,464,103,467,104,471,105,475,89,476,89,471,87,464,85,457,82,452,79,443,75,437,74,431,73,425,72,419\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftLegFront\" href=\"#\" shape=\"poly\" coords=\"131,411,161,411,161,414,162,417,162,421,162,426,161,430,159,434,158,438,156,442,155,447,152,452,150,456,149,458,148,462,147,465,147,468,146,471,146,474,129,475,130,470,132,465,132,459,133,452,133,446,133,441,133,436,131,430,130,425,129,421,130,416\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightAnkleFront\" href=\"#\" shape=\"poly\" coords=\"88,478,105,477,106,479,107,482,107,484,107,487,107,490,106,492,88,492,88,488,88,485,88,481\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftAnkleFront\" href=\"#\" shape=\"poly\" coords=\"129,477,146,476,146,479,146,482,146,485,146,488,146,490,127,493,126,489,126,484,127,481\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightFootFront\" href=\"#\" shape=\"poly\" coords=\"87,493,107,493,113,511,112,513,109,514,106,513,105,511,104,513,102,513,100,513,99,510,98,512,95,513,93,511,91,511,89,511,87,510,85,510,83,510,81,509,84,503,87,496\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftFootFront\" href=\"#\" shape=\"poly\" coords=\"127,494,146,491,152,508,150,509,147,509,146,511,144,511,142,510,141,512,139,512,137,512,135,510,134,512,132,513,130,512,129,510,128,513,125,514,123,514,121,512,121,508,125,500\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightHandFront\" href=\"#\" shape=\"poly\" coords=\"42,266,18,261,15,264,13,267,12,271,10,275,8,278,7,281,5,283,6,285,10,282,12,279,12,277,14,275,16,275,18,275,17,279,16,281,14,285,12,288,12,290,11,295,11,298,12,300,14,298,16,294,18,290,20,286,21,283,22,287,21,290,21,292,20,295,19,298,19,303,20,305,22,303,23,297,25,293,25,289,26,287,27,285,29,286,29,289,28,292,28,294,27,297,27,299,27,302,29,302,32,297,33,293,33,290,33,288,33,285,34,283,36,286,36,288,36,291,36,294,36,297,37,299,39,297,40,291,39,287,40,281,41,274\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftHandFront\" href=\"#\" shape=\"poly\" coords=\"192,267,216,261,219,263,221,266,221,269,223,273,224,276,226,278,228,281,229,284,227,284,225,282,223,280,221,278,220,276,218,275,216,275,217,278,218,281,220,286,223,296,223,299,221,299,219,296,218,292,215,286,213,283,211,284,212,287,213,291,214,294,215,297,215,300,215,303,214,305,212,302,211,298,210,293,208,287,207,284,205,283,205,285,205,288,206,292,206,295,206,298,206,301,204,303,203,300,202,297,202,293,201,289,201,285,200,282,199,284,199,286,198,289,198,292,198,296,197,299,195,299,194,295,194,291,195,285,195,280,193,275,192,271\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"headBack\" href=\"#\" shape=\"poly\" coords=\"400,42,400,37,400,32,399,26,401,21,404,16,409,13,414,11,422,10,428,10,433,11,437,12,441,15,445,20,448,25,448,29,447,33,446,36,446,39,447,42,448,40,449,42,449,48,448,51,447,56,445,56,443,56,442,60,441,62,440,64,439,66,407,66,406,63,404,61,404,58,403,56,400,54,399,51,398,47,398,43,397,40\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"neckBack\" href=\"#\" shape=\"poly\" coords=\"407,66,439,66,440,69,440,72,440,76,440,79,443,82,447,85,447,94,401,94,400,85,403,83,406,81,407,75,407,70\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightShoulderBack\" href=\"#\" shape=\"poly\" coords=\"448,87,450,117,465,139,492,139,492,133,492,128,491,120,490,114,488,108,486,101,482,97,477,94,472,92,464,91,455,89\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftShoulderBack\" href=\"#\" shape=\"poly\" coords=\"398,86,398,118,381,139,355,141,355,135,355,129,355,122,356,117,358,110,360,104,363,99,367,95,373,92,380,91,386,91,391,90,395,88\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"thoracicSpineBack\" href=\"#\" shape=\"poly\" coords=\"412,95,433,95,438,188,413,188\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightArmBack\" href=\"#\" shape=\"poly\" coords=\"466,141,492,141,493,146,493,151,494,159,494,163,494,168,495,172,495,176,496,179,474,184,472,179,470,173,468,166,467,158,466,148\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftArmBack\" href=\"#\" shape=\"poly\" coords=\"355,143,381,141,381,146,381,151,381,155,381,159,380,163,380,168,379,171,377,174,375,179,374,181,373,184,350,180,351,175,353,168,354,162,354,155,354,149\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightElbowBack\" href=\"#\" shape=\"poly\" coords=\"475,186,496,181,498,184,499,188,500,191,502,195,504,199,478,205,478,200,477,196,476,190\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftElbowBack\" href=\"#\" shape=\"poly\" coords=\"350,182,372,186,371,189,370,192,370,196,370,200,370,204,343,199,346,194,347,190,349,185\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightForearmBack\" href=\"#\" shape=\"poly\" coords=\"478,206,504,200,506,203,507,206,507,210,507,215,508,222,508,226,509,232,510,235,510,239,511,243,512,248,497,253,496,248,492,241,489,234,485,228,482,220,480,211\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftForearmBack\" href=\"#\" shape=\"poly\" coords=\"342,201,369,206,369,209,368,212,366,218,364,222,362,226,360,230,359,234,356,238,355,242,352,245,351,249,350,252,334,247,336,240,338,232,339,223,339,216,339,209\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightWristBack\" href=\"#\" shape=\"poly\" coords=\"498,256,513,251,515,254,516,256,518,258,520,260,522,261,499,268,499,264,499,260\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftWristBack\" href=\"#\" shape=\"poly\" coords=\"334,249,350,254,349,257,348,259,348,261,348,264,348,268,325,260,328,258,331,257,333,254\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"lowerBack\" href=\"#\" shape=\"poly\" coords=\"382,190,464,189,464,195,463,203,463,207,463,212,462,218,463,223,464,227,466,232,468,237,470,240,377,242,379,236,381,231,383,228,383,223,384,213,384,206,384,199\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightButtockBack\" href=\"#\" shape=\"poly\" coords=\"427,242,470,242,473,247,473,251,474,256,475,260,476,266,476,271,475,276,476,280,476,284,475,289,475,292,427,294,427,290,426,288,424,288,423,284,423,279,422,272,422,268,422,263,424,256,426,250,427,247\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftButtockBack\" href=\"#\" shape=\"poly\" coords=\"376,244,425,242,425,244,425,247,424,250,424,252,422,256,422,260,420,264,420,267,420,269,420,272,421,276,421,279,421,282,422,286,421,289,420,292,420,294,371,295,371,288,371,281,371,273,371,267,371,260,373,255,374,250,375,247\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightThighBack\" href=\"#\" shape=\"poly\" coords=\"427,296,475,294,475,302,475,310,475,318,474,324,473,330,472,337,470,341,469,347,467,351,466,356,465,360,465,365,464,370,464,375,463,378,436,379,435,371,435,362,434,354,433,345,432,337,431,326,429,317,427,308\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftThighBack\" href=\"#\" shape=\"poly\" coords=\"371,296,419,295,419,300,419,305,418,311,418,318,417,323,415,330,414,337,414,342,413,348,413,353,413,358,413,363,412,369,411,374,411,377,384,378,383,370,382,361,380,353,378,344,375,336,373,326,373,316,371,307\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightKneeBack\" href=\"#\" shape=\"poly\" coords=\"436,381,464,380,464,384,463,388,463,390,463,393,463,397,464,402,465,406,467,410,437,410,439,405,440,402,440,397,439,393,439,389,437,384\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftKneeBack\" href=\"#\" shape=\"poly\" coords=\"384,381,410,380,409,383,409,386,408,390,407,392,407,396,407,399,408,404,409,409,379,410,381,405,383,400,383,396,384,391,384,385\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightLegBack\" href=\"#\" shape=\"poly\" coords=\"437,412,467,412,468,415,468,420,468,424,467,428,466,431,464,436,462,441,460,447,457,453,454,459,453,463,453,467,452,471,452,476,452,479,433,480,436,474,438,465,439,459,439,448,439,439,438,433,436,428,436,422,436,416\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftLegBack\" href=\"#\" shape=\"poly\" coords=\"379,412,410,411,411,414,411,418,411,421,410,426,410,430,409,434,408,441,408,446,408,451,409,456,409,461,410,465,410,468,411,473,412,477,413,481,394,482,394,476,395,471,394,466,392,458,389,452,386,446,382,439,380,431,379,425,379,416\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightAnkleBack\" href=\"#\" shape=\"poly\" coords=\"433,482,452,481,452,484,452,486,452,489,453,492,454,495,432,497,433,494,434,491,433,486,433,484\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftAnkleBack\" href=\"#\" shape=\"poly\" coords=\"413,483,414,486,414,489,414,492,414,495,415,498,392,498,393,495,395,492,395,489,394,486,396,483\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightFootBack\" href=\"#\" shape=\"poly\" coords=\"431,499,454,497,456,500,457,504,457,508,458,513,454,515,448,516,439,516,433,515,430,514,428,512,428,508\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftFootBack\" href=\"#\" shape=\"poly\" coords=\"391,500,415,500,417,503,418,507,419,510,419,513,415,515,408,516,398,515,392,515,388,513,389,509,390,505\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"rightHandBack\" href=\"#\" shape=\"poly\" coords=\"499,268,523,261,526,263,526,265,528,268,528,272,530,275,532,278,534,281,535,283,533,284,530,281,528,278,527,276,525,274,523,274,523,277,524,280,525,283,526,285,527,289,528,292,529,294,530,297,530,299,527,300,526,295,524,291,522,288,521,285,520,282,518,281,518,284,519,287,520,290,521,293,521,295,522,297,522,300,523,303,521,304,519,304,518,300,517,295,514,287,513,283,511,283,511,286,512,291,513,297,513,301,511,302,509,300,509,295,508,288,507,282,505,282,505,285,505,288,504,291,504,294,504,296,503,298,501,298,500,292,502,288,501,282,500,277,499,273\" />";
            bodyControlStartTag += "<area alt=\"\" title=\"leftHandBack\" href=\"#\" shape=\"poly\" coords=\"322,261,347,269,347,273,347,276,346,280,346,285,346,295,345,298,343,298,343,293,343,288,342,285,341,282,340,284,339,288,338,292,337,295,337,300,335,301,334,298,335,293,335,289,335,286,335,283,333,283,332,287,329,296,329,299,327,302,325,302,325,295,327,287,328,283,329,280,327,281,325,286,321,294,320,297,318,298,317,295,320,288,323,279,324,274,322,273,319,277,318,279,316,281,315,283,312,283,311,281,314,278,316,274,318,268,320,264\" />";
            bodyControlStartTag += "</map></td><td id=\"selectedBodyPartsBack\" width=\"150px\">";
            bodyControlStartTag += "<script>InitializePartsOfTheBody(); $(document).ready(function () {$(\"#selections\").dialog({autoOpen: false,position: { my: 'left+0', at: 'top+0' },";
            bodyControlStartTag += "resizable: false,title: '',closeOnEscape: true,draggable: false }); });</script></td></tr></table>";
            string bodyControlEndTag = "<div align=\"left\" style=\"clear: both; width: 540px; height: auto; border: 1px solid black;\" id=\"selections\"></div>";
            //string bodyControlEndTag = "";

            StringBuilder html = new StringBuilder();
            html.Append(format);
            html.Replace("<%ElementId%/>", element.Id.ToString());
            html.Replace("<%ActionId%/>", element.ActionId);
            QuestionnaireElementTextVersion text = element.TextVersions.Where(t => t.SupportsPlatform(this.currentDisplayPlatform) && t.SupportsInstance(this.instance)).SingleOrDefault();
            html.Replace("<%Text%/>", text == null ? string.Empty : this.FormatText(text.Text));
            html.Replace("<%OptionGroupText%/>", group == null ? string.Empty : group.TextVersions.ElementAt(0).Text);
            html.Replace("<%BodyMap%/>", bodyControlStartTag);
            html.Replace("<%OptionsForBodyPart%/>", bodyControlEndTag);
            if (element.GetType() == typeof(PCHI.Model.Questionnaire.QuestionnaireItem))
            {
                PCHI.Model.Questionnaire.QuestionnaireItem item = (PCHI.Model.Questionnaire.QuestionnaireItem)element;
                html.Replace("<%DisplayId%/>", this.FormatText(item.DisplayId));
            }

            return html.ToString();
        }

        /// <summary>
        /// Formats the Item Option Html text, replaces certain text elements with the appropriate text from an Element or Item Option Variable
        /// </summary>
        /// <param name="option">The Item Option to get the values from</param>
        /// <param name="format">The Format to modify</param>
        /// <returns>A formatted html text</returns>
        private string FormatOptionHtml(QuestionnaireItemOption option, string format)
        {
            StringBuilder html = new StringBuilder();
            html.Append(this.FormatElementHtml(option.Group.Item, format));
            html.Replace("<%OptionIdText%/>", this.FormatText(option.DisplayId));
            html.Replace("<%OptionText%/>", this.FormatText(option.Text));
            html.Replace("<%Value%/>", option.Value.ToString());
            html.Replace("<%ItemOptionId%/>", option.Id.ToString());
            html.Replace("<%GroupId%/>", option.Group.Id.ToString());

            return html.ToString();
        }

        /// <summary>
        /// Formats the text to be HTML compliant.
        /// Newlines are converted to the Html equivalent and Html specifica characters are converted
        /// </summary>
        /// <param name="text">The text to format</param>
        /// <returns>The formatted text</returns>
        private string FormatText(string text)
        {
            if (text == null) return string.Empty;
            return text.Replace("\n", "<br />");
        }
        #endregion
    }
}