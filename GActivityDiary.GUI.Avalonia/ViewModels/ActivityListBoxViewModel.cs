using Avalonia.Controls.Selection;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
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

        public ActivityListBoxViewModel(DbContext db)
            : base(db)
        {
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

        public List<int> PageSizeOptions => _pageSizes.ToList();

        public override void Update(Guid? targetActivityId = null)
        {
            _tokenSource = new();
            _updateTask = Task.Run(() => UpdateAsync(PageNumber - 1, PageSize, targetActivityId), _tokenSource.Token);
        }

        public void GoToLastPageForce()
        {
            CollectionCount = Db.Activities.Query().Count();
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
            CollectionCount = Db.Activities.Query().Count();
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
            if (PageNumber != PageCount)
            {
                PageNumber = PageCount;
            }
        }

        private async void UpdateAsync(int pageIndex, int pageSize, Guid? targetActivityId = null)
        {
            IsProgressBarEnable = true;
            CollectionCount = Db.Activities.Query().Count();
            PageCount = (CollectionCount + PageSize - 1) / PageSize;
            IsProgressBarIndeterminate = true;
            Activities.Clear();
            var activities = await Db.Activities.GetAllAsync(pageIndex, pageSize);
            Activities = new ObservableCollection<Activity>(activities);
            if (targetActivityId.HasValue)
            {
                var targetActivty = Db.Activities.GetById(targetActivityId.Value);
                SelectedActivity = targetActivty;
            }
            IsProgressBarIndeterminate = false;
            IsProgressBarEnable = false;
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
