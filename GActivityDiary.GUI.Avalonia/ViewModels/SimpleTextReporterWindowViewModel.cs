using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using GActivityDiary.Core.Reports;
using GActivityDiary.Core.Reports.Text;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class SimpleTextReporterWindowViewModel : ViewModelBase
    {
        private ReportGroupingType _reportGroupingType = ReportGroupingType.Nothing;
        private string _reportText = "";
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public SimpleTextReporterWindowViewModel(DbContext dbContext)
        {
            DbContext = dbContext;

            ActivityTypes = dbContext.ActivityTypes.GetAll();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

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

        public ActivityType? SelectedActivityType { get; set; }

        public IEnumerable<ActivityType> ActivityTypes { get; set; }

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
            if (_startDate.HasValue && EndDate.HasValue)
            {
                DateTime startDate = _startDate.Value.Date;
                DateTime endDate = _endDate.Value.Date.AddDays(1);
                if (SelectedActivityType != null)
                {
                    var activities = DbContext.Activities.Find(x => x.ActivityType != null
                                                                    && x.ActivityType.Id == SelectedActivityType.Id
                                                                    && x.StartAt >= startDate
                                                                    && x.EndAt < endDate);
                    ReportText = simpleTextReporter.GetReport(activities, startDate, endDate);
                }
                else
                {
                    ReportText = simpleTextReporter.GetReport(startDate, endDate);
                }
            }
            else
            {
                ReportText = "Dont selected period";
            }
            DateTimeInterval dateTimeInterval = new(_startDate.Value.Date, _endDate.Value.Date.AddDays(1).AddTicks(-1));
            
        }
    }
}
