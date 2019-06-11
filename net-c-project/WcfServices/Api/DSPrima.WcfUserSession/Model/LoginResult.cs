using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// Indicates the result of the login action
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// Indicates the login was successfull
        /// </summary>
        Success = 1,

        /// <summary>
        /// Indicates the login has failed (usually due to a user/password failure)
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Indicates the user is locked out
        /// </summary>
        UserIsLockedOut = 3,

        /// <summary>
        /// Indicates the registration has not yet been completed
        /// </summary>
        RegistrationNotCompleted = 4
    }
}
