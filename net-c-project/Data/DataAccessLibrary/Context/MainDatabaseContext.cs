using Microsoft.AspNet.Identity.EntityFramework;
using PCHI.DataAccessLibrary.Model;
using PCHI.Model.Episodes;
using PCHI.Model.Messages;
using PCHI.Model.Notifications;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Instructions;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Security;
using PCHI.Model.Tag;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.Context
{
    /// <summary>
    /// The DbContext that handles all communication with the database
    /// </summary>
    public class MainDatabaseContext : DbContext
    {
        /// <summary>
        /// Gets the Internal Object Context instance
        /// There are quite a few tricks for the EntityFramework that depend on the ObjectContext being available. This should allow their use.
        /// </summary>
        public ObjectContext ObjectContext { get { return ((IObjectContextAdapter)this).ObjectContext; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDatabaseContext"/> class
        /// </summary>
        public MainDatabaseContext()
            : base("MainContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region Questionnaire
        /// <summary>
        /// Gets or sets all the ProConcepts
        /// </summary>
        public DbSet<QuestionnaireConcept> QuestionnaireConcepts { get; set; }

        /// <summary>
        /// Gets or sets all the ProInstruments
        /// </summary>
        public DbSet<Questionnaire> Questionnaires { get; set; }

        /// <summary>
        /// Gets or sets all the ProDomains
        /// </summary>
        public DbSet<ProDomain> ProDomains { get; set; }

        /// <summary>
        /// Gets or sets all the ProDomainResultRanges
        /// </summary>
        public DbSet<ProDomainResultRange> ProDomainResultRanges { get; set; }

        /// <summary>
        /// Gets or sets all the ProSections
        /// </summary>
        public DbSet<QuestionnaireSection> QuestionnaireSections { get; set; }

        /// <summary>
        /// Gets or sets all the Instructions for a given ProSection
        /// </summary>
        public DbSet<QuestionnaireSectionInstruction> QuestionnaireSectionInstructions { get; set; }

        /// <summary>
        /// Gets or sets the Instructions for an Item
        /// </summary>
        public DbSet<QuestionnaireItemInstruction> QuestionnaireItemInstructions { get; set; }

        /// <summary>
        /// Gets or sets the ProText Elements
        /// </summary>
        public DbSet<QuestionnaireElement> QuestionnaireElements { get; set; }

        /// <summary>
        /// Gets or sets the text instances for the element
        /// </summary>
        public DbSet<QuestionnaireElementTextVersion> QuestionnaireElementTextVersions { get; set; }

        /// <summary>
        /// Gets or sets the text instances for the element
        /// </summary>
        public DbSet<QuestionnaireItemOptionGroupTextVersion> QuestionnaireItemOptionGroupTextVersions { get; set; }

        /// <summary>
        /// Gets or sets all the ProItemOptionGroups
        /// </summary>
        public DbSet<QuestionnaireItemOptionGroup> QuestionnaireItemOptionGroups { get; set; }

        /// <summary>
        /// Gets or sets all the ProItemOptions
        /// </summary>
        public DbSet<QuestionnaireItemOption> QuestionnaireItemOptions { get; set; }

        /// <summary>
        /// Gets or sets all the Questionnaire Introduction Messages
        /// </summary>
        public DbSet<QuestionnaireIntroductionMessage> QuestionnaireIntroductionMessages { get; set; }

        /// <summary>
        /// Gets or sets the descriptions for the questionnaire
        /// </summary>
        public DbSet<QuestionnaireDescription> QuestionnaireDescriptions { get; set; }
        #endregion

        #region Questionnaire Format definitions
        /// <summary>
        /// Gets or sets the Definition of containers
        /// </summary>
        public DbSet<ContainerFormatDefinition> ContainerFormatDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the Format definition of Elements.
        /// Holds both the <see cref="ItemFormatDefinition"/> and <see cref="TextFormatDefinition"/>
        /// </summary>
        public DbSet<ElementFormatDefinition> ElementFormatDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the definitions for format of the Item Group Options
        /// </summary>
        public DbSet<ItemGroupOptionsFormatDefinition> ItemGroupOptionsFormatDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the text definition for each forEach section found in <see cref="ItemGroupOptionsFormatDefinition"/>
        /// </summary>
        public DbSet<ItemGroupOptionsForEachOptionDefinition> ItemGroupOptionsForEachOptionDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the Questionnaire Formats
        /// </summary>
        public DbSet<Format> QuestionnaireFormats { get; set; }

        /// <summary>
        /// Gets or sets the Questionnaire Format Containers
        /// </summary>
        public DbSet<FormatContainer> QuestionnaireFormatContainers { get; set; }

        /// <summary>
        /// Gets or sets the items in the Questionnaire Format Container
        /// </summary>
        public DbSet<FormatContainerElement> QuestionnaireFormatContainerElements { get; set; }

        /// <summary>
        /// Gets or sets the formats that are defined for each Item and ItemGroup (and associated options) for a set of given elements
        /// </summary>
        public DbSet<ItemFormatContainer> QuestionnaireGroupFormats { get; set; }

        /// <summary>
        /// Gets or sets the a Group format for a specific <see cref="QuestionnaireResponseType"/>
        /// </summary>
        public DbSet<ItemGroupFormat> ItemGroupFormats { get; set; }

        /// <summary>
        /// Gets or sets the list of Tags
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the list of tags for a Questionnaire
        /// </summary>
        public DbSet<QuestionnaireTag> QuestionnaireTags { get; set; }

        /// <summary>
        /// Gets or sets the list of QuestionnaireUserResponseGroupTag
        /// </summary>
        public DbSet<QuestionnaireUserResponseGroupTag> QuestionnaireUserResponseGroupTags { get; set; }

        /// <summary>
        /// Gets or sets the definitions to extract data from a Questionnaire
        /// </summary>
        public DbSet<QuestionnaireDataExtraction> QuestionnaireDataExtractions { get; set; }

        /// <summary>
        /// Gets or sets the list of ProDomainResultSets
        /// </summary>
        public DbSet<ProDomainResultSet> ProDomainResultSet { get; set; }

        /// <summary>
        /// Gets or sets the list of ProDomainResults
        /// </summary>
        public DbSet<ProDomainResult> ProDomainResult { get; set; }
        #endregion

        #region Users
        /// <summary>
        /// Gets or sets all the Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the IdentityRoles available
        /// </summary>
        public DbSet<IdentityRole> IdentityRoles { get; set; }

        /// <summary>
        /// Gets or sets the IdentityRolesFunctionality 
        /// </summary>
        public DbSet<IdentityRolePermission> IdentityRolePermission { get; set; }

        /// <summary>
        /// Gets or sets the mapping between roles and users
        /// </summary>
        public DbSet<IdentityUserRole> IdentityUserRoles { get; set; }

        /// <summary>
        /// Gets or sets the List of User Entities
        /// </summary>
        public DbSet<ProxyUserPatientMap> ProxyUserPatientMapping { get; set; }

        /// <summary>
        /// Gets or sets the list of patients
        /// </summary>
        public DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// Gets or sets the details for a session
        /// </summary>
        public DbSet<SessionDetails> SessionStore { get; set; }

        /// <summary>
        /// Gets or sets the list of security Codes
        /// </summary>
        public DbSet<UserSecurityCode> UserSecurityCodes { get; set; }

        /// <summary>
        /// Gets or sets the list of tags for a Patient
        /// </summary>
        public DbSet<PatientTag> PatientTags { get; set; }
        #endregion

        #region Messages
        /// <summary>
        /// Gets or sets the list of errors
        /// </summary>
        public DbSet<PCHIError> ErrorsMessages { get; set; }

        /// <summary>
        /// Gets or sets the text definitions stored
        /// </summary>
        public DbSet<TextDefinition> TextDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the Text replacement codes used
        /// </summary>
        public DbSet<TextReplacementCode> TextReplacementCodes { get; set; }

        /// <summary>
        /// Gets or sets the Page Text data
        /// </summary>
        public DbSet<PageText> PageTexts { get; set; }
        #endregion

        #region Notifications
        /// <summary>
        /// Gets or sets the notifications assigned
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }
        #endregion

        #region Episodes
        /// <summary>
        /// Gets or sets the list of Episodes
        /// </summary>
        public DbSet<Episode> Episodes { get; set; }

        /// <summary>
        /// Gets or sets the diagnosis codes
        /// </summary>
        public DbSet<DiagnosisCode> DiagnosisCodes { get; set; }

        /// <summary>
        /// Gets or sets the treatment codes
        /// </summary>
        public DbSet<TreatmentCode> TreatmentCodes { get; set; }

        /// <summary>
        /// Gets or sets Episode History
        /// </summary>
        public DbSet<EpisodeHistory> EpisodeHistory { get; set; }

        /// <summary>
        /// Gets or sets the assigned questionnaires
        /// </summary>
        public DbSet<AssignedQuestionnaire> AssignedQuestionnaires { get; set; }

        /// <summary>
        /// Gets or sets the resolved patient schedules
        /// </summary>
        public DbSet<ScheduledQuestionnaireDate> ScheduledQuestionnaireDates { get; set; }

        /// <summary>
        /// Gets or sets the list of available milestones
        /// </summary>
        public DbSet<Milestone> Milestones { get; set; }

        /// <summary>
        /// Gets or sets the Episodes milestones
        /// </summary>
        public DbSet<EpisodeMilestone> EpisodeMilestones { get; set; }

        /// <summary>
        /// Gets or sets all the QuestionnaireUserResponseGroups
        /// </summary>
        public DbSet<QuestionnaireUserResponseGroup> QuestionnaireUserResponseGroups { get; set; }

        /// <summary>
        /// Gets or sets all the Responses to a questionnaire
        /// </summary>
        public DbSet<QuestionnaireResponse> QuestionnaireResponses { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the audit logs
        /// </summary>
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        ///  This method is called when the model for a derived context has been initialized,
        ///  but before the model has been locked down and used to initialize the context.
        ///  The default implementation of this method does nothing, but it can be overridden
        ///  in a derived class such that the model can be further configured before it
        ///  is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
            #region Questionnaire Mapping
            modelBuilder.Entity<ProInstrument>().HasMany(p => p.Domains);
            modelBuilder.Entity<Questionnaire>().HasMany(p => p.Sections);
            modelBuilder.Entity<ProDomain>().HasMany(pd => pd.ResultRanges);
            modelBuilder.Entity<QuestionnaireSection>().HasMany(ps => ps.Elements);
            modelBuilder.Entity<QuestionnaireSection>().HasMany(ps => ps.Instructions);
            modelBuilder.Entity<QuestionnaireElement>().HasMany(qe => qe.TextVersions);
            modelBuilder.Entity<QuestionnaireItem>().HasMany(pi => pi.OptionGroups);
            modelBuilder.Entity<QuestionnaireItem>().HasMany(pi => pi.Instructions);
            modelBuilder.Entity<QuestionnaireItemOptionGroup>().HasMany(piog => piog.Options);
            modelBuilder.Entity<QuestionnaireResponse>().HasOptional(r => r.Option);
            modelBuilder.Entity<QuestionnaireResponse>().HasOptional(r => r.Item);
            modelBuilder.Entity<QuestionnaireUserResponseGroup>().HasMany(r => r.Responses);

            modelBuilder.Entity<ProDomainResultSet>().HasMany(pdrs => pdrs.Results).WithRequired(r => r.ProDomainResultSet).WillCascadeOnDelete(true);
            #endregion

            #region Format Mapping
            modelBuilder.Entity<ContainerFormatDefinition>().HasMany(dsfd => dsfd.ChildContainers);
            modelBuilder.Entity<ContainerFormatDefinition>().HasMany(dsfd => dsfd.ElementFormatDefinitions);
            modelBuilder.Entity<ItemFormatDefinition>().HasMany(ifd => ifd.ItemGroupOptionsFormatDefinitions);
            modelBuilder.Entity<ItemGroupOptionsFormatDefinition>().HasMany(igofd => igofd.ForEachOption);

            // Sql Server doesn't allow cascade delete on self referencing table, so we have to explicitely disable them.
            modelBuilder.Entity<Format>().HasMany(qf => qf.Containers).WithOptional(c => c.QuestionnaireFormat).WillCascadeOnDelete(false);
            modelBuilder.Entity<FormatContainer>().HasMany(fc => fc.Children).WithOptional(t => t.Parent).WillCascadeOnDelete(false);
            modelBuilder.Entity<FormatContainer>().HasMany(qfs => qfs.Elements).WithRequired(f => f.FormatContainer).WillCascadeOnDelete(true);

            modelBuilder.Entity<ItemFormatContainer>().HasMany(qfs => qfs.ItemGroupFormats).WithRequired(ifc => ifc.ItemFormatContainer).WillCascadeOnDelete(true);
            #endregion

            #region User Mapping
            modelBuilder.Entity<User>().Property(p => p.UserName).IsRequired().HasMaxLength(450).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_User_UserName") { IsUnique = true }));
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            #endregion

            modelBuilder.Entity<ScheduledQuestionnaireDate>().HasOptional(s => s.ResponseGroup).WithOptionalPrincipal(r => r.ScheduledQuestionnaireDate);
        }

        /// <summary>
        /// Checks if the given object is already in the local context.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="entity">The entity to look for</param>
        /// <returns>True if the object already is in the context, false otherwise</returns>
        internal bool Exists<T>(T entity) where T : class
        {
            return this.Set<T>().Local.Any(e => e == entity);
        }

        /// <summary>
        /// Detaches the given Entity
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="entity">The Entity to detach</param>
        internal void Detach<T>(T entity) where T : class
        {
            if (this.Exists(entity))
            {
                this.Entry(entity).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            var databaseEntries = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified).ToDictionary(e => e, e => e.State);

            // Call the original SaveChanges()
            int result = 0;
            Exception exception = null;
            try
            {
                result = base.SaveChanges();
            }
            catch (AggregateException ex)
            {
                exception = new Exception(ex.InnerExceptions.Select(e => e.Message).Aggregate((s1, s2) => { return s1 + "\n" + s2; }));
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            /*
            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            string errorMessage = exception != null ? exception.Message : null;
            int count = 0;
            List<AuditLog> logs = new List<AuditLog>();
            foreach (var ent in databaseEntries)
            {
                count++;
                logs.AddRange(this.GetAuditRecordsForChange(ent.Key, ent.Value, errorMessage));
            }

            if(logs.Count > 0)
            {
                if (this.CreatingAuditLogs != null)
                {
                    this.CreatingAuditLogs(logs);
                }

                this.AuditLogs.AddRange(logs);
            }

            // Save the audit log entries
            base.SaveChanges();
             * */
            if (exception != null) throw exception;
            return result;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">Any cancellation token</param>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult<int>(this.SaveChanges());
        }

        /*
        /// <summary>
        /// Called whenever audit logs are created so they can be updated with any relevant data
        /// </summary>
        public event CreatingAuditLogsEventHandler CreatingAuditLogs;

        /// <summary>
        /// Creates the audit records for the changes to the given DbEntityEntry
        /// </summary>
        /// <param name="dbEntry">The DbEntityEntry that has changed</param>
        /// <param name="userId">Any error message to record</param>
        /// <returns>The list of audit logs created</returns>
        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, EntityState state, string errorMessage)
        {
            List<AuditLog> result = new List<AuditLog>();

            // TODO test if this works
            if (dbEntry.Entity.GetType() == typeof(AuditLog)) return result;

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            //TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0 || p.Name == "Id").ToList();

            string keyName = keyNames[0].Name; //dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

            var properties = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotMappedAttribute), false).Count() == 0);

            if (state == EntityState.Added || state == EntityState.Modified)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()

                foreach (PropertyInfo pinfo in properties)
                {
                    object value = pinfo.GetValue(dbEntry.Entity);
                    string columnName = pinfo.Name;
                    string valueToStore = null;

                    if (value != null && (pinfo.PropertyType.IsPrimitive || value.GetType() == typeof(string)))
                    {
                        valueToStore = value.ToString();
                    }
                    else if (value != null && pinfo.PropertyType.GenericTypeArguments.Count() > 0 && pinfo.PropertyType.GenericTypeArguments[0].IsPrimitive)
                    {
                        valueToStore = value.ToString();
                    }
                    else if (value != null && !(value is System.Collections.IEnumerable))
                    {
                        var valueKeys = value.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0 || p.Name == "Id").ToList();
                        if (valueKeys.Count > 0)
                        {
                            columnName = pinfo.Name + "_" + valueKeys[0].Name;
                            if (value != null)
                            {
                                valueToStore = value.GetType().GetProperty(valueKeys[0].Name).GetValue(value).ToString();
                            }
                        }
                        else if (value != null)
                        {
                            valueToStore = value.ToString();
                        }
                    }

                    string recordId = null;
                    if (keyNames.Count > 1)
                    {
                        foreach (PropertyInfo kn in keyNames)
                        {
                            recordId += " , " + dbEntry.CurrentValues.GetValue<object>(kn.Name).ToString();
                        }
                    }
                    else
                    {
                        recordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString();
                    }

                    result.Add(new AuditLog()
                        {
                            Id = Guid.NewGuid(),
                            UserId = null,
                            EventDateUTC = changeTime,
                            EventType = state.ToString(),    // Added
                            ObjectName = tableName,
                            RecordId = recordId,
                            ColumnName = columnName,
                            Value = valueToStore, //pinfo.GetValue(dbEntry.Entity).ToString()// dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            Message = errorMessage
                        });
                }
            }
            else if (state == EntityState.Deleted)
            {
                result.Add(new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        UserId = null,
                        EventDateUTC = changeTime,
                        EventType = state.ToString(), // Deleted
                        ObjectName = tableName,
                        RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                        //ColumnName = propertyName,
                        //Value = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString()
                        Message = errorMessage
                    });
            }

            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }
        */
    }
}
