using GActivityDiary.Core.Converters.Time;
using ReactiveUI;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class TimeConverterWindowViewModel : ViewModelBase
    {
        private double? _hours1;
        private double? _minutes1;
        private double? _totalHours1;
        private double? _totalMinutes1;

        private double? _totalHours2;
        private double? _hours2;
        private double? _minutes2;

        private double? _totalMinutes3;
        private double? _hours3;
        private double? _minutes3;

        #region Hours + Minutes to total Hours and total Minutes
        public double? Hours1
        {
            get => _hours1;
            set
            {
                this.RaiseAndSetIfChanged(ref _hours1, value);
                Convert1();
            }
        }

        public double? Minutes1
        {
            get => _minutes1;
            set
            {
                this.RaiseAndSetIfChanged(ref _minutes1, value);
                Convert1();
            }
        }

        public double? TotalHours1
        {
            get => _totalHours1;
            set => this.RaiseAndSetIfChanged(ref _totalHours1, value);
        }

        public double? TotalMinutes1
        {
            get => _totalMinutes1;
            set => this.RaiseAndSetIfChanged(ref _totalMinutes1, value);
        }
        #endregion

        #region Total Hours to Hours + Minutes
        public double? TotalHours2
        {
            get => _totalHours2;
            set
            {
                this.RaiseAndSetIfChanged(ref _totalHours2, value);
                Convert2();
            }
        }

        public double? Hours2
        {
            get => _hours2;
            set => this.RaiseAndSetIfChanged(ref _hours2, value);
        }

        public double? Minutes2
        {
            get => _minutes2;
            set => this.RaiseAndSetIfChanged(ref _minutes2, value);
        }
        #endregion

        #region Total Minutes to Hours + Minutes
        public double? TotalMinutes3
        {
            get => _totalMinutes3;
            set
            {
                this.RaiseAndSetIfChanged(ref _totalMinutes3, value);
                Convert3();
            }
        }

        public double? Hours3
        {
            get => _hours3;
            set => this.RaiseAndSetIfChanged(ref _hours3, value);
        }

        public double? Minutes3
        {
            get => _minutes3;
            set => this.RaiseAndSetIfChanged(ref _minutes3, value);
        }
        #endregion

        /// <summary>
        /// Hours + Minutes to total Hours and total Minutes.
        /// </summary>
        private void Convert1()
        {
            if (!_hours1.HasValue && !_minutes1.HasValue)
            {
                TotalHours1 = null;
                TotalMinutes1 = null;
            }
            double hours = _hours1 ?? 0;
            double minutes = _minutes1 ?? 0;
            double totalHours = TimeConverter.GetHours(hours, minutes);
            double totalMinutes = TimeConverter.GetMinutes(hours, minutes);
            TotalHours1 = totalHours > 0 ? totalHours : null;
            TotalMinutes1 = totalMinutes > 0 ? totalMinutes : null;
        }

        /// <summary>
        /// Total Hours to Hours + Minutes.
        /// </summary>
        private void Convert2()
        {
            if (!_totalHours2.HasValue)
            {
                Hours2 = null;
                Minutes2 = null;
            }
            double totalHours = _totalHours2!.Value;
            var (hours, minutes) = TimeConverter.GetHoursAndMinutesFromHours(totalHours);
            Hours2 = hours;
            Minutes2 = minutes;
        }

        /// <summary>
        /// Total Minutes to Hours + Minutes.
        /// </summary>
        private void Convert3()
        {
            if (!_totalMinutes3.HasValue)
            {
                Hours3 = null;
                Minutes3 = null;
            }
            double totalMinutes = _totalMinutes3!.Value;
            var (hours, minutes) = TimeConverter.GetHoursAndMinutes(totalMinutes);
            Hours3 = hours;
            Minutes3 = minutes;
        }
    }
}
