using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.Elements
{
    /// <summary>
    /// The abstract base class for all Element Format Definitions
    /// </summary>
    [KnownType(typeof(ItemFormatDefinition))]
    [KnownType(typeof(TextFormatDefinition))]
    public abstract class ElementFormatDefinition
    {
        /// <summary>
        /// Gets or sets the name of this ElementFormatDefinition
        /// </summary>
        [Key]
        [MaxLength(250)]
        public string ElementFormatDefinitionName { get; set; }
        
        /// <summary>
        /// Gets or sets the Html to be printed at the start of the printing of an Item
        /// </summary>
        public string StartHtml { get; set; }
                
        /// <summary>
        /// Gets or sets the Html text to be printed for this item
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the Html to be printed at the end of the printing of an Item
        /// </summary>
        public string EndHtml { get; set; }

        /// <summary>
        /// Gets or sets the foreign key name for the <see cref="ContainerFormatDefinition"/>
        /// </summary>
        [ForeignKey("ContainerFormatDefinition")]
        public string ContainerDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ContainerFormatDefinition"/> this definition belongs to
        /// </summary>
        public virtual ContainerFormatDefinition ContainerFormatDefinition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementFormatDefinition"/> class
        /// </summary>
        public ElementFormatDefinition()
        {
        }       
    }
}
