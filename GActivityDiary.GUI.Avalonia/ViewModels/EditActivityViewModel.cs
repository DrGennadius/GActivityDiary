﻿using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class EditActivityViewModel : ViewModelBase
    {
        private string _name = "";
        private Activity _activity;

        public EditActivityViewModel(ActivityListBoxViewModelBase activityListBoxViewModel, Activity activity)
        {
            ActivityListBoxViewModel = activityListBoxViewModel;

            _activity = activity;
            Name = activity.Name;
            Description = activity.Description;
            StartAtDate = activity.StartAt?.Date;
            StartAtTime = activity.StartAt?.TimeOfDay;
            EndAtDate = activity.EndAt?.Date;
            EndAtTime = activity.EndAt?.TimeOfDay;
            Tags = string.Join(", ", activity.Tags.Select(x => x.Name));

            var canExecute = this.WhenAnyValue(
                x => x.Name,
                (name) => !string.IsNullOrWhiteSpace(name));

            SaveActivityCmd = ReactiveCommand.Create(() => SaveActivity(), canExecute);
            DeleteActivityCmd = ReactiveCommand.Create(() => DeleteActivity());
            CancelCmd = ReactiveCommand.Create(() => Cancel());
        }

        [Required]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string? Description { get; set; }

        public string Tags { get; set; }

        public DateTimeOffset? StartAtDate { get; set; }

        public DateTimeOffset? EndAtDate { get; set; }

        public TimeSpan? StartAtTime { get; set; }

        public TimeSpan? EndAtTime { get; set; }

        public ActivityListBoxViewModelBase ActivityListBoxViewModel { get; }

        public ReactiveCommand<Unit, Unit> SaveActivityCmd { get; }

        public ReactiveCommand<Unit, Unit> DeleteActivityCmd { get; }

        public ReactiveCommand<Unit, Unit> CancelCmd { get; }

        public void SaveActivity()
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
            var tags = TagHelper.GetOrCreateTags(DB.Instance.Tags, Tags);
            _activity.Tags = new HashSet<Tag>(tags);
            DB.Instance.Activities.Save(_activity);
            ActivityListBoxViewModel.Update(_activity.Id);
        }

        private void DeleteActivity()
        {
            DB.Instance.Activities.Delete(_activity);
            ActivityListBoxViewModel.Update();
            ActivityListBoxViewModel.CreateActivity();
        }

        private void Cancel()
        {
            ActivityListBoxViewModel.ViewActivity(_activity);
        }
    }
}