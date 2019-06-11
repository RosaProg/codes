using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteSupportLibrary.Controls
{
    public class ProResults
    {
        public static Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> GetProDomainResultSetForQuestionnaireAndPatient(string patientId, int episodeId, int questionnaireId, out Dictionary<Episode, Dictionary<string, Dictionary<string, object>>> viewData)
        {
            ProClient proClient = new ProClient();
            viewData = new Dictionary<Episode, Dictionary<string, Dictionary<string, object>>>();

            var result = proClient.GetProDomainResultsForCurrentPatient(patientId, episodeId, questionnaireId);
            if (!result.Succeeded) return new Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>>();
            Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> sets = result.ProDomainResultSets;

            return ProResults.Calculate(sets, ref viewData);
        }

        public static Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> GetProDomainResultSetForPatient(string patientId, int episodeId, out Dictionary<Episode, Dictionary<string, Dictionary<string, object>>> viewData)
        {
            ProClient proClient = new ProClient();
            viewData = new Dictionary<Episode, Dictionary<string, Dictionary<string, object>>>();

            var result = proClient.GetProDomainResults(patientId, episodeId);
            if (!result.Succeeded) return new Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>>();
            Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> sets = result.ProDomainResultSets;

            return ProResults.Calculate(sets, ref viewData);
        }

        private static Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> Calculate(Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> sets, ref Dictionary<Episode, Dictionary<string, Dictionary<string, object>>> viewData)
        {
            Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> result = new Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>>();

            foreach (Episode e in sets.Keys)
            {
                result.Add(e, new Dictionary<string, List<ProDomainResultSet>>());
                viewData.Add(e, new Dictionary<string, Dictionary<string, object>>());
                foreach (string questionnaire in sets[e].Keys)
                {
                    Dictionary<string, object> viewDataValues = new Dictionary<string, object>();
                    result[e].Add(questionnaire, ProResults.Calculate(sets[e][questionnaire], ref viewDataValues));
                    viewData[e].Add(questionnaire, viewDataValues);
                }
            }

            return result;
        }

        private static List<ProDomainResultSet> Calculate(List<ProDomainResultSet> sets, ref Dictionary<string, object> ViewData)
        {
            double maxValue = 0.0;
            foreach (ProDomainResultSet resultSet in sets)
            {
                if (resultSet.Results.Max(m => m.Score) > maxValue)
                {
                    maxValue = resultSet.Results.Max(m => m.Score);
                }
            }
            ViewData["instrumentName"] = sets[0].Results.ElementAt(0).Domain.Instrument.Name;
            int whichResult;
            DateTime maxDate = sets.Max(t => t.GroupEndTime);
            DateTime minDate = sets.Min(t => t.GroupEndTime);
            //minDate.Subtract(new TimeSpan(1, 0, 0, 0));
            TimeSpan timeDifference = maxDate - minDate;
            if (timeDifference.TotalHours > 1)
            {
                ViewData["intervalType"] = "ChartIntervalType.Hours";
            }
            else if (timeDifference.TotalDays > 1)
            {
                ViewData["intervalType"] = "ChartIntervalType.Days";
            }
            List<string> names = new List<string>();
            foreach (ProDomainResult result in sets[0].Results)
            {
                names.Add(result.Domain.Name);
            }
            string dateFormat = "yyyy-MM-dd";
            ViewData["maxDate"] = maxDate.ToString(dateFormat);
            ViewData["minDate"] = minDate.ToString(dateFormat);
            ViewData["DateStep"] = (int)((double)(maxDate - minDate).Days) / 5d + 1;
            ViewData["maxValue"] = maxValue + (maxValue / 10);
            ViewData["interval"] = (int)(maxValue + 0.5) / 10;
            ViewData["names"] = names;
            for (whichResult = 0; whichResult < sets[0].Results.Count; whichResult++)
            {
                List<KeyValuePair<string, double>> list = new List<KeyValuePair<string, double>>();
                foreach (ProDomainResultSet resultSet in sets)
                {
                    double score = resultSet.Results.ElementAt(whichResult).Score;
                    // TODO Chart doesn't seem to want to display values of 0, temporarily changing value to 0.001 to fix that.
                    list.Add(new KeyValuePair<string, double>(
                        resultSet.GroupEndTime.ToString(dateFormat), score > -0.001 && score < 0.001 ? 0.001 : (int)(score + 0.5)));
                }
                ViewData[sets[0].Results.ElementAt(whichResult).Domain.Name] = list;
            }

            return (sets);
        }
    }
}