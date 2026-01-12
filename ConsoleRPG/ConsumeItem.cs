
namespace ConsoleRPG
{
    public class ConsumeItem : Item
    {
        public string EffectDesc { get; set; }
        public ConsumeItem()
        {
            EffectDesc = string.Empty;
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }
        public ConsumeItem(int id, string name, string description, int price, int stackCount, 
                          ItemType itemType = ItemType.ConsumeItem, string effectDesc = null) : base(id, name, description, price,itemType, stackCount)
        {
            EffectDesc = effectDesc ?? "";
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }

        public ConsumeItem(ConsumeItem reference) : base(reference)
        {
            EffectDesc = reference.EffectDesc;
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }
        public void MergeItem(Character owner, Item duplicateItem)
        {
            // 중복된 아이템이 있을경우 받아온다 null 이 아니면 두 아이템의 개수를 합친다.
            StackCount += duplicateItem is ConsumeItem consume ? consume.StackCount : 0;
        }

        public virtual void UseEffect(Character owner)
        {
            if (StackCount > 1)
                StackCount--;
            else
                owner.Inventory.Remove(this);
        }
        public override Item DeepCopy() => new ConsumeItem(this);
    }

    public class HealingPotion : ConsumeItem
    {
        private int healValue;
        public int HealValue
        {
            get => healValue;
            set
            {
                healValue = value;
                EffectDesc = $"체력 {healValue} 회복";
            }
        }

        public HealingPotion() : base()
        {
        }
        public HealingPotion(int id, string name, string description, int price, int stackCount,
                            int healValue, ItemType itemType = ItemType.ConsumeItem, string effectDesc = null) : base (id, name, description, price, stackCount, itemType, effectDesc)
        {
            HealValue = healValue;
        }
        public HealingPotion(HealingPotion reference) : base(reference)
        {
            HealValue = reference.HealValue;
        }
        public override void UseEffect(Character owner)
        {
            base.UseEffect(owner);
            owner.Healing(healValue);
        }
        public override Item DeepCopy() => new HealingPotion(this);
    }
    public class ManaPotion : ConsumeItem
    {
        private int manaValue;
        public int ManaValue
        {
            get => manaValue;
            set
            {
                manaValue = value;
                EffectDesc = $"마나 {manaValue} 회복";
            }
        }
        public ManaPotion() : base()
        {
        }
        public ManaPotion(int id, string name, string description, int price, int stackCount,
                            int healValue, ItemType itemType = ItemType.ConsumeItem, string effectDesc = null) : base(id, name, description, price, stackCount, itemType, effectDesc)
        {
            ManaValue = healValue;
        }
        public ManaPotion(ManaPotion reference) : base(reference)
        {
            ManaValue = reference.ManaValue;
        }
        public override void UseEffect(Character owner)
        {
            base.UseEffect(owner);
            owner.Healing(manaValue);
        }
        public override Item DeepCopy() => new ManaPotion(this);
    }
}
