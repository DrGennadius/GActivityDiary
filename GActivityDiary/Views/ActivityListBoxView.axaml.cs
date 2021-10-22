using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class ActivityListBoxView : UserControl
    {
        public ActivityListBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
