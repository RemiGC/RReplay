using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RReplay
{
    public static class RedDragonExeFolder
    {
        public static string GetPath()
        {
            // Ask the uninstall if they know where wargame is
            var gameInstallPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 251060",
                "InstallLocation", null);

            if (gameInstallPath != null && GamePathContainsExe(gameInstallPath) )
            {
                return gameInstallPath;
            }
            // Ask Steam where the path is

            // ask the user where the path is
            return null;
        }

        public static bool GamePathContainsExe(string path)
        {
            var fullPath = Path.Combine(path, "WarGame3.exe");
            if(File.Exists(fullPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
