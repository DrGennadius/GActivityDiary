using GActivityDiary.Core.Common;
using GActivityDiary.Core.Converters.Text;
using GActivityDiary.Core.Converters.Time;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Tests.DateTimeOperations
{
    public class DateTimeConvertersTests
    {
        [Test]
        public void TextTimeSpanConverterEngTest()
        {
            LanguageProfile languageProfile = new()
            {
                Hour = new()
                {
                    Singular = "hour",
                    Plural = "hours",
                    Short = "h"
                },
                Minute = new()
                {
                    Singular = "minute",
                    Plural = "minutes",
                    Short = "m"
                }
            };

            TextTimeSpanConverter converter = new(languageProfile);

            Dictionary<TimeSpan, string> timeSpanAndTextDict = new()
            {
                { new(1, 1, 0), "1 hour 1 minute" },
                { new(22, 50, 0), "22 hours 50 minutes" }
            };

            foreach (var item in timeSpanAndTextDict)
            {
                string text = converter.ToText(item.Key);
                Assert.AreEqual(text, item.Value);

                TimeSpan timeSpan = converter.FromText(item.Value);
                Assert.AreEqual(timeSpan, item.Key);
            }

            Assert.Pass();
        }

        [Test]
        public void TimeConverterTest()
        {
            Assert.AreEqual(TimeConverter.GetHours(1, 30), 1.5);
            Assert.AreEqual(TimeConverter.GetMinutes(1, 30), 90);
            Assert.AreEqual(TimeConverter.GetHoursAndMinutesFromHours(1.5), (1, 30));
            Assert.AreEqual(TimeConverter.GetHoursAndMinutes(90), (1, 30));

            Assert.Pass();
        }
    }
}
