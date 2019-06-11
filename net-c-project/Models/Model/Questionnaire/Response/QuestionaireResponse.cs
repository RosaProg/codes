using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCHI.Model.Questionnaire;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// This class defines a Questionnaire Response
    /// </summary>
    public class QuestionnaireResponse
    {
        /// <summary>
        /// Gets or sets the Database Id of this Questionnaire Response
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the QuestionnaireResponse
        /// </summary>
        [Required(ErrorMessage = "An Response must belong to an response group")]
        public virtual QuestionnaireUserResponseGroup QuestionnaireUserResponseGroup { get; set; }

        /// <summary>
        /// Gets or sets the Item this Questionnaire Response belongs to
        /// </summary>
        [Required(ErrorMessage = "An response must be for an Item")]
        public QuestionnaireItem Item { get; set; }

        /// <summary>
        /// Gets or sets the Item Option this response is for
        /// </summary>
        public QuestionnaireItemOption Option { get; set; }

        /// <summary>
        /// Gets or sets the Value for this Response 
        /// </summary>
        public double? ResponseValue { get; set; }

        /// <summary>
        /// Gets or sets the text for this response
        /// </summary>
        public string ResponseText { get; set; }
    }
}
