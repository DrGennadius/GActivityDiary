using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class TimeConverterWindow : Window
    {
        public TimeConverterWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
