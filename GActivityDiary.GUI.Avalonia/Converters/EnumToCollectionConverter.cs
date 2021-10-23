using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using GActivityDiary.Core.Helpers;
using System;
using System.Globalization;
using System.Linq;

namespace GActivityDiary.GUI.Avalonia.Converters
{
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetAllValuesAndDescriptions(value.GetType())
                             .Select(x => x.Description);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
