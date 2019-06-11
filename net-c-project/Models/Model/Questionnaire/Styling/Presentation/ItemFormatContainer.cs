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
    /// Holds an Item Format specification. Including the Definition for that item and the QuestionnaireItem elements that are part of it
    /// </summary>
    public class ItemFormatContainer : FormatContainer
    {
        /// <summary>
        /// Gets or sets the foreign key of the Item Format Definition to use.
        /// Alternatively you can use the ItemFormatDefinition object for saving this container
        /// </summary>
        [ForeignKey("ItemFormatDefinition")]
        public string ItemFormatDefinition_Name { get; set; }

        /// <summary>
        /// Gets or sets the ItemFormatDefinition used for the Items in this section
        /// </summary>
        public virtual ItemFormatDefinition ItemFormatDefinition { get; set; }

        /// <summary>
        /// Gets or sets the collection of Groups formatting for each Questionnaire ResponseType
        /// </summary>
        public virtual ICollection<ItemGroupFormat> ItemGroupFormats { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemFormatContainer"/> class
        /// </summary>
        public ItemFormatContainer() : base()
        {
            this.ItemGroupFormats = new List<ItemGroupFormat>();            
        }
    }
}
