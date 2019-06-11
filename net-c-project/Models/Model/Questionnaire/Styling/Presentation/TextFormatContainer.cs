using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// The container for formatting a set of Text Elements
    /// </summary>
    public class TextFormatContainer : FormatContainer
    {
        /// <summary>
        /// Gets or sets the foreign key of the Text Format Definition to use.
        /// Alternatively you can use the TextFormatDefinition object for saving this container
        /// </summary>
        [ForeignKey("TextFormatDefinition")]
        public string TextFormatDefinition_Name { get; set; }

        /// <summary>
        /// Gets or sets the ItemFormatDefinition used for the Items in this section
        /// </summary>
        public virtual TextFormatDefinition TextFormatDefinition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatContainer"/> class
        /// </summary>
        public TextFormatContainer() : base()
        {
        }
    }
}
