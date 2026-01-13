namespace ConsoleRPG
{
    public class DungeonGateScene : BaseScene
    {
        public override string Title { get; protected set; } = "던 전 입 구";
        private int selectionIndex = 0; // 키 조작
        public override void EnterScene()
        {
            Options.Clear();
            if (Game.Player.Hp >= 20)
                Options.Add(Managers.Scene.GetOption("DungeonEnter"));

            DrawScene();
        }
        public override void NextScene()
        {
            Renderer.PrintOptions(22, Options, true, selectionIndex);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter && Game.Player.Hp >= 20)
                    Managers.Scene.GetOption("DungeonEnter").Excute();

                if (key.Key == ConsoleKey.Escape)
                    Managers.Scene.GetOption("Main").Excute();
            }
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(5, $"Lv. {Game.Player.Level}");
            Renderer.Print(7, $"{Game.Player.Name} ( {Game.Player.Job} )");
            Renderer.Print(9, $"공격력 : {Game.Player.Damage}");
            Renderer.Print(11, $"방어력 : {Game.Player.Defence}");
            Renderer.Print(13, $"체 력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
            Renderer.Print(15, $"마 나 : {Game.Player.Mp} / {Game.Player.DefaultMpMax}");
            Renderer.Print(17, $"Gold : {Game.Player.Gold:##,#0} G");
            if (Game.Player.Hp < 20)
            {
                Renderer.Print(22, "체력이 부족하여 던전에 입장할 수 없습니다(체력 20이상 필요)");
                Renderer.PrintKeyGuide("[ESC : 메인화면]");
            }
            else
                Renderer.PrintKeyGuide("[ESC : 메인화면] [Enter : 던전 입장]");
            Renderer.Print(19, $"다음 단계 : {Game.Stage.StageLevel} 스테이지");
        }
    }
}
