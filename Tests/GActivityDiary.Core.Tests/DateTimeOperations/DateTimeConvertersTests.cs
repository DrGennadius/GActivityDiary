using GActivityDiary.Core.Common;
using GActivityDiary.Core.Converters.Text;
using GActivityDiary.Core.Converters.Time;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
                Assert.AreEqual(item.Value, text);

                TimeSpan timeSpan = converter.FromText(item.Value);
                Assert.AreEqual(item.Key, timeSpan);
            }

            Assert.Pass();
        }

        [Test]
        public void TimeConverterTest()
        {
            Assert.AreEqual(1.5, TimeConverter.GetHours(1, 30));
            Assert.AreEqual(90, TimeConverter.GetMinutes(1, 30));
            Assert.AreEqual((1, 30), TimeConverter.GetHoursAndMinutesFromHours(1.5));
            Assert.AreEqual((1, 30), TimeConverter.GetHoursAndMinutes(90));

            Assert.Pass();
        }
    }
}
