using RReplay.Properties;
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
            string CountryFlagPath = Path.Combine(Settings.Default.exeFolder, "Icons", "Flags", (string)value + "_flag.png");

            if ( !File.Exists(CountryFlagPath) )
            {
                CountryFlagPath = Path.Combine(Settings.Default.exeFolder, "Icons", "Flags", "neut_flag.png");
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
