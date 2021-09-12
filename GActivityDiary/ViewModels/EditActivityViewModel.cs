using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace GActivityDiary.ViewModels
{
    public class EditActivityViewModel : ViewModelBase
    {
        private Activity? _activity;

        public EditActivityViewModel(ActivityListBoxViewModel activityListBoxViewModel, Activity activity)
        {
            ActivityListBoxViewModel = activityListBoxViewModel;
            if (activity != null)
            {
                _activity = activity;
                Name = activity.Name;
                Description = activity.Description;
                StartAtDate = activity.StartAt?.Date;
                StartAtTime = activity.StartAt?.TimeOfDay;
                EndAtDate = activity.EndAt?.Date;
                EndAtTime = activity.EndAt?.TimeOfDay;
            };
            SaveActivityCmd = ReactiveCommand.Create(() => SaveActivity());
            DeleteActivityCmd = ReactiveCommand.Create(() => DeleteActivity());
        }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public DateTimeOffset? StartAtDate { get; set; }

        public DateTimeOffset? EndAtDate { get; set; }

        public TimeSpan? StartAtTime { get; set; }

        public TimeSpan? EndAtTime { get; set; }

        public ActivityListBoxViewModel ActivityListBoxViewModel { get; }

        public ReactiveCommand<Unit, Unit> SaveActivityCmd { get; }

        public ReactiveCommand<Unit, Unit> DeleteActivityCmd { get; }

        public void SaveActivity()
        {
            if (_activity != null)
            {
                DateTime? startAt = StartAtDate?.Date;
                if (startAt.HasValue && StartAtTime.HasValue)
                {
                    startAt = startAt.Value.Add(StartAtTime.Value);
                }
                DateTime? endAt = EndAtDate?.Date;
                if (endAt.HasValue && EndAtTime.HasValue)
                {
                    endAt = endAt.Value.Add(EndAtTime.Value);
                }
                _activity.Name = Name;
                _activity.Description = Description;
                _activity.StartAt = startAt;
                _activity.EndAt = endAt;
                DB.Instance.Activities.Save(_activity);
                ActivityListBoxViewModel.GetActivities();
            }
        }

        public void DeleteActivity()
        {
            if (_activity != null)
            {
                DB.Instance.Activities.Delete(_activity);
                ActivityListBoxViewModel.Activities.Remove(_activity);
                _activity = null;
            }
        }
    }
}
