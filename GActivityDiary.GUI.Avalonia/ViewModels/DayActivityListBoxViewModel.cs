using Avalonia.Controls.Selection;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate.Linq;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class DayActivityListBoxViewModel : ActivityListBoxViewModelBase
    {
        private DateTimeOffset? _selectedDate;

        private int _collectionCount;

        public DayActivityListBoxViewModel(DbContext db)
            : base(db)
        {
            SelectedDate = DateTime.Now;
        }

        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    Update();
                }
                this.RaiseAndSetIfChanged(ref _selectedDate, value);
            }
        }

        public int CollectionCount
        {
            get => _collectionCount;
            set => this.RaiseAndSetIfChanged(ref _collectionCount, value);
        }

        public override void Update(Guid? targetActivityId = null)
        {
            _tokenSource = new();
            _updateTask = Task.Run(() => UpdateAsync(targetActivityId), _tokenSource.Token);
        }

        private async void UpdateAsync(Guid? targetActivityId = null)
        {
            DateTime now = DateTime.Now;
            DateTime startAt = SelectedDate.HasValue
                ? new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, SelectedDate.Value.Day, 0, 0, 0)
                : new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTime endAt = startAt.AddDays(1);
            CollectionCount = DbContext.Activities.Query()
                .Where(x => x.StartAt >= startAt && x.EndAt < endAt)
                .Count();
            Activities.Clear();
            var activities = await DbContext.Activities.Query()
                .Where(x => x.StartAt >= startAt && x.EndAt < endAt)
                .ToListAsync();
            Activities = new ObservableCollection<Activity>(activities);
            if (targetActivityId.HasValue)
            {
                var targetActivty = DbContext.Activities.GetById(targetActivityId.Value);
                SelectedActivity = targetActivty;
            }
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Activity> e)
        {
            var selectionModel = sender as SelectionModel<Activity>;
            var activity = selectionModel?.SelectedItem;
            if (activity != null)
            {
                ViewActivity(activity);
            }
        }
    }
}
