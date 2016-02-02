using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class GameModeToString : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ((int)value)
            {
                case 1:
                    return "Destruction";
                case 2:
                    return "Conquest2";
                case 3:
                    return "Conquest3";
                case 4:
                    return "Conquest4";
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
                case "Conquest2":
                    return 2;
                case "Conquest3":
                    return 3;
                case "Conquest4":
                    return 4;
                default:
                    return 0;

            }
        }
    }
}
