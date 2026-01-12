namespace ConsoleRPG
{
    public class CharacterInfoScene : BaseScene
    {
        public override string Title { get; protected set; } = "상 태 보 기";

        public override void EnterScene()
        {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Back"));
            DrawScene();
        }
        public override void NextScene()
        {
            do
            {
                GetInput();
            } while (Managers.Scene.CurrentScene is CharacterInfoScene);
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, "캐릭터의 정보가 표시됩니다.");

            // 캐릭터 정보 표시
            Renderer.Print(5, $"Lv. {Game.Player.Level}", true);
            Renderer.Print(5, $"{Game.Player.Name} ( {Game.Player.Job} )",isHighlightNumber: true, margin: 10);
            Renderer.Print(8, $"공격력 : {Game.Player.DefaultDamage}");
            Renderer.Print(10, $"방어력 : {Game.Player.DefaultDefence}");
            Renderer.Print(12, $"체  력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
            Renderer.Print(14, $"마  나 : {Game.Player.Mp} / {Game.Player.DefaultMpMax}");
            Renderer.Print(16, $"경험치 : {Game.Player.TotalExp} / {Game.Player.NextLevelExp}");
            Renderer.Print(18, $"치명타 :{Game.Player.Critical * 100:00}%");
            Renderer.Print(20, $"회피율 :{Game.Player.Avoid * 100:00}%");
            Renderer.Print(22, $"Gold : {Game.Player.Gold} G");

            Renderer.PrintKeyGuide("[ESC : 뒤로가기]");
        }
    }
}
