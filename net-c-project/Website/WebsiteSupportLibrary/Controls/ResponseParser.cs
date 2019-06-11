using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteSupportLibrary.Models
{
    public class ResponseParser
    {
        public static List<QuestionnaireResponse> ParseResponses(Dictionary<string, string> formCollection)
        {
            List<QuestionnaireResponse> questionnaireResponses = new List<QuestionnaireResponse>();
            foreach (string key in formCollection.Keys)
            {
                if (key.Contains("."))
                {
                    string[] keyValues = key.Split('.');
                    switch (keyValues[0])
                    {
                        case "Radio":
                        case "DropDown":
                            string[] values = formCollection[key].Split('.');
                            int questionnaireItemId = Convert.ToInt32(keyValues[1]);
                            int questionnaireItemOptionId = Convert.ToInt32(values[0]);
                            double responseValue = Convert.ToDouble(values[1]);
                            QuestionnaireResponse response = new QuestionnaireResponse() 
                            {
                                Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId }, 
                                Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                ResponseValue = responseValue 
                            };
                            questionnaireResponses.Add(response);
                            break;
                        case "Slider":
                            questionnaireItemId = Convert.ToInt32(keyValues[1]);
                            responseValue = Convert.ToDouble(formCollection[key]);
                            questionnaireItemOptionId = Convert.ToInt32(keyValues[3]);
                            response = new QuestionnaireResponse() 
                            {
                                Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId },
                                Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                ResponseValue = responseValue,
                                ResponseText = null
                            };
                            questionnaireResponses.Add(response);
                            break;
                        case "CheckBox":
                            values = formCollection[key].Split('.');
                            questionnaireItemId = Convert.ToInt32(keyValues[1]);
                            questionnaireItemOptionId = Convert.ToInt32(values[0]);
                            string responseText;
                            if (Double.TryParse(values[1], out responseValue))
                                response = new QuestionnaireResponse()
                                {
                                    Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId },
                                    Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                    ResponseValue = responseValue
                                };
                            else
                            {
                                responseText = values[1];
                                response = new QuestionnaireResponse()
                                {
                                    Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId },
                                    Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                    ResponseText = responseText
                                };
                            }
                                
                            
                            questionnaireResponses.Add(response);
                            break;
                        case "DatePicker":
                        case "TextBox":
                        case "TextArea":
                            questionnaireItemId = Convert.ToInt32(keyValues[1]);
                            questionnaireItemOptionId = Convert.ToInt32(keyValues[3]);
                            responseText = formCollection[key];
                            response = new QuestionnaireResponse() 
                            {
                                Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId },
                                Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                ResponseText = responseText,
                                ResponseValue = null
                            };
                            questionnaireResponses.Add(response);
                            break;
                        case "HiddenCheckBox":
                            values = formCollection[key].Split('.');
                            questionnaireItemId = Convert.ToInt32(keyValues[1]);
                            questionnaireItemOptionId = Convert.ToInt32(keyValues[3]);
                            responseText = keyValues[2] + "." + values[0];
                            response = new QuestionnaireResponse()
                            {
                                Item = new PCHI.Model.Questionnaire.QuestionnaireItem() { Id = questionnaireItemId },
                                Option = new QuestionnaireItemOption() { Id = questionnaireItemOptionId },
                                ResponseText = responseText,
                                ResponseValue = null
                            };
                            questionnaireResponses.Add(response);
                            break;
                        default:
                            // TODO for the other option types there are
                            break;
                    }
                }
            }

            return questionnaireResponses;
        }
    }
}