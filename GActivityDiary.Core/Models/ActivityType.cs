using System;

namespace GActivityDiary.Core.Models
{
    /// <summary>
    /// Activity type.
    /// </summary>
    public class ActivityType : IEntity
    {
        public ActivityType()
        {
            Name = "Unknown Activity Type";
            Cost = 0;
        }

        public ActivityType(string name, decimal cost)
        {
            Name = name;
            Cost = cost;
        }

        /// <summary>
        /// UUID
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Cost { get; set; }
    }
}
