namespace ConsoleRPG
{
    public class RestScene : BaseScene
    {
        public override string Title { get; protected set; } = $"여 관";

        public override void EnterScene()
        {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("UseInn"));
            Options.Add(Managers.Scene.GetOption("Back"));
            DrawScene();
        }

        public override void NextScene()
        {
            do
            {
                Renderer.PrintOptions(15, Options, true, selectOptionIndex);
                GetInput();
            }
            while (Managers.Scene.CurrentScene is RestScene);
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, "집에서 휴식 하시겠습니까?");
            Renderer.Print(5, $"당신의 체력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
            Renderer.Print(7, $"당신의 마나 : {Game.Player.Mp} / {Game.Player.DefaultMpMax}");
            Renderer.Print(9, $"보유 골드 : {Game.Player.Gold} G");
            Renderer.Print(11, "이용 골드 : 100 G");

            Renderer.PrintKeyGuide("[ESC : 뒤로가기");
        }
    }
}
