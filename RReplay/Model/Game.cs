using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Xml.Serialization;

namespace RReplay.Model
{
    public class Game
    {
        [JsonProperty]
        public byte GameMode
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte IsNetworkMode
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte NbMaxPlayer
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte NbPlayer
        {
            get;
            private set;
        }

        [JsonProperty]
        public UInt64 Seed
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte Private
        {
            get;
            private set;
        }

        [JsonProperty]
        public string ServerName
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte WithHost
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint TimeLeft
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint Version
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte GameState
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte NeedPassword
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte GameType
        {
            get;
            private set;
        }

        [JsonProperty]
        public string Map
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint InitMoney
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint TimeLimit
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint ScoreLimit
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte NbIA
        {
            get;
            private set;
        }

        [JsonProperty]
        public int VictoryCond
        {
            get;
            private set;
        }

        [JsonProperty]
        public sbyte IncomeRate
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte WarmupCountdown
        {
            get;
            private set;
        }

        [JsonProperty]
        public int DeploiementTimeMax
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte DebriefingTimeMax
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte LoadingTimeMax
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte NbMinPlayer
        {
            get;
            private set;
        }

        [JsonProperty]
        public int DeltaMaxTeamSize
        {
            get;
            private set;
        }

        [JsonProperty]
        public byte MaxTeamSize
        {
            get;
            private set;
        }

        [JsonProperty]
        public Int64 NationConstraint
        {
            get;
            private set;
        }

        [JsonProperty]
        public uint ThematicConstraint
        {
            get;
            private set;
        }

        [JsonProperty]
        public Int64 DateConstraint
        {
            get;
            private set;
        }

        private Game()
        {
        }
    }
}
