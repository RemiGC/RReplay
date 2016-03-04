using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;
using RReplay.Properties;
using System.IO;
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
            // Upgrade settings from past config
            if ( Settings.Default.UpgradeRequired )
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;

                //Introduced in V.0.5
                if ( string.IsNullOrEmpty(Settings.Default.exeFolder) )
                {
                    Settings.Default.exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }

                //Introduced in V.0.5
                if ( string.IsNullOrEmpty(Settings.Default.redDragonExe) )
                {
                    Settings.Default.redDragonExe = RedDragonExeFolder.GetPath();
                }
            }

            // First setup
            if ( Settings.Default.firstRun )
            {
                Settings.Default.firstRun = false;
                Settings.Default.replaysFolder = ReplayFolderPicker.GetDefaultReplayGamesFolder();
                Settings.Default.exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Settings.Default.redDragonExe = RedDragonExeFolder.GetPath();
                Settings.Default.Save();
            }

            // Check if the replays folder we have is good, if not ask for a new one
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

            bool filePresent = false;

            // check if we have all the base icons we need
            if ( !ExeFolder.IsBaseFilesPresent(Settings.Default.exeFolder))
            {
                // Maybe they moved the .exe folder from the last execution
                if(Settings.Default.exeFolder != Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) )
                {
                    string newPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    if( ExeFolder.IsBaseFilesPresent(newPath) )
                    {
                        Settings.Default.exeFolder = newPath; // yup
                        filePresent = true;
                    }
                }
            }
            else
            {
                filePresent = true;
            }

            if(!filePresent)
            {
                // TODO ask for a new icons folder maybe.
                return false;
            }

            return true;
        }
    }
}
