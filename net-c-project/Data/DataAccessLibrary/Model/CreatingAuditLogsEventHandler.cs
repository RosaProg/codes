using PCHI.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.Model
{
    /// <summary>
    /// Called whenever audit logs are created so they can be updated with any relevant data
    /// </summary>
    /// <param name="AuditLogs">The audit logs that have been created and are ready for storing. Can be updated with additional data at this point</param>
    public delegate void CreatingAuditLogsEventHandler(List<AuditLog> AuditLogs);
}
