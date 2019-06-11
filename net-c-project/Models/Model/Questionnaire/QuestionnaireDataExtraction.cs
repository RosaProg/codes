using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire
{
    /// <summary>
    /// Defines a single response to extract from a submitted questionnaire and under which tag to store the extracted response.
    /// </summary>
    public class QuestionnaireDataExtraction
    {
        /// <summary>
        /// Gets or sets the name of the questionnaire to get the data from
        /// </summary>
        [Key, Column(Order = 0), MaxLength(50)]
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// Gets or sets the action Id of the item to get the data from
        /// </summary>
        public string ItemActionId { get; set; }

        /// <summary>
        /// Gets or sets the action Id of the option group where to get the value from
        /// If null, it will get all the values of all responses to all options in all options groups for the given Item and assign them to the same Tag. The OptionNr is ignored in this case
        /// </summary>
        public string OptionGroupActionId { get; set; }

        /// <summary>
        /// Gets or sets the action Id of the option where to get the value from
        /// If null, it will get all the values of all the responses for the Given Option Group and assign them to the same tag
        /// </summary>
        public string OptionActionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Tag to store the data in
        /// </summary>
        [Key, Column(Order = 1), MaxLength(50)]
        public string TagName { get; set; }
    }
}
