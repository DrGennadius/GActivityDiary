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
                var input = new ActivityTypeWindowViewModel(dbContext, this);

                var result = await ShowDialog.Handle(input);
                if (result != null)
                {
                    ActivityTypes.Add(result);
                }
            });
        }

        public ObservableCollection<ActivityType> ActivityTypes
        {
            get => _activityTypes;
            set => this.RaiseAndSetIfChanged(ref _activityTypes, value);
        }

        public ICommand AddActivityTypeCommand { get; }

        public Interaction<ActivityTypeWindowViewModel, ActivityType?> ShowDialog { get; }
    }
}
