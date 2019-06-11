using PCHI.Model.Research;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Researcher;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.InterfaceProxies.Researcher
{
    /// <summary>
    /// Holds the communication logic for researching purposes
    /// </summary>
    public class ResearcherClient : BaseClient<IResearcher>, IResearcher
    {
        /// <summary>
        /// Gets the data needed for the researcher search page
        /// </summary>
        /// <returns>An operation Result containing any errors that have occurred and the requested research data</returns>
        public OperationResultAsSearchData GetSearchData()
        {
            return this.Channel.GetSearchData();
        }

        /// <summary>
        /// Searches based upon the given filters
        /// </summary>
        /// <param name="group">The definition to search on</param>
        /// <returns>The result of the search</returns>
        public OperationResultAsLists Search(SearchGroup group)
        {
            return this.Channel.Search(group);
        }
    }
}
