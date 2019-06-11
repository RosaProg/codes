using System.Configuration;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// The configuration of the Email service
    /// </summary>
    public class EmailServiceConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the host address of the Smtp Server
        /// </summary>
        [ConfigurationProperty("SmtpHost", IsRequired = true)]
        public string SmtpHost
        {
            get
            {
                return (string)this["SmtpHost"];
            }

            set
            {
                this["SmtpHost"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the port of the Smtp Server (default 25)
        /// </summary>
        [ConfigurationProperty("SmtpPort", DefaultValue = 25, IsRequired = false)]
        public int SmtpPort
        {
            get
            {
                return (int)this["SmtpPort"];
            }

            set
            {
                this["SmtpPort"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the User to login with. If null no authentication is set
        /// </summary>
        [ConfigurationProperty("SmtpUser", IsRequired = false)]
        public string SmtpUser
        {
            get
            {
                return (string)this["SmtpUser"];
            }

            set
            {
                this["SmtpUser"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the password of the user to login with
        /// If the user is null, no authentication is set
        /// </summary>
        [ConfigurationProperty("SmtpPassword", IsRequired = false)]
        public string SmtpPassword
        {
            get
            {
                return (string)this["SmtpPassword"];
            }

            set
            {
                this["SmtpPassword"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the address to send from
        /// </summary>
        [ConfigurationProperty("SmtpFromAddress", IsRequired = true)]
        public string SmtpFromAddress
        {
            get
            {
                return (string)this["SmtpFromAddress"];
            }

            set
            {
                this["SmtpFromAddress"] = value;
            }
        }
    }
}