using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Kasyno.Helpers
{
    public class SuitToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string suit = value as string;

            return suit switch
            {
                "Hearts" => Brushes.Red,
                "Diamonds" => Brushes.Red,
                "Clubs" => Brushes.Black,
                "Spades" => Brushes.Black,
                _ => Brushes.Gray,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
