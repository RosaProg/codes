using PCHI.DataAccessLibrary;
using PCHI.DataAccessLibrary.AccessHandelers;
using PCHI.Model.Episodes;
using PCHI.Model.Messages;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Security
{
    /// <summary>
    /// Handles security checking and the current session
    /// Call <see cref="M:SetCurrentSession"/> to activate a session for a User on the current Thread.
    /// <see cref="M:VerifyAccess"/> takes the provided Action and checks it against the permissions for the user of the current session. Certain Actions are alway allowed if they are for the current user against itself or it's patients.
    /// If the user is null, an Exception is thrown.
    /// </summary>
    public class SecuritySession
    {
        /// <summary>
        /// The name with which to store the WcfUserSessionSecurity instance int he Call Context
        /// </summary>
        private const string CallContextName = "PCHI.SecuritySession";

        #region Static Functionality

        /// <summary>
        /// Instance of the Storage class
        /// </summary>
        private static SecuritySessionStorage storage = new SecuritySessionStorage();

        /// <summary>
        /// Gets or sets the current instance of the SecuritySession
        /// </summary>
        public static SecuritySession Current
        {
            get
            {
                object data = CallContext.LogicalGetData(SecuritySession.CallContextName);
                if (data != null) return data as SecuritySession;
                return new SecuritySession(null, null);
            }

            set
            {
                CallContext.LogicalSetData(SecuritySession.CallContextName, value);
            }
        }

        /// <summary>
        /// Sets the user for the current session to the given User
        /// </summary>
        /// <param name="sessionId">The Id of the session to set the user for</param>
        /// <param name="user">The user to set the session to. Note: The UserEntities collection must be filled</param>
        public static void SetCurrentSession(string sessionId, User user)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) return;

            SecuritySession session = new SecuritySession(sessionId, user);
            string role = SecuritySession.storage.Role(sessionId);
            if (role != null) session.SetUserSelectedRole(sessionId, role);

            SecuritySession.Current = session;
        }

        /// <summary>
        /// Removes the given session from the storage
        /// </summary>
        /// <param name="sessionId">The Id of the session to remove</param>
        public static void RemoveSession(string sessionId)
        {
            SecuritySession.storage.RemoveSession(sessionId);
        }

        #endregion

        #region NonStatic Functionality

        /// <summary>
        /// Gets or sets the user for the current session
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the Id of the session
        /// </summary>
        private string SessionId { get; set; }

        /// <summary>
        /// The Private instance of the Selected Role
        /// </summary>
        private string role;

        /// <summary>
        /// Gets the role selected by the User
        /// </summary>
        public string Role { get { return this.role; } }

        /// <summary>
        /// Gets or sets the last action taken by the user.
        /// This is automatically updated whenever the "VerifyAccess" is called.
        /// </summary>
        public Actions LastAction { get; set; }

        /// <summary>
        /// The <see cref="AccessHandlerManager"/> instance to use
        /// </summary>
        private AccessHandlerManager manager = new AccessHandlerManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuritySession"/> class        
        /// </summary>
        /// <param name="sessionId">The Id of the session</param>
        /// <param name="user">The user to set for this instance of the SecuritySession. Note: The UserEntities collection must be filled</param>
        public SecuritySession(string sessionId, User user)
        {
            this.SessionId = sessionId;
            this.User = user;
        }

        /// <summary>
        /// Sets the user Entity for the current session to use
        /// </summary>
        /// <param name="sessionId">The id of the session</param>
        /// <param name="role">The Role selected for this session</param>
        public void SetUserSelectedRole(string sessionId, string role)
        {
            if (this.SessionId == sessionId)
            {
                this.role = role;
                SecuritySession.storage.StoreSession(sessionId, role);
                SecuritySession.Current = this;
            }
            else
            {
                throw new ArgumentException("The ID of the current session doesn't match the Id of the session specified");
            }
        }

        /// <summary>
        /// Checks if the current user be longs to the given Entity
        /// </summary>
        /// <param name="patient">The patient to check against</param>
        /// <returns>True if the entity belongs to the current user, false otherwise</returns>
        public bool PatientBelongsToCurrentUser(Patient patient)
        {
            if (this.User == null) return false;
            return this.User.ProxyUserPatientMap.Any(m => m.Patient.Id == patient.Id);
        }

        #endregion

        /// <summary>
        /// Checks if the given action on the given patient is allowed
        /// Throws an exception if permission has beend denied
        /// </summary>
        /// <param name="action">The action to chek for</param>
        /// <param name="patientId">The patient the action is on</param>        
        /// <param name="episodeId">The optional Id of the episode to check</param>
        /// <param name="userName">The username to check for</param>
        /// <param name="userId">The Id of the user to check</param>
        /// <exception cref="PCHIException">Any permission Denied will result in a Exception with Permission denied as the error code</exception>
        public void VerifyAccess(Actions action, string patientId = null, int? episodeId = null, string userName = null, string userId = null)
        {
            if (this.User == null) throw this.manager.MessageHandler.GetError(ErrorCodes.NO_ACTIVE_SESSION_SET);
            this.LastAction = action;

            // Indicates the passed user or PatientId belongs to the user of the current session
            bool isOwner = false;

            // Indicates the user of the current session is the physician of the given patient
            bool isPhysician = false;

            this.CheckStatus(ref isOwner, ref isPhysician, patientId, episodeId, userName, userId);

            /*
            if ((!string.IsNullOrWhiteSpace(userId) && this.User.Id == userId) || (!string.IsNullOrWhiteSpace(userName) && this.User.UserName == userName)) isOwner = true;
            if (!string.IsNullOrWhiteSpace(patientId) && this.User.ProxyUserPatientMap.Select(m => m.Patient.Id).Contains(patientId)) isOwner = true;
            if ((episodeId.HasValue || !string.IsNullOrWhiteSpace(patientId)) && this.manager.EpisodeAccessHandler.GetAssignedMileStones(patientId, episodeId).Any(m => m.PractitionerId == this.User.ExternalId)) isPhysician = true;
            */
            List<Permission> permissions = this.manager.UserAccessHandler.GetPermissionsForUser(this.User.Id);

            bool permissionGranted = false;

            switch (action)
            {
                #region System
                // Saving text that can be shown on a page again
                case Actions.SAVE_PAGE_TEXT:
                    if (permissions.Contains(Permission.SAVE_PAGE_TEXT)) permissionGranted = true;
                    break;
                #endregion

                #region Questionnaires
                // Loading Pro Domain Results 
                case Actions.GET_PRO_DOMAIN_RESULTS:
                    if (isOwner || isPhysician) permissionGranted = true;
                    break;

                // Save a questionnaire
                case Actions.SAVE_QUESTIONNAIRE:
                    if (permissions.Contains(Permission.MODIFY_QUESTIONNAIRES)) permissionGranted = true;
                    break;

                // Add a tag to a questionnaire
                case Actions.ADD_QUESTIONNAIRE_TAG:
                    if (permissions.Contains(Permission.MODIFY_QUESTIONNAIRES)) permissionGranted = true;
                    break;

                // Save a format definition
                case Actions.SAVE_FORMAT_DEFINITION:
                    if (permissions.Contains(Permission.MODIFY_QUESTIONNAIRES)) permissionGranted = true;
                    break;

                // Save a questionnaire Format
                case Actions.SAVE_FORMAT:
                    if (permissions.Contains(Permission.MODIFY_QUESTIONNAIRES)) permissionGranted = true;
                    break;

                // Save one or more responses to a questionnaire for a Patient
                case Actions.SAVE_QUESTIONNAIRE_RESPONSE:
                case Actions.SUBMIT_QUESTIONNAIRE_RESPONSE:
                case Actions.SAVE_QUESTIONNAIRE_RESPONSE_ANONYMOUS:
                case Actions.SUBMIT_QUESTIONNAIRE_RESPONSE_ANONYMOUS:
                    if (isOwner) permissionGranted = true;
                    break;

                // Gets a filled in and completed current condition questionnaire and responses
                case Actions.GET_COMPLETED_CURRENT_CONDITION_QUESTIONNAIRE:
                    if (isOwner || isPhysician) permissionGranted = true;
                    break;

                #endregion

                #region Episodes
                // Views the episodes that has been assigned to a patient
                case Actions.VIEW_PATIENT_ASSIGNED_EPISODES:
                    if (isOwner || isPhysician || permissions.Contains(Permission.VIEW_PATIENT_ASSIGNED_EPISODES)) permissionGranted = true;
                    break;

                case Actions.VIEW_PATIENT_ASSIGNED_EPISODES_WITH_DETAILS:
                    if (isOwner || isPhysician) permissionGranted = true;
                    break;

                // Assign an episode to a patient
                case Actions.ASSIGN_EPISODE:
                    if (isPhysician || permissions.Contains(Permission.CREATE_EPISODES)) permissionGranted = true;
                    break;

                // Add a milestones to an episode
                case Actions.ADD_MILESTONE_TO_EPISODE:
                    if (isPhysician || permissions.Contains(Permission.MODIFY_EPISODES)) permissionGranted = true;
                    break;

                // Schedule a questionnaire for a Episode
                case Actions.SCHEDULE_QUESTIONNAIRE_FOR_EPISODE:
                    if (isPhysician) permissionGranted = true;
                    break;

                // Retrieve a assigned questionnaire for a single episode
                case Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_FOR_EPISODE:
                    if (isPhysician || isOwner) permissionGranted = true;
                    break;

                // Retrieve all questionnaires for all Episodes
                case Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_BY_EPISODE:
                    if (isPhysician || isOwner) permissionGranted = true;
                    break;
                #endregion

                #region Patient
                // Retrieve a questionnaire, format and UserResponseGroup for a patient and episode
                case Actions.GET_QUESTIONNAIRE_FOR_PATIENT:
                    if (isOwner) permissionGranted = true;
                    break;

                // Retrieve Questionnaire Responses for a Patient
                case Actions.GET_QUESTIONNAIRE_RESPONSES_FOR_PATIENT:
                    if (isOwner) permissionGranted = true;
                    break;

                // Create or update a patient
                case Actions.CREATE_OR_UPDATE_PATIENT:
                    if (permissions.Contains(Permission.CREATE_PATIENT)) permissionGranted = true;
                    break;

                // Updates the data for a Patient
                case Actions.UPDATE_PATIENT_DATA:
                    if (isOwner || permissions.Contains(Permission.UPDATE_PATIENT_DATA)) permissionGranted = true;
                    break;

                // Find a specific patient
                case Actions.FIND_PATIENT:
                    if (isOwner || isPhysician || permissions.Contains(Permission.VIEW_PATIENT_DATA)) permissionGranted = true;
                    break;

                // Get a list of patients assigned to a specific user
                case Actions.GET_PATIENTS_ASSIGNED_TO_USER:
                    if (isOwner || permissions.Contains(Permission.UPDATE_OTHER_USERS_DATA)) permissionGranted = true;
                    break;

                #endregion

                #region User
                // Get a user and it's data
                case Actions.GET_USER:
                    if (isOwner || permissions.Contains(Permission.UPDATE_OTHER_USERS_DATA)) permissionGranted = true;
                    break;

                // User is changing a password
                case Actions.CHANGE_PASSWORD:
                    if (isOwner || permissions.Contains(Permission.UPDATE_OTHER_USERS_PASSWORD)) permissionGranted = true;
                    break;

                // Updates the data, but not the password for a user
                case Actions.UPDATE_USER_DATA:
                    if (isOwner || permissions.Contains(Permission.UPDATE_OTHER_USERS_DATA)) permissionGranted = true;
                    break;

                // Loads the audit trail for a User
                case Actions.GET_AUDIT_TRAIL:
                    if (isOwner || permissions.Contains(Permission.VIEW_AUDIT_TRAILS)) permissionGranted = true;
                    break;
                #endregion
            }

            if (permissionGranted) return;
            throw this.manager.MessageHandler.GetError(ErrorCodes.PERMISSION_DENIED);
        }

        /// <summary>
        /// Checks the status of the user versus the given data
        /// </summary>
        /// <param name="isOwner">Sets if the user is the owner of the object to check (true) or not (false)</param>
        /// <param name="isPractitioner">Sets if the user is the practitioner of the patient that object to check belongs to(true) or not (false)</param>
        /// <param name="patientId">The optional Patient Id</param>
        /// <param name="episodeId">The optional Episode Id</param>        
        /// <param name="userName">The optional user name</param>
        /// <param name="userId">The optional user Id</param>
        /// <param name="episode">The optional Episode</param>
        public void CheckStatus(ref bool isOwner, ref bool isPractitioner, string patientId = null, int? episodeId = null, string userName = null, string userId = null, Episode episode = null)
        {
            if ((!string.IsNullOrWhiteSpace(userId) && this.User.Id == userId) || (!string.IsNullOrWhiteSpace(userName) && this.User.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))) isOwner = true;
            if (!string.IsNullOrWhiteSpace(patientId) && this.User.ProxyUserPatientMap.Select(m => m.Patient.Id).Contains(patientId)) isOwner = true;
            if ((episodeId.HasValue || !string.IsNullOrWhiteSpace(patientId)) && this.manager.EpisodeAccessHandler.GetAssignedMileStones(patientId, episodeId).Any(m => m.PractitionerId == this.User.ExternalId)) isPractitioner = true;
            if (episode != null && episode.MileStones.Count > 0 && episode.MileStones.Any(e => e.PractitionerId == this.User.ExternalId)) isPractitioner = true;
            if (episode != null && this.User.ProxyUserPatientMap.Any(m => m.Patient.Id == (episode.Patient != null ? episode.Patient.Id : this.manager.EpisodeAccessHandler.GetEpisodeById(episode.Id).Patient.Id))) isOwner = true;
        }

        /// <summary>
        /// Verifies if the given username belongs to the current user and if not throws an Exception
        /// </summary>
        /// <param name="userName">The name of the user to verify</param>
        /// <exception cref="PCHIError">If the user doesn't match the current user, a USER_UNKNOWN exception is thrown</exception>
        public void VerifyUser(string userName)
        {
            if (this.User == null || this.User.UserName != userName) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
        }

        /// <summary>
        /// Filters the episodes based upon access
        /// </summary>
        /// <param name="episodes">The list of episodes to filter</param>
        /// <returns>The filters list of episodes</returns>
        internal List<Episode> Filter(IEnumerable<Episode> episodes)
        {
            List<Episode> result = new List<Episode>();
            foreach (Episode e in episodes)
            {
                bool isOwner = false;
                bool isPhysician = false;
                this.CheckStatus(ref isOwner, ref isPhysician, episode: e);
                if (isOwner || isPhysician) result.Add(e);
            }

            return result;
        }

        /// <summary>
        /// Filters the list of patient data based upon the Current Users permissions
        /// </summary>
        /// <param name="patients">The list to filter</param>
        /// <returns>The filtered list</returns>
        internal List<Patient> Filter(List<Patient> patients)
        {
            List<Permission> permissions = this.manager.UserAccessHandler.GetPermissionsForUser(this.User.Id);
            if (permissions.Contains(Permission.VIEW_PATIENT_DATA)) return patients;

            Dictionary<string, List<EpisodeMilestone>> patientMilestones = this.manager.UserAccessHandler.FindMileStones(patients, this.User.ExternalId);
            return patients.Where(p => patientMilestones.Keys.Contains(p.Id)).ToList();
        }
    }
}
