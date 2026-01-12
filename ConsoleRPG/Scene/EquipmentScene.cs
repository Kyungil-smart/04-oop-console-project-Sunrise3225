using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    public class EquipmentScene : BaseScene
    {
        public override string Title { get; protected set; } = "장 비 창";

        enum EquipStep
        {
            Show,      // 기본 모드
            Equipment, // 관리/장착 모드
        }
        EquipStep step;
        List<Item> gearList = new List<Item>();
        List<TableFormatter<Item>> formatters = new List<TableFormatter<Item>>();

        public override void EnterScene()
        {
            // 씬 설정
            step = EquipStep.Show;
            selectOptionIndex = 0;

            // 선택지 설정
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Back"));

            // 테이블 및 아이템 설정
            formatters = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Effect", "Desc" });
            gearList = Game.Player.Inventory.Items.Where(item => item.Type == ItemType.Gear).ToList(); // 장비 아이템만 따로 리스트 캐싱

            Renderer.DrawBorder(Title);
        }

        public override void NextScene()
        {
            do
            {
                DrawStep();
                GetInput();
            }
            while (Managers.Scene.CurrentScene is EquipmentScene);
        }
        void DrawStep()
        {
            if (step == EquipStep.Show)
            {
                int row = 4;
                row = Renderer.Print(row, "장 비 창 - 보 기");
                Renderer.DrawTable(row, gearList, formatters);
                Renderer.PrintKeyGuide("[Enter : 관리모드] [ESC : 뒤로가기]");
            }
            else
            {
                int row = 4;
                row = Renderer.Print(row, "장 비 창 - 관 리");
                Renderer.DrawTable(row, gearList, formatters, selectOptionIndex);

                Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 장착] [ESC : 보기모드]");
            }
        }

        #region Input
        protected override void OnCommandMoveTop()
        {
            if (step == EquipStep.Equipment && selectOptionIndex > 0)
                selectOptionIndex--;
        }
        protected override void OnCommandMoveBottom()
        {
            if (step == EquipStep.Equipment && selectOptionIndex < gearList.Count - 1)
                selectOptionIndex++;
        }
        protected override void OnCommandInteract()
        {
            if (step == EquipStep.Show)
                step = EquipStep.Equipment;
            else if (step == EquipStep.Equipment)
                EquipFromInventory();
        }
        protected override void OnCommandExit()
        {
            if (step == EquipStep.Show)
                Options[0].Excute();
            else if (step == EquipStep.Equipment)
                step = EquipStep.Show;
        }
        #endregion
        void EquipFromInventory()
        {
            Item? item = gearList.ElementAtOrDefault(selectOptionIndex);

            if (item is Gear gear)
            {
                switch (gear.GearType)
                {
                    case GearType.Weapon:
                        Game.Player.Equipment.Equip(GearSlot.Weapon, gear);
                        break;
                    case GearType.Shield:
                        Game.Player.Equipment.Equip(GearSlot.Shield, gear);
                        break;
                    case GearType.Armor:
                        Game.Player.Equipment.Equip(GearSlot.Armor, gear);
                        break;

                }
            }
        }
    }
}
