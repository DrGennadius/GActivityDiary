using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Reports;
using GActivityDiary.Core.Reports.Text;
using ReactiveUI;
using System;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class SimpleTextReporterWindowViewModel : ViewModelBase
    {
        private ReportGroupingType _reportGroupingType;
        private string _reportText = "";
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public SimpleTextReporterWindowViewModel(DbContext dbContext)
        {
            DbContext = dbContext;

            GenerateCmd = ReactiveCommand.Create(() => Generate());
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        public ReactiveCommand<Unit, Unit> GenerateCmd { get; }

        public ReportGroupingType ReportGroupingType
        {
            get => _reportGroupingType;
            set => this.RaiseAndSetIfChanged(ref _reportGroupingType, value);
        }

        public string ReportText
        {
            get => _reportText;
            set => this.RaiseAndSetIfChanged(ref _reportText, value);
        }

        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => this.RaiseAndSetIfChanged(ref _startDate, value);
        }

        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => this.RaiseAndSetIfChanged(ref _endDate, value);
        }

        private void Generate()
        {
            if (!_startDate.HasValue || !_endDate.HasValue)
            {
                ReportText = "";
                return;
            }
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(DbContext, languageProfile);
            simpleTextReporter.GroupingType = _reportGroupingType;
            DateTimeInterval dateTimeInterval = new(_startDate.Value.Date, _endDate.Value.Date.AddDays(1).AddTicks(-1));
            ReportText = simpleTextReporter.GetReport(dateTimeInterval);
        }
    }
}
