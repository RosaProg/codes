using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Instructions
{
    /// <summary>
    /// Used for mapping Instructions to Items
    /// </summary>
    public class QuestionnaireItemInstruction : QuestionnaireInstruction
    {
        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireItem"/> this instruction belongs to
        /// </summary>
        [Required(ErrorMessage = "An item instruction must belong to an item")]
        public QuestionnaireItem Item { get; set; }
    }
}
