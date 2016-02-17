using RReplay.Properties;
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
            string specializationFlagPath = null;

            if ( (string)value != "All" )
            {
                string path = Path.Combine(Settings.Default.exeFolder, "Icons", "Deck", (string)value + ".png");

                if ( File.Exists(path) )
                {
                    specializationFlagPath = path;
                }
            }

            return specializationFlagPath;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
