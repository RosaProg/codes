using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Questionnaire.Styling.Presentation;
using System;
using System.Runtime.Serialization;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines an OperationResult that can return a Questionnaire, Format and UserResponseGroup
    /// </summary>
    [DataContract]
    public class OperationResultAsUserQuestionnaire : OperationResult
    {
        /// <summary>
        /// Gets or sets a questionnaire
        /// </summary>
        [DataMember]
        public PCHI.Model.Questionnaire.Questionnaire Questionnaire { get; set; }

        /// <summary>
        /// Gets or sets the format
        /// </summary>
        [DataMember]
        public Format Format { get; set; }

        /// <summary>
        /// Gets or sets any response the User has already filled in.
        /// </summary>
        [DataMember]
        public QuestionnaireUserResponseGroup QuestionnaireUserResponseGroup { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsUserQuestionnaire"/> class
        /// </summary>
        /// <param name="ex">Any exception that has occurred. If no exception has occurred set to null and Succeeded is marked as True. If the exception is not null, succeeded is marked as false and the Errorcode and ErrorMessages are filled in</param>
        /// <param name="questionnaire">Holds the questionnaire</param>
        /// <param name="format">Holds the format</param>
        /// <param name="group">Any response Groups</param>
        public OperationResultAsUserQuestionnaire(Exception ex, PCHI.Model.Questionnaire.Questionnaire questionnaire, Format format, QuestionnaireUserResponseGroup group)
            : base(ex)
        {
            this.Questionnaire = questionnaire;
            this.Format = format;
            this.QuestionnaireUserResponseGroup = group;
        }
    }
}
