using DSPrima.WcfUserSession.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;

namespace DSPrima.WcfUserSession.Service
{
    /// <summary>
    /// Defines the base class for a SecureUserService
    /// Alternatively, the <see cref="WcfUserSessionBehaviour"/> attribute can be used instead of inheriting from this class
    /// </summary>
    [WcfUserSessionBehaviour]
    public class SecureUserServiceBase
    {
    }
}