using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Holds additional session details
    /// </summary>
    public class ClientSessionDetails
    {
        /// <summary>
        /// Gets or sets the list of User Roles available for this session
        /// </summary>        
        public List<string> AvailableRoles { get; set; }

        /// <summary>
        /// Gets or sets the list of Patients available for the user
        /// Key is the Patient ID
        /// Value is the Patient Display Name
        /// </summary>
        public Dictionary<string, string> AvailablePatients { get; set; }

        /// <summary>
        /// Gets or sets the Role currently selected for this user
        /// </summary>
        public string SelectedRole { get; set; }

        /// <summary>
        /// Gets or sets the list of functionalities a user has access to with the currently selected role
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSessionDetails"/> class
        /// </summary>
        public ClientSessionDetails()
        {
            this.AvailableRoles = new List<string>();
            this.AvailablePatients = new Dictionary<string, string>();
            this.Permissions = new List<Permission>();
        }
    }
}
