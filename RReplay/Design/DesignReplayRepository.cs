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
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\czech_before_1985_naschlap.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\replay_2016-02-10_00-02-46.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\replay_2015-10-05_16-48-18.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\Sotko_Anti_Chinese_Cheese_Defense.wargamerpl2"));
            replayCollection.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\patton 1.wargamerpl2"));

            var collec = from replay in replayCollection
                         from players in replay.Players
                         select players;
            callback(replayCollection, collec.ToList(), errorParsing, null);
        }
    }
}