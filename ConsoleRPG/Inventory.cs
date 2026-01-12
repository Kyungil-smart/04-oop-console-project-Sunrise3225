
namespace ConsoleRPG
{
    public class Inventory
    {
        List<Item> items;
        public List<Item> Items => items;

        public Character Parent { get; } // 인벤토리 주인

        public int Count { get => items.Count; } // 인벤토리 내 아이템 개수

        Action<Item>? onAdded;
        Action<Item>? onRemoved;

        public int MaxPad { get; private set; }

        public Inventory(Character _parent)
        {
            items = new List<Item>();
            Parent = _parent;
            MaxPad = 4;
        }
        public void Add(Item item)
        {
            if (!HasSameItem(item, out Item res))
                {
                // 중복되는 아이템 없는 경우에만 add
                items.Add(item);
                item.OnAdd(Parent);
            }
            else if (res.StackCount.HasValue)
            {
                // 개수를 쌓을 수 있는 아이템은 인벤토리 내의 소소품의 stackCount 를 더함
                res.OnAdd(Parent, item);
            }
            onAdded?.Invoke(item);
        }
        public void Remove(Item item)
        {
            items.Remove(item);
            item.OnRemove(Parent);
            onRemoved?.Invoke(item);
        }

        // 인벤토리에 같은 아이템이 있는지 찾는다
        public bool HasSameItem(Item item) => HasSameItem(item, out _);

        public bool HasSameItem(Item item, out Item res)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    res = items[i];
                    return true;
                }
            }
            res = null;
            return false;
        }
    }
}
