using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class CreateActivityViewModel : ViewModelBase
    {
        private string _name = "";

        public CreateActivityViewModel(DbContext db, ActivityListBoxViewModelBase activityListBoxViewModel)
        {
            Db = db;

            ActivityListBoxViewModel = activityListBoxViewModel;

            DateTime now = TimeRounderHelper.Floor(DateTime.Now);
            DateTime inHour = now.AddHours(1);
            StartAtDate = now.Date;
            StartAtTime = now.TimeOfDay;
            EndAtDate = inHour.Date;
            EndAtTime = inHour.TimeOfDay;

            var canExecute = this.WhenAnyValue(
                x => x.Name,
                (name) => !string.IsNullOrWhiteSpace(name));

            CreateActivityCmd = ReactiveCommand.Create(() => CreateActivity(), canExecute);
            CancelCmd = ReactiveCommand.Create(() => Cancel());
        }

        // Database context.
        public DbContext Db { get; private set; }

        [Required]
        public string Name 
        { 
            get => _name; 
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string? Description { get; set; }

        public string? Tags { get; set; }

        public DateTimeOffset? StartAtDate { get; set; }

        public DateTimeOffset? EndAtDate { get; set; }

        public TimeSpan? StartAtTime { get; set; }

        public TimeSpan? EndAtTime { get; set; }

        public ActivityListBoxViewModelBase ActivityListBoxViewModel { get; }

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
            var tags = TagHelper.GetOrCreateTags(Db.Tags, Tags);
            Activity activity = new()
            {
                Name = Name,
                Description = Description,
                StartAt = startAt,
                EndAt = endAt,
                Tags = new HashSet<Tag>(tags)
            };
            var uid = Db.Activities.Save(activity);
            Db.Commit();
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
