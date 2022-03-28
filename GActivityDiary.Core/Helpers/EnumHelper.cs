using GActivityDiary.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace GActivityDiary.Core.Helpers
{
    /// <summary>
    /// Helper of getting <see cref="DescriptionAttribute"/> values from <see cref="Enum"/>.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Get a description of the <see cref="Enum"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
            {
                return (attributes.First() as DescriptionAttribute).Description;
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        /// <summary>
        /// Get a collection of values - descriptions of all values of the <see cref="Enum"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<EnumValueDescription> GetAllValuesAndDescriptions(Type type)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(type)} is not an enumeration.");
            }

            return Enum.GetValues(type)
                       .Cast<Enum>()
                       .Select((e) => new EnumValueDescription(e, e.GetDescription()))
                       .ToList();
        }

        /// <summary>
        /// Get all values of enum.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetAllValues(Type type)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(type)} is not an enumeration.");
            }

            return Enum.GetValues(type)
                       .Cast<Enum>();
        }
    }
}
