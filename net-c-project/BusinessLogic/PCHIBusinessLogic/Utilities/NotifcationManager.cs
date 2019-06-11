using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PCHI.DataAccessLibrary;
using PCHI.Model.Messages;
using PCHI.Model.Notifications;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// Manages the logic regarding notification and when to send them.
    /// </summary>
    public class NotifcationManager
    {
        /// <summary>
        /// Holds notification data
        /// </summary>
        private struct NotificationData
        {
            /// <summary>
            /// The target of the notifiction
            /// </summary>
            public object NotificationTarget;

            /// <summary>
            /// The type of notifications that target has
            /// </summary>
            public List<NotificationType> Notifications;
        }

        /// <summary>
        /// Runs the notification checker and sends outstanding notifications and reminders
        /// </summary>
        public static void SendNotifications()
        {
            AccessHandlerManager ahm = new AccessHandlerManager();
            List<Notification> notifications = ahm.NotificationHandler.GetNotifications();
            Dictionary<string, NotificationData> notificationData = new Dictionary<string, NotificationData>();

            foreach (Notification n in notifications)
            {
                switch (n.NotificationType)
                {
                    case NotificationType.RegistrationComplete:
                        Task<User> t = ahm.UserAccessHandler.FindByIdAsync(n.NotificationObjectId);
                        t.Wait();
                        User user = t.Result;
                        NotifcationManager.AssignNotification(user, n.NotificationType, notificationData);
                        n.NotificationSendTime = DateTime.Now;
                        n.NotificationCompleted = true;
                        ahm.NotificationHandler.UpdateNotification(n);
                        break;
                    case NotificationType.NewQuestionnaire:
                        QuestionnaireUserResponseGroup group = ahm.QuestionnaireAccessHandler.GetSmallQuestionnaireUserResponseGroupById(int.Parse(n.NotificationObjectId));
                        Patient patient = ahm.UserAccessHandler.FindPatient(group.Patient.Id);
                        if (group != null && !group.Completed && group.Patient.ProxyUserPatientMap.Any(m => m.User.EmailConfirmed) && (n.NotificationSendTime == null || n.NotificationSendTime < DateTime.Now.AddHours(-4)))
                        {
                            NotifcationManager.AssignNotification(patient, n.NotificationType, notificationData);
                            n.NotificationSendTime = DateTime.Now;
                            n.NotificationCompleted = true;
                            ahm.NotificationHandler.UpdateNotification(n);
                        }
                        else
                        {
                            n.NotificationCompleted = true;
                            ahm.NotificationHandler.UpdateNotification(n);
                        }

                        break;
                }
            }

            TextParser parser = new TextParser(ahm);

            foreach (string email in notificationData.Keys)
            {
                NotificationData data = notificationData[email];
                StringBuilder textBuilder = new StringBuilder();
                StringBuilder htmlBuilder = new StringBuilder();

                ReplaceableObjectKeys objectKey = data.NotificationTarget.GetType() == typeof(User) ? ReplaceableObjectKeys.User : ReplaceableObjectKeys.Patient;

                TextDefinition start = parser.ParseMessage("NotificationStart", new Dictionary<ReplaceableObjectKeys, object>() { { objectKey, data.NotificationTarget } });
                TextDefinition end = parser.ParseMessage("NotificationEnd", new Dictionary<ReplaceableObjectKeys, object>() { { objectKey, data.NotificationTarget } });

                textBuilder.Append(start.Text);
                htmlBuilder.Append(start.Html);

                foreach (NotificationType t in data.Notifications)
                {
                    TextDefinition td;
                    td = parser.ParseMessage(t.ToString(), new Dictionary<ReplaceableObjectKeys, object>() { { objectKey, data.NotificationTarget } });
                    textBuilder.Append(td.Text);
                    htmlBuilder.Append(td.Html);

                    /*
                    switch (t)
                    {
                        case NotificationType.RegistrationComplete:
                            //textBuilder.AppendLine(Text)
                            td = parser.ParseMessage(NotificationType.RegistrationComplete.ToString(), null);
                            textBuilder.Append(td.Text);
                            htmlBuilder.Append(td.Html);
                            break;
                        case NotificationType.NewQuestionnaire:
                            td = parser.ParseMessage(NotificationType.RegistrationComplete.ToString(), null);
                            textBuilder.Append(td.Text);
                            htmlBuilder.Append(td.Html);                            
                            break;
                    }*/
                }

                textBuilder.Append(end.Text);
                htmlBuilder.Append(end.Html);

                SmtpMailClient.SendMail(email, "Replay Notification", textBuilder.ToString(), htmlBuilder.ToString());
            }
        }

        /// <summary>
        /// Assigns a Notification to the user if it isn't assigned already.
        /// </summary>
        /// <param name="patient">The patient to assign the notification to</param>
        /// <param name="notificationType">The type of notification </param>
        /// <param name="notificationData">The notification data to add the notification to</param>
        private static void AssignNotification(Patient patient, NotificationType notificationType, Dictionary<string, NotificationData> notificationData)
        {
            foreach (User u in patient.ProxyUserPatientMap.Select(u => u.User))
            {
                NotifcationManager.AssignNotification(u, notificationType, notificationData);
            }

            if (string.IsNullOrWhiteSpace(patient.Email)) return;

            if (notificationData.ContainsKey(patient.Email) && notificationData[patient.Email].NotificationTarget.GetType() != typeof(Patient)) return;

            if (!notificationData.ContainsKey(patient.Email))
            {
                NotificationData data = new NotificationData() { NotificationTarget = patient, Notifications = new List<NotificationType>() };
                notificationData.Add(patient.Email, data);
            }

            if (!notificationData[patient.Email].Notifications.Contains(notificationType))
            {
                notificationData[patient.Email].Notifications.Add(notificationType);
            }
        }

        /// <summary>
        /// Assigns the given Notification to the given User.
        /// </summary>
        /// <param name="user">The user to add the notification for</param>
        /// <param name="notificationType">The type of notification</param>
        /// <param name="notificationData">The list of notification Data to add it to</param>
        private static void AssignNotification(User user, NotificationType notificationType, Dictionary<string, NotificationData> notificationData)
        {
            if (!notificationData.ContainsKey(user.Email))
            {
                NotificationData data = new NotificationData() { NotificationTarget = user, Notifications = new List<NotificationType>() };
                notificationData.Add(user.Email, data);
            }

            if (!notificationData[user.Email].Notifications.Contains(notificationType))
            {
                notificationData[user.Email].Notifications.Add(notificationType);
            }
        }
    }
}
