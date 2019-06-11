using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Defines the list of permissions available
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// Indicates the user has the permission to update data (not passwords) for other users
        /// </summary>
        UPDATE_OTHER_USERS_DATA,

        /// <summary>
        /// Indicates the user has the permission to update the passwords (not other data) for other users
        /// </summary>
        UPDATE_OTHER_USERS_PASSWORD,

        /// <summary>
        /// Indicates the user has the permission to allow adding/modifying questionnaires, formats and format definitions
        /// </summary>
        MODIFY_QUESTIONNAIRES,

        /// <summary>
        /// Indicates the user has the permission to Create episodes for other users
        /// </summary>
        CREATE_EPISODES,

        /// <summary>
        /// Indicates the user has the permission to modify episode information, including scheduling questionnaires
        /// </summary>
        MODIFY_EPISODES,

        /// <summary>
        /// Indicates the user has the permission to view patient personal data. This does not include medical information
        /// Note: physicians can always see the patient's data providing the patient is their patient
        /// </summary>
        VIEW_PATIENT_DATA,

        /// <summary>
        /// Indicates the user has the permission to view patient episodes
        /// Note: physicians can always see the patient's episodes providing the patient is their patient
        /// </summary>
        VIEW_PATIENT_ASSIGNED_EPISODES,

        /// <summary>
        /// Indicates the user has the permission to create patients and by extension users asociated with Patients
        /// </summary>
        CREATE_PATIENT,

        /// <summary>
        /// Indicates the user has the permission to create users other then patients
        /// </summary>
        CREATE_USER,

        /// <summary>
        /// Indicates the user has the permission to update the data for patients
        /// </summary>
        UPDATE_PATIENT_DATA,

        /// <summary>
        /// Grants the user the permission to save text for pages
        /// </summary>
        SAVE_PAGE_TEXT,

        /// <summary>
        /// Grants the permission to see the audit trails for all users
        /// </summary>
        VIEW_AUDIT_TRAILS
    }
}
