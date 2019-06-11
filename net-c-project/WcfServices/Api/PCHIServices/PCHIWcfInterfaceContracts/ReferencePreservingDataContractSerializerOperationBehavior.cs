using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts
{
    /// <summary>
    /// Defines the DataContractSerializerOperationBehaviour for preserving the References
    /// </summary>
    public class ReferencePreservingDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencePreservingDataContractSerializerOperationBehavior"/> class
        /// </summary>
        /// <param name="operationDescription">The Operation Descriptoin to use</param>
        public ReferencePreservingDataContractSerializerOperationBehavior(OperationDescription operationDescription) : base(operationDescription) { }

        /// <summary>
        /// Creates a new Serializer
        /// </summary>
        /// <param name="type">The Type of the object to serialize</param>
        /// <param name="name">The name of the object to serialize</param>
        /// <param name="ns">The namespace to use</param>
        /// <param name="knownTypes">Known types for the object</param>
        /// <returns>A new XmlObjectSerializer to use</returns>
        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes,
                0x7FFF, // maxItemsInObjectGraph
                false,  // ignoreExtensionDataObject
                true,   // preserveObjectReferences
                null);  // dataContractSurrogate
        }

        /// <summary>
        /// Creates a new Serializer
        /// </summary>
        /// <param name="type">The Type of the object to serialize</param>
        /// <param name="name">The name of the object to serialize</param>
        /// <param name="ns">The namespace to use</param>
        /// <param name="knownTypes">Known types for the object</param>
        /// <returns>A new XmlObjectSerializer to use</returns>
        public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes,
                0x7FFF, // maxItemsInObjectGraph
                false,  // ignoreExtensionDataObject
                true,   // preserveObjectReferences
                null);  // dataContractSurrogate
        }
    }
}
