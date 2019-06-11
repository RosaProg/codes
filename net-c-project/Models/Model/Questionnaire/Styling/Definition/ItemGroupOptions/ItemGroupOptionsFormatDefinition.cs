using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions
{
    /// <summary>
    /// Defines a format definition for the item group options
    /// </summary>
    public class ItemGroupOptionsFormatDefinition
    {
        /// <summary>
        /// Gets or sets the name of this format definition
        /// </summary>
        [Key]
        [MaxLength(250)]
        public string GroupOptionDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the foreign key name for the <see cref="ItemFormatDefinition"/>
        /// </summary>
        [ForeignKey("ItemFormatDefinition")]
        public string ItemFormatDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the ItemFormatDefinition this Group Options definition can be used with.
        /// </summary>
        public virtual ItemFormatDefinition ItemFormatDefinition { get; set; }

        /// <summary>
        /// Gets or sets the Html to be placed at the start of the processing of an Options Group
        /// </summary>
        public string StartHtml { get; set; }

        /// <summary>
        /// Gets or sets the Html to be places at the end of the processing of an Options Group
        /// </summary>
        public string EndHtml { get; set; }

        /// <summary>
        /// Gets or sets the Html to be placed at the start of Each entry in the ForEachOption list
        /// </summary>
        public string ForEachOptionStart { get; set; }

        /// <summary>
        /// Gets or sets the Html to be placed at the end of Each entry in the ForEachOption list
        /// </summary>
        public string ForEachOptionEnd { get; set; }

        /// <summary>
        /// Gets or sets each line that has to be formatted and printed for each option.
        /// All the options are processed for a line before the next line in this list is processed.
        /// </summary>
        public virtual ICollection<ItemGroupOptionsForEachOptionDefinition> ForEachOption { get; set; }

        /// <summary>
        /// Gets or sets how the Anchors are supposed to be displayed.
        /// This is only used when <see cref="QuestionnaireItemOptionGroup.ResponseType"/> is range
        /// </summary>
        public AnchorDisplay AnchorDisplay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemGroupOptionsFormatDefinition"/> class
        /// </summary>
        public ItemGroupOptionsFormatDefinition()
        {
            this.ForEachOption = new List<ItemGroupOptionsForEachOptionDefinition>();
        }
    }
}
