using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class SpecializationCodeToImageSource: IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string exe = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string specializationFlagPath = Path.Combine(exe, "Icons", "Flags", (string)value + "_flag.png");

            if ( (string)value != "All" )
            {
                string path = Path.Combine(exe, "Icons", "Deck", (string)value + ".png");

                if ( File.Exists(path) )
                {
                    specializationFlagPath = path;
                }
            }

            if ( !File.Exists(specializationFlagPath) )
            {
                specializationFlagPath = null;
            }

            return specializationFlagPath;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
