using Avalonia.Controls.Selection;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.ViewModels
{
    public class ActivityListBoxViewModel : ViewModelBase
    {
        private ViewModelBase? _singleActivityContent = null;

        private ObservableCollection<Activity> _activities = new();

        private Activity? _selectedActivity = null;

        private bool _isShowCreateActivityButton = false;

        public ActivityListBoxViewModel()
        {
            Activities = new ObservableCollection<Activity>(DB.Instance.Activities.GetAll());
            CreateActivityCmd = ReactiveCommand.Create(() => CreateActivity());
            SingleActivityContent = new CreateActivityViewModel(this);
            Selection = new SelectionModel<Activity>();
            Selection.SelectionChanged += SelectionChanged;
        }

        public ReactiveCommand<Unit, Unit> CreateActivityCmd { get; }

        public SelectionModel<Activity> Selection { get; }

        public ViewModelBase? SingleActivityContent
        {
            get => _singleActivityContent;
            set
            {
                IsShowCreateActivityButton = !(value is CreateActivityViewModel);
                this.RaiseAndSetIfChanged(ref _singleActivityContent, value);
            }
        }

        public ObservableCollection<Activity> Activities
        {
            get => _activities;
            set => this.RaiseAndSetIfChanged(ref _activities, value);
        }

        public Activity? SelectedActivity
        {
            get => _selectedActivity;
            set => this.RaiseAndSetIfChanged(ref _selectedActivity, value);
        }

        public bool IsShowCreateActivityButton 
        { 
            get => _isShowCreateActivityButton; 
            set => this.RaiseAndSetIfChanged(ref _isShowCreateActivityButton, value); 
        }

        public void CreateActivity()
        {
            SingleActivityContent = new CreateActivityViewModel(this);
        }

        private void SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<Activity> e)
        {
            var selectionModel = sender as SelectionModel<Activity>;
            var activity = selectionModel?.SelectedItem;
            if (activity != null)
            {
                SingleActivityContent = new EditActivityViewModel(this, activity);
            }
        }
    }
}
