using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class ActivityView : UserControl
    {
        public ActivityView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
