using GalaSoft.MvvmLight.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using RReplay.Model;
using RReplay.Properties;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace RReplay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            Setup();
            DispatcherHelper.Initialize();
        }

        private static void Setup()
        {
            if (Settings.Default.firstRun)
            {
                Settings.Default.firstRun = false;
                Settings.Default.replaysFolder = ReplayRepository.GetDefaultReplayGamesFolder();
            }
        }
    }
}
