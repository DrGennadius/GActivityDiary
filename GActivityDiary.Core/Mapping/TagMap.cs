using FluentNHibernate.Mapping;
using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Mapping
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Guid();
            Map(x => x.Name)
                .Not.Nullable();
        }
    }
}
