using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Security;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines an Operation result that can contain one or more types of Lists
    /// </summary>
    [DataContract]
    public class OperationResultAsLists : OperationResult
    {
        /// <summary>
        /// Gets or sets a list that can contain string values
        /// </summary>
        [DataMember]
        public List<string> Strings { get; set; }

        /// <summary>
        /// Gets or sets a list that can contain ProDomainResultSet values
        /// </summary>
        [DataMember]
        public List<ProDomainResultSet> ProDomainResultSets { get; set; }

        /// <summary>
        /// Gets or sets a list that can contain ProInstrument values
        /// </summary>
        [DataMember]
        public List<PCHI.Model.Questionnaire.Questionnaire> Questionnaires { get; set; }

        /// <summary>
        /// Gets or sets a list that can contain Format Values
        /// </summary>
        [DataMember]
        public List<Format> Formats { get; set; }

        /// <summary>
        /// Gets or sets a the list of patients
        /// </summary>
        [DataMember]
        public List<PatientDetails> Patients { get; set; }

        /// <summary>
        /// Gets or sets a the list of users
        /// </summary>
        [DataMember]
        public List<UserDetails> Users { get; set; }

        /// <summary>
        /// Gets or sets the list of Episodes
        /// </summary>
        [DataMember]
        public List<Episode> Episodes { get; set; }

        /// <summary>
        /// Gets or sets the list of Assigned Questionnaires
        /// </summary>
        [DataMember]
        public List<AssignedQuestionnaire> AssignedQuestionnaires { get; set; }

        /// <summary>
        /// Gets or sets the list of AuditTrailEntries
        /// </summary>
        [DataMember]
        public List<AuditTrailEntry> AuditTrail { get; set; }

        /// <summary>
        /// Gets or sets a list of QuestionnaireUserResponseGroups
        /// </summary>
        [DataMember]
        public List<QuestionnaireUserResponseGroup> QuestionnaireUserResponseGroups { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsLists"/> class
        /// </summary>
        /// <param name="ex">Any exception that has occurred. If no exception has occurred set to null and Succeeded is marked as True. If the exception is not null, succeeded is marked as false and the Errorcode and ErrorMessages are filled in</param>
        public OperationResultAsLists(Exception ex)
            : base(ex)
        {
            this.ProDomainResultSets = new List<ProDomainResultSet>();
        }        
    }
}
