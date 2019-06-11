using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// Defines a tag for the QuestionnaireUserResponseGroup
    /// </summary>
    public class QuestionnaireUserResponseGroupTag
    {
        /// <summary>
        /// Gets or sets the TagIdentifier of the <see cref="QuestionnaireUserResponseGroupTag"/>
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A QuestionnaireUserResponseGroupTag must have a name")]
        [MaxLength(50)]
        [Key, Column(Order = 0)]
        public string TagName { get; set; }
        
        /// <summary>
        /// Gets or sets the string value
        /// </summary>
        [MaxLength(500)]
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the Group Id
        /// </summary>
        [Key, Column(Order = 1), ForeignKey("QuestionnaireUserResponseGroup")]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Patient"/> it has to map to
        /// </summary>
        [Required(ErrorMessage = "A QuestionnaireUserResponseGroup is required when specifying a QuestionnaireUserResponseGroupTag")]
        public QuestionnaireUserResponseGroup QuestionnaireUserResponseGroup { get; set; }
    }
}
