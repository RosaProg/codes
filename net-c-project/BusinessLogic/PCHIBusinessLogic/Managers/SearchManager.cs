using PCHI.DataAccessLibrary;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Research;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// Defines the functionality for the different search and research calls
    /// </summary>
    public class SearchManager
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        internal SearchManager(AccessHandlerManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Does a search on a QuestionnairUserResponseGroup
        /// </summary>
        /// <param name="group">The Search Group that contains the search settings</param>
        /// <returns>A list of QuestionnaireUserResponseGroups that have been found</returns>
        public List<QuestionnaireUserResponseGroup> Search(SearchGroup group)
        {            
            return this.manager.SearchHandler.SearchQuestionnaireUserResponseGroups(group);
        }
    }
}
