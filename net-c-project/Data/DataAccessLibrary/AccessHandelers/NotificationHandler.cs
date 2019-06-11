using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles the storing and retrieving of Notifications in the database
    /// </summary>
    public class NotificationHandler
    {
        /// <summary>
        /// The context manager to use to get data from the database
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="DatabaseContext"/> to use</param>
        internal NotificationHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="notificationType">The notification Type</param>
        /// <param name="notificationObjectId">The object Id that is notified</param>
        public void CreateNotification(NotificationType notificationType, string notificationObjectId)
        {
            this.context.Notifications.Add(new Notification(notificationType, notificationObjectId));
            this.context.SaveChanges();
        }

        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="notificationType">The notification Type</param>
        /// <param name="notificationObjectId">The object Id that is notified</param>
        public void CreateNotification(NotificationType notificationType, int notificationObjectId)
        {
            this.context.Notifications.Add(new Notification(notificationType, notificationObjectId.ToString()));
            this.context.SaveChanges();
        }

        /// <summary>
        /// Updates the given Notification
        /// </summary>
        /// <param name="notification">The notification to update</param>
        public void UpdateNotification(Notification notification)
        {
            this.context.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets all Notifications that have not been send yet
        /// </summary>
        /// <returns>The list of outstanding notifications found</returns>
        public List<Notification> GetNotifications()
        {
            return this.context.Notifications.Where(n => n.NotificationCompleted == false).ToList();
        }
    }
}
