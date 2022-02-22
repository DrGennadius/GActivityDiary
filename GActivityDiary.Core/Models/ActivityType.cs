using System;

namespace GActivityDiary.Core.Models
{
    /// <summary>
    /// Activity type.
    /// </summary>
    public class ActivityType : IEntity
    {
        public ActivityType(string name, decimal cost)
        {
            Name = name;
            Cost = cost;
        }

        public ActivityType(string name)
            : this(name, 0)
        {
        }

        public ActivityType()
            : this("Unknown Activity Type")
        {
        }

        /// <summary>
        /// UUID
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Cost { get; set; }
    }
}
