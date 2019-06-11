using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteSupportLibrary.Common
{

    //public class ProInstrument : Questionnaire
    //{   
    //    public virtual ICollection<ProDomain> Domains { get; set; }

    //    public ProInstrument()
    //        : base()
    //    {
    //        this.Domains = new List<ProDomain>();
    //    }
    //}

    //public class ProDomain
    //{

    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }        
    //    public virtual Questionnaire Instrument { get; set; }
    //    public string ScoreFormula { get; set; }
    //    public virtual List<ProDomainResultRange> ResultRanges { get; set; }
    //    public UserTypes AppliesTo { get; set; }

    //    public ProDomain()
    //    {
    //        this.AppliesTo = UserTypes.Patient | UserTypes.Physician;
    //        this.ResultRanges = new List<ProDomainResultRange>();
    //    }
    //}

    //public class ProDomainResultRange
    //{
    //    public int Id { get; set; }
    //    public double Start { get; set; }
    //    public double End { get; set; }
    //    public string Meaning { get; set; }
    //    public virtual ProDomain Domain { get; set; }
    //    public ProDomainResultRange()
    //    {

    //    }
    //}


    //public abstract class Questionnaire
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public QuestionnaireStatus Status { get; set; }
    //    public virtual QuestionnaireConcept Concept { get; set; }
    //    public bool IsActive { get; set; }
    //    public virtual List<QuestionnaireSection> Sections { get; set; }
    //    public List<Tag> Tags { get; set; }

    //    public Questionnaire()
    //    {
    //        this.IsActive = true;
    //        this.Sections = new List<QuestionnaireSection>();
    //        this.Tags = new List<Tag>();
    //    }
    //}

    //public class QuestionnaireConcept
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }

    //    public QuestionnaireConcept()
    //    {
    //    }
    //}

    //public class QuestionnaireInstruction
    //{        
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //}

    //public class QuestionnaireSection
    //{
    //    public int Id { get; set; }
    //    public string ActionId { get; set; }        
    //    public int OrderInInstrument { get; set; }
    //    public List<QuestionnaireSectionInstruction> Instructions { get; set; }
    //    public virtual List<QuestionnaireElement> Elements { get; set; }

    //    public QuestionnaireSection()
    //    {
    //        this.Elements = new List<QuestionnaireElement>();
    //        this.Instructions = new List<QuestionnaireSectionInstruction>();
    //    }
    //}

    //public class QuestionnaireSectionInstruction : QuestionnaireInstruction
    //{
    //    public QuestionnaireSection Section { get; set; }
    //}

        
    //public class QuestionnaireText : QuestionnaireElement
    //{
    //}

    //public abstract class QuestionnaireElement
    //{       
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //    public string ActionId { get; set; }
    //    public int OrderInSection { get; set; }        
    //}


    //public class Tag
    //{
    //    public string TagName { get; set; }
    //    public string Value { get; set; }
    //}


    //public enum QuestionnaireStatus
    //{
    //    Indevelopment,
    //    Ready,
    //    Validated,
    //}

    //public enum QuestionnaireResponseType
    //{
    //    Range,
    //    ConditionalItem,
    //    List,
    //    Text
    //}

    //public class QuestionnaireItem : QuestionnaireElement
    //{        
    //    public string DisplayId { get; set; }
    //    public string SummaryText { get; set; }
    //    public bool IsMandatory { get; set; }
    //    public virtual List<QuestionnaireItemOptionGroup> OptionGroups { get; set; }
    //    public bool IsInQuestionnaireDefinition { get; set; }
    //    public List<QuestionnaireItemInstruction> Instructions { get; set; }

    //    public QuestionnaireItem()
    //    {
    //        this.OptionGroups = new List<QuestionnaireItemOptionGroup>();
    //        this.Instructions = new List<QuestionnaireItemInstruction>();
    //    }
    //}


    //public class QuestionnaireItemOptionGroup
    //{
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //    public int OrderInItem { get; set; }
    //    public QuestionnaireResponseType ResponseType { get; set; }
    //    public int RangeStep { get; set; }        
    //    public List<QuestionnaireItemOption> Options { get; set; }
    //    public QuestionnaireItemOptionGroup()
    //    {
    //        this.Options = new List<QuestionnaireItemOption>();
    //    }
    //}

    //public class QuestionnaireItemInstruction : QuestionnaireInstruction
    //{
    //    public QuestionnaireItem Item { get; set; }
    //}

    //public class QuestionnaireItemOption
    //{
    //    public int Id { get; set; }
    //    public string OptionIdText { get; set; }
    //    public string Text { get; set; }
    //    public double Value { get; set; }
    //    public string Action { get; set; }        
    //}


    //public enum UserTypes
    //{        
    //    Patient = 1,
    //    Physician = 2
    //}
}