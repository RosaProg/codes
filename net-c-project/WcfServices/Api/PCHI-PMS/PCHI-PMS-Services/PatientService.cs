using PCHI.PMS.MessageDataLibrary;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using PCHI.WcfServices.PMS.Contracts;
using PCHI.WcfServices.PMS.Models;
using System;

namespace PCHI.WcfServices.PMS.Services
{
    public class PatientService : IPatient
    {
        public Message CreatePatientAppointment(PatientAppointment patient)
        {            
            if (MessageStore.WasMessageReceived(patient.messageReference)) return new Message() { success = true, messageReference = DateTime.Now.ToString() };

            // TODO Check if message was already received
            PatientClient uc = new PatientClient();
            var result = uc.CreatePatient(patient.Id, patient.Email, patient.Email, patient.Title, patient.FirstName, patient.LastName, patient.DateOfBirth, patient.Mobile);
            if(result.Succeeded)
            {
                PatientEpisodeClient uec = new PatientEpisodeClient();
                var result2 = uec.AssignEpisode(result.Data, patient.BasicCondition, patient.AppointmentDate, null, patient.PractitionerId);

                if (result2.Succeeded)
                {
                    MessageStore.MessageReceived(patient.messageReference, patient.Xml, DateTime.Now);
                    return new Message() { success = true };
                }
                return new Message() { success = false, ErrorMessage = result2.ErrorMessages };
            }
            
            return new Message() { success = false, ErrorMessage = result.ErrorMessages };
        }
    }
}
