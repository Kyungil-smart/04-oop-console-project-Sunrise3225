

namespace ConsoleRPG
{
    public static class Extensions
    {
        // 아이템의 이름이 빈 문자열인 경우 : true
        public static bool IsEmptyItem(this Gear gear) => string.IsNullOrEmpty(gear.Name);

        // 아이템 타입을 문자로 리턴
        public static string String(this ItemType type)
        {
            switch (type)
            {
                case ItemType.Gear:
                    return "장비";
                case ItemType.ConsumeItem:
                    return "소모품";
                default:
                    return "";
            }
        }
        public static string String(this GearType type)
        {
            switch (type)
            {
                case GearType.Weapon:
                    return "무기";
                case GearType.Shield:
                    return "방패";
                case GearType.Armor:
                    return "방어구";
                default:
                    return "";
            }
        }
        public static string StatToString(this Gear gear)
        {
            string stat = string.Empty;

            if (gear.Attack != 0)
                stat += $"Attack {gear.Attack} ";

            if (gear.Defence != 0)
                stat += $"Defence {gear.Defence} ";

            if (gear.Critical != 0)
                stat += $"Critical {gear.Critical} ";

            if (gear.Avoid != 0)
                stat += $"Avoid {gear.Avoid} ";

            return stat;
        }
    }
}
