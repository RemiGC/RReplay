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
            // Use this to connect to the actual data service

            ObservableCollection<Replay> replayList = new ObservableCollection<Replay>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games\\EugenSystems\\WarGame3");

            var replaysFiles = from file in Directory.GetFiles(path, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                               select file;
            foreach ( string file in replaysFiles )
            {
                Console.WriteLine("Processing file : {0}", new FileInfo(file).Name);
                replayList.Add(new Replay(file));
            }

            callback(replayList, null);
        }
    }
}