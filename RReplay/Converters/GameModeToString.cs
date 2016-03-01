using System;
using System.Globalization;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class GameModeToString : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            switch ((int)value)
            {
                case 0:
                    return "Skirmish";
                case 1:
                    return "Multi";
                case 2:
                    return "Unknown2";
                case 3:
                    return "Ranked";
                default:
                    return "Unkown";

            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
