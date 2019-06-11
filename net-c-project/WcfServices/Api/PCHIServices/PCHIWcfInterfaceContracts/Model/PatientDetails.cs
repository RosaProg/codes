using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model
{
    /// <summary>
    /// Provides storage for transfering user settings data to and from the web service
    /// </summary>
    [DataContract]
    public class PatientDetails
    {
        /// <summary>
        /// Gets or sets the user Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Title of the user
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        [DataMember]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [DataMember]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user phonenumber
        /// </summary>
        [DataMember]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the external Id of hte user
        /// </summary>
        [DataMember]        
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the patient allows sharing of data with researchers
        /// </summary>
        [DataMember]
        public bool? ShareDataWithResearcher { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the patient allows sharing of data for Quality Assurance
        /// </summary>
        [DataMember]
        public bool? ShareDataForQualityAssurance { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDetails"/> class
        /// </summary>
        public PatientDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDetails"/> class
        /// </summary>
        /// <param name="entity">The patient to get the data from</param>
        public PatientDetails(Patient entity)
        {
            this.Id = entity.Id;
            this.Title = entity.Title;
            this.FirstName = entity.FirstName;
            this.LastName = entity.LastName;
            this.DateOfBirth = entity.DateOfBirth;
            this.Email = entity.Email;            
            this.PhoneNumber = entity.PhoneNumber;
            this.ExternalId = entity.ExternalId;
            this.ShareDataWithResearcher = entity.ShareDataWithResearcher;
            this.ShareDataForQualityAssurance = entity.ShareDataForQualityAssurance;
        }

        /// <summary>
        /// Updates the patient with data container in this class.
        /// If the patient id doesn't match the Id in this class an error is thrown
        /// </summary>
        /// <param name="patient">The patient to update</param>
        public void UpdateUserEntity(Patient patient)
        {
            if (patient.Id != this.Id) throw new Exception("Id of provided User doesn't match this Id");
            patient.Title = this.Title;
            patient.FirstName = this.FirstName;
            patient.LastName = this.LastName;
            patient.DateOfBirth = this.DateOfBirth;

            patient.Email = this.Email;            
            patient.PhoneNumber = this.PhoneNumber;
            patient.ExternalId = this.ExternalId;

            patient.ShareDataWithResearcher = this.ShareDataWithResearcher;
            patient.ShareDataForQualityAssurance = this.ShareDataForQualityAssurance;
        }
    }
}
