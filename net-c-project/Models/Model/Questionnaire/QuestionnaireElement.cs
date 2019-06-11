using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines the basis for any Element that can be part of a <see cref="QuestionnaireSection"/>
    /// This class can't be instantiated but must be Inherited
    /// </summary>
    [KnownType(typeof(QuestionnaireText))]
    [KnownType(typeof(QuestionnaireItem))]
    public abstract class QuestionnaireElement
    {
        /// <summary>
        /// Gets or sets the database Id of this Element
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text versions available for this Element
        /// </summary>        
        public virtual ICollection<QuestionnaireElementTextVersion> TextVersions { get; set; }

        /// <summary>
        /// Gets or sets the ID to be used by Actions to target this Element or possibly within the Domain formula as well
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the Order of this item with the section
        /// </summary>
        public int OrderInSection { get; set; }

        /// <summary>
        /// Gets or sets the section this item belongs to
        /// </summary>
        [Required(ErrorMessage = "A Pro Element must belong to a section")]
        public virtual QuestionnaireSection Section { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireElement"/> class
        /// </summary>
        public QuestionnaireElement()
        {
            this.TextVersions = new List<QuestionnaireElementTextVersion>();
        }
    }
}
