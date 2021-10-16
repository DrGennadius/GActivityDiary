using GActivityDiary.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Converters.Text
{
    /// <summary>
    /// <see cref="TimeSpan"/> and <see cref="string"/> converter.
    /// </summary>
    public class TextTimeSpanConverter : ITextConverter<TimeSpan>
    {
        public TextTimeSpanConverter(LanguageProfile languageProfile)
        {
            LanguageProfile = languageProfile;
        }

        public LanguageProfile LanguageProfile { get; private set; }

        public string ToText(TimeSpan timeSpan)
        {
            string hoursString = LanguageProfile.Hour?.GetValue(timeSpan.Hours);
            string minutesString = LanguageProfile.Minute?.GetValue(timeSpan.Minutes);
            return $"{timeSpan.Hours} {hoursString} {timeSpan.Minutes} {minutesString}";
        }

        public TimeSpan FromText(string text)
        {
            var hoursWord = LanguageProfile.Hour;
            var minutesWord = LanguageProfile.Minute;
            string hoursPattern = $"\\d+(?=\\s+({hoursWord.Singular}|{hoursWord.Plural}))";
            string minutesPattern = $"\\d+(?=\\s+({minutesWord.Singular}|{minutesWord.Plural}))";
            var hoursMatch = Regex.Match(text, hoursPattern);
            var minutesMatch = Regex.Match(text, minutesPattern);
            int hours = 0;
            int minutes = 0;
            if (hoursMatch.Success)
            {
                hours = Convert.ToInt32(hoursMatch.Value);
            }
            else
            {
                hoursPattern = $"\\d+(?=\\s+{hoursWord.Short})";
                hoursMatch = Regex.Match(text, hoursPattern);
                if (hoursMatch.Success)
                {
                    hours = Convert.ToInt32(hoursMatch.Value);
                }
            }
            if (minutesMatch.Success)
            {
                minutes = Convert.ToInt32(minutesMatch.Value);
            }
            else
            {
                minutesPattern = $"\\d+(?=\\s+{minutesWord.Short})";
                minutesMatch = Regex.Match(text, minutesPattern);
                if (minutesMatch.Success)
                {
                    minutes = Convert.ToInt32(minutesMatch.Value);
                }
            }
            return new TimeSpan(hours, minutes, 0);
        }
    }
}
