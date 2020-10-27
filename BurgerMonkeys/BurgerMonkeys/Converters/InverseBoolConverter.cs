using System;
using System.Globalization;
using Xamarin.Forms;
using BurgerMonkeys.Tools;

namespace BurgerMonkeys.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value.ToBoolean();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
