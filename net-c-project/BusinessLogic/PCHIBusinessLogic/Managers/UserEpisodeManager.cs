using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities;
using PCHI.DataAccessLibrary;
using PCHI.Model.Episodes;
using PCHI.Model.Messages;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DSPrima.Security;
using PCHI.BusinessLogic.Utilities.Model;

namespace PCHI.BusinessLogic.Managers
{
    /// <summary>
    /// The UserQuestionniareManager class is used to manager questionnaire data related to users
    /// </summary>
    public class UserEpisodeManager
    {
        /// <summary>
        /// Holds the <see cref="AccessHandlerManager"/> for internal use
        /// </summary>
        private AccessHandlerManager manager;

        /// <summary>
        /// Holds the order of the actions to execute after the Questionniare Responses have been saved
        /// </summary>
        public List<Action<QuestionnaireUserResponseGroup>> AfterQuestionnaireResponseSave_Actions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEpisodeManager"/> class
        /// </summary>
        /// <param name="manager">The <see cref="AccessHandlerManager"/> instance to use</param>
        /// <param name="afterQuestionnaireResponseSave_Actions">The actions to execute AFTER the Questionnaire Responses have been saved, The Action gets the entire Questionniare User Response group including items, options, questionnaire, patient, etc</param>
        internal UserEpisodeManager(AccessHandlerManager manager, List<Action<QuestionnaireUserResponseGroup>> afterQuestionnaireResponseSave_Actions = null)
        {
            this.manager = manager;
            this.AfterQuestionnaireResponseSave_Actions = afterQuestionnaireResponseSave_Actions == null ? new List<Action<QuestionnaireUserResponseGroup>>() : afterQuestionnaireResponseSave_Actions;
        }

