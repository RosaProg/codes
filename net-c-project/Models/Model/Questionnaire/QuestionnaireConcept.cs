using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a Concept of a questionnaire
    /// </summary>
    public class QuestionnaireConcept
    {
        /// <summary>
        /// Gets or sets the database Id of the <see cref="QuestionnaireConcept"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Short name of the <see cref="QuestionnaireConcept"/>
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the <see cref="QuestionnaireConcept"/>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireConcept"/> class
        /// </summary>
        public QuestionnaireConcept()
        {
        }
    }
}
