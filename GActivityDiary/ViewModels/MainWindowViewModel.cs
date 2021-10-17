using ReactiveUI;
using System.Reactive;

namespace GActivityDiary.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _selectedViewMode = "All";
        private ActivityListBoxViewModelBase? _activityListBoxViewModel;

        public MainWindowViewModel()
        {
            ViewModes = new string[]
            {
                "Day",
                "All"
            };

            ToogleViewModeCmd = ReactiveCommand.Create<string>((param) => ToogleViewMode(param));

            SelectedViewMode = ViewModes[0];
        }

        public ReactiveCommand<string, Unit> ToogleViewModeCmd { get; }

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
                        ActivityListBoxViewModel = new DayActivityListBoxViewModel();
                        break;
                    case "All":
                        ActivityListBoxViewModel = new ActivityListBoxViewModel();
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
    }
}
