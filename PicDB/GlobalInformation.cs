using System;
using System.Collections.Generic;

namespace PicDB
{
    public static class GlobalInformation
    {
        public static string ConnectionString;

        public static string Path;

        /// <summary>
        /// Get the Elements out of the config file and save them into properties
        /// </summary>
        public static void ReadConfigFile()
        {
            var dict = new Dictionary<string, string>();
            var text = System.IO.File.ReadAllLines("config.txt"); //Standart Pfad zum .exe Verzeichnis vom Programm
            foreach (var s in text)
            {
                var splitted = s.Split(',');
                if (splitted.Length == 2) dict.Add(splitted[0], splitted[1]);
                else throw new ArgumentNullException("Config-File corrupted!");
            }

            ConnectionString = (dict.ContainsKey("connectionString")) ? dict["connectionString"] : null;
            Path = (dict.ContainsKey("path")) ? dict["path"] : null;
        }
    }
}
