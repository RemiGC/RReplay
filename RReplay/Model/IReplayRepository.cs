using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RReplay.Model
{
    /// <summary>
    /// Represent a Repository that have access to a collection of replays
    /// </summary>
    public interface IReplayRepository
    {
        /// <summary>
        /// Return a collection of replay, the list of players and their deck, the list of replay that couldn't be parsed and an exception
        /// </summary>
        /// <param name="callback"></param>
        void GetData( Action<ObservableCollection<Replay>, List<Player>, List<Tuple<string, string>>, Exception> callback, IDeckInfoRepository deckService);
    }
}
