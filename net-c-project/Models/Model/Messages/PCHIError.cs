using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines an Error. Includes a Code and a message
    /// </summary>
    public class PCHIError : Exception
    {
        /// <summary>
        /// Gets or sets the Error Code of this Error
        /// </summary>
        [Key]
        public ErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the Error message for this Error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        [NotMapped]
        public override string Message
        {
            get
            {
                return this.ErrorMessage;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PCHIError"/> class
        /// </summary>
        public PCHIError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PCHIError"/> class
        /// </summary>
        /// <param name="errorCode">The error code</param>
        /// <param name="errorMessage">The error message</param>
        public PCHIError(ErrorCodes errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }
    }
}
