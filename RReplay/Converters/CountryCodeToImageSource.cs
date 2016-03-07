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
            Uri uri = new Uri($"pack://application:,,,/RReplay.Ressources;component/Resources/Icons/Flags/{(string)value}_flag.png", UriKind.Absolute);
            return uri;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
