using FaveStack.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace FaveStack
{
    public class FSConfig
    {
        public List<FavoriteObject> list = new();

        public static FSConfig Instance;
        public static void Load()
        {
            if (!File.Exists(Main.MainModFolder + "\\FaveStack\\Config.json"))
            {
                JsonManager.WriteToJsonFile(Main.MainModFolder + "\\FaveStack\\Config.json", new FSConfig());
            }
            Instance = JsonManager.ReadFromJsonFile<FSConfig>(Main.MainModFolder + "\\FaveStack\\Config.json");
        }

        public void Save()
        {
            JsonManager.WriteToJsonFile(Main.MainModFolder + "\\FaveStack\\Config.json", Instance);
        }

        public class FavoriteObject
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string ThumbnailImageURL { get; set; }
        }
    }
}
