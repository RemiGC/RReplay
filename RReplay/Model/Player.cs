using Newtonsoft.Json;
using System;

namespace RReplay.Model
{
    public class Player
    {
        private Deck deck;
        private string gameName;
        [JsonProperty]
        public UInt64 PlayerUserId
        {
            get;
            private set;
        }

        [JsonProperty]
        public string PlayerIALevel
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
                if(deck == null)
                {
                    deck = new Deck(PlayerDeckContent);
                }
                return deck;
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
}
