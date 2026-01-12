
using System.Text;

namespace ConsoleRPG
{
    public static class Renderer
    {
        private static readonly int printMargin = 2;                             // 벽에 붙어서 출력되지 않기 위해 주는 Margin
        private static readonly ConsoleColor bgColor = ConsoleColor.Black;       // 콘솔 기본 백그라운드 컬러
        private static readonly ConsoleColor textColor = ConsoleColor.White;     // 콘솔 기본 텍스트 컬러
        private static readonly ConsoleColor highlightColor = ConsoleColor.Yellow;  // 콘솔 기본 하이라이트 컬러

        private static int width; // 화면 가로 크기
        private static int height; // 화면 세로 크기

        public static void Init()
        {
            Console.Title = "GameName";
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = bgColor;
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }
        public static int Print(int line, string content, bool isHighlightNumber = false, int delay = 0, int margin = 2, bool clear = false)
        {
            if (clear) ClearLine(line);
            Console.SetCursorPosition(margin, line++);
            if (isHighlightNumber)
            {
                foreach (char c in content)
                {
                    if (char.IsDigit(c))
                    {
                        Console.ForegroundColor = highlightColor;
                        Console.Write(c);
                        Console.ForegroundColor = textColor;
                    }
                    else Console.Write(c);
                    if (delay > 0) Thread.Sleep(delay / content.Length);
                }
            }
            else
            {
                if (delay > 0)
                {
                    int characterDelay = delay / content.Length;
                    foreach (char c in content)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                        {
                            delay = 0;
                            characterDelay = 0;
                        }
                        Console.Write(c);
                        Thread.Sleep(characterDelay);
                    }
                }
                else Console.WriteLine(content);
            }
            return line;
        }
        public static int PrintCenter(int line, string content)
        {
            width = Console.WindowWidth; 

            int pad = width - 3 - GetPrintingLength(content);
            if (pad <= 0) // 내용이 너무 길거나 폭이 너무 작음
                return Print(line, content); // 가운데 포기하고 그냥 출력(안 터지게)

            int left = pad / 2;
            int right = pad - left; // 홀수도 안전하게 처리

            return Print(line, "".PadLeft(left) + content + "".PadRight(right));
        }

        public static int PrintOptions(int line, List<ActionOption> options, bool fromZero = true, int selectionLine = 0, int lineGap = 1)
        {
            for (int i = 0; i < options.Count; i++)
            {
                ActionOption option = options[i];
                Console.SetCursorPosition(printMargin, line);

                if (selectionLine == i)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                Console.Write(option.Description);
                Console.ForegroundColor = ConsoleColor.White;
                line += lineGap;
            }
            return line;
        }

