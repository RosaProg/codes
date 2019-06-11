using DSPrima.WcfUserSession.Behaviours;
using PCHI.BusinessLogic;
using PCHI.Model.Research;
using PCHI.Model.Tag;
using PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Researcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.WcfServices.InterfaceClients.Researcher
{
    /// <summary>
    /// Implements the functionality for researching purposes
    /// </summary>
    [WcfUserSessionBehaviour]
    public class ResearcherService : BaseService, IResearcher
    {
        /// <summary>
        /// Gets the data needed for the researcher search page
        /// </summary>
        /// <returns>An operation Result containing any errors that have occurred and the requested research data</returns>
        public OperationResultAsSearchData GetSearchData()
        {
            try
            {
                OperationResultAsSearchData data = new OperationResultAsSearchData(null);
                data.QuestionnaireNames = this.handler.QuestionnaireManager.GetProNames();
                data.PatientTags = this.handler.UserManager.GetPatientTags();
                return data;
            }
            catch (Exception ex)
            {
                return new OperationResultAsSearchData(ex);
            }
        }

        /// <summary>
        /// Searches based upon the given filters
        /// </summary>
        /// <param name="group">The definition to search on</param>
        /// <returns>The result of the search</returns>
        public OperationResultAsLists Search(SearchGroup group)
        {
            try
            {
                return new OperationResultAsLists(null) { QuestionnaireUserResponseGroups = this.handler.SearchManager.Search(group) };
            }
            catch (Exception ex)
            {
                return new OperationResultAsLists(ex);
            }
        }
    }
}
