using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.Views
{
    public partial class EditActivityView : UserControl
    {
        public EditActivityView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
