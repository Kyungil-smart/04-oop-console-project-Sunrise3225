namespace ConsoleRPG
{
    public class StageRewardScene : BaseScene
    {
        public override string Title { get; protected set; } = "보 상";

        public override void EnterScene()
        {
            Options.Clear();
            Renderer.DrawBorder(Title);
            DrawScene();
        }
        public override void NextScene()
        {
            while (Game.StageReward.exp > 0 || Game.StageReward.gold > 0)
            {
                if (Game.StageReward.exp > 0)
                {
                    Game.Player.TotalExp++;
                    Game.StageReward.exp--;
                    if (Game.Player.TotalExp >= Game.Player.NextLevelExp)
                        Game.Player.ChangeExp(0);
                }
                if (Game.StageReward.gold > 7)
                {
                    Game.Player.Gold += 7;
                    Game.StageReward.gold -= 7;
                }
                else
                {
                    Game.Player.Gold += Game.StageReward.gold;
                    Game.StageReward.gold = 0;
                }
                DrawScene();
                Thread.Sleep(10);
                if (Skip())
                    break;
            }
            // 스킵했을 경우 남아있는 수치를 모두 합침
            Game.Player.TotalExp += Game.StageReward.exp;
            Game.StageReward.exp = 0;
            if (Game.Player.TotalExp >= Game.Player.NextLevelExp)
                Game.Player.ChangeExp(0);
            Game.Player.Gold += Game.StageReward.gold;
            Game.StageReward.gold = 0;

            DrawScene();
            Managers.Game.SaveGame();
            GetInput();
            Managers.Scene.EnterScene<DungeonGateScene>();
        }
        protected override void DrawScene()
        {
            int row = Renderer.PrintCenter(3, $"스테이지 {Game.StageReward.stageNumber} {(Game.StageReward.isClear ? "클리어" : "실패")}");

            row = Renderer.PrintCenter(row + 3, "Level");
            row = Renderer.PrintCenter(row, $"{Game.Player.Level}");
            row = DrawEXPBar(row + 1);
            row = Renderer.PrintCenter(row, $"{Game.Player.TotalExp} / {Game.Player.NextLevelExp}");
            row = Renderer.PrintCenter(row + 1, "획득한 골드");
            if (Game.StageReward.gold > 0)
                row = Renderer.PrintCenter(row, $"{Game.Player.Gold} G +{Game.StageReward.gold} G");
            else
                row = Renderer.PrintCenter(row, $"{Game.Player.Gold} G");
            Renderer.PrintKeyGuide("[아무 키] : 던진 입구로 돌아가기");
        }

        int DrawEXPBar(int line)
        {
            string bar;
            int extraCount = 50;
            float rate = (float)Game.Player.TotalExp / Game.Player.NextLevelExp;
            int barCount = (int)(extraCount * rate);

            if (barCount > extraCount)
                barCount = extraCount;

            extraCount -= barCount;
            Console.ForegroundColor = ConsoleColor.Green;
            bar = $"[{new('█', barCount)}{new(' ', extraCount)}]";
            line = Renderer.PrintCenter(line, bar);
            Console.ForegroundColor = ConsoleColor.White;
            return line;
        }
        bool Skip()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                return true;
            }
            return false;
        }
    }
}
