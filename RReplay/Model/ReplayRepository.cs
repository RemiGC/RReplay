using RReplay.Properties;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace RReplay.Model
{
    public class ReplayRepository : IReplayRepository
    {
        public void GetData( Action<ObservableCollection<Replay>, Exception> callback )
        {
            if ( ReplayFolderPicker.ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
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
                ObservableCollection<Replay> replayList = new ObservableCollection<Replay>();
                callback(replayList, new ApplicationException("EmptyReplaysPath"));
            }
        }
    }
}