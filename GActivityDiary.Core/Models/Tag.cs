using System;

namespace GActivityDiary.Core.Models
{
    /// <summary>
    /// A label attached to someone or something for the purpose of identification or to give other information.
    /// </summary>
    public class Tag : IEntity
    {
        private string _name;

        public Tag()
        {
        }

        public Tag(string name)
        {
            Name = name;
        }

        /// <summary>
        /// UUID
        /// </summary>
        public virtual Guid Id { get; set; }

        public virtual string Name { get => _name; set => _name = value.ToLower(); }

        // public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public override string ToString()
        {
            return Name;
        }
    }
}
