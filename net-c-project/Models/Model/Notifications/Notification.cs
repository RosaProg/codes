using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Notifications
{
    /// <summary>
    /// Defines a notification with the data avialable
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the database Id of the Notification
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of notification
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the notification Object Id
        /// </summary>
        public string NotificationObjectId { get; set; }

        /// <summary>
        /// Gets or sets the time the notification was sent
        /// </summary>
        public DateTime? NotificationSendTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the notifications were completed
        /// </summary>
        public bool NotificationCompleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time the Notification was created
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class
        /// </summary>
        public Notification()
        {
            this.DateTimeCreated = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class
        /// </summary>
        /// <param name="notificationType">The type of notifiction</param>
        /// <param name="notificationObjectId">The notification object Id</param>
        public Notification(NotificationType notificationType, string notificationObjectId)
            : this()
        {
            this.NotificationType = notificationType;
            this.NotificationObjectId = notificationObjectId;
        }
    }
}
