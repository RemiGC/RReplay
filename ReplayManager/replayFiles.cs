using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReplayManager
{
    public class ReplayFiles
    {
        public ReplayFiles(string path)
        {
            Players = new List<Player>();
            CompletePath = path;
            FileInfo fileInfo = new FileInfo(path);
            Date = fileInfo.LastWriteTime;
            string json = Utilities.ExtractJSONFromReplayFile(CompletePath);

            JObject jo = JObject.Parse(json);
            Game = jo["game"].ToObject<Game>();
            var players = from player in jo.Children()
                          where player.Path.Contains("player_")
                          select player;

            foreach (var item in players)
            {
                Player player = item.First.ToObject<Player>();
                Players.Add(player);
            }
        }

        public string Filename
        {
            get
            {
                return Path.GetFileNameWithoutExtension(CompletePath);
            }
        }

        public override string ToString()
        {
            return Filename;
        }

        public string CompletePath
        {
            get;
            private set;
        }
        
        public DateTime Date
        {
            private set;
            get;
        }

        public Game Game
        {
            private set;
            get;
        }

        public List<Player> Players
        {
            private set;
            get;
        }

        public int PlayerCount
        {
            get
            {
                return Players.Count;
            }
        }

        public string GameMap
        {
            get
            {
                return Game.Map;
            }
        }

        public string GameServerName
        {
            get
            {
                return Game.ServerName;
            }
        }
    }
}
