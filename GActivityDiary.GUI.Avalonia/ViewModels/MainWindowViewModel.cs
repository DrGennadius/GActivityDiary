using GActivityDiary.Core.DataBase;
using GActivityDiary.GUI.Avalonia.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _selectedViewMode = "All";
        private ActivityListBoxViewModelBase? _activityListBoxViewModel;

        public MainWindowViewModel(DbContext db)
        {
            Db = db;

            ViewModes = new string[]
            {
                "Day",
                "All"
            };

            ToogleViewModeCmd = ReactiveCommand.Create<string>((param) => ToogleViewMode(param));
            OpenTimeConverterWindowCmd = ReactiveCommand.Create(() => OpenTimeConverterWindow());
            OpenSimpleTextReporterWindowCmd = ReactiveCommand.Create(() => OpenSimpleTextReporterWindow());
            OpenActivityTypesWindowCmd = ReactiveCommand.Create(() => OpenActivityTypesWindow());

            SelectedViewMode = ViewModes[0];
        }

        // Database context.
        public DbContext Db { get; private set; }

        public ReactiveCommand<string, Unit> ToogleViewModeCmd { get; }

        public ReactiveCommand<Unit, Unit> OpenTimeConverterWindowCmd { get; }

        public ReactiveCommand<Unit, Unit> OpenSimpleTextReporterWindowCmd { get; }

        public ReactiveCommand<Unit, Unit> OpenActivityTypesWindowCmd { get; }

        public string[] ViewModes { get; }

        public string SelectedViewMode
        {
            get => _selectedViewMode;
            set
            {
                ActivityListBoxViewModel?.Stop();
                switch (value)
                {
                    case "Day":
                        ActivityListBoxViewModel = new DayActivityListBoxViewModel(Db);
                        break;
                    case "All":
                        ActivityListBoxViewModel = new ActivityListBoxViewModel(Db);
                        break;
                    default:
                        break;
                }
                this.RaiseAndSetIfChanged(ref _selectedViewMode, value);
            }
        }

        public ActivityListBoxViewModelBase? ActivityListBoxViewModel
        {
            get => _activityListBoxViewModel;
            set => this.RaiseAndSetIfChanged(ref _activityListBoxViewModel, value);
        }

        private void ToogleViewMode(string viewModeName)
        {
            SelectedViewMode = viewModeName;
        }

        private void OpenTimeConverterWindow()
        {
            TimeConverterWindow timeConverterWindow = new()
            {
                DataContext = new TimeConverterWindowViewModel()
            };
            timeConverterWindow.Show();
        }

        private void OpenSimpleTextReporterWindow()
        {
            SimpleTextReporterWindow simpleTextReporterWindow = new()
            {
                DataContext = new SimpleTextReporterWindowViewModel(Db)
            };
            simpleTextReporterWindow.Show();
        }

        private void OpenActivityTypesWindow()
        {
            ActivityTypeTableWindow activityTypeTableWindow = new()
            {
                DataContext = new ActivityTypeTableWindowViewModel(Db)
            };
            activityTypeTableWindow.Show();
        }
    }
}
