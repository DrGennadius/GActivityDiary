using System;

namespace GActivityDiary.Core.Common
{
    /// <summary>
    /// Enum value and description.
    /// </summary>
    public class EnumValueDescription
    {
        public EnumValueDescription(Enum value, string description)
        {
            Value = value;
            Description = description;
        }

        /// <summary>
        /// Value as <see cref="Enum"/>.
        /// </summary>
        public Enum Value { get; set; }

        /// <summary>
        /// Description (<see cref="string"/>) for <see cref="Enum"/> value.
        /// </summary>
        public string Description { get; set; }
    }
}
