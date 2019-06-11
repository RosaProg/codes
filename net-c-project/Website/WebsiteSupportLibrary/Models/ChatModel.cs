using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteSupportLibrary.Models
{
    public class QuestionnaireModel
    {
        public string Title { get; set; }
        public string QuestionnaireId { get; set; }
        public string GroupId { get; set; }
        public string Anonymous { get; set; }
        public int CurrentItem { get; set; }
        public List<QuestionnaireItem> Items { get; set; }
        public bool IsPro { get; set; }


        // Format properties
        public bool ShowProgressBar { get; set; }
        public bool CanSavePartial { get; set; }        


        public QuestionnaireModel()
        {
            this.Items = new List<QuestionnaireItem>();
        }

        public void BuildModel()
        {

        }
    }

    public class QuestionnaireItem
    {
        public string ActionId { get; set; }
        public string Status { get; set; }
        public Question Question { get; set; }
        [AllowHtml]
        public ResponsePanel ResponsePanel { get; set; }
        public Answer Answer { get; set; }
        public string AnsweredStatus { get; set; }

        public List<string> ItemNames { get; set; }
        public List<PossibleAnswers> PossibleAnswers { get; set; }
        public List<string> AnswerNames { get; set; }
        public List<string> AnswerValues { get; set; }
        [AllowHtml]
        public string AnswerAction { get; set; }
        
        public QuestionnaireItem()
        {
            this.ItemNames = new List<string>();
            this.PossibleAnswers = new List<PossibleAnswers>();
        }
    }

    public class Question
    {
        [AllowHtml]
        public string HtmlContent { get; set; }
    }

    public class ResponsePanel
    {
        [AllowHtml]
        public string HtmlContent { get; set; }
        
        // Format properties
        public bool CanSkip { get; set; }
        public bool PreventEdit { get; set; }
    }

    public class Answer
    {
        [AllowHtml]
        public string HtmlTemplate { get; set; }
        public string Value { get; set; }
    }

    public class PossibleAnswers
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string AnswerText { get; set; }
        public string Action { get; set; }
    }

    public enum ItemStatus
    {
        Future,
        Historical,
        Current
    }


    public enum ItemAnsweredStatus
    {
        NotAnswered,
        Answered,
        Skipped
    }
}