using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace GActivityDiary.Converters
{
    public class StringRadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)parameter == (string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : BindingOperations.DoNothing;
        }
    }
}
