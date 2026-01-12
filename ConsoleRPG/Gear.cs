namespace ConsoleRPG
{
    public enum GearType
    {
        Weapon,
        Shield,
        Armor,
        None,
    }
    public class Gear : Item
    {
        public GearType GearType { get; set; }
        public float Attack { get; set; }    // 공격력
        public float Defence { get; set; }   // 방어력
        public float Critical { get; set; }  // 치명타
        public float Avoid { get; set; }     // 회피율
        public bool IsEquip { get; set; } // 착용 여부 확인

        public Gear() { }
        public Gear(int id, string name, string description, int price, GearType gearType, float attack, float defence,
                    float critical, float avoid, ItemType itemType = ItemType.Gear, bool isEquip = false) : base(id, name, description, price, itemType)
        {
            GearType = gearType;
            Attack = attack;
            Defence = defence;
            Critical = critical;
            Avoid = avoid;
            IsEquip = isEquip;
            OnRemoved += (owner) => { if (owner.Equipment.Equipped[(GearSlot)GearType] == this) owner.Equipment.Unequip((GearSlot)GearType); };

        }
        public Gear(Gear reference) : base(reference)
        {
            GearType = reference.GearType;
            Attack = reference.Attack;
            Defence = reference.Defence;
            Critical = reference.Critical;
            Avoid = reference.Avoid;
            IsEquip = reference.IsEquip;
            OnRemoved += (owner) => { if (owner.Equipment.Equipped[(GearSlot)GearType] == this) owner.Equipment.Unequip((GearSlot)GearType); };
        }
        // 장비 슬롯이 비어있는 경우 빈 아이템 생성
        public static Gear Empty = new(-1, string.Empty, string.Empty, 0, GearType.None, 0, 0, 0, 0, 0);
        public override Item DeepCopy() => new Gear(this);
    }
}
