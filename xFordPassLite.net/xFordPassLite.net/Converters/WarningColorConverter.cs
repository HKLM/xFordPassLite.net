using System;
using System.Globalization;

using Xamarin.CommunityToolkit.Extensions.Internals;

using Xamarin.Forms;

namespace xFordPassLite.net.Converters
{
    public class WarningColorConverter : ValueConverterExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string && (string)value != null)
            {
                switch (value)
                {
                    case "UNLOCKED":
                    case "NOTSET":
                    case "Ajar":
                    case "BetFully_10PercentOpen":
                        return Color.Crimson;
                    case "LOCKED":
                    case "SET":
                    case "ACTIVE":
                        return Color.RoyalBlue;
                    default:
                        break;
                }
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}