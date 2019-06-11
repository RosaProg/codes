using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCHI.Model.Users
{
    /// <summary>
    /// Defines the mapping between a User and a Patient the user can act on behalve of.
    /// </summary>
    public class ProxyUserPatientMap
    {
        /// <summary>
        /// Gets or sets the database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user for the mapping
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the patient for the mapping
        /// </summary>
        public virtual Patient Patient { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyUserPatientMap"/> class
        /// </summary>
        protected ProxyUserPatientMap()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyUserPatientMap"/> class
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="patient">The patient</param>
        public ProxyUserPatientMap(User user, Patient patient)
        {
            this.User = user;
            this.Patient = patient;
        }
    }
}
