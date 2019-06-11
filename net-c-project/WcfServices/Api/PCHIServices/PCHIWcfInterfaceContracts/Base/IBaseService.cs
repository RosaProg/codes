using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base
{
    /// <summary>
    /// Defines a base service contract that each service must implement
    /// </summary>
    [ServiceContract]
    public interface IBaseService
    {
        /// <summary>
        /// Returns a string with some identifying features of this service.
        /// </summary>
        /// <returns>A string detailing certain the status of certain elements of the service</returns>
        [OperationContract]
        string Ping();
    }
}
