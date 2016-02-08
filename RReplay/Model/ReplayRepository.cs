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
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void GetData( Action<ObservableCollection<Replay>, List<Tuple<string, string>>, Exception> callback )
        {
            if ( ReplayFolderPicker.ReplaysPathContainsReplay(Settings.Default.replaysFolder) )
            {
                var replayList = new ObservableCollection<Replay>();
                var errorParsing = new List<Tuple<string,string>>();

                var replaysFiles = from file in Directory.GetFiles(Settings.Default.replaysFolder, "*.wargamerpl2", SearchOption.TopDirectoryOnly)
                                    select file;


                foreach ( string file in replaysFiles )
                {
                    try
                    {
                        replayList.Add(new Replay(file));
                    }
                    catch ( JsonSerializationException ex )
                    {
                        errorParsing.Add(new Tuple<string, string>(file, ex.Message));
                    }

                }

                callback(replayList, errorParsing, null);
            }
            else
            {
                var replayList = new ObservableCollection<Replay>();
                var errorParsing = new List<Tuple<string, string>>();
                callback(replayList, errorParsing, new ApplicationException("EmptyReplaysPath"));
            }
        }
    }
}