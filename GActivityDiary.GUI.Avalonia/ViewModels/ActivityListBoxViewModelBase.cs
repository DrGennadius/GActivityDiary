using Avalonia.Controls.Selection;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public abstract class ActivityListBoxViewModelBase : ViewModelBase
    {
        private bool _isShowCreateActivityButton = false;
        private bool _isCollectionEmpty = false;

        private ViewModelBase? _singleActivityContent = null;
        private ObservableCollection<Activity> _activities = new();
        private Activity? _selectedActivity = null;
        protected CancellationTokenSource _tokenSource = new();
        protected Task? _updateTask = null;

        public ActivityListBoxViewModelBase(DbContext db)
        {
            DbContext = db;

            CreateActivityCmd = ReactiveCommand.Create(() => CreateActivity());
            EditActivityCmd = ReactiveCommand.Create<Activity>(x => EditActivity(x));

            SingleActivityContent = new CreateActivityViewModel(db, this);

            Selection = new SelectionModel<Activity>();
            Selection.SelectionChanged += SelectionChanged;

            this.WhenAnyValue(x => x.Activities.Count)
                .Subscribe(x => IsCollectionEmpty = x == 0);
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        public ReactiveCommand<Unit, Unit> CreateActivityCmd { get; }

        public ReactiveCommand<Activity, Unit> EditActivityCmd { get; }

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

        public abstract void Update(Guid? uid = null);

        public void Stop()
        {
            // TODO: Solve this.
            if (_updateTask != null && !_updateTask.IsCompleted)
            {
                _tokenSource.Cancel();
                _updateTask.Wait();
            }
        }

        public bool IsCollectionEmpty
        {
            get => _isCollectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _isCollectionEmpty, value);
        }

        public void ViewActivity(Activity activity)
        {
            SingleActivityContent = new ActivityViewModel(this, activity);
        }

        public void EditActivity(Activity activity)
        {
            SingleActivityContent = new EditActivityViewModel(DbContext, this, activity);
        }

        public void CreateActivity()
        {
            SingleActivityContent = new CreateActivityViewModel(DbContext, this);
        }

        public SelectionModel<Activity> Selection { get; }

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
