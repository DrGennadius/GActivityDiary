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
        /// UUID
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime? StartAt { get; set; }

        public virtual DateTime? EndAt { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        /// <summary>
        /// List of tags for the activity.
        /// </summary>
        public virtual ISet<Tag> Tags { get; set; }
    }
}
