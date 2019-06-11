using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// Class that provides static methods for sending emails
    /// Uses the <see cref="EmailServiceConfiguration"/> class for configuration purposes
    /// </summary>
    public class SmtpMailClient
    {
        /// <summary>
        /// Sends an email using SMTP mail
        /// </summary>
        /// <param name="to">The address to send to</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="text">The text to send</param>
        /// <param name="html">The html to send</param>
        public static void SendMail(string to, string subject, string text, string html)
        {
            SmtpMailClient.SendMail(new string[] { to }, null, null, subject, text, html);
        }

        /// <summary>
        /// Send an email using SMTP
        /// </summary>
        /// <param name="to">The list of TO adresses</param>
        /// <param name="cc">The list of CC adresses</param>
        /// <param name="bcc">The list of BCC adresses</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="text">The text to send</param>
        /// <param name="html">The html to send</param>
        public static void SendMail(IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, string text, string html)
        {
            EmailServiceConfiguration config = (EmailServiceConfiguration)System.Configuration.ConfigurationManager.GetSection("EmailServiceConfiguration");

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(config.SmtpFromAddress);
            foreach (string s in to)
            {
                msg.To.Add(new MailAddress(s));
            }

            if (cc != null)
            {
                foreach (string s in cc)
                {
                    msg.CC.Add(new MailAddress(s));
                }
            }

            if (bcc != null)
            {
                foreach (string s in bcc)
                {
                    msg.Bcc.Add(new MailAddress(s));
                }
            }

            msg.Subject = subject;
            if (text != null) msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            if (html != null) msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient(config.SmtpHost, config.SmtpPort);
            if (config.SmtpUser != null)
            {
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(config.SmtpUser, config.SmtpPassword);
                smtpClient.Credentials = credentials;
            }

            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception e)
            {
                throw new Exception("We had some problems sending the email :(", e);
            }
        }
    }
}