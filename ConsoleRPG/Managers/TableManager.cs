namespace ConsoleRPG
{
    public class TableManager
    {
        private Dictionary<string, TableFormatter<Item>> items = new(); // Item 표에서 사용할 컬럼 포맷터를 키로 보관
        private Dictionary<string, TableFormatter<Character>> jobs = new(); // 직업 표에서 사용할 컬럼 포맷터를 키로 보관

        public List<TableFormatter<T>> GetFormatters<T>(string[] keys) // 외부에서 호출
        {
            List<TableFormatter<T>> list = new();
            for (int i = 0; i < keys.Length; i++)
            {
                TableFormatter<T>? formatter = typeof(Item) == typeof(T) 
                    ? items[keys[i]] as TableFormatter<T> 
                    : jobs[keys[i]] as TableFormatter<T>;

                if (formatter == null) // 등록 안되거나 캐스팅 실패 시 스킵
                    continue;

                list.Add(formatter);
            }
            return list;
        }
        public void Init()
        {
            #region Item 관련
            items["Index"] = new("Index", "", 2);
            items["Name"] = new("Name", "이름", 22, item =>
            {
                if (item is Gear gear) // 장비 출력 방식
                {
                    if (gear.IsEquip) return $"[E] {gear.Name}";
                    else
                        return gear.Name;
                }
                else
                    return item.Name;
            });
            // 스택형 아이템 개수 표시
            items["StackCount"] = new("StackCount", "개수", 8, item => item.StackCount.HasValue ? $"{item.StackCount.Value} 개" : "");
            items["ItemType"] = new("ItemType", "타입", 15, item =>
            {
                if (item is Gear gear)
                    return gear.GearType.String();
                else
                    return item.Type.String();
            });
            items["Effect"] = new("Effect", "효과", 34, item =>
            {
                if (item is Gear gear)
                    return gear.GearType.String();
                else if (item is ConsumeItem consume)
                    return consume.EffectDesc;
                else
                    return string.Empty;
            });
            items["Desc"] = new("Desc", "설명", 30, item => item.Description);
            items["Cost"] = new("Cost", "비용", 10, item => item.Price.ToString());
            items["SellCost"] = new("SellCost", "비용", 10, item => ((int)(item.Price * 0.8f)).ToString());
            items["ShopCount"] = new("ShopCount", "보유 개수", 11, item =>
            {
                if (Game.Player.Inventory.HasSameItem(item, out Item res))
                    return res.StackCount.HasValue ? $"{res.StackCount.Value} 개" : "보유중";
                else
                    return item.StackCount.HasValue ? "0 개" : "미보유";
            });
            #endregion

            #region 직업 관련
            jobs["Job"] = new("Job", "직업", 10, ch => ch.Job.ToString());
            jobs["Damage"] = new("DefaultDamage", "공격력", 10, ch => ch.DefaultDamage.ToString());
            jobs["Defence"] = new("DefaultDefence", "방어력", 10, ch => ch.DefaultDefence.ToString());
            jobs["HpMax"] = new("DefaultHpMax", "체 력", 10, ch => ch.DefaultHpMax.ToString());
            jobs["MpMax"] = new("DefaultMpMax", "마 나", 10, ch => ch.DefaultMpMax.ToString());
            jobs["Critical"] = new("Critical", "크리율", 10, ch => ch.Critical.ToString("0%"));
            jobs["Avoid"] = new("Avoid", "회피율", 10, ch => ch.Avoid.ToString("0%"));
            #endregion

        }
    }
    
    public class TableFormatter<T>
    {
        public string key;                      // 컬럼 식별하는 내부 key
        public string description;              // 표에 출력될 설명
        public int length;                      // 컬럼이 차지할 문자수
        public Func<T, string>? dataSelector;   // 한 줄에서 실제 데이터(T)를 문자열로 바꾸는 함수

        public TableFormatter(string key, string description, int length, Func<T, string>? dataSelector = null)
        {
            this.key = key;
            this.description = description;
            this.length = length;
            this.dataSelector = dataSelector;
        }
        public string GetTitle() => Renderer.GetTableElementString(length, description, true);
        public string GetLine() => new('=', length);
        public string GetString(int index) => Renderer.GetTableElementString(length, index.ToString(), false);
        public string GetString(T item) => Renderer.GetTableElementString(length, dataSelector(item), false);
    }
}