        public static void PrintBattleText(int line, List<Creature> monsters, bool fromZero = true, int selectionLine = 0)
        {
            int margin = 33;
            int printWidthPos = Console.WindowWidth / 2 - margin;
            for (int i = 0; i < 2 + monsters.Count; i++)
            {
                Print(line + i, new string(' ', printWidthPos), false, 0, margin);
            }
            if (selectionLine >= 0)
                Print(line, "공격할 몬스터를 선택하세요.");

            Print(line++, "-------------------------", false, 0, margin);
            Print(line++, "        몬스터             ", false, 0, margin);
            Print(line++, "-------------------------", false, 0, margin);
            for (int i = 0; i < monsters.Count; i++)
            {
                Creature monster = monsters[i];
                Console.SetCursorPosition(margin, line);
                Console.Write(new string(' ', printWidthPos));
                Console.SetCursorPosition(margin, line);

                if (monster.IsDead())
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (selectionLine == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write(fromZero ? i : i + 1);
                Console.Write(". ");
                if (monster.IsDead())
                {
                    Console.WriteLine($"{monster.Name, -8} : Dead");
                    Console.SetCursorPosition(margin, ++line);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    PrintHPBar(monster);
                    line++;
                }
                else
                {
                    Console.WriteLine($"{monster.Name, -8} : {monster.Hp}/{monster.DefaultHpMax}");
                    Console.SetCursorPosition(margin, ++line);
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintHPBar(monster);
                    line++;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
            Print(line++, "-------------------------", false, 0, margin);
        }

        //몬스터 HP 출력
        public static void PrintHPBar(Creature monster)
        {
            // HP 상태바 길이 조절
            int statusBarLength = 15;
            // HP 백분율 계산
            int hpPercentage = (int)((double)monster.Hp / monster.DefaultHpMax * statusBarLength);
            string statusBar = new string('█', hpPercentage) + new string(' ', statusBarLength - hpPercentage);
            // HP 상태 바 출력
            Console.WriteLine($"[{statusBar}]");
        }

        public static void PrintSelectAction(int line, List<string> actionText, bool fromZero = true, int selectionLine = 0)
        {
            int printWidthPos = 30;
            line++;
            for (int i = 0; i < height - 21; i++)
            {
                Print(line + i, new string(' ', printWidthPos));
            }
            Print(line++, "---------------------------");
            PrintPlayerState(line);
            line += 3;
            Print(line++, "---------------------------");
            for (int i = 0; i < actionText.Count; i++)
            {
                // 라인 클리어
                Console.SetCursorPosition(printMargin, line);
                Console.Write(new string(' ', printWidthPos));
                Console.SetCursorPosition(printMargin, line);

                Console.Write(fromZero ? i : i + 1);
                Console.Write(". ");

                if (selectionLine == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(actionText[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
            Print(line++, "---------------------------");

        }

        //플레이어 상태 출력
        public static void PrintPlayerState(int line)
        {
            Print(line, new string(' ', 30));
            Print(line, $"내 캐릭터 : {Game.Player.Name,-8} [{Game.Player.Job}]");
            line++;
            // 상태바 길이 조절
            int statusBarLength = 18;

            // HP, MP 백분율 계산
            int hpPercentage = (int)((double)Game.Player.Hp / Game.Player.HpMax * statusBarLength);
            string HPBar = new string('█', hpPercentage) + new string(' ', statusBarLength - hpPercentage);

            int mpPercentage = (int)((double)Game.Player.Mp / Game.Player.MpMax * statusBarLength);
            string MPBar = new string('█', mpPercentage) + new string(' ', statusBarLength - mpPercentage);

            Print(line, "".PadLeft(25, ' '), false, 0, 2);
            // HP,MP 상태 바 출력
            Console.SetCursorPosition(printMargin, line);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{HPBar}] {Game.Player.Hp}/{Game.Player.HpMax}  ");
            line++;

            Print(line, "".PadLeft(25, ' '), false, 0, 2);
            Console.SetCursorPosition(printMargin, line);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[{MPBar}] {Game.Player.Mp}/{Game.Player.MpMax}  ");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public static void ClearPlayerLine()
        {
            for (int line = 7; line <= 11; line++)
            {
                int printWidthPos = Console.WindowWidth / 2;
                Renderer.ClearLine(line, printWidthPos - 3, printWidthPos);
            }
        }

        public static void ClearLine(int line, int exclusionLength = 0, int margin = 2) => 
                                     Print(line, "".PadLeft(width - 3 - exclusionLength, ' '), false, 0, margin);

        public static void PrintKeyGuide(string keyGuide)
        {
            ClearLine(height - 2);
            Print(height - 2, keyGuide);
        }

        public static int DrawTable<T>(int startRow, List<T> items, List<TableFormatter<T>> formatters, int selectedIndex = -1)
        {
            int row = startRow;

            string top = "┌";
            string mid = "├";
            string bottom = "└";

            // 타이틀 및 구분선 그리기
            string title = "│";
            string horizontal = "│";

            for (int i = 0; i < formatters.Count; i++)
            {
                TableFormatter<T> formatter = formatters[i];

                // 헤더 텍스트
                title += $"{formatter.GetTitle()}│";
                horizontal += $"{new string('─', formatter.length)}│";

                // 테두리 라인 구성
                top += new string('─', formatter.length);
                mid += new string('─', formatter.length);
                bottom += new string('─', formatter.length);

                if (i < formatters.Count - 1)
                {
                    top += "┬";
                    mid += "┼";
                    bottom += "┴";
                }
                else
                {
                    top += "┐";
                    mid += "┤";
                    bottom += "┘";
                }
            }

            // 상단 테두리
            Print(row++, top);
            // 헤더
            Print(row++, title);
            // 헤더-본문 구분선
            Print(row++, mid);

            // 본문 행 그리기
            for (int i = 0; i < items.Count; i++)
            {
                T item = items[i];
                string content = "│";

                for (int j = 0; j < formatters.Count; j++)
                {
                    TableFormatter<T> formatter = formatters[j];

                    if (formatter.key == "Index") 
                        content += $"{formatter.GetString(i + 1)}│";
                    else 
                        content += $"{formatter.GetString(item)}│";
                }

                if (selectedIndex == i)
                {
                    Console.ForegroundColor = highlightColor;
                    Print(row++, content);
                    Console.ForegroundColor = textColor;
                }
                else
                {
                    Print(row++, content);
                }
            }
            // 하단 테두리
            Print(row++, bottom);

            return row;
        }

        public static string GetTableElementString(int maxLength, string data, bool isTitle = false)
        {
            int dataLength = GetPrintingLength(data);
            if (data == "=") 
                return new string('=', maxLength);

            StringBuilder builder = new();
            int spaceCount = maxLength - dataLength;
            int margin = isTitle ? 2 : 1;
            int leftCount = Math.Clamp(spaceCount / 2, 0, margin);

            builder.Append(' ', leftCount);
            builder.Append(data);
            builder.Append(' ', spaceCount - leftCount);

            return builder.ToString();
        }

        #region Border
        public static void DrawBorder(string title = "")
        {
            Console.Clear();
            width = Console.WindowWidth;
            height = Console.WindowHeight;

            char H = '═';
            char V = '║';
            char TL = '╔', TR = '╗', BL = '╚', BR = '╝';

            // Top
            Console.SetCursorPosition(0, 0);
            Console.Write(TL);
            Console.Write(new string(H, width - 2));
            Console.Write(TR);

            // Sides
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(V);
                Console.SetCursorPosition(width - 1, i);
                Console.Write(V);
            }

            // Title line (optional)
            if (!string.IsNullOrEmpty(title))
            {
                // 가로 라인 한 줄 넣고 제목 중앙
                Console.SetCursorPosition(1, 2);
                Console.Write(new string(H, width - 2));

                int titleLen = GetPrintingLength(title);
                int start = (width - titleLen) / 2;
                if (start < 1) start = 1;

                Console.SetCursorPosition(start, 1);
                Console.Write(title);
            }

            // Bottom
            Console.SetCursorPosition(0, height - 1);
            Console.Write(BL);
            Console.Write(new string(H, width - 2));
            Console.Write(BR);
        }

        #endregion

        #region Utils

        public static int GetPrintingLength(string line) => line.Sum(c => IsKorean(c) ? 2 : 1);
        private static bool IsKorean(char c) => '가' <= c && c <= '힣';

        #endregion

    }
}
