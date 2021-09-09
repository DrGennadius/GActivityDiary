namespace GActivityDiary.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ActivityListBoxViewModel ActivityListBoxViewModel { get; set; } = new ActivityListBoxViewModel();
    }
}
