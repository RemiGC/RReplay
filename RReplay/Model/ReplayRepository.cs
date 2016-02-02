using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using RReplay.Properties;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace RReplay.Model
{
    public class ReplayRepository : IReplayRepository
    {
        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath(
                                                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
                                                uint dwFlags,
                                                IntPtr hToken,
                                                out IntPtr pszPath
                                                );

        public void GetData( Action<ObservableCollection<Replay>, Exception> callback )
        {
            if (ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
            {
                ObservableCollection<Replay> replayList = new ObservableCollection<Replay>();

                var replaysFiles = from file in Directory.GetFiles(Settings.Default.replaysFolder, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                                    select file;
                foreach ( string file in replaysFiles )
                {
                    replayList.Add(new Replay(file));
                }

                callback(replayList, null);
            }
            else
            { 
                callback(null, new ApplicationException("EmptyReplaysPath"));
            }
        }

        public static string GetDefaultReplayGamesFolder()
        {
            IKnownFolder folder = KnownFolders.SavedGames;
            return AddGameToSavedGamesFolder(folder.Path);
        }

        public static string AddGameToSavedGamesFolder(string savedGamesPath)
        {
            return Path.Combine(savedGamesPath, "EugenSystems\\WarGame3");
        }

        public static bool ReplaysPathContainsReplay( string path )
        {
            if (Directory.Exists(path) )
            {
                return (from file in Directory.GetFiles(path, "*.wargamerpl2", SearchOption.TopDirectoryOnly) select file).Count() > 0;
            } else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Ask a new replays folder to the user and place it in the Settings.Default.replaysFolder settings. Recursively call itself it the new path doesn't contains replay files
        /// </summary>
        /// <param name="oldPath">The old Path to show in the message box</param>
        /// <returns>true if the new path is valid. False if the user click cancel</returns>
        public static bool GetNewReplayFolder( string oldPath )
        {
            // Select a new folder command link
            var anotherReplayFolderCMDLink = new TaskDialogCommandLink("anotherFolder", "Select another replay folder\nThe folder must have .wargamerpl2 files");
            anotherReplayFolderCMDLink.Click += ( s, d ) =>
            {
                var openFolderDialog = new CommonOpenFileDialog();
                openFolderDialog.IsFolderPicker = true;
                CommonFileDialogResult result = openFolderDialog.ShowDialog();
                if ( result == CommonFileDialogResult.Ok )
                {
                    Settings.Default.replaysFolder = openFolderDialog.FileName;
                    var s2 = (TaskDialogCommandLink)s;
                    var taskDialog = (TaskDialog)(s2.HostingDialog);
                    taskDialog.Close(TaskDialogResult.Ok);
                }
                else
                {
                    var s2 = (TaskDialogCommandLink)s;
                    var taskDialog = (TaskDialog)(s2.HostingDialog);
                    taskDialog.Close(TaskDialogResult.Cancel);
                }
            };

            // Exit Application command link
            var exitApplicationCMDLink = new TaskDialogCommandLink("exitApplication", "Exit the application");
            exitApplicationCMDLink.Click += ( s, d ) =>
            {
                var s2 = (TaskDialogCommandLink)s;
                var taskDialog = (TaskDialog)(s2.HostingDialog);
                taskDialog.Close(TaskDialogResult.Cancel);
            };

            // Task Dialog settings
            var td = new TaskDialog();
            td.Caption = "Empty Replay Folder";
            td.Controls.Add(anotherReplayFolderCMDLink);
            td.Controls.Add(exitApplicationCMDLink);
            td.Icon = TaskDialogStandardIcon.Error;
            td.InstructionText = String.Format("The Replay folder is empty.");
            td.Text = String.Format("The folder {0} doesn't countains any .wargamerpl2 files.", oldPath);


            TaskDialogResult tdResult = td.Show();

            if ( tdResult == TaskDialogResult.Ok )
            {
                if ( !GetNewReplayFolder(Settings.Default.replaysFolder) )
                {
                    return GetNewReplayFolder(Settings.Default.replaysFolder);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }
    }
}