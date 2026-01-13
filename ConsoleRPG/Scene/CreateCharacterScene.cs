namespace ConsoleRPG
{
    public class CreateCharacterScene : BaseScene
    {
        private enum CreateStep
        {
            Name,
            Job,
            CreateCharacter,
            Exit,
        }
        private Character? selectPlayer;
        private List<TableFormatter<Character>> formatters = new();
        private CreateStep step = CreateStep.Name;
        private string createName = string.Empty;
        private string errorMessage = string.Empty;
        public override string Title { get; protected set; } = "캐릭터 선택창";

        public override void EnterScene()
        {
            step = CreateStep.Name;
            formatters = Managers.Table.GetFormatters<Character>
                (new string[] { "Job", "Damage", "Defence", "HpMax", "MpMax", "Critical", "Avoid" });

            DrawScene();
        }
        public override void NextScene()
        {
            while (step == CreateStep.Name)
            {
                DrawStep();
                ReadName();
            }
            while (step == CreateStep.Job)
            {
                DrawStep();
                GetInput();
            }
        }
        protected override async void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, "아득한 옛날, 하늘의 신들은 서로의 신념을 두고 끝없는 전쟁을 벌였습니다.", delay: 1000);
            Renderer.Print(5, "천둥과 불꽃이 하늘을 가르고, 별들이 부서질 만큼 격렬한 싸움이 이어졌습니다.", delay: 1000);
            Renderer.Print(7, "마침내 전쟁은 끝났고, 한 쪽의 신들은 패배의 낙인을 안게 되었습니다.", delay: 1000);

            Renderer.Print(9, "승리한 신들은 패배한 신들을 하늘에서 몰아내어, 인간의 세계로 추락시켰습니다.", delay: 1000);
            Renderer.Print(11, "그 충격으로 한 신은 모든 기억과 힘을 잃고, 이름조차 흐릿해졌습니다.", delay: 1000);

            Renderer.Print(13, "많은 시간이 흐른 뒤…..", delay: 1000);
            Renderer.Print(15, "20XX년, 이름 없는 산의 깊은 구멍 아래", delay: 1000);
            Renderer.Print(17, "어둠 속에서 한 존재가 눈을 떴습니다.", delay: 1000);

            Renderer.Print(20, "기억을 잃고 떨어진 신의 이름은?", delay: 1500);
        }
        private void DrawStep()
        {
            switch (step)
            {
                case CreateStep.Name:
                    Renderer.Print(25, errorMessage, clear: true);
                    Renderer.PrintKeyGuide("[Enter: 결정]");
                    break;
                case CreateStep.Job:
                    Renderer.Print(11, "  어머, 깨어났네? ");
                    Renderer.Print(12, "  반가워! 난 이곳을 지키는 작은 요정이야.");

                    Renderer.Print(13, "  이 반짝이는 지하 세계에선, 길을 잃기 딱 좋거든!");
                    Renderer.Print(15, "  근데 너... 처음 보는 얼굴인데?");
                    Renderer.Print(16, "  왜 인간 세계에서 이곳으로 떨어진 거야?");
                    Renderer.Print(18, "  흠.. 기억이 흐릇해 보이네. 다들 처음엔 그래");
                    Renderer.Print(19, "  걱정 마! 내가 이곳의 규칙 정도는 알려줄게");
                    Renderer.Print(20, "  살아남으려면, 네가 뭘 할 수 있는지부터 알아야 하거든!");
                    Renderer.Print(22, "  근데, 너의 직업은 뭐야?");

                    Renderer.Print(11, "   ▒▒▒▒▒▒▒▒▒                                           ░▒▒▒▒▒▒", margin: Console.WindowWidth - 63);
                    Renderer.Print(12, "▒▒▒▒▒▒▒▒▒▒ ▒▒▒                                ░▒▒▒▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(13, " ▒▒▒▒▒ ▒▒▒▒▒▒▒▒▒▒▒       ▓       ▓▓▓      ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(14, "   ▒▒▒▒▒▒▒ ▒▒▒▒▒▒▒▒       ▓▓   ▓▓      ░░ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒░", margin: Console.WindowWidth / 2);
                    Renderer.Print(15, "  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒        ███ ░    ░▒▒▒▒▒▒░▒▒▒▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(16, "   ▒▒▒▒▒▒▒▒▒▒▒░▒▒▒▒▒       █████▒░ ▒▒▒▒▒▒▒▒▒░▒▒▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(17, "     ░▒▒▒▒▒▒▒░▒▒▒▒▒▒       ▒███   ░░▓▓▓▓░░▒▒▒▒░▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(18, "     ▒▒▒▒▒░░  ░░░░▓▓▓▓▓  ▓▒██████▓▓▓▓▓▓▓▓▓▓▓░▒▒▒▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(19, "         ▒▒  ░░░▓▓▓▓▓▓▓▓▓░████████░▓▓▓▓▓▓▓▓▓▓░▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(20, "         ▒▒▒▒▒▒▓▓▓▓▓▓    █▒▒███▓▒█░▓▓▓▓▓▓  ▓░▓▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(21, "        ░░▒▒▓▓▓▓▓▓▓░▓▓  ██ ▓███▓░░█░░▓▓▓▓░ ▓▓▓▓▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(22, "         ░▓▓▓▓▓▓▓▓▓▓▓▓▓▒█▒███████▓ ██░▓▓▓▓▓▓▓▓▓▒▒▒░", margin: Console.WindowWidth / 2);
                    Renderer.Print(23, "          ▒▒▓▓▓▓▓▓▓▓▓▒▒ ███████████░██░░░▓▓▓▓▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(24, "          ▒▒▓▓▓▒░░ ▒    ██▒██▒ ███▒▒     ░▒▒▒▒▒▒", margin: Console.WindowWidth / 2);
                    Renderer.Print(25, "           ▒▒              ██▒  ▒██░           ░", margin: Console.WindowWidth / 2);
                    Renderer.Print(26, "                           ██▒   ▒██", margin: Console.WindowWidth / 2);
                    Renderer.Print(27, "                           ██▒    ▓██▓", margin: Console.WindowWidth / 2);
                    Renderer.Print(28, "                            █▓       ▓▓▓", margin: Console.WindowWidth / 2);
                    Renderer.Print(29, "                            ▓█        ▓▓▓", margin: Console.WindowWidth / 2);
                    Renderer.Print(30, "                            ▓▓░        ▓▓▓▒", margin: Console.WindowWidth / 2);

                    int row = 2;
                    row = Renderer.DrawTable(row, Game.Characters.ToList(), formatters, selectOptionIndex) + 1;
                    Renderer.Print(row, errorMessage, clear: true);
                    Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 결정]");
                    break;
            }
        }
        private void NextStep()
        {
            errorMessage = string.Empty;
            step += 1;
            if (step == CreateStep.Job)
                Renderer.DrawBorder();
            else if (step == CreateStep.CreateCharacter)
                CreateCharacter();
            else if (step == CreateStep.Exit)
                Managers.Scene.EnterScene<MainScene>();
        }

        #region Input
        protected override void OnCommandMoveTop()
        {
            if (step == CreateStep.Job && selectOptionIndex > 0)
                selectOptionIndex--;
        }
        protected override void OnCommandMoveBottom()
        {
            if (step == CreateStep.Job && selectOptionIndex < Game.Characters.Length - 1)
                selectOptionIndex++;
        }
        protected override void OnCommandInteract()
        {
            if (step == CreateStep.Job)
                ReadJob();
        }
        protected override void OnCommandExit()
        {
        }
        #endregion

        void ReadName()
        {
            Console.CursorVisible = true;
            Renderer.ClearLine(23);
            Console.SetCursorPosition(2, 23);
            string? name = Console.ReadLine();
            OnNameChanged(name);
            Console.CursorVisible = false;
        }
        void ReadJob()
        {
            selectPlayer = Game.Characters[selectOptionIndex];
            NextStep();
        }
        void OnNameChanged(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "오류 : 이름을 입력해 주세요";
                return;
            }
            if (Renderer.GetPrintingLength(name) > 10)
            {
                errorMessage = "오류 : 이름이 너무 깁니다. 10글자 이내로 작성해 주세요.";
                return;
            }
            // 이름 결정
            createName = name;
            NextStep();
        }

        // 캐릭터 생성
        private void CreateCharacter()
        {
            Game.Player = new Character
            (
                createName,
                selectPlayer.Job,
                selectPlayer.Level,
                (int)selectPlayer.DefaultDamage,
                (int)selectPlayer.DefaultDefence,
                (int)selectPlayer.DefaultHpMax,
                (int)selectPlayer.DefaultMpMax,
                selectPlayer.Gold,
                selectPlayer.Critical,
                selectPlayer.Avoid,
                selectPlayer.PlayerSkill
            );

            Gear basicWeapon = Game.Items[0].DeepCopy() as Gear;
            Gear basicShield = Game.Items[1].DeepCopy() as Gear;
            Gear basicArmor = Game.Items[2].DeepCopy() as Gear;
            // 인벤토리 추가
            Game.Player.Inventory.Add(basicWeapon);
            Game.Player.Inventory.Add(basicShield);
            Game.Player.Inventory.Add(basicArmor);
            // 부위별 장착
            Game.Player.Equipment.Equip((GearSlot)basicWeapon.GearType, basicWeapon);
            Game.Player.Equipment.Equip((GearSlot)basicShield.GearType, basicShield);
            Game.Player.Equipment.Equip((GearSlot)basicArmor.GearType, basicArmor);

            // 게임 데이터에 캐릭터 데이터 저장
            Managers.Game.data.character = Game.Player;
            Managers.Game.data.stage = Game.Stage;
            Managers.Game.SaveGame();

            NextStep();
        }
    }
}
