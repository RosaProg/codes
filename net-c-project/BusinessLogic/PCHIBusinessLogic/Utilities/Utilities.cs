using DSPrima.Security;
using PCHI.Model.Questionnaire;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Utilities
{
    /// <summary>
    /// provides utility functionality
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Filtes the different text versions inside a Questionnaire and it's sections and items down the line.
        /// </summary>
        /// <param name="q">The questionnaire to modify</param>
        /// <param name="instance">The instance to filter on</param>
        /// <param name="platform">The platform to filter on</param>
        /// <param name="audience">The audience to filter on</param>
        public static void Filter(ref Questionnaire q, Instance instance, Platform platform, UserTypes audience)
        {
            q.IntroductionMessages = q.IntroductionMessages.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
            q.Descriptions = q.Descriptions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
            foreach (QuestionnaireSection s in q.Sections)
            {
                s.Instructions = s.Instructions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                foreach (QuestionnaireElement e in s.Elements)
                {
                    e.TextVersions = e.TextVersions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                    if (e.GetType() == typeof(QuestionnaireItem))
                    {
                        QuestionnaireItem item = (QuestionnaireItem)e;
                        Utilities.Filter(ref item, instance, platform, audience);
                    }
                }
            }
        }

        /// <summary>
        /// Filters an items TextVersion instances
        /// </summary>
        /// <param name="item">The item to filter in</param>
        /// <param name="instance">The instance to filter on</param>
        /// <param name="platform">The platform to filter on</param>
        /// <param name="audience">The audience to filter on</param>
        internal static void Filter(ref QuestionnaireItem item, Instance instance, Platform platform, UserTypes audience)
        {
            item.TextVersions = item.TextVersions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
            item.Instructions = item.Instructions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
            if (item.OptionGroups != null)
            {
                foreach (QuestionnaireItemOptionGroup group in item.OptionGroups)
                {
                    group.TextVersions = group.TextVersions.Where(i => i.SupportsInstance(instance) && i.SupportsPlatform(platform) && i.SupportsAudience(audience)).ToList();
                }
            }
        }
    }
}
