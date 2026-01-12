using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class InventoryInfoScene : BaseScene
    {
        public override string Title { get; protected set; } = "인 벤 토 리";
        private List<TableFormatter<Item>> formatters = new();

        public override void EnterScene()
        {
            // 선택지 설정
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Back"));

            // 테이블 설정
            formatters = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Desc", "StackCount" });

            Renderer.DrawBorder(Title);
            DrawScene();
        }
        public override void NextScene()
        {
            do
            {
                GetInput();
            } while (Managers.Scene.CurrentScene is InventoryInfoScene);
        }
        protected override void DrawScene()
        {
            int row = 4;
            row = Renderer.Print(row, "이 곳은 인벤토리");
            row = Renderer.Print(row, "인벤토리 정보를 보여줍니다.");

            Renderer.DrawTable(++row, Game.Player.Inventory.Items, formatters);

            Renderer.PrintKeyGuide("[ESC : 뒤로가기]");
        }
    }
}
