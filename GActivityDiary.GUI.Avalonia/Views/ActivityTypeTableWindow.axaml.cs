using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GActivityDiary.Core.Models;
using GActivityDiary.GUI.Avalonia.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class ActivityTypeTableWindow : ReactiveWindow<ActivityTypeTableWindowViewModel>
    {
        public ActivityTypeTableWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<ActivityTypeWindowViewModel, ActivityType?> interaction)
        {
            var dialog = new ActivityTypeWindow
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<ActivityType?>(this);
            interaction.SetOutput(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
