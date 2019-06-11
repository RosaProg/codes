using PCHI.Model.Questionnaire;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Messages
{
    /// <summary>
    /// Defines a Texversion. A piece of text that is only applicable for a given platform and instance.
    /// </summary>
    public class TextVersion
    {
        /// <summary>
        /// Gets or sets the database identifier of this element Text
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the text for the Element
        /// </summary>      
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the platform this text can be used for
        /// </summary>
        public Platform SupportedPlatforms { get; set; }

        /// <summary>
        /// Gets or sets the instance this text can be used for
        /// </summary>
        public Instance SupportedInstances { get; set; }

        /// <summary>
        /// Gets or sets the Audience the Text is for
        /// </summary>
        public UserTypes Audience { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextVersion"/> class
        /// Sets the SupportedInstances and SupportPlatforms to a default value
        /// </summary>
        public TextVersion()
        {
            this.SupportedInstances = Instance.Baseline | Instance.Followup;
            this.SupportedPlatforms = Platform.Classic | Platform.Chat | Platform.Mobile;
        }

        #region  Platform
        /// <summary>
        /// Sets the given attributes on this item
        /// </summary>
        /// <param name="platform">The QuestionnaireItemAttributes to set</param>
        /// <param name="platforms">Any additional platforms to set</param>
        public void SetSupportedPlatforms(Platform platform, params Platform[] platforms)
        {
            this.SupportedPlatforms = platform;
            for (int i = 1; i < platforms.Length; i++)
            {
                this.SupportedPlatforms = this.SupportedPlatforms | platforms[i];
            }
        }

        /// <summary>
        /// Checks if this Item supports the specified Platform
        /// </summary>
        /// <param name="platform">The Platform to look for</param>
        /// <returns>True if the Platform is set, false otherwise</returns>
        public bool SupportsPlatform(Platform platform)
        {
            return this.SupportedPlatforms.HasFlag(platform);
        }

        /// <summary>
        /// Gets all Platform set on this item.
        /// </summary>
        /// <returns>The list of Platform set</returns>
        public List<Platform> GetSupportedPlatforms()
        {
            Array values = Enum.GetValues(typeof(Platform));
            List<Platform> result = new List<Platform>();
            foreach (Platform item in values)
            {
                if (this.SupportedPlatforms.HasFlag(item)) result.Add(item);
            }

            return result;
        }
        #endregion

        #region Instance
        /// <summary>
        /// Sets the given attributes on this item
        /// </summary>
        /// <param name="instance">The QuestionnaireItemAttributes to set</param>
        /// <param name="instances">Any additional instances to set</param>
        public void SetSupportedInstances(Instance instance, params Instance[] instances)
        {
            this.SupportedInstances = instance;
            for (int i = 1; i < instances.Length; i++)
            {
                this.SupportedInstances = this.SupportedInstances | instances[i];
            }
        }

        /// <summary>
        /// Checks if this Item supports the specified Instance
        /// </summary>
        /// <param name="instance">The Instance to look for</param>
        /// <returns>True if the Instance is set, false otherwise</returns>
        public bool SupportsInstance(Instance instance)
        {
            return this.SupportedInstances.HasFlag(instance);
        }

        /// <summary>
        /// Gets all Instance set on this item.
        /// </summary>
        /// <returns>The list of Instance set</returns>
        public List<Instance> GetSupportedInstances()
        {
            Array values = Enum.GetValues(typeof(Instance));
            List<Instance> result = new List<Instance>();
            foreach (Instance item in values)
            {
                if (this.SupportedInstances.HasFlag(item)) result.Add(item);
            }

            return result;
        }
        #endregion

        #region Audience
        /// <summary>
        /// Sets the given Audiences on this item
        /// </summary>
        /// <param name="userType">The Audience to set</param>
        /// <param name="userTypes">Any additional Audiances to set</param>
        public void SetSupportedAudiences(UserTypes userType, params UserTypes[] userTypes)
        {
            this.Audience = userType;
            for (int i = 1; i < userTypes.Length; i++)
            {
                this.Audience = this.Audience | userTypes[i];
            }
        }

        /// <summary>
        /// Checks if this Item supports the specified Instance
        /// </summary>
        /// <param name="userType">The Instance to look for</param>
        /// <returns>True if the Instance is set, false otherwise</returns>
        public bool SupportsAudience(UserTypes userType)
        {
            return this.Audience.HasFlag(UserTypes.All) || this.Audience.HasFlag(userType);
        }

        /// <summary>
        /// Gets all Instance set on this item.
        /// </summary>
        /// <returns>The list of Instance set</returns>
        public List<UserTypes> GetSupportedAudiences()
        {            
            Array values = Enum.GetValues(typeof(UserTypes));
            if (this.Audience == UserTypes.All) return new List<UserTypes>(values.Cast<UserTypes>());
            List<UserTypes> result = new List<UserTypes>();
            foreach (UserTypes item in values)
            {
                if (this.Audience.HasFlag(item)) result.Add(item);
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Gets a string representation of this instance
        /// </summary>
        /// <returns>The Text value</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}
