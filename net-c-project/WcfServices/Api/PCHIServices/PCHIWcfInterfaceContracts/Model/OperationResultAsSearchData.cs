using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Holds data specifically for setting up the seachers
    /// </summary>
    [DataContract]
    public class OperationResultAsSearchData : OperationResult
    {
        /// <summary>
        /// Gets or sets a list of patient tags
        /// </summary>
        [DataMember]
        public List<string> PatientTags { get; set; }

        /// <summary>
        /// Gets or sets a list of questionnaire Names
        /// </summary>
        [DataMember]
        public List<string> QuestionnaireNames { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsSearchData"/> class
        /// </summary>
        /// <param name="ex">Any exception that may have occurred</param>
        public OperationResultAsSearchData(Exception ex) : base(ex)
        {
            this.PatientTags = new List<string>();
            this.QuestionnaireNames = new List<string>();
        }
    }
}
