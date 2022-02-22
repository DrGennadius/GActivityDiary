using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GActivityDiary.GUI.Avalonia.ViewModels;
using ReactiveUI;
using System;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class ActivityTypeWindow : ReactiveWindow<ActivityTypeWindowViewModel>
    {
        public ActivityTypeWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(d => d(ViewModel!.SaveActivityTypeCmd.Subscribe(Close)));
            this.WhenActivated(d => d(ViewModel!.CancelCmd.Subscribe(Close)));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
