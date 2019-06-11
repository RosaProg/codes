using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Defines a result of an operation.
    /// </summary>
    [DataContract]
    public class OperationResultAsBool : OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether something is true or false. 
        /// Exact meaning varies per function, check the function returning this instance for more details on the specific meaning.
        /// </summary>
        [DataMember]
        public bool Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultAsBool"/> class
        /// </summary>
        /// <param name="ex">Any exception that has occurred. If no exception has occurred set to null and Succeeded is marked as True. If the exception is not null, succeeded is marked as false and the Errorcode and ErrorMessages are filled in</param>
        /// <param name="data">The data requested when success</param>
        public OperationResultAsBool(Exception ex, bool data)
            : base(ex)
        {
            this.Data = data;
        }
    }
}
