namespace PCHI.Model.Security
{
    /// <summary>
    /// Holds the list of actions available.
    /// </summary>
    public enum Actions
    {
        #region System
        /// <summary>
        /// Saving text that can be shown on a page again
        /// </summary>
        SAVE_PAGE_TEXT,
        #endregion

        #region Questionnaires
        /// <summary>
        /// Loading Pro Domain Results 
        /// </summary>
        GET_PRO_DOMAIN_RESULTS,

        /// <summary>
        /// Save a questionnaire
        /// </summary>
        SAVE_QUESTIONNAIRE,

        /// <summary>
        /// Add a tag to a questionnaire
        /// </summary>
        ADD_QUESTIONNAIRE_TAG,

        /// <summary>
        /// Save a format definition
        /// </summary>
        SAVE_FORMAT_DEFINITION,

        /// <summary>
        /// Save a questionnaire Format
        /// </summary>
        SAVE_FORMAT,

        /// <summary>
        /// Save one or more responses to a questionnaire for a Patient
        /// </summary>
        SAVE_QUESTIONNAIRE_RESPONSE,

        /// <summary>
        /// Submit one or more responses to a questionnaire for a Patient
        /// </summary>
        SUBMIT_QUESTIONNAIRE_RESPONSE,

        /// <summary>
        /// Saving responses to a questionnaire anonymously
        /// </summary>
        SAVE_QUESTIONNAIRE_RESPONSE_ANONYMOUS,

        /// <summary>
        /// Submitting responses to a questionnaire anonymously
        /// </summary>
        SUBMIT_QUESTIONNAIRE_RESPONSE_ANONYMOUS,

        /// <summary>
        /// Gets a filled in and completed current condition questionnaire and responses
        /// </summary>
        GET_COMPLETED_CURRENT_CONDITION_QUESTIONNAIRE,

        #endregion

        #region Episodes
        /// <summary>
        /// Views a slimmed down version of the episodes that has been assigned to a patient
        /// </summary>
        VIEW_PATIENT_ASSIGNED_EPISODES,

        /// <summary>
        /// Views the episodes that has been assigned to a patient with all details
        /// </summary>
        VIEW_PATIENT_ASSIGNED_EPISODES_WITH_DETAILS,

        /// <summary>
        /// Assign an episode to a patient
        /// </summary>
        ASSIGN_EPISODE,

        /// <summary>
        /// Add a milestones to an episode
        /// </summary>
        ADD_MILESTONE_TO_EPISODE,

        /// <summary>
        /// Schedule a questionnaire for a Episode
        /// </summary>
        SCHEDULE_QUESTIONNAIRE_FOR_EPISODE,

        /// <summary>
        /// Retrieve a assigned questionnaire for a single episode
        /// </summary>
        RETRIEVE_ASSIGNED_QUESTIONNAIRE_FOR_EPISODE,

        /// <summary>
        /// Retrieve all questionnaires for all Episodes
        /// </summary>
        RETRIEVE_ASSIGNED_QUESTIONNAIRE_BY_EPISODE,
        #endregion

        #region Patient
        /// <summary>
        /// Retrieve a questionnaire, format and UserResponseGroup for a patient and episode
        /// </summary>
        GET_QUESTIONNAIRE_FOR_PATIENT,

        /// <summary>
        /// Getting the questionnaire using the Anonymous access code
        /// </summary>
        GET_QUESTIONNAIRE_ANONYMOUS,

        /// <summary>
        /// Retrieve Questionnaire Responses for a Patient
        /// </summary>
        GET_QUESTIONNAIRE_RESPONSES_FOR_PATIENT,

        /// <summary>
        /// Create or update a patient
        /// </summary>
        CREATE_OR_UPDATE_PATIENT,

        /// <summary>
        /// Updates the data for a Patient
        /// </summary>
        UPDATE_PATIENT_DATA,

        /// <summary>
        /// Find a specific patient
        /// </summary>
        FIND_PATIENT,

        /// <summary>
        /// Get a list of patients assigned to a specific user
        /// </summary>
        GET_PATIENTS_ASSIGNED_TO_USER,

        #endregion

        #region User
        /// <summary>
        /// Get a user and it's data
        /// </summary>
        GET_USER,

        /// <summary>
        /// User is changing a password
        /// </summary>
        CHANGE_PASSWORD,

        /// <summary>
        /// Updates the data, but not the password for a user
        /// </summary>
        UPDATE_USER_DATA,

        /// <summary>
        /// Indicates the user is completing it's registrations
        /// </summary>
        COMPLETE_REGISTRATION,

        /// <summary>
        /// Indicates the user has forgotten the password 
        /// </summary>
        FORGOT_PASSWORD,

        /// <summary>
        /// Indicates someone is requestion the security question for a user
        /// </summary>
        GET_SECURITY_QUESTION,

        /// <summary>
        /// User is resetting his password
        /// </summary>
        RESET_PASSWORD,

        /// <summary>
        /// Indicates the login process has completed
        /// </summary>
        LOGIN_COMPLETED,

        /// <summary>
        /// Indicates the login process has started
        /// </summary>
        LOGIN_STARTED,

        /// <summary>
        /// The user is logging out
        /// </summary>
        LOGOUT,

        /// <summary>
        /// The user is retrieving it's audit trail
        /// </summary>
        GET_AUDIT_TRAIL,

        /// <summary>
        /// Creating a user
        /// </summary>
        CREATE_USER,

        #endregion
    }
}
