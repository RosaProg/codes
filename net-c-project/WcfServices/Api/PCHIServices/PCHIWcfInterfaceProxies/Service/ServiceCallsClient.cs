using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service
{
    /// <summary>
    /// Defines the client for calls available for services
    /// </summary>
    public class ServiceCallsClient : BaseClient<IServiceCalls>, IServiceCalls
    {
        /// <summary>
        /// Runs the Questionnaire schedule parser and updates
        /// </summary>        
        public void ScheduleQuestionnaires()
        {
            this.Channel.ScheduleQuestionnaires();
        }

        /// <summary>
        /// Runs the notification checked and sends outstanding notifications and reminders
        /// </summary>        
        public void SendNotifications()
        {
            this.Channel.SendNotifications();
        }

        /// <summary>
        /// Only used for testing specific queries, not useful for anything else.
        /// </summary>        
        public void Test()
        {
            this.Channel.Test();
        }
    }
}
