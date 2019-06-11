using PCHI.Model.Episodes;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Questionnaire.Response
{
    /// <summary>
    /// Holds a group of responses to a Questionnaire for a user
    /// </summary>
    public class QuestionnaireUserResponseGroup
    {
        /// <summary>
        /// Gets or sets the database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date the questionnaire was scheduled
        /// </summary>                
        public virtual ScheduledQuestionnaireDate ScheduledQuestionnaireDate { get; set; }

        /// <summary>
        /// Gets or sets the User this response belongs to
        /// </summary>
        [Required(ErrorMessage = "A user response group must belong to a Patient")]
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// Gets or sets the Questionnaire the response is for
        /// </summary>        
        public virtual PCHI.Model.Questionnaire.Questionnaire Questionnaire { get; set; }

        /// <summary>
        /// Gets or sets the name for the QuestionnaireFormat to show
        /// </summary>        
        public string QuestionnaireFormatName { get; set; }

        /// <summary>
        /// Gets or sets the start time of the answering of the Questionnaire
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time the questionnaire was completed
        /// </summary>
        public DateTime? DateTimeCompleted { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether all answers for this questionnaire have been filled in.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="QuestionnaireUserResponseGroupStatus"/> status of the questionniare response group
        /// </summary>
        public QuestionnaireUserResponseGroupStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the Date and time the QuestionnaireUserResponseGroup was created
        /// </summary>
        public DateTime DatetimeCreated { get; set; }
                
        /// <summary>
        /// Gets or sets the date the questionnaire is due
        /// This should be a date with time elements set to 0
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the list of Questionnaire Responses
        /// </summary>
        public virtual ICollection<QuestionnaireResponse> Responses { get; set; }

        /// <summary>
        /// Gets or sets the list of QuestionnaireUserResponseGroupTags
        /// </summary>
        public virtual ICollection<QuestionnaireUserResponseGroupTag> QuestionnaireUserResponseGroupTags { get; set; }

        /// <summary>
        /// Gets or sets a list of ProDomainResultSets that belong to this QuestionnaireUserResponsGroup
        /// </summary>
        public virtual ICollection<ProDomainResultSet> ProDomainResultSet { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireUserResponseGroup"/> class
        /// </summary>
        public QuestionnaireUserResponseGroup()
        {
            this.Responses = new List<QuestionnaireResponse>();
        }
    }
}
