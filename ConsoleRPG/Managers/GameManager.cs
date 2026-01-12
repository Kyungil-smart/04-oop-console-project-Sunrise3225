using Newtonsoft.Json;

namespace ConsoleRPG
{
    [Serializable]
    public class GameData
    {
        public Character character;
        public Stage stage;
    }
    public class GameManager
    {
        public GameData data = new GameData();
        private string path;

        public void Init()
        {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/SaveData.json";
            if (LoadGame()) return;
        }
        public void SaveGame()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
            string jsonStr = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(path, jsonStr);
        }
        public bool LoadGame()
        {
            if (!File.Exists(path))
                return false;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            string file = File.ReadAllText(path);
            GameData loaded = JsonConvert.DeserializeObject<GameData>(file, settings);

            if (loaded == null) return false;
            if (loaded.character == null) return false;
            if (string.IsNullOrEmpty(loaded.character.Name)) return false;

            data = loaded;
            return true;
        }
    }
}
