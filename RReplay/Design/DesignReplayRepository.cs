using RReplay.Model;
using System;
using System.Collections.ObjectModel;

namespace RReplay.Design
{
    public class DesignReplayRepository : IReplayRepository
    {
        public void GetData( Action<ObservableCollection<Replay>, Exception> callback )
        {
            ObservableCollection<Replay> replayList = new ObservableCollection<Replay>();
            /*string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games\\EugenSystems\\WarGame3");

            var replaysFiles = from file in Directory.GetFiles(path, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                               select file;
            foreach ( string file in replaysFiles )
            {
                Console.WriteLine("Processing file : {0}", new FileInfo(file).Name);
                replayList.Add(new Replay(file));
            }*/

            replayList.Add(new Replay(@"C:\Users\Rémi\Saved Games\EugenSystems\WarGame3\replay_2014-10-26_17-04-41.wargamerpl2"));
            replayList.Add(new Replay(@"C:\Users\Rémi\Saved Games\EugenSystems\WarGame3\TKBLU.wargamerpl2"));
            replayList.Add(new Replay(@"C:\Users\Rémi\Saved Games\EugenSystems\WarGame3\TEAM_Killer_replay_2014-06-14_23-55-58.wargamerpl2"));
            replayList.Add(new Replay(@"C:\Users\Rémi\Saved Games\EugenSystems\WarGame3\replay_2014-10-26_16-27-14.wargamerpl2"));
            callback(replayList, null);
        }
    }
}