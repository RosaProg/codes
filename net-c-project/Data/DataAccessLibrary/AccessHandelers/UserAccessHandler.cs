using LinqKit;
using Microsoft.AspNet.Identity.EntityFramework;
using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// The implementation for the UserStore
    /// </summary>
    public class UserAccessHandler : UserStore<User>
    {
        /// <summary>
        /// The context manager to use to get data from the database
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> to use</param>
        internal UserAccessHandler(MainDatabaseContext context)
            : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Finds and returns the User based upon the email
        /// </summary>
        /// <param name="userEmail">The email of the user</param>
        /// <returns>The found User or null if not found</returns>
        public User GetUserByEmail(string userEmail)
        {
            return this.context.Users.Where(u => u.Email.ToLower() == userEmail).Include(u => u.ProxyUserPatientMap.Select(p => p.Patient)).SingleOrDefault();
        }

        /// <summary>
        /// Gets the user by Username
        /// </summary>
        /// <param name="userName">The name of the user</param>
        /// <returns>The user if found, or null if not found</returns>
        public User GetUserByUsername(string userName)
        {
            var user = this.context.Users.Where(u => u.UserName == userName).Include(u => u.ProxyUserPatientMap.Select(p => p.Patient)).SingleOrDefault();
            if (user != null) user.RoleNames = this.GetRolesAsync(user).Result;
            return user;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> FindUsers()
        {
            var users = this.context.Users.Include(u => u.ProxyUserPatientMap.Select(p => p.Patient)).ToList();
            foreach (User user in users)
            {
                var query = this.context.IdentityRoles.Where(r => this.context.IdentityUserRoles.Where(u => u.UserId == user.Id).Select(r2 => r2.RoleId).Contains(r.Id));
                user.RoleNames = query.Select(r => r.Name).ToList();
            }

            return users;
        }

        /// <summary>
        /// Find and returns the list of functionality assigned to a user via it's roles
        /// </summary>
        /// <param name="userId">The Id of the user to get the list for</param>
        /// <returns>The list found</returns>
        public List<Permission> GetPermissionsForUser(string userId)
        {
            IQueryable<Permission> query = this.context.IdentityRolePermission.Where(f => this.context.IdentityUserRoles.Where(u => u.UserId == userId).Select(u => u.RoleId).Contains(f.Role.Id)).Select(f => f.Permission);

            return query.ToList();
        }

        #region UserEntities
        /// <summary>
        /// Gets the Patient with the given External ID
        /// </summary>
        /// <param name="externalId">the External ID of the Entityt</param>
        /// <returns>The Patient found or null if not found</returns>
        public Patient GetPatientByExternalId(string externalId)
        {
            return this.context.Patients.Where(u => u.ExternalId == externalId).FirstOrDefault();
        }

        /// <summary>
        /// Updates the given Patients data
        /// </summary>
        /// <param name="p">The Patient to update</param>
        public void Update(Patient p)
        {
            this.context.Entry(p).State = EntityState.Modified;
            if (p.ProxyUserPatientMap.Count > 0)
            {
                List<ProxyUserPatientMap> existing = this.context.ProxyUserPatientMapping.Where(map => map.Patient.Id == p.Id).Include(map => map.User).ToList();
                existing.Where(e => !p.ProxyUserPatientMap.Select(map => map.User.Id).Contains(e.User.Id)).ToList().ForEach(m => this.context.Entry(m).State = EntityState.Deleted);

                p.ProxyUserPatientMap.Where(map => !existing.Select(e => e.User.Id).Contains(map.User.Id)).ToList().ForEach(m => this.context.Entry(m).State = EntityState.Added);

                foreach (ProxyUserPatientMap map in p.ProxyUserPatientMap)
                {
                    this.context.Entry(map.User).State = EntityState.Unchanged;
                }
            }

            this.context.SaveChanges();
        }

        /// <summary>
        /// Adds the given Patient to the database
        /// </summary>
        /// <param name="p">The Patient to add</param>
        public void Add(Patient p)
        {
            this.context.Patients.Add(p);
            foreach (ProxyUserPatientMap map in p.ProxyUserPatientMap)
            {
                this.context.ProxyUserPatientMapping.Add(map);
                this.context.Entry(map.User).State = EntityState.Unchanged;
            }

            this.context.SaveChanges();
        }

        /// <summary>
        /// Finds a list of users based upon any of the data provided.
        /// All parameters are optional
        /// </summary>
        /// <param name="id">The Id of the based</param>
        /// <param name="firstName">The first name of the user</param>
        /// <param name="lastName">The last name of the user</param>
        /// <param name="dob">The date of birth</param>
        /// <param name="email">The email</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="externalId">The external ID of the user</param>
        /// <returns>The list of users found</returns>
        public List<Patient> FindPatient(string id = null, string firstName = null, string lastName = null, System.DateTime? dob = null, string email = null, string phoneNumber = null, string externalId = null)
        {
            var query = this.context.Patients.OfType<Patient>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(u => u.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(u => u.FirstName.StartsWith(firstName));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(u => u.LastName.StartsWith(lastName));
            }

            if (dob.HasValue)
            {
                query = query.Where(u => u.DateOfBirth == dob);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.StartsWith(email));
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                query = query.Where(u => u.PhoneNumber.StartsWith(phoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(externalId))
            {
                query = query.Where(u => u.ExternalId.StartsWith(externalId));
            }

            query = query.Include(e => e.ProxyUserPatientMap.Select(p => p.User));
            return query.ToList();
        }

        /// <summary>
        /// Finds the Entity with the given ID
        /// </summary>
        /// <param name="patientId">The Id of the PCHIUserEntity to find </param>
        /// <returns>The PCHIUserEntity found or null</returns>
        public Patient FindPatient(string patientId)
        {
            return this.context.Patients.Where(e => e.Id == patientId).Include(e => e.ProxyUserPatientMap.Select(p => p.User)).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// Gets a list of outstanding questionnaires for the current user not assigned to a Episode
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the outstanding questionnaires for</param>
        /// <param name="notCompletedOnly">If true only outstanding questionnaires are returend, if false all questionnaires</param>
        /// <returns>A list of Questionnaires outstanding for the current user</returns>
        public List<QuestionnaireUserResponseGroup> GetOutstandingQuestionnairesForPatient(string patientId, bool notCompletedOnly = true)
        {
            var q = this.context.QuestionnaireUserResponseGroups.Where(g => g.Patient.Id == patientId && g.ScheduledQuestionnaireDate == null).Include(g => g.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).Include(g => g.Questionnaire).Include(g => g.Responses);
            if (notCompletedOnly) q = q.Where(g => !g.Completed);
            q = q.OrderBy(g => g.Completed).OrderByDescending(g => g.DateTimeCompleted);
            return q.ToList();
        }

        /// <summary>
        /// Gets a list of outstanding questionnaires for the given patient
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the outstanding questionnaires for</param>
        /// <param name="notCompletedOnly">If true only outstanding questionnaires are returend, if false all questionnaires</param>
        /// <returns>A list of Questionnaires outstanding for the current user</returns>
        public Dictionary<Episode, List<QuestionnaireUserResponseGroup>> GetQuestionnairesInEpisodeForPatient(string patientId, bool notCompletedOnly = true)
        {
            var q = this.context.QuestionnaireUserResponseGroups.Where(g => g.Patient.Id == patientId && g.ScheduledQuestionnaireDate != null).Include(g => g.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).Include(g => g.Questionnaire.IntroductionMessages).Include(g => g.Responses);
            if (notCompletedOnly) q = q.Where(g => !g.Completed);
            q = q.OrderBy(g => g.Completed).OrderByDescending(g => g.DateTimeCompleted);
            return q.ToList().GroupBy(g => g.ScheduledQuestionnaireDate.AssignedQuestionnaire.Episode).ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Gets a list of Patients for the given user ID
        /// </summary>
        /// <param name="userId">The Id of the user to get the Entities for</param>
        /// <param name="username">Thge username of the user</param>
        /// <returns>The list of user entities found</returns>
        public List<Patient> GetPatients(string userId = null, string username = null)
        {
            if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(username)) return new List<Patient>();
            var query = this.context.ProxyUserPatientMapping.Where(u => u.Id != null);
            if (!string.IsNullOrWhiteSpace(userId)) query = query.Where(e => e.User.Id == userId);
            if (!string.IsNullOrWhiteSpace(username)) query = query.Where(e => e.User.UserName == username);
            query = query.Include(p => p.Patient);
            return query.Select(p => p.Patient).ToList();
        }

        /// <summary>
        /// Gets a list of functionalities that belong to a given Role
        /// </summary>
        /// <param name="roleName">The name of the role to get the functionalitites for</param>
        /// <returns>A list of functionalitites for a given Role</returns>
        public List<Permission> GetPermissionsForRole(string roleName)
        {
            IQueryable<Permission> query = this.context.IdentityRolePermission.Where(f => f.Role.Name == roleName).Select(f => f.Permission);
            return query.ToList();
        }

        /// <summary>
        /// Stores the security code for the user. IF a code of the existing type already exists, it is overwritten instead
        /// </summary>
        /// <param name="code">The code to store</param>
        public void StoreSecurityCode(UserSecurityCode code)
        {
            UserSecurityCode existing = this.context.UserSecurityCodes.Where(c => code.User.Id == c.User.Id && code.SecurityCodePurpose == c.SecurityCodePurpose).Include(c => c.User).FirstOrDefault();
            if (existing != null)
            {
                existing.Code = code.Code;
            }
            else
            {
                code.User = this.context.Users.Where(u => u.Id == code.User.Id).First();
                this.context.UserSecurityCodes.Add(code);
                this.context.Entry(code.User).State = EntityState.Unchanged;
            }

            this.context.SaveChanges();
        }

        /// <summary>
        /// Retrieves a security code for the given User of the given type
        /// </summary>
        /// <param name="userId">The Id of the user to get the security code for</param>
        /// <param name="purpose">The  purpose of security code to retrieve</param>
        /// <returns>The security code found or null</returns>
        public UserSecurityCode GetSecurityCode(string userId, string purpose)
        {
            return this.context.UserSecurityCodes.Where(c => c.User.Id == userId && c.SecurityCodePurpose == purpose).Include(c => c.User).FirstOrDefault();
        }

        /// <summary>
        /// Delets the security code for the given User of the given type
        /// </summary>
        /// <param name="userId">The Id of the user to delete the security code for</param>
        /// <param name="purpose">The purpose of security code to delete</param>        
        public void DeleteSecurityCode(string userId, string purpose)
        {
            var value = this.context.UserSecurityCodes.Where(c => c.User.Id == userId && c.SecurityCodePurpose == purpose).FirstOrDefault();
            if (value != null)
            {
                this.context.UserSecurityCodes.Remove(value);
                this.context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns a list of available roles
        /// </summary>
        /// <returns>The list of available roles</returns>
        public List<string> GetAvailableRoles()
        {
            return this.context.IdentityRoles.Select(r => r.Name).ToList();
        }

        /// <summary>
        /// Finds and returns all users that belong to a certain role
        /// </summary>
        /// <param name="role">The role to look for</param>
        /// <returns>List of matching users</returns>  
        public List<User> GetRoleMembers(string role)
        {
            return this.context.Users.Where(u => this.context.IdentityUserRoles.Where(iur => iur.RoleId == this.context.IdentityRoles.Where(r => r.Name == role).Select(r => r.Id).FirstOrDefault()).Select(iur => iur.UserId).Contains(u.Id)).ToList();
        }

        /// <summary>
        /// Finds all Episode Milestones for the given list of patients.
        /// Includes the Episode and Patient data
        /// </summary>
        /// <param name="patients">The patients to find the milestones for</param>
        /// <param name="practitionerId">The optional practitioner ID of the milestones to include</param>
        /// <returns>The List of milestones(value) group by patient ID (key)</returns>
        public Dictionary<string, List<EpisodeMilestone>> FindMileStones(List<Patient> patients, string practitionerId = null)
        {
            var pr = PredicateBuilder.False<EpisodeMilestone>();
            foreach (Patient p in patients)
            {
                pr = pr.Or(t => t.Episode.Patient.Id == p.Id);
            }

            if (!string.IsNullOrWhiteSpace(practitionerId)) pr = pr.And(t => t.PractitionerId == practitionerId);

            var q = this.context.EpisodeMilestones.AsExpandable().Where(pr).Include(e => e.Episode.Patient).GroupBy(t => t.Episode.Patient.Id);
            return q.ToDictionary(t => t.Key, t => t.ToList());
        }

        /// <summary>
        /// Adds or updates the given list of PatientTags in the database
        /// </summary>
        /// <param name="patientTags">The list of Patient Tags to add or update</param>
        public void AddOrUpdatePatientTags(IEnumerable<PatientTag> patientTags)
        {
            patientTags.ForEach(t => { t.PatientId = t.Patient.Id; t.Patient = null; });
            this.context.PatientTags.AddOrUpdate(patientTags.ToArray());
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets a list of Patient Tags from the database
        /// </summary>
        /// <param name="patientId">The Id of the patient to get the tags for</param>
        /// <returns>The list of tags</returns>
        public List<PatientTag> GetPatientTags(string patientId)
        {
            return this.context.PatientTags.Where(p => p.Patient.Id == patientId).Include(t => t.Patient).ToList();
        }

        /// <summary>
        /// Gets a list of patient tag names
        /// </summary>
        /// <returns>A list of unique patient tag names</returns>
        public List<string> GetPatientTagNames()
        {
            return this.context.PatientTags.Select(p => p.TagName).Distinct().ToList();
        }
    }
}
