namespace ConsoleRPG
{
    public enum GearSlot
    {
        Weapon,
        Shield,
        Armor,
    }
    public class Equipment
    {
        private Dictionary<GearSlot, Gear> equipped = new Dictionary<GearSlot, Gear>();
        public IReadOnlyDictionary<GearSlot, Gear> Equipped => equipped;
        public Equipment()
        {
            GearSlot[] slots = Enum.GetValues<GearSlot>();

            foreach (GearSlot slot in slots)
            {
                if (equipped.GetValueOrDefault(slot) != null)
                    continue;
                equipped[slot] = Gear.Empty;
            }
        }
        public void Equip(GearSlot slot, Gear gear)
        {
            equipped.TryGetValue(slot, out Gear? item);
            // 같은 장비를 착용중인지 확인
            if (item == gear)
            {
                Unequip(slot);
                return;
            }
            // 해당 장비창이 비어있지 않은지 확인
            if (!equipped[slot].IsEmptyItem())
            {
                Unequip(slot);
            }

            equipped[slot] = gear; // 장비 착용
            StatAdd(equipped[slot]);  // 장비 스탯 추가
            equipped[slot].IsEquip = true;
            Managers.Game.SaveGame();
        }
        public void Unequip(GearSlot slot)
        {
            StatSubtract(equipped[slot]); // 장비 스탯 삭제
            equipped[slot].IsEquip = false;
            equipped[slot] = Gear.Empty;
            Managers.Game.SaveGame();
        }
        public void StatAdd(Gear gear)
        {
            Game.Player.DefaultDamage += gear.Attack;
            Game.Player.DefaultDefence += gear.Defence;
            Game.Player.DefaultCritical += (gear.Critical * 0.01f);
            Game.Player.DefaultAvoid += (gear.Avoid * 0.01f);
        }
        public void StatSubtract(Gear gear)
        {
            Game.Player.DefaultDamage -= gear.Attack;
            Game.Player.DefaultDefence -= gear.Defence;
            Game.Player.DefaultCritical -= (gear.Critical * 0.01f);
            Game.Player.DefaultAvoid -= (gear.Avoid * 0.01f);
        }
    }
}
