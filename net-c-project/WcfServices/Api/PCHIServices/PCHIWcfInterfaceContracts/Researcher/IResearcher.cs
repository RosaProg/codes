using PCHI.Model.Research;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Researcher
{
    /// <summary>
    /// Defines the functionality for the Researcher logic
    /// </summary>
    [ServiceContract]
    public interface IResearcher : IBaseService
    {
        /// <summary>
        /// Gets the data needed for the researcher search page
        /// </summary>
        /// <returns>An operation Result containing any errors that have occurred and the requested research data</returns>
        [OperationContract]
        OperationResultAsSearchData GetSearchData();

        /// <summary>
        /// Searches based upon the given filters
        /// </summary>
        /// <param name="group">The definition to search on</param>
        /// <returns>The result of the search</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        OperationResultAsLists Search(SearchGroup group);
    }
}
