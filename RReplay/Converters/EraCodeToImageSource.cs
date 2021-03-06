﻿using RReplay.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace RReplay.Converters
{
    public class EraCodeToImageSource : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string eraPath = "";

            if ( (string)value != "All" )
            {
                string path = Path.Combine(Settings.Default.exeFolder, "Icons", "Deck", (string)value + ".png");

                if ( File.Exists(path) )
                {
                    eraPath = path;
                }
            }

            if ( !File.Exists(eraPath) )
            {
                eraPath = null;
            }

            return eraPath;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