        #region Episodes
        /// <summary>
        /// Retrieves a list of a slimmed down version of all the Episodes for the given Patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the episodes for</param>
        /// <returns>A list of Episodes</returns>
        public List<Episode> GetEpisodesForPatient(string patientId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.VIEW_PATIENT_ASSIGNED_EPISODES, patientId);
                if (this.manager.UserAccessHandler.FindPatient(patientId) == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                var result = this.manager.EpisodeAccessHandler.GetEpisodesForPatient(patientId);
                foreach (Episode e in result)
                {
                    e.AssignedQuestionnaires.Clear();
                    e.DiagnosisCodes = null;
                    e.TreatmentCodes.Clear();
                }

                Logger.Audit(new Audit(Model.Security.Actions.VIEW_PATIENT_ASSIGNED_EPISODES, AuditEventType.READ, typeof(Patient), "Id", patientId));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.VIEW_PATIENT_ASSIGNED_EPISODES, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves a list of Episodes for the given Patient. Includes all episode details
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the episodes for</param>
        /// <returns>A list of Episodes</returns>
        public List<Episode> GetEpisodesWithDetailsForPatient(string patientId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.VIEW_PATIENT_ASSIGNED_EPISODES_WITH_DETAILS, patientId);
                if (this.manager.UserAccessHandler.FindPatient(patientId) == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                var result = SecuritySession.Current.Filter(this.manager.EpisodeAccessHandler.GetEpisodesForPatient(patientId));

                Logger.Audit(new Audit(Model.Security.Actions.VIEW_PATIENT_ASSIGNED_EPISODES_WITH_DETAILS, AuditEventType.READ, typeof(Patient), "Id", patientId));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.VIEW_PATIENT_ASSIGNED_EPISODES_WITH_DETAILS, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Assigns an Episode to the given patient
        /// </summary>
        /// <param name="patientId">The Id of the Patient to assign the episode to</param>
        /// <param name="condition">The condition the episode is for</param>
        /// <param name="externalEpisodeId">The external Episode Id</param>
        /// <returns>The episode assigned</returns>
        public Episode AssignEpisode(string patientId, string condition, string externalEpisodeId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.ASSIGN_EPISODE, patientId);
                Patient p = new UserManager(this.manager).FindPatient(patientId);
                if (p == null) throw this.manager.MessageHandler.GetError(ErrorCodes.USER_UNKNOWN);
                Episode e = this.manager.EpisodeAccessHandler.AssignEpisode(p, condition, externalEpisodeId);
                QuestionnaireUserResponseGroup g = this.manager.QuestionnaireAccessHandler.GetLastQuestionnaireResponsesForPatient(p.Id, BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire, null);
                if (g == null) this.manager.QuestionnaireAccessHandler.CreateQuestionnaireUserResponseGroup(p.Id, BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire, null, null);

                Logger.Audit(new Audit(Model.Security.Actions.ASSIGN_EPISODE, AuditEventType.ADD, typeof(Patient), "Id", patientId));
                return e;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.ASSIGN_EPISODE, AuditEventType.ADD, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Adds a milestone to the Episode
        /// </summary>
        /// <param name="episodeId">The Id of the episode to add the milestone to</param>
        /// <param name="milestoneName">The name of the milestone to add</param>
        /// <param name="date">The date of the milestone</param>
        /// <param name="practitionerId">The Id of the practitioner with whom is the appointment</param>        
        public void AddMileStoneToEpisode(int episodeId, string milestoneName, DateTime date, string practitionerId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.ADD_MILESTONE_TO_EPISODE, episodeId: episodeId);

                Episode e = this.manager.EpisodeAccessHandler.GetEpisodeById(episodeId);
                if (e == null) throw this.manager.MessageHandler.GetError(ErrorCodes.EPISODE_DOESNT_EXIST);
                Milestone m = this.manager.EpisodeAccessHandler.GetMileStoneByName(milestoneName);
                if (m == null) throw this.manager.MessageHandler.GetError(ErrorCodes.MILESTONE_DOESNT_EXIST);
                User practitioner = this.manager.UserAccessHandler.Users.Where(u => u.ExternalId == practitionerId).FirstOrDefault();
                this.manager.EpisodeAccessHandler.AddMileStoneToEpisode(e, m, date, practitionerId);

                Logger.Audit(new Audit(Model.Security.Actions.ADD_MILESTONE_TO_EPISODE, AuditEventType.ADD, e));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.ADD_MILESTONE_TO_EPISODE, AuditEventType.ADD, typeof(Episode), "Id", episodeId.ToString(), false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Assigns a Questionnaire to an episode
        /// </summary>
        /// <param name="questionnaire">The name of the questionnaire to assign</param>
        /// <param name="episodeId">The episode to assign the questionnaire to</param>
        /// <param name="schedule">The schedule to assign the questionnaire with</param>
        public void ScheduleQuestionnaireForEpisode(string questionnaire, int episodeId, string schedule)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SCHEDULE_QUESTIONNAIRE_FOR_EPISODE, episodeId: episodeId);

                AssignedQuestionnaire aq = new AssignedQuestionnaire();
                aq.QuestionnaireName = questionnaire;
                aq.ScheduleString = schedule;

                this.manager.EpisodeAccessHandler.AssignQuestionaireToEpisode(aq, episodeId);
                Logger.Audit(new Audit(Model.Security.Actions.SCHEDULE_QUESTIONNAIRE_FOR_EPISODE, AuditEventType.ADD, typeof(Episode), "Id", episodeId.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.SCHEDULE_QUESTIONNAIRE_FOR_EPISODE, AuditEventType.ADD, typeof(Episode), "Id", episodeId.ToString(), false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of questionnaires that have been assigend to Episode and when they where answered
        /// </summary>
        /// <param name="episodeId">The Id of the episode</param>
        /// <returns>The list of questionnaires assigned</returns>        
        public List<AssignedQuestionnaire> GetAssignedQuestionnairesForEpisode(int episodeId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_FOR_EPISODE, episodeId: episodeId);
                var result = this.manager.EpisodeAccessHandler.GetAssignedQuestionnaires(episodeId);
                Logger.Audit(new Audit(Model.Security.Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_FOR_EPISODE, AuditEventType.READ, typeof(Episode), "Id", episodeId.ToString()));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_FOR_EPISODE, AuditEventType.READ, typeof(Episode), "Id", episodeId.ToString(), false, ex.Message));
                throw ex;
            }
        }

