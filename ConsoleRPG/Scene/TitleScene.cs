using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class TitleScene : BaseScene
    {
        public override void EnterScene()
        {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("NewGame"));

            if (Managers.Game.data.character != null)
                Options.Add(Managers.Scene.GetOption("LoadGame"));

            DrawScene();
        }
        public override void NextScene()
        {
            do
            {
                Renderer.PrintOptions(20, Options, true, selectOptionIndex, lineGap: 2);
                GetInput();
            }
            while (lastCommand != Command.Interact);
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder();

            Renderer.PrintCenter(3,  " ██████   ██████  ██    ██  ██████   ██████  ██       ██████   █████    █████    ██████  ");
            Renderer.PrintCenter(4,  "██       ██    ██ ███   ██ ██       ██    ██ ██       ██       ██   ██  ██   ██ ██       ");
            Renderer.PrintCenter(5,  "██       ██    ██ ████  ██  █████   ██    ██ ██       █████    ██████   █████   ██  ███  ");
            Renderer.PrintCenter(6,  "██       ██    ██ ██ ██ ██      ██  ██    ██ ██       ██       ██  ██   ██      ██   ██ ");
            Renderer.PrintCenter(7, " ██████    █████  ██  ████ ██████    ██████  ███████  ██████   ██   ██  ██       ██████  ");


            Renderer.Print(11, "           b A", margin: Console.WindowWidth - 90);
            Renderer.Print(12, "           $b Vb.", margin: Console.WindowWidth - 90);
            Renderer.Print(13, "           '$b  V$b.", margin: Console.WindowWidth - 90);
            Renderer.Print(14, "            $$b V$$b.", margin: Console.WindowWidth - 90);
            Renderer.Print(15, "            '$$b. V$$$$oooooooo.", margin: Console.WindowWidth - 90);
            Renderer.Print(16, "             '$$P* V$$$$$\"\"**$$$b.    .o$$P", margin: Console.WindowWidth - 90);
            Renderer.Print(17, "              \" .oooZ$$$$b..o$$$$$$$$$$$$C", margin: Console.WindowWidth - 90);
            Renderer.Print(18, "              .$$$$$$$$$$$$$$$$$$$$$$$$$$$b.", margin: Console.WindowWidth - 90);
            Renderer.Print(19, "              $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$", margin: Console.WindowWidth - 90);
            Renderer.Print(20, "        .o$$$o$$$$$$$$P\"\"*$$$$$$$$$P\"\"\"*$$$P", margin: Console.WindowWidth - 90);
            Renderer.Print(21, "       .$$$**$$$$P\"q$C    \"$$$b.$$P", margin: Console.WindowWidth - 90);
            Renderer.Print(22, "       $$P   \"$$$b  \"$ . .$$$$$b.      *\"", margin: Console.WindowWidth - 90);
            Renderer.Print(23, "       $$      $$$.     \"***$$$$$$$b. A.", margin: Console.WindowWidth - 90);
            Renderer.Print(24, "       V$b.Z$$b.  .       \"*$$$$$b$$:", margin: Console.WindowWidth - 90);
            Renderer.Print(25, "        V$$.  \"*$$$b.  b.         \"$$$$$", margin: Console.WindowWidth - 90);
            Renderer.Print(26, "         \"$$b     \"*$.  *b.         \"$$$b", margin: Console.WindowWidth - 90);
            Renderer.Print(27, "           \"$$b.     \"L  \"$$o.        \"*\" ", margin: Console.WindowWidth - 90);
            Renderer.Print(28, "             \"*$$o.        \"*$$o.          .o$$$$$$$$b.", margin: Console.WindowWidth - 90);
            Renderer.Print(29, "                 \"*$$b.       \"$$b.       .$$$$$*\"   \"\"*.", margin: Console.WindowWidth - 90);
            Renderer.Print(30, "                    \"*$$o.      \"$$$o.    $$$$$'", margin: Console.WindowWidth - 90);
            Renderer.Print(31, "                       \"$$o       \"$$$b.  \"$$$$   ...oo..", margin: Console.WindowWidth - 90);
            Renderer.Print(32, "                         \"$b.      \"$$$$b. \"$$$$$$$P*\"\"\"\"\"", margin: Console.WindowWidth - 90);
            Renderer.Print(33, "                        . \"$$       \"$$$$b  \"$$$$P\"", margin: Console.WindowWidth - 90);
            Renderer.Print(34, "                         L.\"$.      .$$$$$.  $$$$", margin: Console.WindowWidth - 90);
            Renderer.Print(35, "                          $$$;      o$$$$$$  $$$$", margin: Console.WindowWidth - 90);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
        }
    }
}
