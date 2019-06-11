using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.PMS.Models
{
    /// <summary>
    /// Defines a Patient and appointment to add to the system
    /// </summary>
    public class PatientAppointment : MessageDataObject
    {
        /// <summary>
        /// Gets or sets the title of the patient
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the basic condition the appointment is for
        /// </summary>
        public string BasicCondition { get; set; }

        /// <summary>
        /// Gets or sets the Id of the practitioner
        /// </summary>
        public string PractitionerId { get; set; }

        /// <summary>
        /// Gets or sets the appointment date
        /// </summary>
        public DateTime AppointmentDate { get; set; }
    }
}
