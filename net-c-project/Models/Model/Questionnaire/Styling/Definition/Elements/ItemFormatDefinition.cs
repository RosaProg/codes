using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Definition.Elements
{
    /// <summary>
    /// Specifies the Item format definition
    /// </summary>
    public class ItemFormatDefinition : ElementFormatDefinition
    {
        /// <summary>
        /// Gets or sets the ItemGroupOptionsFormatDefinitions used
        /// </summary>
        public virtual ICollection<ItemGroupOptionsFormatDefinition> ItemGroupOptionsFormatDefinitions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemFormatDefinition"/> class
        /// </summary>
        public ItemFormatDefinition()
        {
            this.ItemGroupOptionsFormatDefinitions = new List<ItemGroupOptionsFormatDefinition>();
        }
    }
}
