using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Describes a Text version for the Questionnaire Descriptions
    /// </summary>
    public class QuestionnaireDescription : TextVersion
    {
        /// <summary>
        /// Gets or sets the <see cref="Questionnaire"/> this Introduction Message belongs to
        /// </summary>
        public virtual Questionnaire Questionnaire { get; set; }
    }
}
