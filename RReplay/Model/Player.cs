using Newtonsoft.Json;
using System;

namespace RReplay.Model
{
    public class Player
    {
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

        private Player()
        {

        }
    }
}
