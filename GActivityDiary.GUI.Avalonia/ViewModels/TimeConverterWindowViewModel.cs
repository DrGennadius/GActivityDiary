using GActivityDiary.Core.Converters.Time;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.GUI.Avalonia.ViewModels
{
    public class TimeConverterWindowViewModel : ViewModelBase
    {
        private double? _hours1;
        private double? _minutes1;
        private double? _totalHours1;
        private double? _totalMinutes1;

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
            set
            {
                this.RaiseAndSetIfChanged(ref _totalHours1, value);
            }
        }

        public double? TotalMinutes1
        {
            get => _totalMinutes1;
            set
            {
                this.RaiseAndSetIfChanged(ref _totalMinutes1, value);
            }
        }

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
    }
}
