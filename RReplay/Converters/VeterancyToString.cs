using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class VeterancyToString : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string name = "";
            switch ( (byte)value )
            {
                case 0:
                    name = "Rookie";
                    break;
                case 1:
                    name = "Trained";
                    break;
                case 2:
                    name = "Hardened";
                    break;
                case 3:
                    name = "Veteran";
                    break;
                case 4:
                    name = "Elite";
                    break;
            }
            return name;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
