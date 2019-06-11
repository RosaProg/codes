using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles the access to and from the database for Episodes
    /// </summary>
    public class EpisodeAccessHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        #region Episodes
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeAccessHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal EpisodeAccessHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a list of Episodes for the given User
        /// </summary>
        /// <param name="patientId">The Id of the user to get the episodes for</param>
        /// <returns>A list of Episodes</returns>
        public List<Episode> GetEpisodesForPatient(string patientId)
        {
            List<Episode> episodes = this.context.Episodes.Where(e => e.Patient.Id == patientId).Include(e => e.MileStones.Select(m => m.Milestone)).Include(e => e.AssignedQuestionnaires.Select(a => a.Schedules.Select(s => s.ResponseGroup))).Include(e => e.Patient.ProxyUserPatientMap.Select(m => m.User)).ToList();
            foreach (Episode e in episodes)
            {
                foreach (EpisodeMilestone m in e.MileStones)
                {
                    User user = this.context.Users.Where(u => u.ExternalId == m.PractitionerId).FirstOrDefault();
                    if (user != null) m.PractitionerName = user.DisplayName;
                }
            }

            return episodes;
        }

        /// <summary>
        /// Assigns an Episode to the given user
        /// </summary>
        /// <param name="patient">The user to assign the episode to</param>
        /// <param name="condition">The condition the episode is for</param>
        /// <param name="externalEpisodeId">The external Episode Id</param>
        /// <returns>The episode assigned</returns>
        public Episode AssignEpisode(Patient patient, string condition, string externalEpisodeId)
        {
            Episode e = new Episode() { Patient = patient, Condition = condition, ExternalId = externalEpisodeId };
            this.context.Episodes.Add(e);
            if (this.context.Entry(patient).State == EntityState.Added) this.context.Entry(patient).State = EntityState.Unchanged;
            this.context.SaveChanges();
            return e;
        }

        /// <summary>
        /// Adds a milestone to an episode
        /// </summary>
        /// <param name="episode">The episode to add the milestone to</param>
        /// <param name="milestone">The milestone to add</param>
        /// <param name="date">The date for the milestone</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        public void AddMileStoneToEpisode(Episode episode, Milestone milestone, DateTime date, string practitionerId)
        {
            EpisodeMilestone em = new EpisodeMilestone() { Episode = episode, Milestone = milestone, MilestoneDate = date, PractitionerId = practitionerId };
            this.context.EpisodeMilestones.Add(em);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets the Episode by id
        /// </summary>
        /// <param name="episodeId">The episode Id</param>
        /// <returns>The episode found or null</returns>
        public Episode GetEpisodeById(int episodeId)
        {
            return this.context.Episodes.Where(e => e.Id == episodeId)
                .Include(e => e.Patient)
                .Include(e => e.MileStones)
                .Include(e => e.TreatmentCodes)
                .Include(e => e.EpisodeHistory)
                .Include(e => e.DiagnosisCodes)
                .Include(e => e.AssignedQuestionnaires)
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets the milestone by name
        /// </summary>
        /// <param name="milestoneName">The name of the milestone to find</param>
        /// <returns>The milestone found or null</returns>
        public Milestone GetMileStoneByName(string milestoneName)
        {
            if (this.context.Milestones.Any(m => m.Name == milestoneName))
            {
                return this.context.Milestones.Where(m => m.Name == milestoneName).SingleOrDefault();
            }
            else
            {
                Milestone m = new Milestone() { Name = milestoneName };
                this.context.Milestones.Add(m);
                this.context.SaveChanges();
                return m;
            }
        }

        /// <summary>
        /// Gets a list of all available milestones
        /// </summary>
        /// <returns>The list of available milestones</returns>
        public List<Milestone> GetAvailableMilestones()
        {
            return this.context.Milestones.ToList();
        }

        /// <summary>
        /// Assigns a questionnaire to an Episode
        /// </summary>
        /// <param name="aq">The AssignedQuestionnaire instance to assign to the episode</param>
        /// <param name="episodeId">The Id of the episode to assign it to</param>
        public void AssignQuestionaireToEpisode(AssignedQuestionnaire aq, int episodeId)
        {
            aq.Episode = this.context.Episodes.Where(e => e.Id == episodeId).Single();
            this.context.AssignedQuestionnaires.Add(aq);
            this.context.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Returns all assigned questionnaires and the calculated schedules for episodes that have not been completed yet
        /// </summary>
        /// <returns>A list of assigned questionnares</returns>
        public List<AssignedQuestionnaire> GetAssignedQuestionnaires()
        {
            return this.context.AssignedQuestionnaires.Where(a => !a.Episode.IsCompletedStatus).Include(a => a.Schedules.Select(s => s.ResponseGroup)).Include(a => a.Episode.Patient).Include(a => a.Episode.MileStones.Select(m => m.Milestone)).ToList();
        }

        /// <summary>
        /// Returns all assigned questionnaires and the calculated schedules for episodes that have not been completed yet
        /// </summary>
        /// <param name="episodeId">The episode Id for which to get the Assigned Questionnaires</param>
        /// <returns>A list of assigned questionnares</returns>
        public List<AssignedQuestionnaire> GetAssignedQuestionnaires(int episodeId)
        {
            var q = this.context.AssignedQuestionnaires.Where(e => e.Episode.Id == episodeId).Include(a => a.Schedules.Select(s => s.ResponseGroup.Responses)).Include(a => a.Episode.Patient);
            return q.ToList();
        }

        /// <summary>
        /// Schedules a questionnaire date for a AssignedQuestionnaire
        /// </summary>
        /// <param name="questionnaireName">The name of the questionnaire to schedule</param>
        /// <param name="aq">the AssignedQuestionnaire class it will belong to</param>
        /// <param name="calculatedDate">The date and time that has been calculated</param>
        /// <param name="scheduleString"> The schedule string that resulted in this date and time</param>
        /// <returns>The schedule questionnaire date instance</returns>
        public ScheduledQuestionnaireDate ScheduleDateForQuestionnaire(string questionnaireName, AssignedQuestionnaire aq, DateTime calculatedDate, string scheduleString)
        {
            ScheduledQuestionnaireDate sqd = new ScheduledQuestionnaireDate();
            sqd.AssignedQuestionnaire = this.context.AssignedQuestionnaires.Where(a => a.Id == aq.Id).Single();
            sqd.ScheduleHasBeenExecuted = false;
            sqd.CalculatedDate = calculatedDate;
            sqd.ScheduleString = scheduleString;
            this.context.ScheduledQuestionnaireDates.Add(sqd);
            this.context.SaveChanges();

            return sqd;
        }

        /// <summary>
        /// Gets the list of assigned Milestones to the patient        
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the milestones for</param>
        /// <param name="episodeId">The Id of the episode to get the milestones for</param>
        /// <returns>The list of assigned milestones. or a empty list If both variables are null (or whitespace for the patient Id)</returns>
        public List<EpisodeMilestone> GetAssignedMileStones(string patientId = null, int? episodeId = null)
        {
            if (string.IsNullOrWhiteSpace(patientId) && !episodeId.HasValue) return new List<EpisodeMilestone>();

            var q = this.context.EpisodeMilestones.Where(m => 1 == 1);
            if (!string.IsNullOrWhiteSpace(patientId)) q = q.Where(m => m.Episode.Patient.Id == patientId);
            if (episodeId.HasValue && episodeId > 0) q = q.Where(m => m.Episode.Id == episodeId);
            return q.ToList();
        }

        /// <summary>
        /// Updates the Scheduled QuestionnaireDate
        /// </summary>
        /// <param name="scheduledQuestionnaire">The schedules questionnaire to update</param>
        public void UpdateScheduleQuestionnaireDate(ScheduledQuestionnaireDate scheduledQuestionnaire)
        {
            this.context.Entry(scheduledQuestionnaire).State = EntityState.Modified;
            this.context.Entry(scheduledQuestionnaire.ResponseGroup).State = EntityState.Unchanged;
            this.context.Entry(scheduledQuestionnaire.AssignedQuestionnaire).State = EntityState.Unchanged;
            this.context.SaveChanges();
        }
    }
}
