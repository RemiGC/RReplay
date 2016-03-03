using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class FactoryNumberToImageSource : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string name = "";
            switch ( (int)value )
            {
                case 3:
                    name = "Logistic";
                    break;
                case 6:
                    name = "Infantry";
                    break;
                case 7:
                    name = "Plane";
                    break;
                case 8:
                    name = "Vehicle";
                    break;
                case 9:
                    name = "Tank";
                    break;
                case 10:
                    name = "Recon";
                    break;
                case 11:
                    name = "Helo";
                    break;
                case 12:
                    name = "Naval";
                    break;
                case 13:
                    name = "Support";
                    break;
            }

            string factoryImagePath = Path.Combine(Settings.Default.exeFolder, "Icons", "Hud", name + ".png");

            if ( !File.Exists(factoryImagePath) )
            {
                return null;
            }
            else
            {
                return factoryImagePath;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}