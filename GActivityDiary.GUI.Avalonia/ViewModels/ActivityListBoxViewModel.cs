using Avalonia.Controls.Selection;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NHibernate.Criterion;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class ActivityListBoxViewModel : ActivityListBoxViewModelBase
    {
        private readonly int[] _pageSizes = { 5, 10, 15, 30, 50, 100 };

        private bool _isProgressBarIndeterminate;
        private bool _isProgressBarEnable;

        private int _collectionCount;
        private int _pageSize;
        private int _pageCount;
        private int _pageNumber = 1;

        private string _tagsSearchText = "";
        private ActivityType _selectedActivityType;

        public ActivityListBoxViewModel(DbContext db)
            : base(db)
        {
            ActivityTypes = db.ActivityTypes.GetAll();

            GoToFirstPageCmd = ReactiveCommand.Create(() => GoToFirstPage());
            GoToLastPageCmd = ReactiveCommand.Create(() => GoToLastPage());

            _pageSize = 15;
            GoToLastPageForce();
        }

        public ReactiveCommand<Unit, Unit> GoToFirstPageCmd { get; }

        public ReactiveCommand<Unit, Unit> GoToLastPageCmd { get; }

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
                Update();
                this.RaiseAndSetIfChanged(ref _pageSize, value);
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
                Update();
            }
        }

        public string TagsSearchText
        {
            get => _tagsSearchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _tagsSearchText, value);
                Update();
            }
        }

        public List<int> PageSizeOptions => _pageSizes.ToList();

        public ActivityType SelectedActivityType 
        { 
            get => _selectedActivityType; 
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedActivityType, value);
                Update();
            }
        }

        public IEnumerable<ActivityType> ActivityTypes { get; set; }

        public override void Update(Guid? targetActivityId = null)
        {
            Stop();
            _tokenSource = new();
            _updateTask = Task.Run(() => UpdateAsync(targetActivityId), _tokenSource.Token);
        }

        public void GoToLastPageForce()
        {
            Task.Run(GoToLastPageForceAsync);
        }

        public async Task GoToLastPageForceAsync()
        {
            await CalcPageCountAsync();
            PageNumber = PageCount;
        }

        private async Task CalcPageCountAsync()
        {
            CollectionCount = await ActivityHelper.GetCountByTagsAsync(DbContext, _tagsSearchText);
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
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
            Task.Run(GoToLastPageAsync);
        }

        private async Task GoToLastPageAsync()
        {
            await CalcPageCountAsync();
            if (PageNumber != PageCount)
            {
                PageNumber = PageCount;
            }
        }

        private async void UpdateAsync(Guid? targetActivityId = null)
        {
            IsProgressBarEnable = true;
            await CalcPageCountAsync();
            IsProgressBarIndeterminate = true;
            UpdateActivities();
            if (targetActivityId.HasValue)
            {
                var targetActivty = DbContext.Activities.GetById(targetActivityId.Value);
                SelectedActivity = targetActivty;
            }
            IsProgressBarIndeterminate = false;
            IsProgressBarEnable = false;
        }

        private async void UpdateActivities()
        {
            Activities.Clear();
            var activitiesCriteria = DbContext.Session.CreateCriteria<Activity>();
            if (!string.IsNullOrWhiteSpace(_tagsSearchText))
            {
                var tagIds = await TagHelper.GetTagIdsAsync(DbContext, _tagsSearchText);
                activitiesCriteria.CreateCriteria("Tags")
                                  .Add(Restrictions.In("Id", tagIds.ToArray()));
            }
            if (_selectedActivityType != null)
            {
                activitiesCriteria.CreateCriteria("ActivityType")
                                  .Add(Restrictions.Eq("Id", _selectedActivityType.Id));
            }
            var activities = await activitiesCriteria.SetFirstResult((_pageNumber - 1) * _pageSize)
                                                     .SetMaxResults(_pageSize)
                                                     .ListAsync<Activity>();
            Activities = new ObservableCollection<Activity>(activities);
        }

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
