using FluentNHibernate.Mapping;
using GActivityDiary.Core.Models;

namespace GActivityDiary.Core.Mapping
{
    public class ActivityMap : ClassMap<Activity>
    {
        public ActivityMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Guid();
            Map(x => x.CreatedAt)
                .Not.Nullable();
            Map(x => x.StartAt);
            Map(x => x.EndAt);
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Description);
            HasManyToMany(x => x.Tags)
                .Cascade.SaveUpdate()
                .Not.LazyLoad();
        }
    }
}
