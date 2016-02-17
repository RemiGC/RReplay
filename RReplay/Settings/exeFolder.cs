using System.Collections.Generic;
using System.IO;

namespace RReplay
{
    public class ExeFolder
    {
        public static bool IsBaseFilesPresent(string filePath)
        {
            List<string> baseFiles = new List<string>();

            baseFiles.Add(Path.Combine("Icons","Flags", "neut_flag.png"));
            baseFiles.Add(Path.Combine("Icons","Units", "NoImage.jpg"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "Airborne.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "armored.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "before80.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "before85.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "marines.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "mechanized.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "motorized.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "navy.png"));
            baseFiles.Add(Path.Combine("Icons", "Deck", "support.png"));
            baseFiles.Add("NATO.xml");
            baseFiles.Add("PACT.xml");

            foreach (var file in baseFiles)
            {
                string path = Path.Combine(filePath, file);
                if(!File.Exists(path))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
