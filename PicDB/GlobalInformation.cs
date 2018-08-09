using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB
{
    public static class GlobalInformation
    {
        public static string ConnectionString;

        public static string Path;

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

            ConnectionString = dict["connectionString"];
            Path = dict["path"];
        }
    }
}
