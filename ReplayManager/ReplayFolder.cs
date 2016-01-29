using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReplayManager
{
    public class ReplayFolder
    {
        public string Path
        {
            get;
            private set;
        }

        public string Game
        {
            get;
            private set;
        }

        public List<ReplayFiles> Replays
        {
            get;
            private set;
        }

        public ReplayFolder(string folder, string game)
        {
            Path = folder;
            Game = game;
            Replays = new List<ReplayFiles>();
            BuildReplayList();
        }

        private void BuildReplayList()
        {
            Replays.Clear();
            var replaysFiles = from file in Directory.GetFiles(Path, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                               //where new FileInfo(file).Name == "replay_2014-07-04_20-54-12.wargamerpl2"
                               select file;
            foreach (string file in replaysFiles)
            {
                Console.WriteLine("Processing file :{0}", new FileInfo(file).Name);
                Replays.Add(new ReplayFiles(file));
            }
        }
    }
}
