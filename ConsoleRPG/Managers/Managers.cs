
namespace ConsoleRPG
{
    public static class Managers
    {
        private static SceneManager scene = new SceneManager();
        public static GameManager game = new GameManager();
        private static TableManager table = new TableManager();

        public static SceneManager Scene => scene;
        public static GameManager Game => game;
        public static TableManager Table => table;
    }
}
