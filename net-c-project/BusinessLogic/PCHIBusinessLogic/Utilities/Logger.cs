using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic.Security;
using PCHI.BusinessLogic.Utilities.Model;
using PCHI.DataAccessLibrary;
using PCHI.DataAccessLibrary.AccessHandelers;
using PCHI.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// The default logger class to handle all logging
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The handler for the Audit entries
        /// </summary>
        private static AuditHandler handler;

        /// <summary>
        /// Logs the given Audit to the audit trail
        /// </summary>
        /// <param name="audits">The audit entries to log</param>
        public static void Audit(params Audit[] audits)
        {
            List<AuditLog> logs = new List<AuditLog>();
            foreach (Audit audit in audits)
            {
                logs.Add(Logger.BuildAuditLog(audit));
            }

            if (Logger.handler == null) handler = new AccessHandlerManager().AuditHandler;

            Logger.handler.StoreAudit(logs);
        }

        /// <summary>
        /// Builds the Audit Logs from the given Audit
        /// </summary>
        /// <param name="audit">The audit to build the log from</param>
        /// <returns>The audit log</returns>
        private static AuditLog BuildAuditLog(Audit audit)
        {
            AuditLog log = new AuditLog();
            log.Id = Guid.NewGuid();
            log.EventDateUTC = DateTime.UtcNow;
            log.EventType = audit.AuditEventType;
            log.EventTypeName = audit.AuditEventType.ToString();
            log.Success = audit.Success;
            log.FieldName = audit.FieldName;

            if (WcfUserSessionSecurity.Current.RequestHeader != null)
            {
                var header = WcfUserSessionSecurity.Current.RequestHeader;
                log.UserId = WcfUserSessionSecurity.Current.User != null ? WcfUserSessionSecurity.Current.User.Id : "<unknown>";
                log.UserIp = header.UserIp;
                log.ClientIps = header.ClientIp;
                log.ClientName = header.ClientName;
            }
            else
            {
            }

            log.Action = audit.Action;
            log.ActionName = audit.Action.ToString();
            log.ObjectType = audit.ObjectType;
            log.RecordId = audit.ObjectId;
            log.Message = audit.Message;

            return log;
        }
    }
}
