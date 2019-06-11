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
    /// This is used for separating multiple response to the same Item in different groups.
    /// One response of any of the groups must then be chosen.
    /// </summary>
    public class QuestionnaireItemOptionGroup
    {
        /// <summary>
        /// Gets or sets the database Id of this <see cref="QuestionnaireItemOptionGroup"/>
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the Action Id of the Option Group
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the text versions available for this Element
        /// </summary>        
        public virtual ICollection<QuestionnaireItemOptionGroupTextVersion> TextVersions { get; set; }

        /// <summary>
        /// Gets or sets the order of this <see cref="QuestionnaireItemOptionGroup"/> inside an Item
        /// </summary>
        public int OrderInItem { get; set; }

        /// <summary>
        /// Gets or sets the type of response expected for this <see cref="QuestionnaireItemOptionGroup"/>
        /// </summary>
        public QuestionnaireResponseType ResponseType { get; set; }

        /// <summary>
        /// Gets or sets the size of the steps to take when the Response Type is Range between the lowest and highest value of the options.
        /// </summary>
        public double RangeStep { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireItem"/> this Group<see cref="QuestionnaireItemOptionGroup"/> belongs to
        /// </summary>
        [Required(ErrorMessage = "An option group must belong to an item")]
        public virtual QuestionnaireItem Item { get; set; }

        /// <summary>
        /// Gets or sets the default value for the Options in the group.        
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the Collection of <see cref="QuestionnaireItemOption"/>s that belong to this group
        /// </summary>
        public virtual ICollection<QuestionnaireItemOption> Options { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireItemOptionGroup"/> class
        /// </summary>
        public QuestionnaireItemOptionGroup()
        {
            this.Options = new List<QuestionnaireItemOption>();
            this.TextVersions = new List<QuestionnaireItemOptionGroupTextVersion>();
        }
    }
}
