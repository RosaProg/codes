using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Users
{
    /// <summary>
    /// Indicates a Patient
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the Database Id of the User Entity
        /// </summary>
        [MaxLength(128)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Title of the user
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the external Id of the User
        /// </summary>
        [Index, MaxLength(128)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets the name to display
        /// </summary>
        [NotMapped]
        public string DisplayName { get { return this.Title + " " + this.FirstName + " " + this.LastName; } }

        /// <summary>
        /// Gets or sets the User proxies this Entity Belongs to
        /// </summary>        
        public virtual ICollection<ProxyUserPatientMap> ProxyUserPatientMap { get; set; }

        /// <summary>
        /// Gets or sets the date of birth
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        
        /// <summary>
        /// Gets or sets the patient Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the patient Phone Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of Patient Quesitonnaire Tags
        /// </summary>
        public virtual ICollection<PatientTag> PatientTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the patient allows sharing of data with researchers
        /// </summary>
        public bool? ShareDataWithResearcher { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the patient allows sharing of data for Quality Assurance
        /// </summary>
        public bool? ShareDataForQualityAssurance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class
        /// </summary>
        public Patient()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ProxyUserPatientMap = new List<ProxyUserPatientMap>();
        }
    }
}
