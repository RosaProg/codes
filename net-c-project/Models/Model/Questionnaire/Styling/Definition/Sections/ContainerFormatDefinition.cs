using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.Sections
{
    /// <summary>
    /// Holds the format definition for a format container
    /// </summary>
    public class ContainerFormatDefinition
    {
        /// <summary>
        /// Gets or sets the unique name for this section
        /// </summary>
        [Key]
        [MaxLength(250)]
        public string ContainerDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the parent section this section belongs to
        /// </summary>
        public virtual ContainerFormatDefinition ParentContainer { get; set; }

        /// <summary>
        /// Gets or sets the end tags to be printed on the screen before processing starts
        /// </summary>
        public string StartHtml { get; set; }

        /// <summary>
        /// Gets or sets the end tags to be printed on the screen after processing is complet
        /// </summary>
        public string EndHtml { get; set; }

        /// <summary>
        ///  Gets or sets after how many elements the End and then Start tag are to be repeated.
        /// </summary>
        public int StartEndRepeat { get; set; }

        /// <summary>
        /// Gets or sets the child sections that belong to this section
        /// </summary>
        public virtual ICollection<ContainerFormatDefinition> ChildContainers { get; set; }

        /// <summary>
        /// Gets or sets the child elements that belong to this section
        /// </summary>
        public virtual ICollection<ElementFormatDefinition> ElementFormatDefinitions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerFormatDefinition"/> class
        /// </summary>
        public ContainerFormatDefinition()
        {
            this.ChildContainers = new List<ContainerFormatDefinition>();
            this.ElementFormatDefinitions = new List<ElementFormatDefinition>();
        }
    }
}
