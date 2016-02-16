using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class CountryCodeToImageSource : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string exe = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string CountryFlagPath = Path.Combine(exe, "Icons", "Flags", (string)value + "_flag.png");

            if ( !File.Exists(CountryFlagPath) )
            {
                CountryFlagPath = Path.Combine(exe, "Icons", "Flags", "neut_flag.png");
            }

            if ( !File.Exists(CountryFlagPath) )
            {
                CountryFlagPath = null;
            }

            return CountryFlagPath;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
