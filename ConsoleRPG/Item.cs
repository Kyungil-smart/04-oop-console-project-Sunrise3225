
using System.Diagnostics;
using System.Security.Principal;
using System.Xml.Linq;

namespace ConsoleRPG
{
    public enum ItemType
    {
        Gear,
        ConsumeItem,
    }
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }

        public int? StackCount { get; set; } // 아이템 누적

        public Item() { }
        public Item(int id, string name, string description, int price, ItemType type, int? stackCount = null)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            Type = type;
            StackCount = stackCount;
        }
        public Item(Item reference)
        {
            ID = reference.ID;
            Name = reference.Name;
            Description = reference.Description;
            Price = reference.Price;
            Type = reference.Type;
            StackCount = reference.StackCount;
        }
        // 아이템이 사용될 때 호출될 이벤트 (장비면 장착, 소모품이면 사용되고 count - 1)
        protected event Action<Character>? OnUsed;
        // 아이템이 인벤토리에 추가될 때 호출 (소모품의 경우 인벤에 같은 아이템이 있다면 count + 1)
        protected event Action<Character, Item>? OnAdded;
        // 아이템이 인벤토리에서 삭제될 때 호출 (장착중인 아이템이 삭제된다면 장착해제도 같이 진행)
        protected event Action<Character>? OnRemoved;

        public void Use(Character owner) => OnUsed?.Invoke(owner);
        public void OnAdd(Character owner, Item item = null) => OnAdded?.Invoke(owner, item);
        public void OnRemove(Character owner) => OnRemoved?.Invoke(owner);
        public virtual Item DeepCopy() => new Item(this);
    }
}
