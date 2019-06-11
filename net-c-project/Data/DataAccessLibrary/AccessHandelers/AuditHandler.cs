using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Episodes;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handlers access related to permissioning
    /// </summary>
    public class AuditHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal AuditHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Stores the given audit logs to the database
        /// </summary>
        /// <param name="logs">The logs to store</param>
        public void StoreAudit(IEnumerable<AuditLog> logs)
        {
            this.context.AuditLogs.AddRange(logs);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets a list of all Audit Trail entries for a specific User
        /// </summary>
        /// <param name="userId">The Id of the user to get the Audit Trail entries for</param>
        /// <returns>The audit trail</returns>
        public List<AuditTrailEntry> GetAudit(string userId)
        {
            var query = (from u in this.context.Users
                         join a in this.context.AuditLogs on u.Id equals
                         (a.UserId == "<unknown>" && a.ObjectType == typeof(User).Name && a.FieldName == "Id" ? a.RecordId
                         : a.UserId == "<unknown>" && a.ObjectType == typeof(User).Name && a.FieldName == "UserName" ? (from u2 in this.context.Users where u2.UserName == a.RecordId select u2.Id).FirstOrDefault()
                         : a.UserId)

                         join targetU in this.context.Users on
                         (a.ObjectType == typeof(User).Name && a.FieldName == "Id" ? a.RecordId
                         : a.ObjectType == typeof(User).Name && a.FieldName == "UserName" ? (from u2 in this.context.Users where u2.UserName == a.RecordId select u2.Id).FirstOrDefault()
                         : null) equals targetU.Id into targetUser
                         from targetU2 in targetUser.DefaultIfEmpty()

                         join p in this.context.Patients on
                         (a.ObjectType == typeof(Patient).Name ? a.RecordId :
                         a.ObjectType == typeof(Episode).Name ? (from e in this.context.Episodes where e.Id.ToString() == a.RecordId select e.Patient.Id).FirstOrDefault() :
                         a.ObjectType == typeof(QuestionnaireUserResponseGroup).Name ? (from q in this.context.QuestionnaireUserResponseGroups where q.Id.ToString() == a.RecordId select q.Patient.Id).FirstOrDefault() :
                         null) equals p.Id into Pat
                         from p2 in Pat.DefaultIfEmpty()

                         join q2 in this.context.Questionnaires on
                         (a.ObjectType == typeof(QuestionnaireUserResponseGroup).Name ? (from q in this.context.QuestionnaireUserResponseGroups where q.Id.ToString() == a.RecordId select q.Questionnaire.Id).FirstOrDefault() :
                         0) equals q2.Id into questionnaire
                         from q3 in questionnaire.DefaultIfEmpty()

                         join ep in this.context.Episodes on
                         (a.ObjectType == typeof(Episode).Name ? a.RecordId :
                         null) equals ep.Id.ToString() into episode
                         from e2 in episode.DefaultIfEmpty()

                         where u.Id == userId || a.RecordId == userId
                         select new AuditTrailEntry() { User = u, TargetUser = targetU2, AuditLog = a, Patient = p2, Questionnaire = q3, Episode = e2 }).OrderByDescending(a => a.AuditLog.EventDateUTC);

            return query.ToList();

            /* LINQPad query
from u in Users
join a in AuditLogs on u.Id equals
(a.UserId == "<unknown>" && a.ObjectType == typeof(User).Name && a.FieldName == "Id" ? a.RecordId
: a.UserId == "<unknown>" && a.ObjectType == typeof(User).Name && a.FieldName == "UserName" ? (from u2 in Users where u2.UserName == a.RecordId select u2.Id).FirstOrDefault()
: a.UserId)

join targetU in Users on 
(  a.ObjectType == typeof(User).Name && a.FieldName == "Id" ? a.RecordId
: a.ObjectType == typeof(User).Name && a.FieldName == "UserName" ? (from u2 in Users where u2.UserName == a.RecordId select u2.Id).FirstOrDefault()
: null
) equals targetU.Id into targetUser
from targetU2 in targetUser.DefaultIfEmpty()

join p in Patients on
(a.ObjectType == typeof(Patient).Name ? a.RecordId :
a.ObjectType == typeof(Episode).Name ? (from e in Episodes where e.Id.ToString() == a.RecordId select e.Patient.Id).FirstOrDefault() :
a.ObjectType == typeof(QuestionnaireUserResponseGroup).Name ? (from q in QuestionnaireUserResponseGroups where q.Id.ToString() == a.RecordId select q.Patient.Id).FirstOrDefault() :
null) equals p.Id into Pat
from p2 in Pat.DefaultIfEmpty()

join q2 in Questionnaires on 
(a.ObjectType == typeof(QuestionnaireUserResponseGroup).Name ? (from q in QuestionnaireUserResponseGroups where q.Id.ToString() == a.RecordId select q.Questionnaire.Id).FirstOrDefault() :
0) equals q2.Id into questionnaire
from q3 in questionnaire.DefaultIfEmpty()

join ep in Episodes on 
( a.ObjectType == typeof(Episode).Name ? a.RecordId :
null) equals ep.Id.ToString() into episode
from e2 in episode.DefaultIfEmpty()
     select new {User = u, TargetUser = targetU2, AuditLog = a, Patient = p2, Questionnaire = q3, Episode = e2}
             */
        }
    }
}
