using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GActivityDiary.ViewModels;
using GActivityDiary.Views;

namespace GActivityDiary
{
    public class App : Application
    {
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
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                desktop.MainWindow.Closing += MainWindow_Closing;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var window = sender as MainWindow;
            window!.WindowState = WindowState.Minimized;
            window!.ShowInTaskbar = false;
            e.Cancel = true;
        }
    }
}
