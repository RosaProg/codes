using DSPrima.ScheduleParser;
using PCHI.DataAccessLibrary;
using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// Manages the logic around assigning scheduled questionnaires.
    /// </summary>
    public class QuestionnaireScheduler
    {
        /// <summary>
        /// Defines storage for items to be scheduled
        /// </summary>
        private struct ToSchedule
        {
            /// <summary>
            /// Defines the schedule string
            /// </summary>
            public string ScheduleString;

            /// <summary>
            /// Defines the date and time it is to be scheduled
            /// </summary>
            public DateTime DateTimeToSchedule;
        }

        /// <summary>
        /// Runs through the schedules available and checks which Questionnairs must be assigned.
        /// </summary>
        public static void ScheduleQuestionnaires()
        {
            AccessHandlerManager ahm = new AccessHandlerManager();
            List<AssignedQuestionnaire> questionnaires = ahm.EpisodeAccessHandler.GetAssignedQuestionnaires();
            foreach (AssignedQuestionnaire aq in questionnaires)
            {
                var milestones = aq.Episode.MileStones.GroupBy(m => m.Milestone.Name.ToLower()).ToDictionary(k => k.Key, v => (IEnumerable<DateTime>)v.Select(m => m.MilestoneDate).ToList());
                var scheduledDates = new ScheduleParser().ParseSchedule(aq.ScheduleString, aq.Episode.DateCreated, milestones);

                /*
                foreach (var scheduled in aq.Schedules)
                {
                    if (scheduled.ResponseGroup != null && scheduled.ResponseGroup.Status == QuestionnaireUserResponseGroupStatus.New && (!scheduledDates.ContainsKey(scheduled.ScheduleString) || !scheduledDates[scheduled.ScheduleString].Contains(scheduled.CalculatedDate.Value)))
                    {
                        // TODO should we remove?
                    }
                }*/

                // Find which dates we have to schedule
                List<ToSchedule> toSchedule = new List<ToSchedule>();
                var schedules = aq.Schedules.GroupBy(s => s.ScheduleString).ToDictionary(k => k.Key, v => v.Select(s => s.CalculatedDate).ToList());
                foreach (var dates in scheduledDates)
                {
                    if (!schedules.ContainsKey(dates.Key))
                    {
                        foreach (DateTime date in dates.Value)
                        {
                            toSchedule.Add(new ToSchedule() { ScheduleString = dates.Key, DateTimeToSchedule = date });
                        }
                    }
                    else
                    {
                        foreach (DateTime date in dates.Value)
                        {
                            if (!schedules[dates.Key].Contains(date))
                            {
                                toSchedule.Add(new ToSchedule() { ScheduleString = dates.Key, DateTimeToSchedule = date });
                            }
                        }
                    }
                }

                // Create the new ScheduledQuestionnaireDate instances
                List<ScheduledQuestionnaireDate> newSchedules = new List<ScheduledQuestionnaireDate>();
                foreach (ToSchedule ts in toSchedule)
                {
                    ScheduledQuestionnaireDate sqd = ahm.EpisodeAccessHandler.ScheduleDateForQuestionnaire(aq.QuestionnaireName, aq, ts.DateTimeToSchedule, ts.ScheduleString);
                    newSchedules.Add(sqd);
                }

                newSchedules.AddRange(aq.Schedules.Where(s => s.ScheduleHasBeenExecuted = false));

                if (newSchedules.Count > 0)
                {
                    newSchedules.OrderBy(s => s.CalculatedDate);

                    // Schedule the last one that is not schedule and after this date                    
                    DateTime today = DateTime.Now;
                    var lastEntry = newSchedules.Where(s => s.CalculatedDate < today).OrderBy(s => s.CalculatedDate).LastOrDefault();
                    if (lastEntry != null)
                    {
                        ahm.QuestionnaireAccessHandler.CreateQuestionnaireUserResponseGroup(aq.Episode.Patient.Id, aq.QuestionnaireName, null, lastEntry);
                        lastEntry.ScheduleHasBeenExecuted = true;
                        ahm.EpisodeAccessHandler.UpdateScheduleQuestionnaireDate(lastEntry);
                    }

                    // Mark all the other ones as not completed
                    var toDisable = newSchedules.Where(s => s.CalculatedDate < (lastEntry != null ? lastEntry.CalculatedDate : today));
                    foreach (var sqd in toDisable)
                    {
                        sqd.ScheduleHasBeenExecuted = true;
                        ahm.EpisodeAccessHandler.UpdateScheduleQuestionnaireDate(sqd);
                        if (sqd.ResponseGroup != null)
                        {
                            ahm.QuestionnaireAccessHandler.SaveQuestionnaireResponse(sqd.ResponseGroup.Questionnaire.Id, sqd.ResponseGroup.Id, true, new List<QuestionnaireResponse>(), QuestionnaireUserResponseGroupStatus.Missed);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Scheduler for demo purposes. An Assigned Questionnaire is scheduled when there are no non-completed QuestionnaireUserResponseGroups
        /// </summary>
        public static void ScheduleQuestionnairesForDemo()
        {
            AccessHandlerManager ahm = new AccessHandlerManager();
            List<AssignedQuestionnaire> questionnaires = ahm.EpisodeAccessHandler.GetAssignedQuestionnaires();
            foreach (AssignedQuestionnaire aq in questionnaires)
            {
                if (aq.Schedules.Where(s => s.ResponseGroup != null && !s.ResponseGroup.Completed).Count() == 0)
                {
                    ScheduledQuestionnaireDate sqd = ahm.EpisodeAccessHandler.ScheduleDateForQuestionnaire(aq.QuestionnaireName, aq, DateTime.Now, aq.ScheduleString);
                    ahm.QuestionnaireAccessHandler.CreateQuestionnaireUserResponseGroup(aq.Episode.Patient.Id, aq.QuestionnaireName, null, sqd);
                }
            }
        }
    }
}
