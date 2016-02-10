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
                    return "Confrontation";
                case 1:
                    return "NATO";
                case 2:
                    return "PACT";
                default:
                    return "Unkown";

            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ( (string)value )
            {
                case "Confrontation":
                    return 0;
                case "NATO":
                    return 1;
                case "PACT":
                    return 2;
                default:
                    return -1;

            }
        }
    }
}
