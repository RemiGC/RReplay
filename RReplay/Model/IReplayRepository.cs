using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RReplay.Model
{
    public interface IReplayRepository
    {
        void GetData( Action<ObservableCollection<Replay>, List<Tuple<string, string>>, Exception> callback );
    }
}
