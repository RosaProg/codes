using System.ComponentModel.DataAnnotations;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Holds the tag for a questionniare
    /// </summary>
    public class QuestionnaireTag
    {
        /// <summary>
        /// Gets or sets the database Id of the <see cref="QuestionnaireTag"/>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Tag"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Tag is required when specifying a QuestionnaireTag")]
        public Tag.Tag Tag { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Questionnaire"/>s it has to map to
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A QuestionnaireName is required when specifying a QuestionnaireTag")]
        public string QuestionnaireName { get; set; }
    }
}
