using System;

namespace GActivityDiary.Core.Common
{
    /// <summary>
    /// Language profile.
    /// </summary>
    public class LanguageProfile
    {
        public LanguageWordInfo Hour { get; set; }

        public LanguageWordInfo Minute { get; set; }

        /// <summary>
        /// Get default 'English' language profile.
        /// </summary>
        /// <returns></returns>
        public static LanguageProfile GetDefaultEng()
        {
            return new LanguageProfile()
            {
                Hour = new LanguageWordInfo()
                {
                    Short = "h",
                    Singular = "hour",
                    Plural = "hours"
                },
                Minute = new LanguageWordInfo()
                {
                    Short = "m",
                    Singular = "minute",
                    Plural = "minutes"
                }
            };
        }
    }
}
