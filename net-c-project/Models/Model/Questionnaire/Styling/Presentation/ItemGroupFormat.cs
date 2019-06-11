using PCHI.Model.Questionnaire.Styling.Definition;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// Determines the ItemGroupOptionsFormatDefinition to be used for a specific Response Type
    /// </summary>
    public class ItemGroupFormat
    {
        /// <summary>
        /// Gets or sets the Database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Section this format belongs to
        /// </summary>
        public virtual ItemFormatContainer ItemFormatContainer { get; set; }
                
        /// <summary>
        /// Gets or sets the Response type this format is for
        /// </summary>
        public QuestionnaireResponseType ResponseType { get; set; }

        /// <summary>
        /// Gets or sets the foreign key of the Item Group Options Format Definition to use.
        /// Alternatively you can use the ItemGroupOptionsFormatDefinition object for saving this Format
        /// </summary>
        [ForeignKey("ItemGroupOptionsFormatDefinition")]
        public string ItemGroupOptionsFormatDefinition_Name { get; set; }

        /// <summary>
        /// Gets or sets the Format Definition to use
        /// </summary>
        public virtual ItemGroupOptionsFormatDefinition ItemGroupOptionsFormatDefinition { get; set; } 
    }
}
