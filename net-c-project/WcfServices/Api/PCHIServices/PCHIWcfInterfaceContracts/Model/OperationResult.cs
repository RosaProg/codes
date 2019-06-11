using PCHI.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines an Operation result, holding a value indicating success or failure an error messages
    /// </summary>
    [DataContract]
    public class OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was a success (true) or not (false)
        /// </summary>
        [DataMember]
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the error code of this Operation Result
        /// </summary>
        [DataMember]
        public ErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the errors
        /// </summary>
        [DataMember]
        public string ErrorMessages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class
        /// </summary>
        /// <param name="ex">Any exception that has occurred. If no exception has occurred set to null and Succeeded is marked as True. If the exception is not null, succeeded is marked as false and the Errorcode and ErrorMessages are filled in</param>
        public OperationResult(Exception ex)
        {
            if (ex != null)
            {
                this.Succeeded = false;
                if (ex.GetType() == typeof(PCHIError))
                {                    
                    this.ErrorCode = ((PCHIError)ex).ErrorCode;
                }

                this.ErrorMessages = ex.Message;
            }
            else
            {
                this.Succeeded = true;
            }
        }
    }
}
