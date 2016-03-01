using System;
using System.Globalization;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class VictoryToString : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ( (int)value )
            {
                case 1:
                    return "Destruction";
                case 2:
                    return "Siege";
                case 3:
                    return "Economy";
                case 4:
                    return "Conquest";
                default:
                    return "Unkown";

            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ( (string)value )
            {
                case "Destruction":
                    return 1;
                case "Siege":
                    return 2;
                case "Economy":
                    return 3;
                case "Conquest":
                    return 4;
                default:
                    return 0;

            }
        }
    }
}
