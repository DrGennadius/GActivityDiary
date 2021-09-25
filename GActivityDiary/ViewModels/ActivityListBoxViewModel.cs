using Avalonia.Controls.Selection;
using GActivityDiary.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GActivityDiary.ViewModels
{
    public class ActivityListBoxViewModel : ViewModelBase
    {
        private ViewModelBase? _singleActivityContent = null;

        private ObservableCollection<Activity> _activities = new();

        private Activity? _selectedActivity = null;

        private CancellationTokenSource _tokenSource = new();

        private Task _loadActivitiesTask;

        private readonly int[] _pageSizes = { 5, 10, 15, 30, 50, 100 };

        private bool _isShowCreateActivityButton = false;
        private bool _isCollectionEmpty = false;
        private bool _isProgressBarIndeterminate;
        private bool _isProgressBarEnable;

        private int _collectionCount;
        private int _pageSize;
        private int _pageCount;
        private int _pageNumber = 1;

        public ActivityListBoxViewModel()
        {
            CreateActivityCmd = ReactiveCommand.Create(() => CreateActivity());
            GoToFirstPageCmd = ReactiveCommand.Create(() => GoToFirstPage());
            GoToLastPageCmd = ReactiveCommand.Create(() => GoToLastPage());

            SingleActivityContent = new CreateActivityViewModel(this);
            Selection = new SelectionModel<Activity>();
            Selection.SelectionChanged += SelectionChanged;

            this.WhenAnyValue(x => x.Activities.Count)
                .Subscribe(x => IsCollectionEmpty = x == 0);

            _pageSize = 15;
            GoToLastPageForce();
        }

        public ReactiveCommand<Unit, Unit> CreateActivityCmd { get; }

        public ReactiveCommand<Unit, Unit> GoToFirstPageCmd { get; }

        public ReactiveCommand<Unit, Unit> GoToLastPageCmd { get; }

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

        public bool IsCollectionEmpty
        {
            get => _isCollectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _isCollectionEmpty, value);
        }

        public bool IsProgressBarIndeterminate
        {
            get => _isProgressBarIndeterminate;
            set => this.RaiseAndSetIfChanged(ref _isProgressBarIndeterminate, value);
        }

        public bool IsProgressBarEnable
        {
            get => _isProgressBarEnable;
            set => this.RaiseAndSetIfChanged(ref _isProgressBarEnable, value);
        }

        public int CollectionCount
        {
            get => _collectionCount;
            set => this.RaiseAndSetIfChanged(ref _collectionCount, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _pageSize, value);
                GetActivities();
            }
        }

        public int PageCount
        {
            get => _pageCount;
            set => this.RaiseAndSetIfChanged(ref _pageCount, value);
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                this.RaiseAndSetIfChanged(ref _pageNumber, value);
                GetActivities();
            }
        }

        public List<int> PageSizeOptions => _pageSizes.ToList();

        private void CreateActivity()
        {
            SingleActivityContent = new CreateActivityViewModel(this);
        }

        public void GetActivities()
        {
            // TODO: Solve this sh*t.
            if (_loadActivitiesTask != null && !_loadActivitiesTask.IsCompleted)
            {
                _tokenSource.Cancel();
                _loadActivitiesTask.Wait();
                _tokenSource = new();
            }
            _loadActivitiesTask = Task.Run(() => GetActivitiesAsync(PageNumber - 1, PageSize), _tokenSource.Token);
        }

        public void GoToLastPageForce()
        {
            CollectionCount = DB.Instance.Activities.Query().Count();
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
            PageNumber = PageCount;
        }

        private void GoToFirstPage()
        {
            if (PageNumber != 1)
            {
                PageNumber = 1;
            }
        }

        private void GoToLastPage()
        {
            CollectionCount = DB.Instance.Activities.Query().Count();
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
            if (PageNumber != PageCount)
            {
                PageNumber = PageCount;
            }
        }

        private async void GetActivitiesAsync(int pageIndex, int pageSize)
        {
            IsProgressBarEnable = true;
            CollectionCount = DB.Instance.Activities.Query().Count();
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
            IsProgressBarIndeterminate = true;
            Activities = new ObservableCollection<Activity>(await DB.Instance.Activities.GetAllAsync(pageIndex, pageSize));
            IsProgressBarIndeterminate = false;
            IsProgressBarEnable = false;
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
