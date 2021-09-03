using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Models
{
    /// <summary>
    /// A label attached to someone or something for the purpose of identification or to give other information.
    /// </summary>
    public class Tag : IEntity
    {
        public Tag()
        { 
        }

        public Tag(string name)
        {
            Name = name;
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
