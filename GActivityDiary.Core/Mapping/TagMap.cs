using FluentNHibernate.Mapping;
using GActivityDiary.Core.Models;

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
