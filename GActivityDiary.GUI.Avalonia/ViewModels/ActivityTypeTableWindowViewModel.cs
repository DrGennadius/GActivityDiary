using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class ActivityTypeTableWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ActivityType> _activityTypes = new();

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        public ActivityTypeTableWindowViewModel(DbContext dbContext)
        {
            DbContext = dbContext;

            ActivityTypes = new ObservableCollection<ActivityType>(dbContext.ActivityTypes.GetAll());

            ShowDialog = new Interaction<ActivityTypeWindowViewModel, ActivityType?>();

            AddActivityTypeCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var input = new ActivityTypeWindowViewModel(dbContext, null);

                var result = await ShowDialog.Handle(input);
                if (result != null)
                {
                    ActivityTypes.Add(result);
                }
            });

            EditActivityTypeCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedActivityType == null)
                {
                    return;
                }
                var input = new ActivityTypeWindowViewModel(dbContext, SelectedActivityType);

                var result = await ShowDialog.Handle(input);
                if (result != null)
                {
                    var item = ActivityTypes.FirstOrDefault(x => x.Id == result.Id);
                    if (item == null)
                    {
                        ActivityTypes.Add(result);
                    }
                    else
                    {
                        int index = ActivityTypes.IndexOf(item);
                        ActivityTypes[index] = result;
                        ActivityTypes = new ObservableCollection<ActivityType>(ActivityTypes);
                    }
                }
            });

            RemoveActivityTypeCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedActivityType == null)
                {
                    return;
                }
                var item = await dbContext.ActivityTypes.GetByIdAsync(SelectedActivityType.Id);
                if (item != null)
                {
                    ActivityTypes.Remove(SelectedActivityType);
                    await dbContext.ActivityTypes.DeleteAsync(item);
                }
            });
        }

        public ObservableCollection<ActivityType> ActivityTypes
        {
            get => _activityTypes;
            set => this.RaiseAndSetIfChanged(ref _activityTypes, value);
        }

        public ActivityType? SelectedActivityType { get; set; }

        public ICommand AddActivityTypeCommand { get; }

        public ICommand RemoveActivityTypeCommand { get; }

        public ICommand EditActivityTypeCommand { get; }

        public Interaction<ActivityTypeWindowViewModel, ActivityType?> ShowDialog { get; }
    }
}
