using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines the keys to identify the replaceable objects
    /// </summary>
    public enum ReplaceableObjectKeys
    {
        /// <summary>
        /// There is no nothing to replace
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        /// The object is a patient
        /// </summary>
        Patient,

        /// <summary>
        /// The object is a user
        /// </summary>
        User,

        /// <summary>
        /// The object is a URI hostname
        /// </summary>
        UriHostName,

        /// <summary>
        /// The object is a praticioner
        /// </summary>
        Practitioner,

        /// <summary>
        /// The object is a code (usually generated and for the user to fill in)
        /// </summary>
        Code
    }
}
