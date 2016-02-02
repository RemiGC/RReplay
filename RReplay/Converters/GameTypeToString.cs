using System;
using System.Globalization;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class GameTypeToString : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ((int)value)
            {
                case 0:
                    return "Destruction";
                case 1:
                    return "Conquest";
                case 2:
                    return "Economy";
                default:
                    return "Unkown";

            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ( (string)value )
            {
                case "Destruction":
                    return 0;
                case "Conquest":
                    return 1;
                case "Economy":
                    return 2;
                default:
                    return -1;

            }
        }
    }
}
