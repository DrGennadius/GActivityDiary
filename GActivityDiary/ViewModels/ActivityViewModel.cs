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
    public class ActivityViewModel : ViewModelBase
    {
        private readonly Activity _activity;

        public ActivityViewModel(ActivityListBoxViewModel activityListBoxViewModel, Activity activity)
        {
            ActivityListBoxViewModel = activityListBoxViewModel;
            _activity = activity;
            Name = activity.Name;
            Description = activity.Description;
            StartAt = activity.StartAt;
            EndAt = activity.EndAt;
            Tags = string.Join(", ", activity.Tags.Select(x => x.Name));

            EditActivityCmd = ReactiveCommand.Create(() => EditActivity());
        }

        public string Name { get; set; } = "";

        public string Description { get; set; }

        public string Tags { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        public ActivityListBoxViewModel ActivityListBoxViewModel { get; }

        public ReactiveCommand<Unit, Unit> EditActivityCmd { get; }

        public bool IsDescriptionVisible => !string.IsNullOrWhiteSpace(Description);

        public bool IsTagsVisible => !string.IsNullOrWhiteSpace(Tags);

        public void EditActivity()
        {
            ActivityListBoxViewModel.EditActivity(_activity);
        }
    }
}
