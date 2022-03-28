using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Reports;

namespace GActivityDiary.GUI.Avalonia.Views
{
    public partial class SimpleTextReporterWindow : Window
    {
        public SimpleTextReporterWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            var comboBox = this.Find<ComboBox>("ReportGroupingTypeDropDown");
            if (comboBox != null)
            {
                comboBox.Items = EnumHelper.GetAllValues(typeof(ReportGroupingType));
                comboBox.SelectedItem = ReportGroupingType.Nothing;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
