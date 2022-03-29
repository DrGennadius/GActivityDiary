using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Extensions;
using GActivityDiary.Core.Helpers;
using System;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {
        public SummaryViewModel(DbContext dbContext)
        {
            DbContext = dbContext;

            DateTime today = DateTime.Today;
            var monthInterval = today.GetMonthInterval();

            var thisMonthActivities = ActivityHelper.GetWithinPeriod(dbContext, monthInterval);
            var dayActivities = ActivityHelper.GetWithinPeriod(thisMonthActivities, today);

            DayActivityHours = ActivityHelper.GetTotalHours(dayActivities, today);
            ThisMonthActivityHours = ActivityHelper.GetTotalHours(thisMonthActivities, monthInterval);

            DayActivityCost = ActivityHelper.GetTotalCost(dayActivities, today);
            ThisMonthActivityCost = ActivityHelper.GetTotalCost(thisMonthActivities, monthInterval);
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        public double DayActivityHours { get; private set; }

        public decimal DayActivityCost { get; private set; }

        public double ThisMonthActivityHours { get; private set; }

        public decimal ThisMonthActivityCost { get; private set; }
    }
}
