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
    /// Holds each line of text for the Item Group Options for each list
    /// </summary>
    public class ItemGroupOptionsForEachOptionDefinition
    {
        /// <summary>
        /// Gets or sets the database Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key name to the <see cref="ItemGroupOptionsFormatDefinition"/>
        /// </summary>
        [ForeignKey("ItemGroupOptionsFormatDefinition")]
        public string GroupOptionDefinitionName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ItemGroupOptionsFormatDefinition"/> this belongs to
        /// </summary>        
        [Required]
        public virtual ItemGroupOptionsFormatDefinition ItemGroupOptionsFormatDefinition { get; set; }

        /// <summary>
        /// Gets or sets the order of this ForEachOption insides it's groupOptions definition
        /// </summary>
        public int OrderInItemGroupOptionsFormatDefinition { get; set; }

        /// <summary>
        /// Gets or sets the text (html/normal) definition to use before the ItemOptionDisplayType
        /// </summary>
        public string StartText { get; set; }

        /// <summary>
        /// Gets or sets how to display the options
        /// </summary>
        public ItemOptionDisplayType ItemOptionDisplayType { get; set; }

        /// <summary>
        /// Gets or sets the text (html/normal) definition to use after the ItemOptionDisplayType
        /// </summary>
        public string EndText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemGroupOptionsForEachOptionDefinition"/> class
        /// </summary>
        public ItemGroupOptionsForEachOptionDefinition()
        {
            this.ItemOptionDisplayType = ItemOptionDisplayType.None;
        }
    }
}
