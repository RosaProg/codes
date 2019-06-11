using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteSupportLibrary.Models
{
    public class PatientModel
    {
        public string Id { get; set; }

        [Display(Name = "First name")]
        public string Firstname { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Filling in an email is required")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string mobileNumber { get; set; }

        [Display(Name = "External Id")]
        public string ExternalId { get; set; }
    }
}