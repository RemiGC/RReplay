using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReplayManager
{
    public class Utilities
    {
        public static string WarGame3ReplayFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Saved Games\\EugenSystems\\WarGame3");
        }

        public static bool isWarGame3ReplayFolderExist()
        {
            return Directory.Exists(WarGame3ReplayFolder());
        }

        public static string ExtractJSONFromReplayFile(string replayFile)
        {

            string file;
            using (FileStream stream = File.OpenRead(replayFile))
            {
                stream.Seek(50L, SeekOrigin.Begin);

                byte[] byteArrayToRead = new byte[2];
                stream.Read(byteArrayToRead, 0, 2);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(byteArrayToRead);

                int bytesCount = BitConverter.ToInt16(byteArrayToRead,0);

                stream.Seek(4L,SeekOrigin.Current);

                byte[] JSONbyte = new byte[bytesCount];
                stream.Read(JSONbyte,0,bytesCount);
                file = Encoding.UTF8.GetString(JSONbyte);
            }

            return file;
        }

    }
}
