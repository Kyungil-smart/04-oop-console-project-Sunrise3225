

using System.Threading;

namespace ConsoleRPG
{
    public class Game
    {
        public static Character? Player { get; set; }
        public static Stage Stage = new Stage();
        public static Skill[] skills =
        {
            new Skill(new List<string>{"대쉬", "힐윈드"}, new List<float>{30, 20}, new List<int>{20, 25}),
            new Skill(new List<string>{"파이어볼", "불기둥"}, new List<float>{40, 25}, new List<int>{30, 50}),
            new Skill(new List<string>{"찌르기", "연속 베기"}, new List<float>{50, 15}, new List<int>{20, 30}),
        };

        // name, job, level, damage, defence, hp, mp, gold, critical, avoid
        public static Character[] Characters =
        {
            new Character("", "전사", 1, 10, 5, 100, 50, 1500, 0.15f, 0.1f, skills[0]),
            new Character("", "마법사", 1, 6, 2, 80, 100, 3000, 0.1f, 0.2f, skills[1]),
            new Character("", "도적", 1, 8, 4, 90, 70, 5000, 0.35f, 0.3f, skills[2]),
        };

        // name, hp, damage, defence, mp, critical, avoid
        public static Monster[] Monsters =
        {
            new Monster("슬라임", 10, 8, 1, 0, 0.1f, 0.1f),
            new Monster("오크", 20, 15, 3, 0, 0.1f, 0.05f),
            new Monster("스켈레톤", 13, 20, 2, 0, 0.15f, 0.02f),
        };
        // id, name, description, price, type, stackCount
        public static Item[] Items =
        {
            new Gear(1, "철제 검", "공격에 사용되는 무기", 1500, GearType.Weapon, 12, 0, 0, 0),
            new Gear(2, "원형 방패", "적의 공격을 방어하는 방패", 1000, GearType.Shield, 0, 6, 0, 0),
            new Gear(3, "사슬 갑옷", "철사 고리를 엮어 만든 갑옷", 2000, GearType.Armor, 0, 3, 0, 5),
            new Gear(4, "다이아몬드 검", "강력한 무기", 3000, GearType.Weapon, 33, 0, 12, 0),
            new Gear(5, "다이아몬드 방패", "강력한 방패", 2000, GearType.Shield, 0, 15, 0, 0),
            new Gear(6, "다이아몬드 갑옷", "강력한 갑옷", 4000, GearType.Armor, 8, 8, 8, 8),
            new HealingPotion(7, "체력 포션", "체력을 회복하는 물약", 150, 1, 50),
            new ManaPotion(8, "마나 포션", "마나를 회복하는 물약", 150, 1, 50),
        };
        public struct CurrentStageReward
        {
            public bool isClear;
            public int stageNumber;
            public List<Item> items;
            public int exp;
            public int gold;
        }
        public static CurrentStageReward StageReward = new CurrentStageReward();

        public void GameStart()
        {
            Renderer.Init();
            Managers.Game.Init();
            Managers.Scene.Init();
            Managers.Table.Init();
            Managers.Scene.EnterScene<TitleScene>();
        }
    }
}
