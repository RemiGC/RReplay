using Newtonsoft.Json;
using RReplay.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace RReplay.Model
{
    public class ReplayRepository : IReplayRepository
    {
        /// <summary>
        /// A repository that build an ObservableCollection<Replay> from the replay in the folder in Settings.Default.replaysFolder
        /// Replays that couldn't be parsed will be added to a list
        /// </summary>
        /// <param name="callback"></param>
        public void GetData( Action<ObservableCollection<Replay>, List<Player>, List<Tuple<string, string>>, Exception> callback )
        {
            if ( ReplayFolderPicker.ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
            {
                var replayCollection = new ObservableCollection<Replay>();
                var errorParsing = new List<Tuple<string, string>>();

                var replaysFiles = from file in Directory.GetFiles(Settings.Default.replaysFolder, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                                   select file;


                foreach ( string file in replaysFiles )
                {
                    try
                    {
                        var replay = new Replay(file);
                        replayCollection.Add(replay);
                    }
                    catch ( JsonSerializationException ex )
                    {
                        errorParsing.Add(new Tuple<string, string>(file, ex.Message));
                    }

                }
                var collec = from replay in replayCollection
                             from players in replay.Players
                             select players;
                callback(replayCollection, collec.ToList(), errorParsing, null);
            }
            else
            {
                var replayCollection = new ObservableCollection<Replay>();
                var errorParsing = new List<Tuple<string, string>>();
                callback(replayCollection, null, errorParsing, new EmptyReplaysPathException(Settings.Default.replaysFolder));
            }
        }
    }

    [SerializableAttribute]
    public class EmptyReplaysPathException : Exception
    {
        public EmptyReplaysPathException()
        {
        }

        public EmptyReplaysPathException( string message )
        : base(String.Format("The folder {0} doesn't contains any .wargamerpl2 files", message))
        {
        }

        public EmptyReplaysPathException( string message, Exception inner )
        : base(String.Format("The folder {0} doesn't contains any .wargamerpl2 files", message), inner)
        {
        }
    }
}