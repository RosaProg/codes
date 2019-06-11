using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Notifications
{
    /// <summary>
    /// Indicates the type of Notifications available
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// The registration has completed
        /// </summary>
        RegistrationComplete,

        /// <summary>
        /// There is a new questionnaire waiting
        /// </summary>
        NewQuestionnaire,

        /// <summary>
        /// Indicates the users password has been changed
        /// </summary>
        PasswordChanged,

        /// <summary>
        /// Indicates the user details have been changed.
        /// </summary>
        UserDetailsChanged
    }
}
