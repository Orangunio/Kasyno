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
            if (value is string iconName)
            {
                if (Enum.TryParse(typeof(FontAwesomeIcon), iconName, true, out var iconObj) && iconObj is FontAwesomeIcon icon)
                {
                    return icon;
                }
                return FontAwesomeIcon.QuestionCircle;
            }
            return FontAwesomeIcon.QuestionCircle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
