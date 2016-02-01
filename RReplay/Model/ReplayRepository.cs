using RReplay.Properties;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
            IntPtr pPath;
            if ( SHGetKnownFolderPath(new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4"), 0, IntPtr.Zero, out pPath) == 0 ) // S_OK
            {
                string path = System.Runtime.InteropServices.Marshal.PtrToStringUni(pPath);
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(pPath);
                return AddGameToSavedGamesFolder(path);
            } else
            {
                return "";
            }
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
    }
}