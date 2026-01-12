namespace ConsoleRPG
{
    public class MainScene : BaseScene
    {
        public override string Title { get; protected set; } = "중 간 계 : 블루 마운틴";
        #region Scene
        public override void EnterScene()
        {
            // 선택지 설정
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("ShowInfo"));
            Options.Add(Managers.Scene.GetOption("Inventory"));
            Options.Add(Managers.Scene.GetOption("Equipment"));
            Options.Add(Managers.Scene.GetOption("Shop"));
            Options.Add(Managers.Scene.GetOption("Dungeon"));
            //Options.Add(Managers.Scene.GetOption("Rest"));

            DrawScene();
        }
        public override void NextScene()
        {
            do
            {
                Renderer.PrintOptions(19, Options, true, selectOptionIndex, lineGap: 2);
                GetInput();
            }
            while (lastCommand != Command.Interact);
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(4, "허허허.. 낯선 얼굴이구나. 자네, 인간이지?", margin: Console.WindowWidth / 2);
            Renderer.Print(5, "나는 이 마을의 장로라네. 다들 편히 장로라 부르지", margin: Console.WindowWidth / 2);
            Renderer.Print(7, "원래라면 밖에서 온 이를 경계해야 마땅하겠지만..", margin: Console.WindowWidth / 2);
            Renderer.Print(8, "자네 눈빛을 보니, 악한 마음으로 온 것 같진 않구만", margin: Console.WindowWidth / 2);
            Renderer.Print(10, "다만 던전 안쪽엔 거친 몬스터들이 있으니, 조심하게", margin: Console.WindowWidth / 2);
            Renderer.Print(11, "마을에서 장비와 마음의 준비를 단단히 하고 가게나", margin: Console.WindowWidth / 2);


            Renderer.Print(13, "                    ▓▓▓▓▓▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(14, "                  ▓▓▓░░░░░░▒▒▒▒▒▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(15, "                ▓▓░░░░░░░░▒▒▒▒▒▒▒▒▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(16, "                ▓░░░░░░░░░░░░░▒▒▒▒▒▒▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(17, "               ▓▓░▒▒▒▒▒▒▒▒░▒░░░░░▒▒▒▒▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(18, "             ▓▓▓▒▒▒▒▒▒░  ▒▒▒▒░  ░░░░░░▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(19, "             ▓▓▒▒▒▒▒▒▒▒  ▒▒▒▒▒  ░░░▒▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(20, "        ░▒▓▓▓▓▓▓▓▓▓▒▒▒▒▓▓▒▒▒▒▒░▓░░▒▓▓▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(21, "      ▒▓▓▓▓▓▓▓▓▓▓▓▓▒▓▒▒▒▒▒▒░░░░░░▒▓▓▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(22, "     ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▒▒▒▒▒▒░░░░▓▓▓▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(23, "   ▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▓▓▓▓▓▓▓░░░▓▓▓▓▓▓▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(24, " ▒▒▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▓▓▒▒▒▒▒░▓░░▓▒▓▓▓▓▓▒▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(25, "▒▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒░ ░░░░▒▓▒▓▒▒▒▓▓▓", margin: Console.WindowWidth / 2);
            Renderer.Print(26, " ░▓▒▒▒▒▒▒▓▓▒▓▓▓▓▓▓▓▓▒▓▒▒▒▒░░░░░░░▒▓▓▒▓▓▓▒▒▒▒", margin: Console.WindowWidth / 2);
            Renderer.Print(27, "   ▒▓▓▒▒▒▒▒▒▒▓▓▓▓▓▓▓▒▒░░░░░░░░░░░▒▓▓▒▒▓▓▒░░░", margin: Console.WindowWidth / 2);
            Renderer.Print(28, "     ▒▓▓▒▒▒▒▒▒▒▒▒▒▓▓▒▒░░░░░░░░░░▒▒▓▓▒  ▒░ ░░░░", margin: Console.WindowWidth / 2);
            Renderer.Print(29, "           ▒▒▒▒▒▒▒▓▓▒░░░░░░░░░░░░▒▓▓▓ ▓▒▓░░▒▒░░", margin: Console.WindowWidth / 2);
            Renderer.Print(30, "             ▒▒▒▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓░  ░▒▒▓▓▒▒░", margin: Console.WindowWidth / 2);
            Renderer.Print(31, "            ░▓▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▒▒▒▓▓▓▓▒     ▒▓▒", margin: Console.WindowWidth / 2);
            Renderer.Print(32, "             ▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░▒  ░▒░    ▓▓▒", margin: Console.WindowWidth / 2);
            Renderer.Print(33, "                ░▒▒░░░░░    ▒▒░░░░        ▓▒▒", margin: Console.WindowWidth / 2);
            Renderer.Print(34, "               ▒▒▒▒▒       ░▒▒▒▒░        ▓▒", margin: Console.WindowWidth / 2);
            Renderer.Print(35, "             ░▒▒▒▒▒        ░▓▓▒▒░        ▓▒", margin: Console.WindowWidth / 2);
            Renderer.Print(36, "            ░▒▒▒▒░          ▒▓▓▒▒       ░▓░", margin: Console.WindowWidth / 2);
            Renderer.Print(37, "          ░▒▒▒▒▓▒            ▒▒▓▓▓▒▒    ▒▓", margin: Console.WindowWidth / 2);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
        }
        #endregion
    }
}
