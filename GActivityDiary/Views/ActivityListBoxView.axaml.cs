using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.Views
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
