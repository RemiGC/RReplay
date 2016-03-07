using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class VeterancyToImageSource : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string name = "";
            switch((byte)value)
            {
                case 0:
                    name = "iconrankrookietired.png";
                    break;
                case 1:
                    name = "iconranktrainedtired.png";
                    break;
                case 2:
                    name = "iconrankhardenedtired.png";
                    break;
                case 3:
                    name = "iconrankveterantired.png";
                    break;
                case 4:
                    name = "iconrankelitetired.png";
                    break;
            }

            return new Uri($"pack://application:,,,/RReplay.Ressources;component/Resources/Icons/Rank/{name}", UriKind.Absolute); ;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
