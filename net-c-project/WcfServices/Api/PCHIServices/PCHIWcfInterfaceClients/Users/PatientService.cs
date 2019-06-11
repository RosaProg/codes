using DSPrima.WcfUserSession.Behaviours;
using Microsoft.AspNet.Identity;
using PCHI.Model.Messages;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Users
{
    /// <summary>
    /// The server side implementation of the IPatientService
    /// </summary>
    [WcfUserSessionBehaviour]
    public class PatientService : BaseService, IPatientService
    {
        /// <summary>
        /// Creates a new user Account if a user with the given external ID doesn't exist
        /// </summary>
        /// <param name="externalId">The external Id of the patient</param>
        /// <param name="userName">The requested username</param>        
        /// <param name="email">The email of the user</param>
        /// <param name="title">The title for the user</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="dateOfBirth">The date of birth</param>
        /// <param name="mobilePhone">The mobile phone number</param>
        /// <returns>An operation result indicating success and the errors. The Data variable contains the new Patient Id</returns>
        public OperationResultAsString CreatePatient(string externalId, string userName, string email, string title, string firstName, string lastName, DateTime dateOfBirth, string mobilePhone)
        {
            try
            {
                Patient patient = this.handler.UserManager.CreateOrUpdatePatient(externalId, userName, email, title, firstName, lastName, dateOfBirth, mobilePhone);
                if (patient != null)
                {
                    return new OperationResultAsString(null, patient.Id);
                }

                return new OperationResultAsString(this.handler.MessageManager.GetError(ErrorCodes.DATA_LOAD_ERROR), string.Empty);
            }
            catch (Exception ex)
            {
                return new OperationResultAsString(ex, null);
            }
        }

        /// <summary>
        /// Gets the details of the given user
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the details for</param>
        /// <returns>The OperationResults continaining Patient Details (if successful)</returns>        
        public OperationResultAsUserDetails GetDetailsForPatient(string patientId)
        {
            try
            {
                Patient patients = this.handler.UserManager.FindPatient(patientId);
                if (patients == null) return new OperationResultAsUserDetails(this.handler.MessageManager.GetError(ErrorCodes.USER_UNKNOWN));

                return new OperationResultAsUserDetails(null) { PatientDetails = new PatientDetails(patients) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsUserDetails(ex);
            }
        }

        /// <summary>
        /// Updates the patient settings
        /// </summary>
        /// <param name="patientId">The Id of the patient to save the settings for</param>
        /// <param name="details">The details of the patient</param>
        /// <returns>An operation result indicating success or failure</returns>     
        public OperationResult SavePatientDetails(string patientId, PatientDetails details)
        {
            try
            {
                Patient patient = this.handler.UserManager.FindPatient(patientId);
                details.UpdateUserEntity(patient);

                this.handler.UserManager.SavePatientDetails(patient);

                return new OperationResult(null);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        /// <summary>
        /// Finds all Patients that meet the given criteria
        /// </summary>
        /// <param name="firstName">The first name of the Patient</param>
        /// <param name="lastName">The last name of the Patient</param>
        /// <param name="dob">The date of birth of the Patient</param>
        /// <param name="email">The Patient email</param>
        /// <param name="phoneNumber">The phone number of the Patient</param>
        /// <param name="externalId">The external ID of the Patient</param>
        /// <returns>The list of Patients</returns>     
        public OperationResultAsLists FindPatient(string firstName, string lastName, DateTime? dob, string email, string phoneNumber, string externalId)
        {
            try
            {
                List<PatientDetails> users = this.handler.UserManager.FindPatient(null, firstName, lastName, dob, email, phoneNumber, externalId).Select(u => new PatientDetails(u)).ToList();

                return new OperationResultAsLists(null) { Patients = users };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }
    }
}
