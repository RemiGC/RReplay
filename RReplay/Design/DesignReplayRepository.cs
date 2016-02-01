using RReplay.Model;
using System;
using System.Collections.ObjectModel;

namespace RReplay.Design
{
    public class DesignReplayRepository : IReplayRepository
    {
        public void GetData( Action<ObservableCollection<Replay>, Exception> callback )
        {
            ObservableCollection<Replay> replayList = new ObservableCollection<Replay>();

            // TODO Find how to not hardcode design time data
            replayList.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\1vs_1_schlamschlacht_ranked_fr_Horus.wargamerpl2"));
            replayList.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\TKBLU.wargamerpl2"));
            replayList.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\TEAM_Killer_replay_2014-06-14_23-55-58.wargamerpl2"));
            replayList.Add(new Replay(@"F:\Dev\RedReplay\RedReplay\RReplay\Design\Data\replay_2014-10-26_16-27-14.wargamerpl2"));
            callback(replayList, null);
        }
    }
}