using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Service
{
    /// <summary>
    /// Defines the calls available for services
    /// </summary>
    [ServiceContract]
    public interface IServiceCalls : IBaseService
    {
        /// <summary>
        /// Runs the Questionnaire schedule parser and updates
        /// </summary>
        [OperationContract]
        void ScheduleQuestionnaires();

        /// <summary>
        /// Runs the notification checked and sends outstanding notifications and reminders
        /// </summary>
        [OperationContract]
        void SendNotifications();

        /// <summary>
        /// Only used for testing specific queries, not useful for anything else.
        /// </summary>
        [OperationContract]
        void Test();
    }
}
