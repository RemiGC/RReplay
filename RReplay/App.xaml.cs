using GalaSoft.MvvmLight.Threading;
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
            DispatcherHelper.Initialize();
        }

        private void Application_Startup( object sender, StartupEventArgs e )
        {
            if ( Setup() )
            {
                var window = new MainWindow();
                window.Show();
            }
            else
            {
                this.Shutdown();
            }
        }

        private bool Setup()
        {
            if ( Settings.Default.UpgradeRequired )
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
            }

            if ( Settings.Default.firstRun )
            {
                Settings.Default.firstRun = false;
                Settings.Default.replaysFolder = ReplayFolderPicker.GetDefaultReplayGamesFolder();
            }

            if ( !ReplayFolderPicker.ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
            {
                string newPath;
                if ( !ReplayFolderPicker.GetNewReplayFolder(Settings.Default.replaysFolder, out newPath) )
                {
                    return false;
                }
                else
                {
                    Settings.Default.replaysFolder = newPath;
                }
            }

            return true;
        }
    }
}
