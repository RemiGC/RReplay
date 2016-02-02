using GalaSoft.MvvmLight.Threading;
using RReplay.Model;
using RReplay.Properties;
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
            if(!ReplayRepository.ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
            {
                if(!ReplayRepository.GetNewReplayFolder(Settings.Default.replaysFolder))
                {
                    //TODO Find how to force shutdown of the apps.
                }
            }


        }
    }
}
