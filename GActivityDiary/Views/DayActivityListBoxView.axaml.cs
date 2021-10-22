using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class DayActivityListBoxView : UserControl
    {
        public DayActivityListBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
