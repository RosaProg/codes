using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a Text Version for the questionnaire introduction messages
    /// </summary>
    public class QuestionnaireIntroductionMessage : TextVersion
    {
        /// <summary>
        /// Gets or sets the <see cref="Questionnaire"/> this Introduction Message belongs to
        /// </summary>
        public virtual Questionnaire Questionnaire { get; set; }
    }
}
