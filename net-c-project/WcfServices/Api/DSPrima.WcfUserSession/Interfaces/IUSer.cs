using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Interfaces
{
    /// <summary>
    /// Provides the definition of what a user must have
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets or sets the Id of the user
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the Username of the user
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Display Name of the user
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this use has enabled two (or more) factor authentication (True) or not (false)
        /// </summary>
        bool MultiStepVerificationEnabled { get; set; }

        /// <summary>
        /// Gets or sets the names of the roles the user belongs to
        /// </summary>
        IEnumerable<string> RoleNames { get; set; }
    }
}
