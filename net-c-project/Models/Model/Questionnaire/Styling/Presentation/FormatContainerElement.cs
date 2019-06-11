using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Styling.Presentation
{
    /// <summary>
    /// Defintes an element that belongs to a specific format container
    /// </summary>
    public class FormatContainerElement
    {
        /// <summary>
        /// Gets or sets the database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Item Formate Container for this Item
        /// </summary>
        public virtual FormatContainer FormatContainer { get; set; }

        /// <summary>
        /// Gets or sets the action Id of the QuestionnaireElement that belongs to this section
        /// </summary>        
        public string QuestionnaireElementActionId { get; set; }

        /// <summary>
        /// Gets or sets the order of the elements in a section
        /// </summary>
        public int OrderInSection { get; set; }
    }
}
