using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace RReplay
{
    public static class ReplayFolderPicker
    {
        private static string _buttonPressed = "None";
        private static string _newPath;

        /// <summary>
        /// Ask a new replays folder to the user and return it.
        /// </summary>
        /// <param name="oldPath">The old Path to show in the message box</param>
        /// <returns>return a string for the new path with the replay</returns>
        public static bool GetNewReplayFolder( string oldPath, out string newPath )
        {
            // Select a new folder command link
            var anotherReplayFolderCMDLink = new TaskDialogCommandLink("anotherFolder", "Select another replay folder\nThe folder must have .wargamerpl2 files");
            anotherReplayFolderCMDLink.Click += AnotherReplayFolderCMDLink_Click;


            // Exit Application command link
            var exitApplicationCMDLink = new TaskDialogCommandLink("exitApplication", "Exit the application");
            exitApplicationCMDLink.Click += ( s, d ) =>
            {
                _buttonPressed = "exitApplication";
                var s2 = (TaskDialogCommandLink)s;
                var taskDialog = (TaskDialog)(s2.HostingDialog);
                //taskDialog.Close(TaskDialogResult.CustomButtonClicked);
            };

            // Task Dialog settings
            var td = new TaskDialog();
            td.Caption = "Empty Replay Folder";
            td.Controls.Add(anotherReplayFolderCMDLink);
            td.Controls.Add(exitApplicationCMDLink);
            td.Icon = TaskDialogStandardIcon.Error;
            td.InstructionText = String.Format("The Replay folder is empty.");
            td.Text = String.Format("The folder {0} doesn't contains any .wargamerpl2 files.", oldPath);

            td.Closing += Td_Closing;
            TaskDialogResult tdResult = td.Show();

            if ( tdResult == TaskDialogResult.CustomButtonClicked )
            {
                if( _buttonPressed == "anotherFolder" && !String.IsNullOrEmpty(_newPath))
                {
                    newPath = _newPath;
                    return true;
                }
                else
                {
                    newPath = null;
                    return false;
                }
            }
            else
            {
                newPath = null;
                return false;
            }

        }

        private static void Td_Closing( object sender, TaskDialogClosingEventArgs e )
        {
            _buttonPressed = e.CustomButton;
        }

        private static void AnotherReplayFolderCMDLink_Click( object s, EventArgs e )
        {
            var openFolderDialog = new CommonOpenFileDialog();

            openFolderDialog.Title = "Select the wargame3 replay folders";
            openFolderDialog.IsFolderPicker = true;


            openFolderDialog.FileOk += ( sender, parameter ) =>
            {
                var dialog = (CommonOpenFileDialog)sender;
                if ( !ReplayFolderPicker.ReplaysPathContainsReplay(dialog.FileName) )
                {
                    parameter.Cancel = true;
                    MessageBox.Show("This folder doesn't contain any .wargamerpl2 files", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            };

            CommonFileDialogResult result = openFolderDialog.ShowDialog();

            if ( result == CommonFileDialogResult.Ok )
            {
                _newPath = openFolderDialog.FileName;
                var s2 = (TaskDialogCommandLink)s;
                var taskDialog = (TaskDialog)(s2.HostingDialog);
                //taskDialog.Close(TaskDialogResult.CustomButtonClicked);
            }
        }


        public static string GetDefaultReplayGamesFolder()
        {
            IKnownFolder folder = KnownFolders.SavedGames;
            return AddGameToSavedGamesFolder(folder.Path);
        }

        public static string AddGameToSavedGamesFolder( string savedGamesPath )
        {
            return Path.Combine(savedGamesPath, "EugenSystems\\WarGame3");
        }

        public static bool ReplaysPathContainsReplay( string path )
        {
            if ( Directory.Exists(path) )
            {
                return (from file in Directory.GetFiles(path, "*.wargamerpl2", SearchOption.TopDirectoryOnly) select file).Count() > 0;
            }
            else
            {
                return false;
            }

        }
    }
}
