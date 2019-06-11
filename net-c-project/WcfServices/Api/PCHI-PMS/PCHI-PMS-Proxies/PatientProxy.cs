using PCHI.WcfServices.PMS.Contracts;
using PCHI.WcfServices.PMS.Models;
using System.ServiceModel;

namespace PCHI.WcfServices.PMS.Proxies
{
    public class PatientProxy : ClientBase<IPatient>
    {
        /// <summary>
        /// Creates a patient and registers the appointment.
        /// Class version
        /// </summary>
        /// <param name="patient">The patientAppointment instance</param>
        /// <returns>The message</returns>
        public Message CreatePatient(PatientAppointment patient)
        {
            return this.Channel.CreatePatientAppointment(patient);
        }

        /// <summary>
        /// Creates a patient and registers the appointment.
        /// XMl version
        /// </summary>
        /// <param name="patientXml">The xml of the patientAppointment</param>
        /// <returns>The xml version of the Response</returns>
        public string CreatePatient(string patientXml)
        {
            return this.Channel.CreatePatientAppointment(PatientAppointment.Deserialize<PatientAppointment>(patientXml)).Xml;
        }
    }
}
