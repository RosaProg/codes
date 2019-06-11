using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// Defines a display format that can be used for formatting a Questionnaires
    /// </summary>
    public class Format
    {
        /// <summary>
        /// Gets or sets the name for this format
        /// </summary>
        [Column(Order = 0), Key]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform(s) this Format supports
        /// </summary>
        [Column(Order = 1), Key]
        public Platform SupportedPlatform { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar is to be shown or not.
        /// Only applies to Formats that actually can display a progress bar such as the chat version.
        /// </summary>
        public QuestionnaireFormatAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets the sections for this format
        /// </summary>
        public virtual ICollection<FormatContainer> Containers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class
        /// </summary>
        public Format()
        {
            this.Containers = new List<FormatContainer>();
            this.Attributes = QuestionnaireFormatAttributes.None;
        }

        /// <summary>
        /// Sets the given attributes on this Format
        /// </summary>
        /// <param name="attributes">The QuestionnaireFormatAttributes to set</param>
        public void SetAttributes(params QuestionnaireFormatAttributes[] attributes)
        {
            if (attributes.Length == 0) this.Attributes = QuestionnaireFormatAttributes.None;
            this.Attributes = attributes[0];
            for (int i = 1; i < attributes.Length; i++)
            {
                this.Attributes = this.Attributes | attributes[i];
            }
        }

        /// <summary>
        /// Checks if this Format has the specified attribute
        /// </summary>
        /// <param name="attribute">The QuestionnaireFormatAttributes to look for</param>
        /// <returns>True if the attribute is set, false otherwise</returns>
        public bool HasAttribute(QuestionnaireFormatAttributes attribute)
        {
            return this.Attributes.HasFlag(attribute);
        }

        /// <summary>
        /// Gets all QuestionnaireFormatAttributes set on this Format.
        /// </summary>
        /// <returns>The list of QuestionnaireFormatAttributes set</returns>
        public List<QuestionnaireFormatAttributes> GetAttributes()
        {
            Array values = Enum.GetValues(typeof(QuestionnaireFormatAttributes));
            List<QuestionnaireFormatAttributes> result = new List<QuestionnaireFormatAttributes>();
            foreach (QuestionnaireFormatAttributes item in values)
            {
                if (this.Attributes.HasFlag(item)) result.Add(item);
            }

            return result;
        }
    }
}
