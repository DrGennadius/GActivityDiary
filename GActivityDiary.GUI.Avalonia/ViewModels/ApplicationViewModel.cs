using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ApplicationViewModel()
        {
            ExitCommand = ReactiveCommand.Create(() =>
            {
                if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                {
                    lifetime.Shutdown();
                }
            });

            ToggleCommand = ReactiveCommand.Create(() =>
            {
            });

            ShowCommand = ReactiveCommand.Create(() =>
            {
                if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                {
                    lifetime.MainWindow.WindowState = WindowState.Normal;
                    lifetime.MainWindow.ShowInTaskbar = true;
                }
            });
        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ReactiveCommand<Unit, Unit> ToggleCommand { get; }

        public ReactiveCommand<Unit, Unit> ShowCommand { get; }
    }
}
