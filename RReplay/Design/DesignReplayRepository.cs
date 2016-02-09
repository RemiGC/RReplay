using RReplay.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RReplay.Design
{
    public class DesignReplayRepository : IReplayRepository
    {
        public void GetData( Action<ObservableCollection<Replay>, List<Player>, List<Tuple<string, string>>, Exception> callback )
        {
            var replayCollection = new ObservableCollection<Replay>();
            var errorParsing = new List<Tuple<string, string>>();

            // TODO Find how to not hardcode design time data
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\1vs_1_schlamschlacht_ranked_fr_Horus.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\TKBLU.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\TEAM_Killer_replay_2014-06-14_23-55-58.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\replay_2014-10-26_16-27-14.wargamerpl2"));

            var collec = from replay in replayCollection
                         from players in replay.Players
                         select players;
            callback(replayCollection, collec.ToList(), errorParsing, null);
        }
    }
}