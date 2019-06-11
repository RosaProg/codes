using DSPrima.WcfUserSession.Proxy;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base
{
    /// <summary>
    /// Base class that implements all logic each client must implement
    /// </summary>
    /// <typeparam name="TChannel">The channel for the client to connect to</typeparam>
    public abstract class BaseClient<TChannel> : SecureClientBase<TChannel> where TChannel : class, IBaseService
    {
        /// <summary>
        /// Returns a string with some identifying features of this service.
        /// </summary>
        /// <returns>A string detailing certain the status of certain elements of the service</returns>
        public string Ping()
        {
            return this.Channel.Ping();
        }
    }
}
