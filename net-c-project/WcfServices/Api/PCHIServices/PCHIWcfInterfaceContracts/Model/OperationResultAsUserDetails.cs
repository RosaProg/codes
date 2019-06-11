using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines an Operation Result that returns a UserSetting upon success
    /// </summary>
    [DataContract]
    public class OperationResultAsUserDetails : OperationResult
    {
        /// <summary>
        /// Gets or sets the provider used for two factor authentication
        /// </summary>
        [DataMember]
        public UserDetails UserDetails { get; set; }

        /// <summary>
        /// Gets or sets the provider used for two factor authentication
        /// </summary>
        [DataMember]
        public PatientDetails PatientDetails { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsUserDetails"/> class
        /// </summary>
        /// <param name="ex">Any exception that has occurred. If no exception has occurred set to null and Succeeded is marked as True. If the exception is not null, succeeded is marked as false and the Errorcode and ErrorMessages are filled in</param>        
        public OperationResultAsUserDetails(Exception ex)
            : base(ex)
        {            
        }
    }
}
