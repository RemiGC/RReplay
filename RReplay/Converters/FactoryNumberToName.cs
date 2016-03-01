using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class FactoryNumberToName : IValueConverter
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
            return name;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            int factory = 0;
            switch ( (string)value )
            {
                case "Logistic":
                    factory = 3;
                    break;
                case "Infantry":
                    factory = 6;
                    break;
                case "Plane":
                    factory = 7;
                    break;
                case "Vehicle":
                    factory = 8;
                    break;
                case "Tank":
                    factory = 9;
                    break;
                case "Recon":
                    factory = 10;
                    break;
                case "Helo":
                    factory = 11;
                    break;
                case "Naval":
                    factory = 12;
                    break;
                case "Support":
                    factory = 13;
                    break;
            }
            return factory;
        }
    }
}
