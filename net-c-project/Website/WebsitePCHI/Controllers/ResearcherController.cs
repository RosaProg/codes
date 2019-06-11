using PCHI.Model.Research;
using PCHI.WcfServices.InterfaceProxies.Researcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsitePCHI.Models;
using WebsiteSupportLibrary.Models.Attributes;

namespace Website.Controllers
{
    public class ResearcherController : Controller
    {
        // GET: Researcher
        [AnyRole("Researcher")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows the view for the researcher to input the variables to do a query in the records
        /// </summary>
        /// <returns>The proper view</returns>
        [AllRoles("Researcher")]
        public ActionResult ResearcherSearch()
        {
            ResearcherClient rc = new ResearcherClient();
            var searchData = rc.GetSearchData();
            if(!searchData.Succeeded)
            {
                ViewBag.ErrorMessage = searchData.ErrorMessages;
            return View();
        }
            else
            {
                ResearcherModel c = new ResearcherModel();
                c.PatientFields = searchData.PatientTags;
                c.QuestionnaireFields = searchData.QuestionnaireNames;
                return View(c);
            }            
        }

        [HttpPost]
        [AllRoles("Researcher")]
        public ActionResult ResearcherSearch(string modelSubmit)
        {
            ViewBag.model = modelSubmit;
            //var x2 = System.Web.Helpers.Json.Decode(modelSubmit);
            var group = new System.Web.Script.Serialization.JavaScriptSerializer(new ResearcherModelResolver()).Deserialize<group>(modelSubmit);            
            ResearcherClient rc = new ResearcherClient();
            var result = rc.Search(this.ProcessGroup(group));
            if(!result.Succeeded)
            {
                ViewBag.ErrorMessage = result.ErrorMessages;
            return View();
        }

            var searchData = rc.GetSearchData();
            ResearcherModel c = new ResearcherModel();
            c.PatientFields = searchData.PatientTags;
            c.QuestionnaireFields = searchData.QuestionnaireNames;

            StringBuilder output = new StringBuilder();
            output.Append("<table>");
            output.Append("<tr><th>Patient Id</th><th>Response Group Id</th><th>Start Time</th><th>End Time</th></tr>");
            foreach(var responses in result.QuestionnaireUserResponseGroups)
            {
                output.Append("<tr>");
                output.Append("<td>").Append(responses.Patient.Id).Append("</td>");
                output.Append("<td>").Append(responses.Id).Append("</td>");
                output.Append("<td>").Append(responses.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss")).Append("</td>");
                output.Append("<td>").Append(responses.DateTimeCompleted.Value.ToString("yyyy-MM-dd HH:mm:ss")).Append("</td>");
                output.Append("</tr>");
            }
            output.Append("</table>");
            ViewBag.Result = output.ToString();

            return View(c);
        }

        public SearchGroup ProcessGroup(group g)
        {
            SearchGroup group = new SearchGroup();

            group.IsAndOperator = g.selectedLogicalOperator == "AND";
            foreach(var c in g.children)
            {
                if(c.GetType() == typeof(group))
                {
                    group.Children.Add(this.ProcessGroup((group)c));
                }
                else
                {
                    group.Children.Add(this.ProcessCondition((condition)c));
                }
            }

            return group;
        }

        public SearchCondition ProcessCondition(condition c)
        {
            SearchCondition condition = null;
            Comparison comparison = this.GetComparison(c.selectedComparison);
            switch(c.selectedClass)
            {
                case "Questionnaire":
                    condition = new SearchQuestionnaire() { Comparison = comparison, Value = c.selectedField };
                    break;
                case "Patient":
                    condition = new SearchPatient() { Comparison = comparison, TagName = c.selectedField, Value = c.value };
                    break;
                case "Response" :
                    condition = new SearchResponseGroup() { Comparison = comparison, SearchField = (c.selectedField == "Date Completed" ? SearchResponseGroupFields.DateTimeCompleted : SearchResponseGroupFields.DateTimeStarted), Value = c.value };
                    break;
            }

            return condition;
        }

        /// <summary>
        /// Calculates the comparison method to use
        /// </summary>
        /// <param name="comparison">The comparison method to calculate</param>
        /// <returns>The calculated Comparison </returns>
        private Comparison GetComparison(string comparison)
        {
            switch(comparison)
            {
                case "=":
                    return Comparison.Equals;
                case "<>":
                    return Comparison.NotEquals;
                case "<":
                    return Comparison.SmallerThan;
                case "<=" :
                    return Comparison.SmallerOrEquals;
                case ">":
                    return Comparison.GreaterThan;
                case ">=":
                    return Comparison.GreaterOrEquals;
                default:
                    return Comparison.Equals;
            }
        }
    }
}