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
            if ( !((string)value).Equals("All",StringComparison.OrdinalIgnoreCase) )
            {
                Uri uri = new Uri($"pack://application:,,,/RReplay.Ressources;component/Resources/Icons/Deck/{(string)value}.png", UriKind.Absolute);
                return uri;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
