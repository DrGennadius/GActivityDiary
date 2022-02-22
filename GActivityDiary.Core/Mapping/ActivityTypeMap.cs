using FluentNHibernate.Mapping;
using GActivityDiary.Core.Models;

namespace GActivityDiary.Core.Mapping
{
    public class ActivityTypeMap : ClassMap<ActivityType>
    {
        public ActivityTypeMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Guid();
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Cost)
                .Not.Nullable();
        }
    }
}
