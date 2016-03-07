using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class ClassNameDebugToImageSource : IValueConverter
    {
        /// <summary>
        /// Convert the ClassNameDebug picked from the game ressource to a path to the unit Image
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return new Uri($"pack://application:,,,/RReplay.Ressources;component/Resources/Icons/Units/{(string)value}.jpg", UriKind.Absolute); ;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
