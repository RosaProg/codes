using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class QuestionnaireForUsersSelectionModel
    {
        [Display(Name = "Select a User")]
        public Dictionary<string, string> Users { get; set; }

        [Display(Name = "Select Questionnaire")]
        public List<Questionnaire> Questionnaires { get; set; }

        [Display(Name = "Select a Format")]
        public List<Format> QuestionnaireFormats { get; set; }
    }    
}