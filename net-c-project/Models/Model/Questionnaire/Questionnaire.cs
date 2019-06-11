using PCHI.Model.Questionnaire.Pro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a base class for all Questionnaire Types. This is the start to getting all the items, response and calculations to show
    /// </summary>
    [KnownType(typeof(ProInstrument))]
    [KnownType(typeof(PCHI.Model.Questionnaire.Survey.Survey))]
    public abstract class Questionnaire
    {
        /// <summary>
        /// Gets or sets the database Id of the <see cref="Questionnaire"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Questionnaire"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Pro Instrument must have a name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the Questionnaire to Display
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the default name of the format to use when loading the a Questionnaire
        /// </summary>
        [MaxLength(50)]
        public string DefaultFormatName { get; set; }

        /// <summary>
        /// Gets or sets the status of the <see cref="Questionnaire"/>
        /// </summary>
        public QuestionnaireStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireConcept"/> this <see cref="Questionnaire"/> is for
        /// </summary>
        [Required(ErrorMessage = "A Pro Instrument needs to be part of a Pro Concept")]
        public virtual QuestionnaireConcept Concept { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this <see cref="Questionnaire"/> is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireSection"/> that belong to this Instrument. This lead to the Questionnaire Items and responses
        /// </summary>
        public virtual ICollection<QuestionnaireSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets the list of Introduction messages that belong to this Questionnaire
        /// </summary>
        public virtual ICollection<QuestionnaireIntroductionMessage> IntroductionMessages { get; set; }

        /// <summary>
        /// Gets or sets teh list of Descriptions for this questionnaire
        /// </summary>
        public virtual ICollection<QuestionnaireDescription> Descriptions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Tag"/> that belong to this Instrument. This lead to the Tag        
        /// </summary>
        [NotMapped]
        public List<Tag.Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the current instance to use
        /// This is set upon selecting the questionnaire to load
        /// </summary>
        [NotMapped]
        public Instance CurrentInstance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Questionnaire"/> class
        /// </summary>
        public Questionnaire()
        {
            this.IsActive = true;
            this.Sections = new List<QuestionnaireSection>();
            this.Tags = new List<Tag.Tag>();
            this.CurrentInstance = Instance.Baseline;
            this.IntroductionMessages = new List<QuestionnaireIntroductionMessage>();
            this.Descriptions = new List<QuestionnaireDescription>();
        }
    }
}
