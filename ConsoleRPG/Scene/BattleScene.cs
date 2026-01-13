namespace ConsoleRPG
{
    public class BattleScene : BaseScene
    {
        public override string Title { get; protected set; } = "지하 던전";
        public List<Creature> Monsters;
        public int MonsterCount;
        public List<string> ActionTextList;
        public List<string> AttackTextList;

        protected int startTextLine;
        protected int startTextLineClearLength;
        protected int line;
        protected int textDelay = 200;

        public delegate void GameEvent(Creature creature);
        public event GameEvent OnCreatureDead;

        public BattleScene()
        {
            startTextLine = 4;
            startTextLineClearLength = Console.WindowWidth / 4 * 3;
            Monsters = new List<Creature>();
            ActionTextList = new List<string>();
            AttackTextList = new List<string>();

            OnCreatureDead += BattleEnd;
        }
        public override void EnterScene()
        {
            // 몬스터 생성
            Monsters = Game.Stage.MonsterSpawn();
            MonsterCount = Monsters.Count;

            ActionTextList.Clear();
            ActionTextList.Add("몬스터 공격");
            ActionTextList.Add("체력 회복");
            ActionTextList.Add("던전 포기");

            AttackTextList.Clear();
            AttackTextList.Add("기본 공격");
            AttackTextList.Add("스킬 사용");

            DrawScene();
        }
        public override void NextScene()
        {
            Renderer.PrintKeyGuide(new string(' ', Console.WindowWidth - 2));
            ClearBuffer();
            for (int count = 9; count > 0; count--)
            {
                Renderer.PrintKeyGuide($"[아무 키 : 보상 맵] {count}초 뒤 보상 맵으로 이동");
                Thread.Sleep(1000);

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    break;
                }
            }
            Managers.Scene.EnterScene<StageRewardScene>();
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, $"{Game.Stage.StageLevel} 스테이지", false, 0, Console.WindowWidth / 2 - 5);
            Renderer.Print(4, new string('-', 55), false, 0, Console.WindowWidth / 2);
            Renderer.Print(5, $"{Game.Player.Name}의 공격", false, 0, Console.WindowWidth / 2);
            Renderer.Print(6, new string('-', 55), false, 0, Console.WindowWidth / 2);
            Renderer.Print(12, new string('-', 55), false, 0, Console.WindowWidth / 2);
            Renderer.Print(13, $"몬스터의 공격", false, 0, Console.WindowWidth / 2);
            Renderer.Print(14, new string('-', 55), false, 0, Console.WindowWidth / 2);
            Renderer.Print(27, new string('-', 55), false, 0, Console.WindowWidth / 2);
            Renderer.PrintBattleText(startTextLine, Monsters, false, -1);


            Renderer.Print(16, "      ▄           ▄       ", margin: 2);
            Renderer.Print(17, " ▄████ ▀▄       ▄▀ █████▄ ", margin: 2);
            Renderer.Print(18, "████▀█▀▄█████████▄▀█▀████ ", margin: 2);
            Renderer.Print(19, "▀▀ ▀▄█████▀▀▀▀▀ ▀███▄▀ ▀▀ ", margin: 2);
            Renderer.Print(20, "    ████▀▄  ██  ▄ ███     ", margin: 2);
            Renderer.Print(21, "   ▄███▀▄▀▀ ▀▀ ▀▀▄▀██▄    ", margin: 2);
            Renderer.Print(22, " ▄▄████ ███▄  ▄███ ███▄▄  ", margin: 2);
            Renderer.Print(23, "  █████  ▀▀    ▀▀  ████   ", margin: 2);
            Renderer.Print(24, "   ▀▀███▄ ▀▄▀▀▄▀ ▄██▀▀    ", margin: 2);
            Renderer.Print(25, "         ▀▄▄  ▄▄█         ", margin: 2);
            Renderer.Print(26, "        ▄▀▀▄▀▀▄▀▀▄        ", margin: 2);

            while (!CheckAllMonstersDead() && !Game.Player.IsDead())
            {
                SelectAction();
                if (CheckAllMonstersDead())
                    break;

                // 몬스터 턴
                // DrawScene() 몬스터 턴 부분 교체
                int logStart = 15;
                int logEnd = Math.Min(25, Console.WindowHeight - 3); // 아래 테두리/키가이드 피하기

                line = logStart;

                foreach (Monster monster in Monsters)
                {
                    if (Game.Player.IsDead())
                        break;
                    if (monster.IsDead())
                        continue;

                    if (line > logEnd) line = logStart;

                    Thread.Sleep(1000);
                    monster.Attack(Game.Player, line);

                    line++;
                }
                selectionIndex = 0;
            }

            if (CheckAllMonstersDead())
                OnCreatureDead(Monsters[0]);
            else if (Game.Player.IsDead())
                OnCreatureDead(Game.Player);
        }

        #region Input Action
        private int selectionIndex = 0;
        private Stack<int> indexStack = new Stack<int>();

        public bool ManagerInput(BattleAction action, bool isSkill = false)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            var commands = key.Key switch
            {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.None
            };
            switch (action)
            {
                case BattleAction.SelectAction:
                    ActionOnCommand(commands);
                    break;
                case BattleAction.SelectSkill: // 스킬 사용
                    bool isSkillNotUsed = true;
                    SkillOnCommand(commands, ref isSkillNotUsed);
                    if (commands == Command.Interact)
                        return isSkillNotUsed;
                    break;
                case BattleAction.SelectAttack: // 공격
                    SelectAttackOnCommand(commands);
                    break;
                case BattleAction.Attack:
                    AttackOnCommand(commands, isSkill);
                    break;
                case BattleAction.UsePotion: // 포션사용
                    bool isPotionNotUsed = true;
                    UsePotionOnCommand(commands, ref isPotionNotUsed);
                    if (commands == Command.Interact)
                        return isPotionNotUsed;
                    break;
            }
            // 첫 화면과 몬스터 공격에선 esc기능을 제외
            if ((action == BattleAction.SelectAction) || (action == BattleAction.Attack))
                return commands != Command.Interact;
            return commands != Command.Interact && commands != Command.Exit;
        }
        void ActionOnCommand(Command cmd)
        {
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIndex > 0)
                        --selectionIndex;
                    break;
                case Command.MoveBottom:
                    if (selectionIndex < ActionTextList.Count - 1)
                        ++selectionIndex;
                    break;
                case Command.Interact:
                    if (selectionIndex == 0)
                    {
                        StackPush();
                        SelectAttack();
                    }
                    else if (selectionIndex == 1)
                    {
                        StackPush();
                        UsePotion();
                    }
                    else if (selectionIndex == 2)
                    {
                        Managers.Scene.GetOption("Back").Excute();
                    }
                    break;
            }
        }

        void SelectAttackOnCommand(Command cmd)
        {
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIndex > 0)
                        --selectionIndex;
                    break;
                case Command.MoveBottom:
                    if (selectionIndex < ActionTextList.Count - 1)
                        ++selectionIndex;
                    break;
                case Command.Interact:
                    if (selectionIndex == 0)
                    {
                        StackPush();
                        MonsterAttack();
                    }
                    else
                    {
                        StackPush();
                        SelectSkill();
                    }
                    break;
                case Command.Exit:
                    StackPop();
                    SelectAction();
                    break;
            }
        }
        void AttackOnCommand(Command cmd, bool isSkill = false)
        {
            int tempSelection;
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIndex > 0)
                    {
                        tempSelection = selectionIndex;
                        --selectionIndex;
                        while (Monsters[selectionIndex].IsDead())
                        {
                            if (--selectionIndex < 0)
                            {
                                selectionIndex = tempSelection;
                                break;
                            }
                        }
                    }
                    break;
                case Command.MoveBottom:
                    if (selectionIndex < Monsters.Count - 1)
                    {
                        tempSelection = selectionIndex;
                        ++selectionIndex;
                        while (Monsters[selectionIndex].IsDead())
                        {
                            if (++selectionIndex > MonsterCount - 1)
                            {
                                selectionIndex = tempSelection;
                                break;
                            }
                        }
                    }
                    break;
                case Command.Interact:
                    int playerLine = 7;
                    Renderer.ClearPlayerLine();
                    // 단일 스킬
                    if (isSkill)
                    {
                        Renderer.Print(playerLine++, $"{Game.Player.PlayerSkill.Names[0]} 사용!", false, textDelay, Console.WindowWidth / 2);
                        Game.Player.Skill(Monsters[selectionIndex], ref playerLine, Game.Player.PlayerSkill.Damage[0]);
                    }
                    else
                        Game.Player.Attack(Monsters[selectionIndex], 7);
                    break;
            }
        }
        void SkillOnCommand(Command cmd, ref bool isSkillNotUsed)
        {
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIndex > 0)
                        --selectionIndex;
                    break;
                case Command.MoveBottom:
                    if (selectionIndex < 1)
                        ++selectionIndex;
                    break;
                case Command.Interact:
                    switch (selectionIndex)
                    {
                        case 0: // 단일 스킬
                            if (Game.Player.Mp < Game.Player.PlayerSkill.MpCost[0])
                            {
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                Renderer.Print(startTextLine, $"마나가 부족합니다!");
                            }
                            else
                            {
                                Game.Player.Mp -= Game.Player.PlayerSkill.MpCost[0];
                                MonsterAttack(true);
                                isSkillNotUsed = false;
                                Renderer.PrintPlayerState(6);
                            }
                            break;
                        case 1: // 광역 스킬
                            if (Game.Player.Mp < Game.Player.PlayerSkill.MpCost[1])
                            {
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                Renderer.Print(startTextLine, $"마나가 부족합니다!");
                            }
                            else
                            {
                                int playerLine = 7;
                                Game.Player.Mp -= Game.Player.PlayerSkill.MpCost[1];
                                Renderer.ClearPlayerLine();
                                Renderer.Print(playerLine++, $"{Game.Player.PlayerSkill.Names[1]} 사용!", false, textDelay, Console.WindowWidth / 2);
                                foreach(Monster monster in Monsters)
                                {
                                    Game.Player.Skill(monster, ref playerLine, Game.Player.PlayerSkill.Damage[1]);
                                    Renderer.PrintBattleText(startTextLine, Monsters, false, -1);
                                }
                                isSkillNotUsed = false;
                                Renderer.PrintPlayerState(6);
                            }
                            break;
                    }
                    break;
                case Command.Exit:
                    StackPop();
                    SelectAttack();
                    break;
            }
        }
        void UsePotionOnCommand(Command cmd, ref bool isPotionNotUsed)
        {
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIndex > 0)
                        --selectionIndex;
                    break;
                case Command.MoveBottom:
                    if (selectionIndex < 2)
                        ++selectionIndex;
                    break;
                case Command.Interact:
                    switch (selectionIndex)
                    {
                        case 0:
                            if (Game.Player.Inventory.HasSameItem(Game.Items[6], out Item? hpPotion))
                            {
                                // 포션이 있을 때
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                if (Game.Player.Hp >= Game.Player.HpMax)
                                    Renderer.Print(startTextLine, "이미 체력이 최대입니다!");
                                else
                                {
                                    hpPotion.Use(Game.Player);
                                    Renderer.Print(startTextLine, "HP 포션을 사용했습니다!");
                                    isPotionNotUsed = false;
                                    Renderer.PrintPlayerState(6);
                                }
                            }
                            else
                            {
                                // 포션이 없을 때
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                Renderer.Print(startTextLine, "HP 포션이 부족합니다!");
                            }
                            break;
                        case 1:
                            if (Game.Player.Inventory.HasSameItem(Game.Items[7], out Item? mpPotion))
                            {
                                // 포션이 있을 때
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                if (Game.Player.Mp >= Game.Player.MpMax)
                                    Renderer.Print(startTextLine, "이미 마나가 최대입니다!");
                                else
                                {
                                    mpPotion.Use(Game.Player);
                                    Renderer.Print(startTextLine, "MP 포션을 사용했습니다!");
                                    isPotionNotUsed = false;
                                    Renderer.PrintPlayerState(6);
                                }
                            }
                            else
                            {
                                // 포션이 없을 때
                                Renderer.ClearLine(startTextLine, startTextLineClearLength);
                                Renderer.Print(startTextLine, "MP 포션이 부족합니다!");
                            }
                            break;
                    }
                    break;
                case Command.Exit:
                    StackPop();
                    SelectAction();
                    break;
            }
        }
        #endregion

        #region Battle Action
        void SelectAction()
        {
            indexStack.Clear();
            Renderer.Print(startTextLine, "원하는 행동을 선택해주세요");
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 이동] [Enter: 선택] [ESC: 뒤로가기]");
            do
            {
                Renderer.PrintSelectAction(startTextLine, ActionTextList, false, selectionIndex);
                ClearBuffer();
            }
            while (ManagerInput(BattleAction.SelectAction));
        }
        void MonsterAttack(bool isSkill = false)
        {
            selectionIndex = 0;
            while (Monsters[selectionIndex].IsDead())
                selectionIndex++;
            // 선택한 옵션 색 초기화
            Renderer.PrintSelectAction(startTextLine, AttackTextList, false, -1);
            Renderer.PrintBattleText(startTextLine, Monsters, false, -1);
            do
            {
                Renderer.PrintBattleText(startTextLine, Monsters, false, selectionIndex);
                ClearBuffer();
            }
            while (ManagerInput(BattleAction.Attack, isSkill));
            Renderer.PrintBattleText(startTextLine, Monsters, false, -1);
        }
        void SelectAttack()
        {
            Renderer.Print(startTextLine, "공격할 방법을 선택해주세요.");
            do
            {
                Renderer.PrintSelectAction(startTextLine, AttackTextList, false, selectionIndex);
                ClearBuffer();
            } while (ManagerInput(BattleAction.SelectAttack));
        }
        void SelectSkill()
        {
            List<string> skill = Game.Player.PlayerSkill.Names;
            Renderer.Print(startTextLine, "사용할 스킬을 선택해주세요.");
            do
            {
                Renderer.PrintSelectAction(startTextLine, skill, false, selectionIndex);
                ClearBuffer();
            }
            while (ManagerInput(BattleAction.SelectSkill));
        }
        void UsePotion()
        {
            Renderer.Print(startTextLine, "사용할 포션을 선택해주세요.");
            int? hpPotionCount;
            int? mpPotionCount;

            do
            {
                if (Game.Player.Inventory.HasSameItem(Game.Items[6], out Item? hpPotion))
                    hpPotionCount = (hpPotion.StackCount == null) ? 0 : hpPotion.StackCount;
                else
                    hpPotionCount = 0;

                if (Game.Player.Inventory.HasSameItem(Game.Items[7], out Item? mpPotion))
                    mpPotionCount = (mpPotion.StackCount == null) ? 0 : mpPotion.StackCount;
                else
                    mpPotionCount = 0;
                List<string> potionStateList = new List<string>
                {
                    $"HP 포션 : {hpPotionCount}개",
                    $"MP 포션 : {mpPotionCount}개"
                };
                Renderer.PrintSelectAction(startTextLine, potionStateList, false, selectionIndex);
                ClearBuffer();
            } while (ManagerInput(BattleAction.UsePotion));
            Renderer.PrintPlayerState(6);
        }
        public bool CheckAllMonstersDead()
        {
            foreach (Monster monster in Monsters)
            {
                if (!monster.IsDead())
                    return false;
            }
            return true;
        }
        public void BattleEnd(Creature creature)
        {
            if (creature is Monster)
            {
                Renderer.Print(14, "던전 클리어!");
                Game.Stage.StageClearReward(); // 클리어 보상
            }
            else
            {
                Renderer.Print(14, "던전 클리어 실패!");
                Game.Stage.StageFailReward(); // 실패 보상
            }
        }
        #endregion

        #region Utils
        void ClearBuffer()
        {
            while (Console.KeyAvailable) // 버퍼에 입력이 있는 경우 처리
                Console.ReadKey(true);   // 입력을 읽고 버퍼를 비움
        }
        void StackPush()
        {
            indexStack.Push(selectionIndex);
            selectionIndex = 0;
        }
        void StackPop()
        {
            selectionIndex = indexStack.Pop();
        }
        #endregion
    }
}
