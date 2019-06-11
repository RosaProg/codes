using DSPrima.WcfUserSession.SecurityHandlers;
using Microsoft.AspNet.Identity;
using PCHI.BusinessLogic.IIdentityMessageServices;
using PCHI.BusinessLogic.Managers;
using PCHI.BusinessLogic.Properties;
using PCHI.BusinessLogic.Security;
using PCHI.DataAccessLibrary;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PCHI.BusinessLogic
{
    /// <summary>
    /// Handles the instantiating of all the Manager to use and allow them to share the same Access layer instances
    /// </summary>
    public class ManagerHandler
    {
        /// <summary>
        /// Holds the private instance of the <see cref="QuestionnaireManager"/> class
        /// </summary>
        private QuestionnaireManager questionnaireManager;

        /// <summary>
        /// Gets the instance of the <see cref="QuestionnaireManager"/>
        /// </summary>
        public QuestionnaireManager QuestionnaireManager { get { return this.questionnaireManager; } }

        /// <summary>
        /// Holds the private instance of the <see cref="QuestionnaireFormatManager"/> class
        /// </summary>
        private QuestionnaireFormatManager questionnaireFormatManager;

        /// <summary>
        /// Gets the instance of the <see cref="QuestionnaireFormatManager"/>
        /// </summary>
        public QuestionnaireFormatManager QuestionnaireFormatManager { get { return this.questionnaireFormatManager; } }

        /// <summary>
        /// Holds the private instance of the <see cref="UserManager"/> class
        /// </summary>
        private UserManager userManager;

        /// <summary>
        /// Gets the instance of the <see cref="UserManager"/>
        /// </summary>
        public UserManager UserManager { get { return this.userManager; } }

        /// <summary>
        /// Holds the private instance of the <see cref="UserEpisodeManager"/> class
        /// </summary>
        private UserEpisodeManager userEpisodeManager;

        /// <summary>
        /// Gets the instance of the <see cref="UserEpisodeManager"/>
        /// </summary>
        public UserEpisodeManager UserEpisodeManager { get { return this.userEpisodeManager; } }

        /// <summary>
        /// Holds the private instance of the <see cref="MessageManager"/> class
        /// </summary>
        private MessageManager messageManager;

        /// <summary>
        /// Gets the instance of the <see cref="MessageManager"/>
        /// </summary>
        public MessageManager MessageManager { get { return this.messageManager; } }

        /// <summary>
        /// Holds the private instance of the <see cref="SearchManager"/> class
        /// </summary>
        private SearchManager searchManager;

        /// <summary>
        /// Gets the instance of the <see cref="SearchManager"/>
        /// </summary>
        public SearchManager SearchManager { get { return this.searchManager; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerHandler"/> class
        /// </summary>
        public ManagerHandler()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerHandler"/> class
        /// </summary>
        /// <param name="ahm">The AccessHandlerManager to use</param>
        public ManagerHandler(AccessHandlerManager ahm)
        {
            if (ahm == null) ahm = new AccessHandlerManager(this.CreatingAuditLogs);

            this.questionnaireManager = new QuestionnaireManager(ahm);
            this.questionnaireFormatManager = new QuestionnaireFormatManager(ahm);
            this.userManager = new UserManager(ahm);
            this.userEpisodeManager = new UserEpisodeManager(ahm);
            this.userEpisodeManager.AfterQuestionnaireResponseSave_Actions.AddRange(
                    new Action<QuestionnaireUserResponseGroup>[] 
                    { 
                        this.userEpisodeManager.ExtractQuestionnaireData, 
                        this.userEpisodeManager.AssignQuestionnaireUserResponseGroupTags,
                        this.userEpisodeManager.CalculateResponseScores
                    });
            this.messageManager = new MessageManager(ahm);
            this.searchManager = new SearchManager(ahm);
        }

        /// <summary>
        /// Fills int he missing details in the Audit Log entry created by the database
        /// </summary>
        /// <param name="auditLogs">The audit logs to fill the missing details in</param>
        public void CreatingAuditLogs(List<PCHI.Model.Security.AuditLog> auditLogs)
        {
            if (WcfUserSessionSecurity.Current.RequestHeader != null)
            {
                var header = WcfUserSessionSecurity.Current.RequestHeader;
                foreach (AuditLog log in auditLogs)
                {
                    log.UserId = WcfUserSessionSecurity.Current.User != null ? WcfUserSessionSecurity.Current.User.Id : "<unknown>";
                    log.UserIp = header.UserIp;
                    log.ClientIps = header.ClientIp;
                    log.ClientName = header.ClientName;
                    log.Action = SecuritySession.Current.LastAction;
                    log.ActionName = log.Action.ToString();
                }
            }
        }
    }
}