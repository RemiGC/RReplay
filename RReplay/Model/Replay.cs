using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RReplay.Model
{
    /// <summary>
    /// Class that represent a single WarGame Replay
    /// </summary>
    public class Replay
    {
        public Game Game { get; private set; }
        public DateTime Date { get; private set; }
        public string CompletePath { get; private set; }

        public Replay( string path )
        {
            Players = new List<Player>();
            CompletePath = path;
            FileInfo fileInfo = new FileInfo(path);
            Date = fileInfo.LastWriteTime;
            string json = ExtractJSONFromReplayFile(CompletePath);

            JObject jo = JObject.Parse(json);
            Game = jo["game"].ToObject<Game>();
            var players = from player in jo.Children()
                          where player.Path.Contains("player_")
                          select player;

            foreach ( var item in players )
            {
                Player player = item.First.ToObject<Player>();
                Players.Add(player);
            }
        }

        public List<Player> Players
        {
            private set;
            get;
        }

        public List<Player> FirstTeamPlayers
        {
            get
            {
                return Players.Where(x => x.PlayerAlliance == 0).ToList();
            }
        }

        public List<Player> SecondTeamPlayers
        {
            get
            {
                return Players.Where(x => x.PlayerAlliance == 1).ToList();
            }
        }

        public int PlayerCount
        {
            get
            {
                return Players.Count;
            }
        }

        public int GameType
        {
            get
            {
                return Game.GameMode;
            }
        }

        public string Map
        {
            get
            {
                return Game.Map;
            }
        }

        public string ServerName
        {
            get
            {
                return Game.ServerName;
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(CompletePath);
            }
        }

        public static string ExtractJSONFromReplayFile( string replayPath )
        {
            string json;
            using ( FileStream stream = File.OpenRead(replayPath) )
            {
                stream.Seek(50L, SeekOrigin.Begin);

                byte[] byteArrayToRead = new byte[2];
                stream.Read(byteArrayToRead, 0, 2);

                if ( BitConverter.IsLittleEndian )
                    Array.Reverse(byteArrayToRead);

                int bytesCount = BitConverter.ToInt16(byteArrayToRead, 0);

                stream.Seek(4L, SeekOrigin.Current);

                byte[] JSONbyte = new byte[bytesCount];
                stream.Read(JSONbyte, 0, bytesCount);
                json = Encoding.UTF8.GetString(JSONbyte);
            }

            return json;
        }
    }
}
