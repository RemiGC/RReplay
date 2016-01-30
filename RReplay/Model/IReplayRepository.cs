using System;
using System.Collections.ObjectModel;

namespace RReplay.Model
{
    public interface IReplayRepository
    {
        void GetData( Action<ObservableCollection<Replay>, Exception> callback );
    }
}
