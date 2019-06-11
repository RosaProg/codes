using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines an Operation Result that contians dictionary items
    /// </summary>
    [DataContract]
    public class OperationResultAsDictionary : OperationResult
    {
        /// <summary>
        /// Gets or sets a Dictionary with both the key and value being a string
        /// </summary>
        [DataMember]
        public Dictionary<string, string> StringDictionary { get; set; }

        /// <summary>
        /// Gets or sets a Dictionary of ProDomainResultSets grouped by Episode and then by QuestionnaireName
        /// </summary>
        [DataMember]
        public Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> ProDomainResultSets { get; set; }
        
        /// <summary>
        /// Gets or sets a List of UserResponseGroups grouped by Episode
        /// </summary>
        [DataMember]
        public Dictionary<Episode, List<QuestionnaireUserResponseGroup>> EpisodeQuestionnaires { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsDictionary"/> class
        /// </summary>
        /// <param name="ex">Any Exceptiont that has occurred</param>
        public OperationResultAsDictionary(Exception ex)
            : base(ex)
        {
        }
    }
}
