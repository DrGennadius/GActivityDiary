using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GActivityDiary.Core.DataBase;
using GActivityDiary.GUI.Avalonia.ViewModels;
using GActivityDiary.GUI.Avalonia.Views;
using System;

namespace GActivityDiary.GUI.Avalonia
{
    public class App : Application
    {
        private WindowState? _storedWindowState;

        public App()
        {
            DataContext = new ApplicationViewModel();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var dataGridType = typeof(DataGrid); // HACK
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DbContext db = new();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(db),
                };
                desktop.MainWindow.Closing += MainWindow_Closing;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is MainWindow window && window.WindowState != WindowState.Minimized)
            {
                _storedWindowState = window!.WindowState;
                window!.WindowState = WindowState.Minimized;
                window!.ShowInTaskbar = false;
                e.Cancel = true;
            }
        }

        private void TrayIconOnClicked(object? sender, EventArgs e)
        {
            if (!_storedWindowState.HasValue)
            {
                return;
            }
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow.WindowState != _storedWindowState.Value)
            {
                desktop.MainWindow.WindowState = _storedWindowState.Value;
                desktop.MainWindow.ShowInTaskbar = true;
            }
        }
    }
}
