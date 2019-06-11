using PCHI.Model.Questionnaire.Instructions;
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
    /// Defines a section inside a <see cref="Questionnaire"/>
    /// This may be used for Display purposes but may in some cases also be ignored.
    /// </summary>
    public class QuestionnaireSection
    {
        /// <summary>
        /// Gets or sets the database Id of <see cref="QuestionnaireSection"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Action Id of this <see cref="QuestionnaireSection"/> that is used for referencing to this section
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the Instrument this <see cref="QuestionnaireSection"/> belongs to
        /// </summary>
        [Required(ErrorMessage = "A section must belong to a questionnaire")]
        public virtual Questionnaire Questionnaire { get; set; }

        /// <summary>
        /// Gets or sets the order of this <see cref="QuestionnaireSection"/> within the list of sections belonging to the instrument
        /// It is recommended to use Orders of 10 (e.g. 10, 20, 30, 40) to define the sections initialy, that makes it easier to move sections around later if needed.
        /// This order is calculated upon storage and should not be set manually.
        /// </summary>
        public int OrderInInstrument { get; set; }

        /// <summary>
        /// Gets or sets the ICollection of <see cref="QuestionnaireSectionInstruction"/>s used        
        /// </summary>
        public ICollection<QuestionnaireSectionInstruction> Instructions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireElement"/>s that belong to this section
        /// </summary>
        public virtual ICollection<QuestionnaireElement> Elements { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireSection"/> class
        /// </summary>
        public QuestionnaireSection()
        {
            this.Elements = new List<QuestionnaireElement>();
            this.Instructions = new List<QuestionnaireSectionInstruction>();
        }
    }
}
