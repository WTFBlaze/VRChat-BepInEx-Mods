using FaveStack.Utils;
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
            if (!File.Exists("Blaz"))
            {
                JsonManager.WriteToJsonFile(Main.MainModFolder + "\\FavStack\\Config.json", new FSConfig());
            }
            Instance = JsonManager.ReadFromJsonFile<FSConfig>(Main.MainModFolder + "\\FavStack\\Config.json");
        }

        public void Save()
        {
            JsonManager.WriteToJsonFile(Main.MainModFolder + "\\FavStack\\Config.json", Instance);
        }

        public class FavoriteObject
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string ThumbnailImageURL { get; set; }
        }
    }
}
