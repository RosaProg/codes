using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines the error codes for the Error messages
    /// </summary>
    public enum ErrorCodes
    {
        /*
                Errors.Add(100, "That error code doesn't exist");                
                Errors.Add(102, "There has been a problem loading the data");
                Errors.Add(210, "Login has failed");
                Errors.Add(211, "You are currently locked out.");
                Errors.Add(212, "Session has expired");
                Errors.Add(213, "Put error of IdentityResult here");
                Errors.Add(214, "User doesn't exist");
                Errors.Add(150, "No questionnaire of this name has been assigned to this user");
                Errors.Add(151, "A questionnaire that already has responses filled in can not be accessed anonymously");
         
         */

        /// <summary>
        /// Inidicates no error has been reported.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The error that is being requested doesn't exist in the database
        /// </summary>
        ERROR_DOES_NOT_EXIST = 100,

        /// <summary>
        /// There has been an issue loading data
        /// </summary>
        DATA_LOAD_ERROR = 102,

        /// <summary>
        /// Login has failed (User or password is wrong)
        /// </summary>
        LOGIN_FAILED = 210,

        /// <summary>
        /// The user is locked out
        /// </summary>
        USER_IS_LOCKEDOUT = 211,

        /// <summary>
        /// The user session has expired
        /// </summary>
        USER_SESSION_EXPIRED = 212,

        /// <summary>
        /// The error message for this error has to come from the Identity Result of the Microsoft.Asp.Identity framework
        /// </summary>
        GENERAL_IDENTITY_RESULT_ERROR = 213,

        /// <summary>
        /// The user is unknown
        /// </summary>
        USER_UNKNOWN = 214,

        /// <summary>
        /// The questionnaire has not been assigned
        /// </summary>
        QUESTIONNAIRE_NOT_ASSIGNED = 250,

        /// <summary>
        /// A questionnaire that has already had some Items reponsed to cannot be access anonymously
        /// </summary>
        ANONYMOUS_QUESTIONNAIRE_CANNOT_BE_CONTINUED_ANONYMOUSLY = 251,

        /// <summary>
        /// The Episode doesn't exist
        /// </summary>
        EPISODE_DOESNT_EXIST,

        /// <summary>
        /// The milestone doesn't exist
        /// </summary>
        MILESTONE_DOESNT_EXIST,

        /// <summary>
        /// The provided code is incorrect
        /// </summary>
        CODE_INCORRECT,

        /// <summary>
        /// Inidicated the user has already completed registration
        /// </summary>
        USER_ALREADY_COMPLETED_REGISTRATION,

        /// <summary>
        /// Indicates a User is required
        /// </summary>
        USER_IS_REQUIRED,

        /// <summary>
        /// Indicates no active session has been set to use
        /// </summary>
        NO_ACTIVE_SESSION_SET,

        /// <summary>
        /// Indicates permission has been denied
        /// </summary>
        PERMISSION_DENIED,

        /// <summary>
        /// Indicates the security question and answer are mandatory
        /// </summary>
        SECURITY_QUESTION_ANSWER_ARE_MANDATORY,

        /// <summary>
        /// Indicates the password is incorrect
        /// </summary>
        PASSWORD_INCORRECT,

        /// <summary>
        /// Indicates the registration has not yet been completed
        /// </summary>
        REGISTRATION_NOT_COMPLETED,

        /// <summary>
        /// Indicates the username is already in use
        /// </summary>
        USERNAME_ALREADY_INUSE,

        /// <summary>
        /// Indicates the username lenght is greater then 450 characters
        /// </summary>
        USERNAME_LENGTH_EXCEEDED,

        /// <summary>
        /// Indicates the username contains illegal characters
        /// </summary>
        USERNAME_CONTAINS_ILLEGAL_CHARACTERS
    }
}
