using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Users
{
    /// <summary>
    /// Defines the types of users we recognize. 
    /// This can be used for selectively applying certain pieces of data to be visible only certain types of users.
    /// Supports Flags
    /// </summary>
    [Flags]
    public enum UserTypes
    {
        /// <summary>
        /// Indicates ALL recognized and unrecognized usertypes
        /// </summary>
        All = 0,

        /// <summary>
        /// Indicates the user type is a patient
        /// </summary>
        Patient = 1,

        /// <summary>
        /// Indicates the user type is a physician
        /// </summary>
        Physician = 2,

        /// <summary>
        /// Indicates a Researcher
        /// </summary>
        Researcher = 4
    }
}
