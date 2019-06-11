using PCHI.BusinessLogic;
using PCHI.BusinessLogic.Utilities;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Service
{
    /// <summary>
    /// Defines the implementation calls available for services
    /// </summary>
    public class ServiceCallsService : BaseService, IServiceCalls
    {
        /// <summary>
        /// Runs the Questionnaire schedule parser and updates
        /// </summary>        
        public void ScheduleQuestionnaires()
        {
            if (ConfigurationManager.AppSettings["Demo"].ToLower() == "true")
            {
                QuestionnaireScheduler.ScheduleQuestionnairesForDemo();
            }
            else
            {
                QuestionnaireScheduler.ScheduleQuestionnaires();
            }
        }

        /// <summary>
        /// Runs the notification checker and sends outstanding notifications and reminders
        /// </summary>        
        public void SendNotifications()
        {
            NotifcationManager.SendNotifications();
        }

        /// <summary>
        /// Only used for testing specific queries, not useful for anything else.
        /// </summary>        
        public void Test()
        {
            PCHI.BusinessLogic.Test.TestThis();
        }
    }
}
