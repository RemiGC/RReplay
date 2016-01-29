using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using ReplayManager;

namespace RedConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Utilities.isWarGame3ReplayFolderExist())
            {
                Console.WriteLine("WarGame 3 replay folder detected");
                Console.WriteLine("Path is: {0}", Utilities.WarGame3ReplayFolder());
                try
                {
                    ReplayFolder replayFolder = new ReplayFolder(Utilities.WarGame3ReplayFolder(), "WarGame3");
                    Console.WriteLine("Number of replay files detected : {0}", replayFolder.Replays.Count);
                    foreach (ReplayFiles replay in replayFolder.Replays)
                    {
                        Console.WriteLine("Server Name: {0}", replay.Game.ServerName);
                        Console.WriteLine("Nb Players: {0}", replay.Players.Count);
                    }

                    //Extract all deck code to a csv file
                    List<Tuple<string, string>> deckList = ExtractAllDeckCode(replayFolder);
                    WriteCSV(deckList, "f:\\dev\\people.csv");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Not WarGame3");
            }
            Console.ReadKey();
        }

        static List<Tuple<string,string>> ExtractAllDeckCode(ReplayFolder replayFolders)
        {
            List<Tuple<string, string>> deckList = new List<Tuple<string, string>>();
            foreach (ReplayFiles replay in replayFolders.Replays)
            {
                foreach (Player player in replay.Players)
                {
                    Tuple<string, string> deck = new Tuple<string, string>(player.PlayerDeckName, player.PlayerDeckContent);
                    deckList.Add(deck);
                }
            }
            return deckList;
        }

        static void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
    }
}
