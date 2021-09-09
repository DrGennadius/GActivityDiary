using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GActivityDiary.Views
{
    public partial class CreateActivityView : UserControl
    {
        public CreateActivityView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
