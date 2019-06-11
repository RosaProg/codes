using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web;
using System.Xml;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts
{
    // From http://blogs.msdn.com/b/sowmy/archive/2006/03/26/561188.aspx and http://stackoverflow.com/questions/4266008/endless-loop-in-a-code-sample-on-serialization

    /// <summary>
    /// Defines a new ReferencePreservingDataContractFormat attribute
    /// </summary>
    public class ReferencePreservingDataContractFormatAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="description">The parameter is not used.</param>
        /// <param name="parameters">The parameter is not used.</param>
        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        /// <summary>
        /// Applies the client side behavior for this Attribute
        /// </summary>
        /// <param name="description">The description of the Operation</param>
        /// <param name="proxy">The Client Operation proxy</param>
        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
            IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(description);
            innerBehavior.ApplyClientBehavior(description, proxy);
        }

        /// <summary>
        /// Applies the Dispatch (server) side behaviour
        /// </summary>
        /// <param name="description">The Operation Description to add the behaviour to</param>
        /// <param name="dispatch">The Dispatche Operation definition</param>
        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(description);
            innerBehavior.ApplyDispatchBehavior(description, dispatch);
        }

        /// <summary>
        /// The function is not used
        /// </summary>
        /// <param name="description">The parameter is not used</param>
        public void Validate(OperationDescription description)
        {
        }
    }
}