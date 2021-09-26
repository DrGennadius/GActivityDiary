using Avalonia.Controls;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.ViewModels
{
    public class CreateActivityViewModel : ViewModelBase
    {
        public CreateActivityViewModel(ActivityListBoxViewModel activityListBoxViewModel)
        {
            ActivityListBoxViewModel = activityListBoxViewModel;
            CreateActivityCmd = ReactiveCommand.Create(() => CreateActivity());
            CancelCmd = ReactiveCommand.Create(() => Cancel());

            DateTime now = DateTime.Now;
            DateTime inHour = now.AddHours(1);
            StartAtDate = now.Date;
            StartAtTime = now.TimeOfDay;
            EndAtDate = inHour.Date;
            EndAtTime = inHour.TimeOfDay;
        }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public string Tags { get; set; }

        public DateTimeOffset? StartAtDate { get; set; }

        public DateTimeOffset? EndAtDate { get; set; }

        public TimeSpan? StartAtTime { get; set; }

        public TimeSpan? EndAtTime { get; set; }

        public ActivityListBoxViewModel ActivityListBoxViewModel { get; }

        public ReactiveCommand<Unit, Unit> CreateActivityCmd { get; }

        public ReactiveCommand<Unit, Unit> CancelCmd { get; }

        public void CreateActivity()
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
            var tags = TagHelper.GetOrCreateTags(DB.Instance.Tags, Tags);
            Activity activity = new()
            {
                Name = Name,
                Description = Description,
                StartAt = startAt,
                EndAt = endAt,
                Tags = new HashSet<Tag>(tags)
            };
            var uid = DB.Instance.Activities.Save(activity);
            DB.Instance.Commit();
            ActivityListBoxViewModel.Update(uid);
        }

        private void Cancel()
        {
            if (ActivityListBoxViewModel.SelectedActivity != null)
            {
                ActivityListBoxViewModel.ViewActivity(ActivityListBoxViewModel.SelectedActivity);
            }
            else
            {
                ActivityListBoxViewModel.CreateActivity();
            }
        }
    }
}
