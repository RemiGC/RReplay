using Newtonsoft.Json;
using System;

namespace RReplay.Model
{
    public class Player
    {
        private Deck deck;
        private string gameName;

        [JsonProperty]
        public ulong PlayerUserId
        {
            get;
            private set;
        }

        [JsonProperty]
        [JsonConverter(typeof(PlayerIALevelConverter))]
        public ulong PlayerIALevel
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint PlayerObserver
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint PlayerAlliance
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint PlayerReady
        {
            get;
            private set;
        }

        [JsonProperty]
        public double PlayerElo
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte PlayerLevel
        {
            get;
            private set;
        }
        
        [JsonProperty]
        public uint PlayerRank
        {
            get;
            private set;
        }

        [JsonProperty]
        public string PlayerName
        {
            get;
            private set;
        }

        [JsonProperty]
        public string PlayerTeamName
        {
            get;
            private set;
        }

        [JsonProperty]
        public string PlayerAvatar
        {
            get;
            private set;
        }

        public ulong SteamID
        {
            get
            {
                return UInt64.Parse(this.PlayerAvatar.Remove(0, 25));
            }
        }

        [JsonProperty]
        public string PlayerDeckName
        {
            get;
            private set;
        }

        [JsonProperty]
        public string PlayerDeckContent
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint PlayerScoreLimit
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint PlayerIncomeRate
        {
            get;
            private set;
        }

        public string GameName
        {
            get
            {
                return gameName;
            }
            set
            {
                gameName = value;
            }
        }
        public Deck Deck
        {
            get
            {
                return deck;
            }
            set
            {
                deck = value;
            }
        }

        public bool IsAI
        {
            get
            {
                return PlayerIALevel < 20;
               
            }
        }

        private Player(string gameName)
        {
            this.gameName = gameName;
        }
        private Player( )
        {
        }
    }

    public class PlayerIALevelConverter : JsonConverter
    {
        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            throw new NotImplementedException();
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            string testVal = (string)serializer.Deserialize(reader, typeof(string));
            object retVal = new object();

            if (testVal == "-1")
            {
                retVal = ulong.MaxValue;
            }
            else
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert( Type objectType )
        {
            return false;
        }
    }
}
