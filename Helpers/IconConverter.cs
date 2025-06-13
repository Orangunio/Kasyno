using FontAwesome.WPF;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Kasyno.Helpers
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string iconName && Enum.TryParse(typeof(FontAwesomeIcon), iconName, out var result))
            {
                return (FontAwesomeIcon)result;
            }
            return FontAwesomeIcon.Question;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
