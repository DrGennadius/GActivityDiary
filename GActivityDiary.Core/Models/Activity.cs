using GActivityDiary.Core.Common;
using System;
using System.Collections.Generic;

namespace GActivityDiary.Core.Models
{
    /// <summary>
    /// Activity. The condition in which things are happening or being done over time. 
    /// </summary>
    public class Activity : IEntity
    {
        public Activity()
        {
            Tags = new HashSet<Tag>();
        }

        /// <summary>
        /// Create a new instance of Activity
        /// </summary>
        /// <param name="name"></param>
        public Activity(string name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// UUID
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? StartAt { get; set; }

        public virtual DateTime? EndAt { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ActivityType ActivityType { get; set; }

        /// <summary>
        /// List of tags for the activity.
        /// </summary>
        public virtual ISet<Tag> Tags { get; set; }

        public virtual TimeSpan? GetDuration()
        {
            return StartAt.HasValue && EndAt.HasValue ? 
                   EndAt.Value - StartAt.Value : 
                   null;
        }

        public virtual DateTimeInterval? GetInterval()
        {
            return StartAt.HasValue && EndAt.HasValue ? 
                   new DateTimeInterval(StartAt.Value, EndAt.Value) : 
                   null;
        }
    }
}
