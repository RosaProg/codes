using PCHI.DataAccessLibrary.AccessHandelers;
using PCHI.DataAccessLibrary.Context;
using PCHI.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary
{
    /// <summary>
    /// Instantiate all the Access Handlers and allow them to be accessed and share the same Database context
    /// </summary>
    public class AccessHandlerManager
    {
        /// <summary>
        /// Holds the private instance of the <see cref="QuestionnaireAccessHandler"/>
        /// </summary>
        private QuestionnaireAccessHandler questionnaireAccessHandler;

        /// <summary>
        /// Gets the instance of the <see cref="QuestionnaireAccessHandler"/>
        /// </summary>
        public QuestionnaireAccessHandler QuestionnaireAccessHandler { get { return this.questionnaireAccessHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="QuestionnaireFormatDefinitionAccessHandlerQuestionnaireFormatAccessHandler"/>
        /// </summary>
        private QuestionnaireFormatAccessHandler questionnaireFormatAccessHandler;

        /// <summary>
        /// Gets the instance of the <see cref="QuestionnaireFormatAccessHandler"/>
        /// </summary>
        public QuestionnaireFormatAccessHandler QuestionnaireFormatAccessHandler { get { return this.questionnaireFormatAccessHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="TagAccessHandler"/>
        /// </summary>
        private TagAccessHandler tagAccessHandler;

        /// <summary>
        /// Gets the instance of the <see cref="TagAccessHandler"/>
        /// </summary>
        public TagAccessHandler TagAccessHandler { get { return this.tagAccessHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="UserAccessHandler"/>
        /// </summary>
        private UserAccessHandler userAccessHandler;

        /// <summary>
        /// Gets the instance of the <see cref="UserAccessHandler"/>
        /// </summary>
        public UserAccessHandler UserAccessHandler { get { return this.userAccessHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="MessageHandler"/>
        /// </summary>
        private MessageHandler messageHandler;

        /// <summary>
        /// Gets the instance of the <see cref="MessageHandler"/>
        /// </summary>
        public MessageHandler MessageHandler { get { return this.messageHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="EpisodeAccessHandler"/>
        /// </summary>
        private EpisodeAccessHandler episodeAccessHandler;

        /// <summary>
        /// Gets the instance of the <see cref="EpisodeAccessHandler"/>
        /// </summary>
        public EpisodeAccessHandler EpisodeAccessHandler { get { return this.episodeAccessHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="NotificationHandler"/>
        /// </summary>
        private NotificationHandler notificationHandler;

        /// <summary>
        /// Gets the instance of the <see cref="NotificationHandler"/>
        /// </summary>
        public NotificationHandler NotificationHandler { get { return this.notificationHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="AuditHandler"/>
        /// </summary>
        private AuditHandler auditHandler;

        /// <summary>
        /// Gets the instance of the <see cref="AuditHandler"/>
        /// </summary>
        public AuditHandler AuditHandler { get { return this.auditHandler; } }

        /// <summary>
        /// Holds the private instance of the <see cref="SearchHandler"/>
        /// </summary>
        private SearchHandler searchHandler;

        /// <summary>
        /// Gets the instance of the <see cref="SearchHandler"/>
        /// </summary>
        public SearchHandler SearchHandler { get { return this.searchHandler; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessHandlerManager"/> class
        /// </summary>
        /// <param name="creatingAuditLogs">The CreatingAuditLogs event handler used to fill in any missing details. CURRENTLY NOT USED!!!</param>
        public AccessHandlerManager(CreatingAuditLogsEventHandler creatingAuditLogs = null)
            : this(new MainDatabaseContext(), creatingAuditLogs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessHandlerManager"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        /// <param name="creatingAuditLogs">The CreatingAuditLogs event handler used to fill in any missing details. CURRENTLY NOT USED!!!</param>
        internal AccessHandlerManager(MainDatabaseContext context, CreatingAuditLogsEventHandler creatingAuditLogs = null)
        {
            //// if(creatingAuditLogs != null) context.CreatingAuditLogs += creatingAuditLogs;

            this.questionnaireAccessHandler = new QuestionnaireAccessHandler(context);
            this.questionnaireFormatAccessHandler = new QuestionnaireFormatAccessHandler(context);
            this.tagAccessHandler = new TagAccessHandler(context);
            this.userAccessHandler = new UserAccessHandler(context);
            this.messageHandler = new MessageHandler(context);
            this.episodeAccessHandler = new EpisodeAccessHandler(context);
            this.notificationHandler = new NotificationHandler(context);
            this.auditHandler = new AuditHandler(context);
            this.searchHandler = new SearchHandler(context);
        }
    }
}
