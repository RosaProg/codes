using PCHI.Model.Questionnaire.Styling.Definition;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// Gets or sets a Format Container that specifies how to display a section on the screen
    /// </summary>
    [KnownType(typeof(ItemFormatContainer))]
    [KnownType(typeof(TextFormatContainer))]
    public class FormatContainer
    {
        /// <summary>
        /// Gets or sets the database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireFormat"/> this is part of
        /// </summary>               
        public virtual Format QuestionnaireFormat { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FormatContainer"/> this belongs to
        /// </summary>
        public virtual FormatContainer Parent { get; set; }

        /// <summary>
        /// Gets or sets the order in which this Container appears inside it's parent container or format
        /// </summary>
        public int OrderInParent { get; set; }

        /// <summary>
        /// Gets or sets the QuestionnaireFormatSection children this belongs to
        /// </summary>
        public virtual ICollection<FormatContainer> Children { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of the Container Format Definition to use.
        /// Alternatively you can use the ContainerFormatDefinition object for saving this container
        /// </summary>
        [ForeignKey("ContainerFormatDefinition")]
        public string ContainerFormatDefinition_Name { get; set; }

        /// <summary>
        /// Gets or sets the Section Format Definition
        /// </summary>
        public virtual ContainerFormatDefinition ContainerFormatDefinition { get; set; }

        /// <summary>
        /// Gets or sets the Questionnaire Elements that belong to this section
        /// </summary>        
        public virtual ICollection<FormatContainerElement> Elements { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatContainer"/> class
        /// </summary>
        public FormatContainer()
        {
            this.Children = new List<FormatContainer>();
            this.Elements = new List<FormatContainerElement>();
        }
    }
}
