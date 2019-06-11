using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Instructions
{
    /// <summary>
    /// Used for mapping Instructions to Sections
    /// </summary>
    public class QuestionnaireSectionInstruction : QuestionnaireInstruction
    {
        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireSection"/> this instruction belongs to
        /// </summary>
        [Required(ErrorMessage = "A section instruction must belong to a section")]
        public QuestionnaireSection Section { get; set; }
    }
}
