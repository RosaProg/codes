using LinqKit;
using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Episodes;
using PCHI.Model.Notifications;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Instructions;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Tag;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles the access to and from the database for Questionnaires
    /// </summary>
    public class QuestionnaireAccessHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireAccessHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal QuestionnaireAccessHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        #region Questionnaires

        /// <summary>
        /// Adds a full Pro Instrument to the database, including all related objects such as ProItem and ProOption
        /// </summary>
        /// <param name="questionnaire">The <see cref="Questionnaire"/> to add to the database</param>
        public void AddFullQuestionnaire(Questionnaire questionnaire)
        {
            if (this.context.QuestionnaireConcepts.Any(pc => pc.Id == questionnaire.Concept.Id || pc.Name.Equals(questionnaire.Concept.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                questionnaire.Concept = this.context.QuestionnaireConcepts.Where(pc => pc.Id == questionnaire.Concept.Id || pc.Name.Equals(questionnaire.Concept.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                this.context.Entry(questionnaire.Concept).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                this.context.QuestionnaireConcepts.Add(questionnaire.Concept);
            }

            this.context.Questionnaires.Add(questionnaire);

            foreach (QuestionnaireIntroductionMessage m in questionnaire.IntroductionMessages)
            {
                m.Questionnaire = questionnaire;
                this.context.QuestionnaireIntroductionMessages.Add(m);
            }

            foreach (QuestionnaireDescription desc in questionnaire.Descriptions)
            {
                desc.Questionnaire = questionnaire;
                this.context.QuestionnaireDescriptions.Add(desc);
            }

            if (questionnaire.GetType() == typeof(ProInstrument))
            {
                ProInstrument inst = (ProInstrument)questionnaire;
                foreach (ProDomain dom in inst.Domains)
                {
                    dom.Instrument = questionnaire;
                    this.context.ProDomains.Add(dom);
                    foreach (ProDomainResultRange range in dom.ResultRanges)
                    {
                        range.Domain = dom;
                        this.context.ProDomainResultRanges.Add(range);
                    }
                }
            }

            int sectionOrderCount = 0;
            foreach (QuestionnaireSection sec in questionnaire.Sections)
            {
                sec.OrderInInstrument = sectionOrderCount++;
                int itemOrderCount = 0;
                sec.Questionnaire = questionnaire;
                this.context.QuestionnaireSections.Add(sec);
                foreach (QuestionnaireSectionInstruction instruction in sec.Instructions)
                {
                    instruction.Section = sec;
                    this.context.QuestionnaireSectionInstructions.Add(instruction);
                }

                foreach (QuestionnaireElement element in sec.Elements)
                {
                    element.OrderInSection = itemOrderCount++;
                    foreach (QuestionnaireElementTextVersion version in element.TextVersions)
                    {
                        version.QuestionnaireElement = element;
                        this.context.QuestionnaireElementTextVersions.Add(version);
                    }

                    if (element.GetType() == typeof(QuestionnaireText))
                    {
                        QuestionnaireText text = (QuestionnaireText)element;
                        text.Section = sec;
                        this.context.QuestionnaireElements.Add(text);
                    }
                    else if (element.GetType() == typeof(QuestionnaireItem))
                    {
                        QuestionnaireItem item = (QuestionnaireItem)element;
                        item.Section = sec;
                        this.context.QuestionnaireElements.Add(item);
                        foreach (QuestionnaireItemInstruction instruction in item.Instructions)
                        {
                            instruction.Item = item;
                            this.context.QuestionnaireItemInstructions.Add(instruction);
                        }

                        int groupOrderCount = 0;
                        foreach (QuestionnaireItemOptionGroup group in item.OptionGroups)
                        {
                            foreach (QuestionnaireItemOptionGroupTextVersion version in group.TextVersions)
                            {
                                version.QuestionnaireItemOptionGroup = group;
                                this.context.QuestionnaireItemOptionGroupTextVersions.Add(version);
                            }

                            group.OrderInItem = groupOrderCount++;
                            group.Item = item;
                            this.context.QuestionnaireItemOptionGroups.Add(group);
                            int optionOrderCount = 0;
                            foreach (QuestionnaireItemOption o in group.Options)
                            {
                                o.OrderInGroup = optionOrderCount++;
                                o.Group = group;
                                this.context.QuestionnaireItemOptions.Add(o);
                            }
                        }
                    }
                }
            }

            try
            {
                this.context.SaveChanges();

                foreach (Tag t in questionnaire.Tags)
                {
                    this.AddTagToQuestionnaireByName(t, questionnaire.Name);
                }
            }
            catch (DbEntityValidationException e)
            {
                string errorResult = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorResult += "Entity of type \" " + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State + "\" has the following validation errors: \n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorResult += "- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\" \n";
                    }
                }

                throw new DbEntityValidationException(errorResult, e);
            }
        }

        /// <summary>
        /// Gets the Last created Full Pro Instrument with the given name
        /// </summary>
        /// <param name="name">The name of the ProInstrument to retrieve</param>
        /// <returns>The ProInstrument found or null</returns>
        public Questionnaire GetFullQuestionnaireByName(string name)
        {
            return this.GetFullQuestionnaire(name: name);
        }

        /// <summary>
        /// Gets a Full Questionnaire with the given Id
        /// </summary>
        /// <param name="id">The Id of the Questionnaire to retrieve</param>
        /// <returns>The ProInstrument found or null</returns>
        public Questionnaire GetFullQuestionnaireById(int id)
        {
            return this.GetFullQuestionnaire(id);
        }

        /// <summary>
        /// Gets the full questionniare. Only the Id or the name has to be provided
        /// </summary>
        /// <param name="id">The Id of the questionnaire. If null or empty is not used</param>
        /// <param name="name">The name of the questionniare. If null or empty is not used</param>
        /// <returns>The questionniare requested</returns>
        private Questionnaire GetFullQuestionnaire(int? id = null, string name = null)
        {
            Questionnaire result = this.QuestionnaireQuery(id, name).SingleOrDefault();
            if (result == null) return null;

            if (result.GetType() == typeof(ProInstrument))
            {
                ProInstrument ins = (ProInstrument)result;
                ins.Domains = this.context.ProDomains.Where(d => d.Instrument.Id == result.Id).Include(d => d.ResultRanges).ToList();
            }

            result.Tags = this.context.QuestionnaireTags.Where(q => q.QuestionnaireName == result.Name).Select(q => q.Tag).ToList();

            result.Sections = result.Sections.OrderBy(p => p.OrderInInstrument).ToList();
            foreach (QuestionnaireSection section in result.Sections)
            {
                section.Instructions.Count();
                section.Elements = section.Elements.OrderBy(e => e.OrderInSection).ToList();
                foreach (QuestionnaireElement el in section.Elements)
                {
                    if (el.GetType() == typeof(QuestionnaireItem))
                    {
                        QuestionnaireItem item = (QuestionnaireItem)el;
                        item.OptionGroups = item.OptionGroups.OrderBy(o => o.OrderInItem).ToList();
                        item.OptionGroups = this.context.QuestionnaireItemOptionGroups.Where(og => og.Item.Id == item.Id).Include(og => og.Options).ToList();
                        foreach (QuestionnaireItemOptionGroup g in item.OptionGroups)
                        {
                            g.TextVersions = this.context.QuestionnaireItemOptionGroupTextVersions.Where(tv => tv.QuestionnaireItemOptionGroup.Id == g.Id).ToList();
                            g.Options = g.Options.OrderBy(o => o.OrderInGroup).ToList();
                        }

                        item.Instructions = this.context.QuestionnaireItemInstructions.Where(i => i.Item.Id == item.Id).ToList();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Maps the relation between the given tag and questionnaire 
        /// </summary>
        /// <param name="tag">Tag for the questionnaire</param>
        /// <param name="questionnaireId">Questionnaire's id to be used</param>
        public void AddTagToQuestionnaireById(Tag tag, int questionnaireId)
        {
            string name = this.context.Questionnaires.Where(q => q.Id == questionnaireId).Select(q => q.Name).ToString();
            this.AddTagToQuestionnaireByName(tag, name);
        }

        /// <summary>
        /// Maps the relation between the given tag and questionnaire
        /// </summary>
        /// <param name="tag">Tag for the questionnaire</param>
        /// <param name="name">Questionnaire's name to be used</param>
        public void AddTagToQuestionnaireByName(Tag tag, string name)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.TagName) || string.IsNullOrWhiteSpace(tag.Value)) return;

            if (this.context.Questionnaires.Any(q => q.Name == name))
            {
                if (this.context.Tags.Any(t => t.TagName == tag.TagName && t.Value == tag.Value))
                {
                    this.context.Tags.Attach(tag);
                }
                else
                {
                    this.context.Tags.Add(tag);
                }

                if (!this.context.QuestionnaireTags.Any(qtg => qtg.QuestionnaireName == name && qtg.Tag.TagName == tag.TagName && qtg.Tag.Value == tag.Value))
                {
                    this.context.QuestionnaireTags.Add(new QuestionnaireTag() { QuestionnaireName = name, Tag = tag });
                    this.context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gets all ProInstruments stored in the database including their concepts and domains
        /// </summary>
        /// <returns>A list of ProInstruments</returns>
        public List<ProInstrument> ListProInstruments()
        {
            return this.context.Questionnaires.OfType<ProInstrument>().Include(pi => pi.Concept).Include(pi => pi.Domains.Select(d => d.ResultRanges)).ToList();
        }

        /// <summary>
        /// Gets a the latest (newest version) of Questionnaires in the database 
        /// </summary>
        /// <typeparam name="T">The type of Questionnaire to look for. If Questionnaire is specified instead of a sub class filtering is not applied</typeparam>
        /// <returns>A list of all questionnaires</returns>        
        public List<Questionnaire> GetAllQuestionnairesWithTags<T>() where T : Questionnaire
        {
            IQueryable<Questionnaire> query = this.context.Questionnaires;
            if (typeof(T) != typeof(Questionnaire)) query = query.OfType<T>();
            query = query.GroupBy(q => q.Name).Select(q => q.FirstOrDefault());
            List<Questionnaire> questionnaires = query.ToList();
            foreach (Questionnaire questionnaire in questionnaires)
            {
                questionnaire.Tags = this.context.QuestionnaireTags.Where(t => t.QuestionnaireName == questionnaire.Name).Select(t => t.Tag).ToList();
            }

            return questionnaires;
        }
        #endregion

        #region Questionnaire Responses
        /// <summary>
        /// Creates a User Response Group for the given user and Questionnaire Name
        /// </summary>
        /// <param name="patientId">The id of the Patient</param>
        /// <param name="questionnaireName">The name of the questionnaire</param>
        /// <param name="formatName">the name of the format to create the response group for. If null the default format name of the questionnaire is used</param>
        /// <param name="sqd">The optional scheduled questionniare date that the user responsegroup must belong to</param>
        /// <returns>The newly created QuestionnaireUserResponseGroup</returns>
        public QuestionnaireUserResponseGroup CreateQuestionnaireUserResponseGroup(string patientId, string questionnaireName, string formatName, ScheduledQuestionnaireDate sqd)
        {
            QuestionnaireUserResponseGroup group = new QuestionnaireUserResponseGroup();
            group.Patient = this.context.Patients.Where(u => u.Id == patientId).Single();
            group.Questionnaire = this.context.Questionnaires.Where(q => q.Name == questionnaireName).OrderByDescending(q => q.Id).First();
            group.QuestionnaireFormatName = formatName == null ? group.Questionnaire.DefaultFormatName : formatName;
            group.DatetimeCreated = DateTime.Now;
            group.Status = QuestionnaireUserResponseGroupStatus.New;
            group.ScheduledQuestionnaireDate = sqd == null ? null : this.context.ScheduledQuestionnaireDates.Where(s => s.Id == sqd.Id).Single();
            this.context.QuestionnaireUserResponseGroups.Add(group);
            this.context.SaveChanges();
            new NotificationHandler(this.context).CreateNotification(NotificationType.NewQuestionnaire, group.Id);

            return group;
        }

        /// <summary>
        /// Saves the given <see cref="QuestionnaireResponse"/>  to the database
        /// Existing answers are overwritten
        /// </summary>        
        /// <param name="questionnaireId">The Id of the questionnaire to save the response for</param>
        /// <param name="groupId">The Id of the group for which to save the responses</param>
        /// <param name="questionnaireCompleted">Indicates whether or user has filled in all responses and the Questionnaire is now completed</param>
        /// <param name="responses">A list of <see cref="QuestionnaireResponse"/> to save</param>
        /// <param name="status">The optional status to set the QuestionnaireUserResponse group to. If not given the status will be adjusted based upon the QuestionnaireCompleted flag and if there are any responses</param>
        /// <exception cref="ArgumentException">Thrown when either the questionniare or group has not been found</exception>
        public void SaveQuestionnaireResponse(int questionnaireId, int groupId, bool questionnaireCompleted, List<QuestionnaireResponse> responses, QuestionnaireUserResponseGroupStatus? status = null)
        {
            Questionnaire questionnaire = this.context.Questionnaires.Where(q => q.Id == questionnaireId).SingleOrDefault();
            QuestionnaireUserResponseGroup group = this.context.QuestionnaireUserResponseGroups.Where(u => u.Questionnaire.Name == questionnaire.Name && u.Id == groupId).Include(u => u.Questionnaire).Include(u => u.Patient).SingleOrDefault();

            if (group == null) throw new ArgumentException("No group has been found for the given User and Questionnaire");
            if (questionnaire == null) throw new ArgumentException("Questionniare doesn't exist");

            if (!group.StartTime.HasValue)
            {
                group.StartTime = DateTime.Now;
            }

            if (questionnaireCompleted)
            {
                group.Completed = true;
                group.Status = QuestionnaireUserResponseGroupStatus.Completed;
                group.DateTimeCompleted = DateTime.Now;
            }
            else if (responses.Count > 0)
            {
                group.Status = QuestionnaireUserResponseGroupStatus.InProgress;
            }

            if (status.HasValue)
            {
                group.Status = status.Value;
            }

            if (responses.Count > 0)
            {
                List<QuestionnaireResponse> existingResponses = this.context.QuestionnaireResponses.Where(r => r.QuestionnaireUserResponseGroup.Id == groupId).Include(r => r.Item).ToList();
                if (group.Questionnaire.Id != questionnaireId)
                {
                    // delete all existing answers as we have a new questionnaire Id
                    group.Questionnaire = this.context.Questionnaires.Where(q => q.Id == questionnaireId).Single();
                }

                // Get all option groups that multi-select enabled
                List<int> multiSelectGroupsIds = this.context.QuestionnaireItemOptionGroups.Where(g => g.Item.Section.Questionnaire.Id == questionnaireId && g.ResponseType == QuestionnaireResponseType.MultiSelect).Select(g => g.Id).ToList();

                // Load all the relevant options to increase performance
                this.context.QuestionnaireItemOptions.Where(o => o.Group.Item.Section.Questionnaire.Id == questionnaireId).Load();

                foreach (QuestionnaireResponse response in responses)
                {
                    // QuestionnaireItem item = this.context.QuestionnaireElements.OfType<QuestionnaireItem>().Where(q => q.Id == response.Item.Id).Include(i => i.OptionGroups.Select(g => g.Options)).Single();
                    QuestionnaireItemOptionGroup optionGroup = response.Option == null ? null : this.context.QuestionnaireItemOptions.Where(o => o.Id == response.Option.Id).Select(o => o.Group).Single();

                    if (multiSelectGroupsIds.Contains(optionGroup.Id))
                    {
                        response.QuestionnaireUserResponseGroup = group;
                        response.Item = response.Item == null ? null : this.context.QuestionnaireElements.OfType<QuestionnaireItem>().Where(el => el.Id == response.Item.Id).SingleOrDefault();
                        response.Option = response.Option == null ? null : this.context.QuestionnaireItemOptions.Where(o => o.Id == response.Option.Id).SingleOrDefault();
                        if (response.Item == null && response.Option == null) throw new ArgumentNullException("Both Item and Option are null in the QuestionnaireResponse and this is not allowed");
                        this.context.QuestionnaireResponses.Add(response);
                    }
                    else
                    {
                        QuestionnaireResponse existing = existingResponses.Where(r => r.Item.Id == response.Item.Id && r.Id == optionGroup.Id).SingleOrDefault();
                        if (existing != null)
                        {
                            existing.Option = response.Option == null ? null : this.context.QuestionnaireItemOptions.Where(o => o.Id == response.Option.Id).SingleOrDefault();
                            existing.ResponseValue = response.ResponseValue;
                            existing.ResponseText = response.ResponseText;
                            this.context.Entry(existing).State = EntityState.Modified;
                        }
                        else
                        {
                            response.QuestionnaireUserResponseGroup = group;
                            response.Item = response.Item == null ? null : this.context.QuestionnaireElements.OfType<QuestionnaireItem>().Where(el => el.Id == response.Item.Id).SingleOrDefault();
                            response.Option = response.Option == null ? null : this.context.QuestionnaireItemOptions.Where(o => o.Id == response.Option.Id).SingleOrDefault();
                            if (response.Item == null && response.Option == null) throw new ArgumentNullException("Both Item and Option are null in the QuestionnaireResponse and this is not allowed");
                            this.context.QuestionnaireResponses.Add(response);
                        }
                    }
                }

                existingResponses.Where(r => this.context.Entry(r).State == EntityState.Unchanged).ToList().ForEach(o => this.context.Entry(o).State = EntityState.Deleted);
            }

            try
            {
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string errorResult = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorResult += "Entity of type \" " + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State + "\" has the following validation errors: \n";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorResult += "- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\" \n";
                    }
                }

                throw new DbEntityValidationException(errorResult, e);
            }
        }

        /// <summary>
        /// Returns all Questionnaire Responses for a given Patient and Questionnaire for all times this Questionnaire was answered by this User
        /// </summary>
        /// <param name="patientId">The Id of the Patient to get the response for</param>
        /// <param name="questionnaireName">The name of the questionnaire you want to get the response for</param>        
        /// <param name="questionnaireId">The optional Id of the questionnaire to filter on</param>
        /// <param name="episodeId">The optional Id of the episode to filter on</param>
        /// <param name="onlyCompleted">If true, only completed questionnaire responses are loaded.</param>
        /// <param name="audience">The list of user types to filter on that the domains must disclose to before they are loaded.</param>
        /// <typeparam name="T">The type of questionaire to look for. If <see cref="Questionnaire"/> is specified no filtering is applied.</typeparam>
        /// <returns>A Dictionary of Questionnaire Responses grouped by Episode and Questionnaire</returns>
        public Dictionary<Episode, Dictionary<string, List<QuestionnaireUserResponseGroup>>> GetAllQuestionnaireResponsesForPatient<T>(string patientId = null, string questionnaireName = null, int? questionnaireId = null, int? episodeId = null, bool onlyCompleted = true, params UserTypes[] audience) where T : Questionnaire
        {
            var query = this.context.QuestionnaireUserResponseGroups.Where(q => q.Id != null);

            if (!string.IsNullOrWhiteSpace(patientId))
            {
                query = query.Where(q => q.Patient.Id == patientId);
            }

            if (!string.IsNullOrWhiteSpace(questionnaireName))
            {
                query = query.Where(q => q.Questionnaire.Name == questionnaireName);
            }

            if (questionnaireId.HasValue && questionnaireId.Value > 0)
            {
                query = query.Where(q => q.Questionnaire.Name == this.context.Questionnaires.Where(qq => qq.Id == questionnaireId).Select(qq => qq.Name).FirstOrDefault());
            }

            if (episodeId.HasValue && episodeId.Value > 0)
            {
                query = query.Where(q => q.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode.Id == episodeId.Value);
            }

            if (typeof(T) != typeof(Questionnaire))
            {
                var subQuery = this.context.Questionnaires.OfType<T>();
                if (questionnaireName != null) subQuery = subQuery.Where(q => q.Name == questionnaireName);

                var subQueryIds = subQuery.Select(q => q.Id);
                query = query.Where(q => subQueryIds.Contains(q.Questionnaire.Id));
            }

            query = query.Include(q => q.Questionnaire).Include(q => q.Responses.Select(r => r.Item)).Include(q => q.Responses.Select(r => r.Option)).Include(g => g.Patient).Include(g => g.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode.MileStones);

            if (onlyCompleted) query = query.Where(q => q.Status == QuestionnaireUserResponseGroupStatus.Completed);

            List<QuestionnaireUserResponseGroup> groups;
            groups = query.ToList();

            foreach (QuestionnaireUserResponseGroup g in groups)
            {
                if (g.Questionnaire.GetType() == typeof(ProInstrument))
                {
                    ProInstrument p = (ProInstrument)g.Questionnaire;
                    var domainQuery = this.context.ProDomains.Where(d => d.Instrument.Name == p.Name && d.Instrument.Id == this.context.Questionnaires.Where(q => q.Name == p.Name).OrderByDescending(q => q.Id).Select(q => q.Id).FirstOrDefault());
                    foreach (UserTypes t in audience)
                    {
                        domainQuery = domainQuery.Where(q => (q.Audience & t) == t);
                    }

                    p.Domains = domainQuery.Include(d => d.ResultRanges).Include(d => d.Instrument).ToList();
                    g.Questionnaire = p;
                }

                foreach (QuestionnaireResponse r in g.Responses)
                {
                    r.Option = this.context.QuestionnaireItemOptions.Where(o => o.Id == r.Option.Id).Include(o => o.Group).SingleOrDefault();
                }
            }

            Dictionary<Episode, Dictionary<string, List<QuestionnaireUserResponseGroup>>> result = new Dictionary<Episode, Dictionary<string, List<QuestionnaireUserResponseGroup>>>();
            result = groups.Where(g => g.ScheduledQuestionnaireDate != null).GroupBy(g => g.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).ToDictionary(g => g.Key, g => g.GroupBy(g2 => g2.Questionnaire.Name).ToDictionary(g2 => g2.Key, g2 => g2.ToList()));
            result.Add(new Episode(), groups.Where(g => g.ScheduledQuestionnaireDate == null).GroupBy(g => g.Questionnaire.Name).ToDictionary(g => g.Key, g => g.ToList()));

            return result;
        }

        /// <summary>
        /// Gets the Last answers given for the user to this Questionnaire provided the QuestionnaireUserResponseGroup is not marked as Completed
        /// </summary>
        /// <param name="userEntityId">The Id of the PCHI User Entity to get the responses for</param>
        /// <param name="questionnaireName">The questionnaire Id to get the responses for</param>
        /// <param name="episodeId">The optional Id of the episode to look for. If specified, it will look ONLY for response groups belonging to this Episode.</param>
        /// <param name="excludeCompleted">If true (default) and the last ResponseGroup is completed Null is returned. If false, the last response group is returned instead</param>
        /// <returns>A QuestionnaireUserResponseGroup containing all responses given for that group</returns>
        public QuestionnaireUserResponseGroup GetLastQuestionnaireResponsesForPatient(string userEntityId, string questionnaireName, int? episodeId, bool excludeCompleted = true)
        {
            var q = this.context.QuestionnaireUserResponseGroups.Where(u => u.Patient.Id == userEntityId && u.Questionnaire.Name == questionnaireName).Include(u => u.Patient).Include(u => u.Responses.Select(r => r.Item)).Include(u => u.Responses.Select(r => r.Option.Group.Item.Section.Questionnaire)).Include(r => r.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).Include(r => r.Questionnaire);
            if (episodeId != null && episodeId.Value > 0)
            {
                q = q.Where(e => e.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode.Id == episodeId.Value);
            }
            else
            {
                q = q.Where(e => e.ScheduledQuestionnaireDate == null);
            }

            QuestionnaireUserResponseGroup group = q.OrderByDescending(u => u.Id).FirstOrDefault();
            if (group == null || (excludeCompleted && group.Completed)) return null;

            return group;
        }

        /// <summary>
        /// Gets the QuestionnaireUserResponseGroups by ResponseGroupId
        /// Only includes the Patient and response
        /// </summary>
        /// <param name="groupId">The id of the group get the the response group for.</param>
        /// <returns>The Questionnaire User Response Group found with the User and Responses loaded</returns>
        public QuestionnaireUserResponseGroup GetSmallQuestionnaireUserResponseGroupById(int groupId)
        {
            return this.context.QuestionnaireUserResponseGroups.Where(g => g.Id == groupId).Include(g => g.Patient).Include(g => g.Responses).FirstOrDefault();
        }

        /// <summary>
        /// Gets the QuestionnaireUserResponseGroups by ResponseGroupId
        /// Includes full questionnaire, patient, responses, items, etc
        /// </summary>
        /// <param name="groupId">The id of the group get the the response group for.</param>
        /// <returns>The Questionnaire User Response Group found with the User and Responses loaded</returns>        
        public QuestionnaireUserResponseGroup GetFullQuestionnaireUserResponseGroupById(int groupId)
        {
            var group = this.GetFullResponseGroupQuery(groupId).FirstOrDefault(); // this.context.QuestionnaireUserResponseGroups.Where(g => g.Id == groupId).Include(g => g.Patient).Include(r => r.Questionnaire).Include(r => r.Responses.Select(i => i.Item.OptionGroups.Select(g => g.Options))).Include(r => r.Responses.Select(o => o.Option.Group.Item)).FirstOrDefault();
            group.Questionnaire = this.GetFullQuestionnaireById(group.Questionnaire.Id);

            return group;
        }

        /// <summary>
        /// Finds the current completed QuestionUserResponseGroup for the given user and questionnaireName.        
        /// </summary>
        /// <param name="userId">The user Id to  get the response group for</param>
        /// <param name="questionnaireName">The name of the questionnaire to get the response group for</param>
        /// <param name="date">The date by which to get the response group.</param>
        /// <returns>If Date has not been specified, the last one is always returend. If the date has been specified, the first QuestionnaireUserResponseGroup that was completed after the given date is returned.</returns>
        public QuestionnaireUserResponseGroup GetCurrentQuestionnaireResponsesForUser(string userId, string questionnaireName, DateTime? date)
        {
            var q = this.context.QuestionnaireUserResponseGroups.Where(u => u.Patient.Id == userId && u.Questionnaire.Name == questionnaireName && u.Status == QuestionnaireUserResponseGroupStatus.Completed).Include(u => u.Responses.Select(r => r.Item)).Include(u => u.Responses.Select(r => r.Option.Group.Item.Section.Questionnaire)).Include(r => r.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).Include(r => r.Questionnaire);
            if (date.HasValue)
            {
                q = q.Where(e => e.DateTimeCompleted > date.Value);
            }
            else
            {
                q = q.Where(e => e.DateTimeCompleted == this.context.QuestionnaireUserResponseGroups.Where(e2 => e2.Patient.Id == userId && e2.Questionnaire.Name == questionnaireName).Max(e2 => e2.DateTimeCompleted).Value);
            }

            QuestionnaireUserResponseGroup group = q.OrderBy(u => u.DateTimeCompleted).FirstOrDefault();
            return group;
        }
        #endregion

        /// <summary>
        /// Gets the Data Extraction definitions for the questionnaire with the given name
        /// </summary>
        /// <param name="questionnaireName">The name of the questionniare</param>
        /// <returns>A list of data extraction definitions</returns>
        public List<QuestionnaireDataExtraction> GetDataExtrationDefinitions(string questionnaireName)
        {
            return this.context.QuestionnaireDataExtractions.Where(q => q.QuestionnaireName == questionnaireName).ToList();
        }

        /// <summary>
        /// Saves the given list of questionniareUserResponseGroup tags to the database
        /// </summary>
        /// <param name="groupTags">The QuestionnaireUserResponseGroupTags to add</param>
        public void AddOrUpdateQuestionnaireUserResponseGroupTags(IEnumerable<QuestionnaireUserResponseGroupTag> groupTags)
        {
            List<QuestionnaireUserResponseGroupTag> tags = groupTags.ToList();
            tags.ForEach(t => { t.GroupId = t.QuestionnaireUserResponseGroup.Id; t.QuestionnaireUserResponseGroup = null; });
            this.context.QuestionnaireUserResponseGroupTags.AddOrUpdate(tags.ToArray());
            this.context.SaveChanges();
        }

        /// <summary>
        /// Saves the given ProDomainResultSet and all results to the database
        /// </summary>
        /// <param name="proDomainResultSet">The ProDomainResultSet to save</param>
        public void SaveProDomainResult(ProDomainResultSet proDomainResultSet)
        {
            if (proDomainResultSet.GroupId < 1)
            {
                proDomainResultSet.GroupId = proDomainResultSet.Group.Id;
            }

            this.context.ProDomainResultSet.Where(pds => pds.Group.Id == proDomainResultSet.GroupId).ToList().ForEach(pds => this.context.Entry(pds).State = EntityState.Deleted);
            this.context.ProDomainResultSet.Add(proDomainResultSet);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets the scores for a domain
        /// </summary>
        /// <param name="patientId">The Id of the pateitn</param>
        /// <param name="questionnaireId">The questionnaire Id to get the results for</param>
        /// <param name="episodeId">The episode Id to get the results for</param>        
        /// <returns>The Pro Domain scores sorted by episode and then by Questionnaire</returns>
        public Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> GetProDomainScores(string patientId, int? questionnaireId, int? episodeId)
        {
            var query = this.context.ProDomainResultSet.Where(q => q.Group.ScheduledQuestionnaireDate != null);

            if (!string.IsNullOrWhiteSpace(patientId))
            {
                query = query.Where(q => q.Group.Patient.Id == patientId);
            }

            if (questionnaireId.HasValue && questionnaireId.Value > 0)
            {
                query = query.Where(q => q.Group.Questionnaire.Name == this.context.Questionnaires.Where(qq => qq.Id == questionnaireId).Select(qq => qq.Name).FirstOrDefault());
            }

            if (episodeId.HasValue && episodeId.Value > 0)
            {
                query = query.Where(q => q.Group.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode.Id == episodeId.Value);
            }

            query = query.Include(q => q.Group.Questionnaire.IntroductionMessages)
                .Include(q => q.Group.Responses.Select(r => r.Item.TextVersions))
                .Include(q => q.Group.Responses.Select(r => r.Option))
                .Include(g => g.Group.Patient)
                .Include(g => g.Group.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode.MileStones)
                .Include(g => g.Results.Select(r => r.Domain.ResultRanges));

            List<ProDomainResultSet> sets;

            sets = query.ToList();
            foreach (ProDomainResultSet set in sets)
            {
                foreach (ProDomainResult domainResult in set.Results)
                {
                    List<string> actionIds = domainResult.Domain.ItemActionIds;
                    domainResult.Responses = set.Group.Responses.Where(r => actionIds.Contains(r.Item.ActionId)).OrderBy(r => r.Item.OrderInSection).ToList();
                }

                set.Results = set.Results.OrderByDescending(r => r.Domain.IsTotalDomain).ThenBy(r => r.Domain.Name).ToList();
            }

            Dictionary<Episode, Dictionary<string, List<ProDomainResultSet>>> result = sets.GroupBy(s => s.Group.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).ToDictionary(g => g.Key, g => g.GroupBy(g2 => g2.Group.Questionnaire.Name).ToDictionary(g2 => g2.Key, g2 => g2.ToList()));

            return result;
        }

        #region Standard Queries

        /// <summary>
        /// Gets the Questionnaire Query
        /// Only the Id or the Name can be provided
        /// </summary>
        /// <param name="id">The Id of the questionnaire. If null or empty is not used</param>
        /// <param name="name">The name of the questionniare. If null or empty is not used</param>
        /// <returns>The query to get the questionnaire</returns>
        internal IQueryable<Questionnaire> QuestionnaireQuery(int? id = null, string name = null)
        {
            IQueryable<Questionnaire> query = null;
            if (id.HasValue)
            {
                query = this.context.Questionnaires.Where(pi => pi.Id == id.Value);
            }
            else
            {
                query = this.context.Questionnaires.Where(pi => pi.Id == this.context.Questionnaires.Where(q => q.Name == name).OrderByDescending(q => q.Id).Select(q => q.Id).FirstOrDefault());
            }

            query = query.Include(pi => pi.Concept).Include(pi => pi.Sections.Select(s => s.Instructions)).Include(pi => pi.Sections.Select(s => s.Elements.Select(e => e.TextVersions))).Include(q => q.IntroductionMessages);

            return query;
        }

        /// <summary>
        /// Returs a query to get the Full QuestionnaireUserResponseGroup
        /// </summary>
        /// <param name="groupId">The optional id of the group to retrieve</param>
        /// <returns>The query to get a QuestionnareUserResponseGroup</returns>
        internal IQueryable<QuestionnaireUserResponseGroup> GetFullResponseGroupQuery(int? groupId = null)
        {
            IQueryable<QuestionnaireUserResponseGroup> result = null;
            if (!groupId.HasValue)
            {
                result = this.context.QuestionnaireUserResponseGroups.Where(g => g.Id != null);
            }
            else
            {
                result = this.context.QuestionnaireUserResponseGroups.Where(g => g.Id == groupId);
            }

            result = result.Include(g => g.Patient).Include(r => r.Questionnaire).Include(r => r.Responses.Select(i => i.Item.OptionGroups.Select(g => g.Options))).Include(r => r.Responses.Select(o => o.Option.Group.Item.Section.Questionnaire));
            return result;
        }

        #endregion
    }
}