        #endregion

        #region Questionnaire Responses

        /// <summary>
        /// Gets the questionnare, format and response group to fill in for the questionnaire
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the questionnaire for</param>
        /// <param name="questionnaireName">The name of the questionnaire to load</param>
        /// <param name="episodeId">The Id of the episode to load the questionnaire for</param>
        /// <param name="platform">The platform for which to load the format</param>        
        /// <param name="q">The resulting questionnaire </param>
        /// <param name="f">The resulting format</param>
        /// <param name="g">The resulting Response group</param>
        /// <exception cref="PCHIError">Thrown when no response group has been assigned to the user for the questionnaire or when the questionnaire has already been completed</exception>
        public void GetQuestionnaireForPatient(string patientId, string questionnaireName, int? episodeId, Platform platform, ref Questionnaire q, ref Format f, ref QuestionnaireUserResponseGroup g)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_QUESTIONNAIRE_FOR_PATIENT, patientId, episodeId);

                g = this.manager.QuestionnaireAccessHandler.GetLastQuestionnaireResponsesForPatient(patientId, questionnaireName, episodeId);
                if (g == null) throw this.manager.MessageHandler.GetError(ErrorCodes.QUESTIONNAIRE_NOT_ASSIGNED);

                q = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireByName(questionnaireName);
                f = this.manager.QuestionnaireFormatAccessHandler.GetFullFormatByName(g.QuestionnaireFormatName, platform);

                q.CurrentInstance = this.manager.QuestionnaireAccessHandler.GetAllQuestionnaireResponsesForPatient<Questionnaire>(patientId, questionnaireName).Count > 0 ? Instance.Followup : Instance.Baseline;
                Instance currentInstance = q.CurrentInstance;
                UserTypes audience = UserTypes.Patient;

                Utilities.Utilities.Filter(ref q, currentInstance, platform, audience);

                /*
                q.IntroductionMessages = q.IntroductionMessages.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                q.Descriptions = q.Descriptions.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                foreach (QuestionnaireSection s in q.Sections)
                {
                    s.Instructions = s.Instructions.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                    foreach (QuestionnaireElement e in s.Elements)
                    {
                        e.TextVersions = e.TextVersions.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                        if (e.GetType() == typeof(QuestionnaireItem))
                        {
                            QuestionnaireItem item = (QuestionnaireItem)e;
                            item.Instructions = item.Instructions.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                            foreach (QuestionnaireItemOptionGroup group in item.OptionGroups)
                            {
                                group.TextVersions = group.TextVersions.Where(i => i.SupportsInstance(currentInstance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                            }
                        }
                    }
                }*/

                TextParser p = new TextParser(this.manager);
                p.UpdateQuestionnaireTexts(q, new Dictionary<Model.Messages.ReplaceableObjectKeys, object>()
                {
                    { ReplaceableObjectKeys.Patient, g.Patient },                    
                });
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_FOR_PATIENT, AuditEventType.READ, g));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_FOR_PATIENT, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets the questionnare, format and response group to fill in for the anonymous questionnaire
        /// </summary>
        /// <param name="anonymous">The anonymous idenfitier string</param>
        /// <param name="platform">The platform to load everything for</param>
        /// <param name="q">The resulting questionnaire </param>
        /// <param name="f">The resulting format</param>
        /// <param name="g">The resulting Response group</param>
        /// <exception cref="ArgumentException">Thrown when no response group has been assigned to the user for the questionnaire and format encoded in the anonymous string</exception>
        /// <exception cref="AccessViolationException">Thrown when the user has already answers 1 or more questions on this questionnaire for the given response group</exception>
        public void GetQuestionnaireAnonymous(string anonymous, Platform platform, ref Questionnaire q, ref Format f, ref QuestionnaireUserResponseGroup g)
        {
            try
            {
                string patientId = null;
                string questionnaireName = null;
                string formatName = null;
                int? episodeId = null;
                this.DecryptAnonymous(anonymous, ref patientId, ref questionnaireName, ref formatName, ref episodeId);
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_ANONYMOUS, AuditEventType.READ, typeof(Patient), "Id", patientId));
                this.GetQuestionnaireForPatient(patientId, questionnaireName, episodeId, platform, ref q, ref f, ref g);
                if (g.Responses.Count > 0) throw this.manager.MessageHandler.GetError(ErrorCodes.ANONYMOUS_QUESTIONNAIRE_CANNOT_BE_CONTINUED_ANONYMOUSLY);
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_ANONYMOUS, AuditEventType.READ, null, null, anonymous, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Saves the questionnaire responses for an anonymous questionnaire
        /// </summary>
        /// <param name="anonymous">The anonymous identifier string</param>
        /// <param name="questionnaireId">The Id of the questionnaire of the anonymous response</param>
        /// <param name="groupId">The group Id</param>
        /// <param name="completed">Whether it is completed or not</param>
        /// <param name="questionnaireResponses">The list of Questionnaire Responses</param>
        public void SaveAnonymouseQuestionnaireResponse(string anonymous, int questionnaireId, int groupId, bool completed, List<QuestionnaireResponse> questionnaireResponses)
        {
            try
            {
                string patientId = null;
                string questionnaireName = null;
                string formatName = null;
                int? episodeId = null;

                this.DecryptAnonymous(anonymous, ref patientId, ref questionnaireName, ref formatName, ref episodeId);
                Logger.Audit(new Audit(completed ? Actions.SUBMIT_QUESTIONNAIRE_RESPONSE_ANONYMOUS : Actions.SAVE_QUESTIONNAIRE_RESPONSE_ANONYMOUS, AuditEventType.READ, typeof(Patient), "Id", patientId));
                this.SaveQuestionnaireResponse(questionnaireId, groupId, completed, questionnaireResponses);
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(completed ? Actions.SUBMIT_QUESTIONNAIRE_RESPONSE_ANONYMOUS : Actions.SAVE_QUESTIONNAIRE_RESPONSE_ANONYMOUS, AuditEventType.READ, null, null, anonymous, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Saves the given <see cref="QuestionnaireResponse"/>  to the database
        /// Existing answers are overwritten
        /// </summary>        
        /// <param name="questionnaireId">The ID of the questionnaire you want to get the response for</param>
        /// <param name="responseGroupId">The Response group ID for which the responses are for</param>
        /// <param name="questionnaireCompleted">Indicates whehter or user has filled in all responses and the Questionnaire is now completed</param>
        /// <param name="responses">A list of <see cref="QuestionnaireResponse"/> to save</param>
        public void SaveQuestionnaireResponse(int questionnaireId, int responseGroupId, bool questionnaireCompleted, List<QuestionnaireResponse> responses)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.SAVE_QUESTIONNAIRE_RESPONSE, this.manager.QuestionnaireAccessHandler.GetSmallQuestionnaireUserResponseGroupById(responseGroupId).Patient.Id);
                this.manager.QuestionnaireAccessHandler.SaveQuestionnaireResponse(questionnaireId, responseGroupId, questionnaireCompleted, responses);

                // TODO Should this be threaded?
                if (questionnaireCompleted && this.AfterQuestionnaireResponseSave_Actions != null && this.AfterQuestionnaireResponseSave_Actions.Count > 0)
                {
                    var responseGroup = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireUserResponseGroupById(responseGroupId);
                    foreach (var action in this.AfterQuestionnaireResponseSave_Actions)
                    {
                        action.Invoke(responseGroup);
                    }
                }

                Logger.Audit(new Audit(questionnaireCompleted ? Actions.SUBMIT_QUESTIONNAIRE_RESPONSE : Actions.SAVE_QUESTIONNAIRE_RESPONSE, AuditEventType.READ, typeof(QuestionnaireUserResponseGroup), "Id", responseGroupId.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(questionnaireCompleted ? Actions.SUBMIT_QUESTIONNAIRE_RESPONSE : Actions.SAVE_QUESTIONNAIRE_RESPONSE, AuditEventType.READ, typeof(QuestionnaireUserResponseGroup), "Id", responseGroupId.ToString(), false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Extracts the data from the responses given and saves them to the Patient Tags
        /// </summary>
        /// <param name="group">The Questionnaire User Response group to get the data from</param>
        public void ExtractQuestionnaireData(QuestionnaireUserResponseGroup group)
        {
            List<QuestionnaireDataExtraction> extractionData = this.manager.QuestionnaireAccessHandler.GetDataExtrationDefinitions(group.Questionnaire.Name);
            List<PatientTag> patientTags = new List<PatientTag>();
            foreach (QuestionnaireDataExtraction data in extractionData)
            {
                var responses = group.Responses.Where(r => r.Item.ActionId == data.ItemActionId).ToList();

                if (responses.Count > 0)
                {
                    List<string> values = new List<string>();
                    if (!string.IsNullOrWhiteSpace(data.OptionGroupActionId))
                    {
                        string value = string.Empty;
                        foreach (var response in responses)
                        {
                            if (response.ResponseValue.HasValue) values.Add(response.ResponseValue.Value.ToString());
                            if (response.ResponseText != null) values.Add(response.ResponseText);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(data.OptionActionId))
                        {
                            var subResponses = responses.Where(o => o.Option.Group.ActionId == data.OptionGroupActionId).ToList();
                            foreach (var response in subResponses)
                            {
                                if (response.ResponseValue.HasValue) values.Add(response.ResponseValue.Value.ToString());
                                if (response.ResponseText != null) values.Add(response.ResponseText);
                            }
                        }
                        else
                        {
                            var response = responses.Where(o => o.Option.Group.ActionId == data.OptionGroupActionId && o.Option.ActionId == data.OptionActionId).SingleOrDefault();
                            if (response != null)
                            {
                                if (response.ResponseValue.HasValue) values.Add(response.ResponseValue.Value.ToString());
                                if (response.ResponseText != null) values.Add(response.ResponseText);
                            }
                        }
                    }

                    if (values.Count > 0)
                    {
                        PatientTag t = new PatientTag();
                        t.Patient = group.Patient;
                        t.TagName = data.TagName;
                        t.TextValue = values.Aggregate((s1, s2) => { return s1 + "|" + s2; });
                        patientTags.Add(t);
                    }
                }
            }

            if (group.Patient.DateOfBirth.HasValue)
            {
                DateTime now = DateTime.Today;
                int age = now.Year - group.Patient.DateOfBirth.Value.Year;
                if (group.Patient.DateOfBirth > now.AddYears(-age)) age--;
                patientTags.Add(new PatientTag() { Patient = group.Patient, TagName = "Age", TextValue = age.ToString() });
            }

            this.manager.UserAccessHandler.AddOrUpdatePatientTags(patientTags);
        }

        /// <summary>
        /// Copies over the Patient Tags to the QuestionnaireUserResponseGroup Tags
        /// </summary>
        /// <param name="group">The QuestionnaireUserResponseGroup to copy the tags to</param>
        public void AssignQuestionnaireUserResponseGroupTags(QuestionnaireUserResponseGroup group)
        {
            List<PatientTag> tags = this.manager.UserAccessHandler.GetPatientTags(group.Patient.Id);
            var groupTags = tags.Where(t=>t.TagName != "Age").Select(t => new QuestionnaireUserResponseGroupTag() { QuestionnaireUserResponseGroup = group, TagName = t.TagName, TextValue = t.TextValue }).ToList();
            if (group.Patient.DateOfBirth.HasValue)
            {                
                DateTime now = DateTime.Today;
                int age = now.Year - group.Patient.DateOfBirth.Value.Year;
                if (group.Patient.DateOfBirth > now.AddYears(-age)) age--;
                groupTags.Add(new QuestionnaireUserResponseGroupTag() { QuestionnaireUserResponseGroup = group, TagName = "Age", TextValue = age.ToString() });
            }

            this.manager.QuestionnaireAccessHandler.AddOrUpdateQuestionnaireUserResponseGroupTags(groupTags);
        }

        /// <summary>
        /// Calculates the scores for all domains belonging to the questionnaire includes and saves them to the database. Any existing domains are overwritten.
        /// This is ONLY done for PROs
        /// </summary>
        /// <param name="group">The group to calculate the scores for</param>
        public void CalculateResponseScores(QuestionnaireUserResponseGroup group)
        {
            if (group.Questionnaire.GetType() == typeof(ProInstrument))
            {
                ProDomainResultSet proDomainResultSet = this.ProcessGroup(group);

                this.manager.QuestionnaireAccessHandler.SaveProDomainResult(proDomainResultSet);
            }
        }

        /// <summary>
        /// Processes a QuestionnaireUserResponseGroup and calculates the domain results for it.
        /// The domain must be loaded in the the questionnaire (PRO) of the group
        /// </summary>
        /// <param name="group">The group to get the result for</param>
        /// <returns>A ProDomainResultSet containing the list of calculated PRO Domain Results</returns>
        private ProDomainResultSet ProcessGroup(QuestionnaireUserResponseGroup group)
        {
            ProDomainResultSet gr = new ProDomainResultSet();
            gr.GroupId = group.Id;

            foreach (ProDomain domain in ((ProInstrument)group.Questionnaire).Domains)
            {
                ProDomainResult result = new ProDomainResult();
                result.DomainId = domain.Id;
                result.Score = DomainScoreEngine.DomainScoreEngine.CalculateResult(group.Responses.ToList(), domain.ScoreFormula);
                gr.Results.Add(result);
            }

            return gr;
        }

        /// <summary>
        /// Returns all Questionnaire Responses for a given Patient and Questionnaire for all times this Questionnaire was answered by this Patient for the given Episode
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the response for</param>
        /// <param name="episodeId">The episode the questionnaire belongs to</param>
        /// <param name="questionnaireId">The ID of the questionnaire you want to get the response for</param>
        /// <returns>A list of questionnaires</returns>
        public List<QuestionnaireUserResponseGroup> GetAllQuestionnaireResponsesForPatient(string patientId, int episodeId, int questionnaireId)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_QUESTIONNAIRE_RESPONSES_FOR_PATIENT, patientId: patientId, episodeId: episodeId);
                Dictionary<Episode, Dictionary<string, List<QuestionnaireUserResponseGroup>>> result = this.manager.QuestionnaireAccessHandler.GetAllQuestionnaireResponsesForPatient<Questionnaire>(patientId, null, questionnaireId, episodeId);
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_RESPONSES_FOR_PATIENT, AuditEventType.READ, typeof(Patient), "Id", patientId));
                if (result.Count > 0) return result.First().Value.SelectMany(g => g.Value).ToList();
                return new List<QuestionnaireUserResponseGroup>();
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.GET_QUESTIONNAIRE_RESPONSES_FOR_PATIENT, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves and Calculates the results for all domains of the given Questionnaire for all of the questionnaires filled in by the User grouped by Episode and internal dictionary by Questionnaire Name
        /// </summary>                
        /// <param name="patientId">The Id of the Patient to get the data for</param>
        /// <param name="episodeId">The optional Id of the episode to use</param>        
        /// <param name="questionnaireId">The option id of the questionnaire to get the results for</param>
        /// <param name="audience">Get only the results for the domains that are disclosing to this user. Can be left empty</param>
        /// <returns>A list of Results</returns>
        public Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> GetProDomainResults(string patientId = null, int? episodeId = null, int? questionnaireId = null, UserTypes audience = UserTypes.All)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_PRO_DOMAIN_RESULTS, patientId, episodeId);
                var scores = this.manager.QuestionnaireAccessHandler.GetProDomainScores(patientId: patientId, questionnaireId: questionnaireId, episodeId: episodeId);

                List<Episode> filtered = SecuritySession.Current.Filter(scores.Keys);
                scores = scores.Where(s => filtered.Contains(s.Key)).ToDictionary(s => s.Key, s => s.Value);

                // Filter out the text for each object
                foreach (var es in scores)
                {
                    foreach (var sp in es.Value)
                    {
                        foreach (var set in sp.Value)
                        {
                            Questionnaire q = set.Group.Questionnaire;
                            Utilities.Utilities.Filter(ref q, Instance.Baseline, Platform.Chat, audience);
                            set.Group.Questionnaire = q;
                            foreach (var response in set.Group.Responses)
                            {
                                var item = response.Item;
                                Utilities.Utilities.Filter(ref item, Instance.Baseline, Platform.Chat, audience);
                                response.Item = item;
                            }

                            set.Results = set.Results.Where(r => r.Domain.Audience == UserTypes.All || (r.Domain.Audience & audience) == audience).ToList();
                        }
                    }
                }

                return scores;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(patientId))
                {
                    Logger.Audit(new Audit(Model.Security.Actions.GET_PRO_DOMAIN_RESULTS, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                }
                else
                {
                    Logger.Audit(new Audit(Model.Security.Actions.GET_PRO_DOMAIN_RESULTS, AuditEventType.READ, typeof(Episode), "Id", episodeId.ToString(), false, ex.Message));
                }

                throw ex;
            }
        }

        /// <summary>
        /// Gets a list of questionnaire response groups for the current user grouped by Episode
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the outstanding questionnaires for</param>
        /// <param name="notCompletedOnly">If true, only outstanding (not completed) ones are returned.</param>
        /// <returns>A list of Questionnaires response groups for the current user</returns>
        public Dictionary<Episode, List<QuestionnaireUserResponseGroup>> GetQuestionnairesForPatientByEpisode(string patientId, bool notCompletedOnly = true)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_BY_EPISODE, patientId: patientId);
                Dictionary<Episode, List<QuestionnaireUserResponseGroup>> result = this.manager.UserAccessHandler.GetQuestionnairesInEpisodeForPatient(patientId, notCompletedOnly);
                var outstanding = this.manager.UserAccessHandler.GetOutstandingQuestionnairesForPatient(patientId, notCompletedOnly);
                if (outstanding.Count > 0)
                {
                    List<QuestionnaireUserResponseGroup> firstVisit = outstanding.Where(q => !q.Completed && (q.Questionnaire.Name == BusinessLogic.Properties.Settings.Default.NewRegistrationQuestionnaire || q.Questionnaire.Name == BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire)).ToList();
                    if (firstVisit != null)
                    {
                        List<EpisodeMilestone> m = this.manager.EpisodeAccessHandler.GetAssignedMileStones(patientId);
                        if (m.Count > 0)
                        {
                            foreach (QuestionnaireUserResponseGroup group in firstVisit)
                            {
                                if (m.OrderBy(e => e.MilestoneDate).Where(e => e.MilestoneDate > group.DatetimeCreated).First().MilestoneDate < DateTime.Now)
                                {
                                    outstanding.Remove(group);
                                    group.Completed = true;
                                    group.Status = QuestionnaireUserResponseGroupStatus.Missed;

                                    // The date has already passed, so we mark the Questionnaire as completed.
                                    this.manager.QuestionnaireAccessHandler.SaveQuestionnaireResponse(group.Questionnaire.Id, group.Id, true, new List<QuestionnaireResponse>(), QuestionnaireUserResponseGroupStatus.Missed);
                                }
                            }
                        }
                    }

                    result.Add(new Episode() { DateCreated = DateTime.Now }, outstanding);
                }

                Logger.Audit(new Audit(Model.Security.Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_BY_EPISODE, AuditEventType.READ, typeof(Patient), "Id", patientId));
                return result;
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.RETRIEVE_ASSIGNED_QUESTIONNAIRE_BY_EPISODE, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Gets the appropriate completed CompletedCurrentCondition questionnaire
        /// </summary>
        /// <param name="patientId">The id of the patient to get the questionnaire for</param>
        /// <param name="episodeId">The optional id of the episode to get the questionnaire for. If not specified, the last one found is returned instead</param>
        /// <param name="q">The questionnaire instance to fill</param>
        /// <param name="g">The responsegroup to fill</param>
        public void GetCompletedCurrentConditionQuestionnaire(string patientId, int? episodeId, ref Questionnaire q, ref QuestionnaireUserResponseGroup g)
        {
            try
            {
                SecuritySession.Current.VerifyAccess(Actions.GET_COMPLETED_CURRENT_CONDITION_QUESTIONNAIRE, patientId: patientId, episodeId: episodeId);

                DateTime? date = null;
                if (episodeId != null && episodeId.Value > 0)
                {
                    Episode e = this.manager.EpisodeAccessHandler.GetEpisodeById(episodeId.Value);
                    date = e.DateCreated;
                }

                g = this.manager.QuestionnaireAccessHandler.GetCurrentQuestionnaireResponsesForUser(patientId, BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire, date);
                if (g == null) throw this.manager.MessageHandler.GetError(ErrorCodes.QUESTIONNAIRE_NOT_ASSIGNED);

                q = this.manager.QuestionnaireAccessHandler.GetFullQuestionnaireByName(BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire);
                q.CurrentInstance = this.manager.QuestionnaireAccessHandler.GetAllQuestionnaireResponsesForPatient<Questionnaire>(patientId, BusinessLogic.Properties.Settings.Default.CurrentConditionQuestionnaire).Count > 0 ? Instance.Followup : Instance.Baseline;
                Logger.Audit(new Audit(Model.Security.Actions.GET_COMPLETED_CURRENT_CONDITION_QUESTIONNAIRE, AuditEventType.READ, typeof(Patient), "Id", patientId));
            }
            catch (Exception ex)
            {
                Logger.Audit(new Audit(Model.Security.Actions.GET_COMPLETED_CURRENT_CONDITION_QUESTIONNAIRE, AuditEventType.READ, typeof(Patient), "Id", patientId, false, ex.Message));
                throw ex;
            }
        }

        #endregion

        #region helperFunctions

        /// <summary>
        /// Encrypts the user Id, questionniareName and format Name as an string that can be used for anonymous questionnaire excess
        /// </summary>
        /// <param name="patientId">The id of the user</param>
        /// <param name="questionnaireName">the name of the questionniare</param>
        /// <param name="formatName">The name of the format</param>
        /// <param name="episodeId">The If of the episode to encrypt as well.</param>
        /// <returns>An encrypted string based upon the parameters</returns>
        private string EncryptAnonymous(string patientId, string questionnaireName, string formatName, int? episodeId)
        {
            string stringToEncrypt = "<root><p>" + patientId + "</p><q>" + questionnaireName + "</q><f>" + formatName + "</f>";
            if (episodeId.HasValue) stringToEncrypt += "<e>" + episodeId.Value + "</e>";
            stringToEncrypt += "</root>";
            return MachineKeyEncryption.Encrypt(stringToEncrypt);
        }

        /// <summary>
        /// Decrypts the string encrypted with <see cref="M:EncryptAnonymous"/> and puts the found data in the userId, questionniareName and formatName references
        /// </summary>
        /// <param name="anonymous">The string to decrypt</param>
        /// <param name="patientId">The found id of the user</param>
        /// <param name="questionnaireName">The found name of the questionniare</param>
        /// <param name="formatName">The found format name</param>
        /// <param name="episodeId">The episode ID to fill if it is there</param>        
        private void DecryptAnonymous(string anonymous, ref string patientId, ref string questionnaireName, ref string formatName, ref int? episodeId)
        {
            XElement element = XElement.Parse(MachineKeyEncryption.Decrypt(anonymous));
            foreach (XNode c in element.Nodes())
            {
                XElement e = (XElement)c;
                switch (e.Name.LocalName)
                {
                    case "p":
                        patientId = e.Value;
                        break;
                    case "q":
                        questionnaireName = e.Value;
                        break;
                    case "f":
                        formatName = e.Value;
                        break;
                    case "e":
                        episodeId = int.Parse(e.Value);
                        break;
                }
            }
        }

        #endregion helperFunctions
    }
}