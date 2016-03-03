using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace RReplay.Model
{
    /// <summary>
    /// Class that represent a single WarGame Replay
    /// </summary>
    public class Replay
    {
        public static uint MinimumVersionSupported = 430000610;

        public Game Game { get; private set; }
        public DateTime Date { get; private set; }
        public string CompletePath { get; private set; }

        public Replay( string path, IUnitInfoRepository repository )
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

                try
                {
                    Player player = item.First.ToObject<Player>();
                    player.GameName = this.Name;
                    if ( IsVersionSupported ) // we dont support deck string prior to this version
                    {
                        player.Deck = new Deck(player.PlayerDeckContent, repository);
                    }
                    Players.Add(player);
                }
                catch( Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public bool IsVersionSupported
        {
            get
            {
                return Game.Version >= MinimumVersionSupported;
            }
        }

        public string JSONDumbFromFile
        {
            get
            {
                JObject jo = JObject.Parse(ExtractJSONFromReplayFile(CompletePath));
                return jo.ToString(Newtonsoft.Json.Formatting.Indented);
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
                return Game.GameType;
            }
        }

        public int GameMode
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

        private static string ExtractJSONFromReplayFile( string replayPath )
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
