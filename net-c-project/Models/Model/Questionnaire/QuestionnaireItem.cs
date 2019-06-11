using PCHI.Model.Questionnaire.Instructions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a  Question, Statement or Task within a Pro
    /// </summary>
    public class QuestionnaireItem : QuestionnaireElement
    {
        /// <summary>
        /// Gets or sets the DisplayId (e.g. Question Number) for this item
        /// </summary>
        public string DisplayId { get; set; }

        /// <summary>
        /// Gets or sets the Summary of a QuestionnaireItem.
        /// This is a short version of the Text
        /// </summary>
        [MaxLength(30)]
        public string SummaryText { get; set; }
        
        /// <summary>
        /// Gets or sets the Option Groups that belong to this section
        /// </summary>
        public virtual ICollection<QuestionnaireItemOptionGroup> OptionGroups { get; set; }

        /// <summary>
        /// Gets or sets the ICollection of Instructions used        
        /// </summary>
        public ICollection<QuestionnaireItemInstruction> Instructions { get; set; }

        /// <summary>
        /// Gets or sets the attributes for a QuestionnaireItem
        /// </summary>
        public QuestionnaireItemAttributes Attributes { get; set; }
        
        /// <summary>
        /// Gets or sets the clarification for scoring clarification such as "Higher is Better"
        /// Only applies to PROs
        /// </summary>
        public string ScoringNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Higher is a better value (true) or not (false)
        /// Only applies to PROs
        /// </summary>
        public bool HigherIsBetter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireItem"/> class
        /// </summary>
        public QuestionnaireItem()
        {
            this.OptionGroups = new List<QuestionnaireItemOptionGroup>();
            this.Instructions = new List<QuestionnaireItemInstruction>();
            this.Attributes = QuestionnaireItemAttributes.None;
        }

        /// <summary>
        /// Sets the given attributes on this item
        /// </summary>
        /// <param name="attributes">The QuestionnaireItemAttributes to set</param>
        public void SetAttributes(params QuestionnaireItemAttributes[] attributes)
        {
            if (attributes.Length == 0) this.Attributes = QuestionnaireItemAttributes.None;
            this.Attributes = attributes[0];
            for (int i = 1; i < attributes.Length; i++)
            {
                this.Attributes = this.Attributes | attributes[i];
            }
        }

        /// <summary>
        /// Checks if this Item has the specified attribute
        /// </summary>
        /// <param name="attribute">The QuestionnaireItemAttributes to look for</param>
        /// <returns>True if the attribute is set, false otherwise</returns>
        public bool HasAttribute(QuestionnaireItemAttributes attribute)
        {
            return this.Attributes.HasFlag(attribute);
        }

        /// <summary>
        /// Gets all QuestionnaireItemAttributes set on this item.
        /// </summary>
        /// <returns>The list of QuestionnaireItemAttributes set</returns>
        public List<QuestionnaireItemAttributes> GetAttributes()
        {
            Array values = Enum.GetValues(typeof(QuestionnaireItemAttributes));
            List<QuestionnaireItemAttributes> result = new List<QuestionnaireItemAttributes>();
            foreach (QuestionnaireItemAttributes item in values)
            {
                if (this.Attributes.HasFlag(item)) result.Add(item);
            }

            return result;
        }
    }
}
