using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GActivityDiary.Core.Reports;
using System.Collections.Generic;

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
            comboBox.Items = new List<ReportGroupingType>()
            {
                ReportGroupingType.Nothing,
                ReportGroupingType.Day
            };
            comboBox.SelectedItem = ReportGroupingType.Nothing;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
