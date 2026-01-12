namespace ConsoleRPG
{
    public class ShopScene : BaseScene
    {
        public override string Title { get; protected set; } = "상 점";

        public List<Item> shopSaleItem = new List<Item>();     // 상점에서 판매하는 아이템
        public List<Item> playerSaleItem = new List<Item>();   // 플레이어가 파는 아이템
        public List<TableFormatter<Item>> formattersBuy = new();
        public List<TableFormatter<Item>> formattersSale = new();

        bool shopModeToggle = true; // true : 구매모드, false : 판매모드
        string message = "";
        List<ActionOption> buyModeOptions = new List<ActionOption>();
        List<ActionOption> saleModeOptions = new List<ActionOption>();

        public override void EnterScene()
        {
            message = "";
            // 아이템 정보 설정
            shopSaleItem = Game.Items.ToList();
            playerSaleItem = Game.Player.Inventory.Items;
            shopSaleItem = shopSaleItem.OrderBy(x => x is Gear ? 0 : 1)
                                       .ThenBy(x => x is Gear gear ? (int)gear.GearType : 999)
                                       .ToList();

            // 선택지 설정
            Options.Clear();
            UpdateSaleListOptions();    // 판매 리스트 선택지 생성
            buyModeOptions.Clear();     // 구매 리스트 선택지 생성
            for (int i = 0; i < shopSaleItem.Count; i++)
            {
                Item buyItem = shopSaleItem[i].DeepCopy();
                buyModeOptions.Add(new ActionOption("", "", () =>
                {
                    if (Game.Player.Inventory.HasSameItem(buyItem) && !buyItem.StackCount.HasValue)
                    {
                        message = "보유 중인 아이템 입니다.";
                        return;
                    }
                    if (Game.Player.Gold < buyItem.Price)
                    {
                        message = "골드가 부족합니다.";
                        return;
                    }
                    Game.Player.Inventory.Add(buyItem.DeepCopy());
                    Game.Player.ChangeGold(-buyItem.Price);
                    message = $"{buyItem.Name}을(를) 샀습니다.";
                }));
            }

            // 테이블 설정
            formattersBuy = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Effect", "Cost", "ShopCount" });
            formattersSale = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Effect", "SellCost", "StackCount" });

            // Draw
            Renderer.DrawBorder(Title);
            DrawScene();
        }

        public override void NextScene()
        {
            do
            {
                if (shopModeToggle)
                    Options = buyModeOptions;
                else
                    Options = saleModeOptions;
                DrawScene();
                GetInput();
            } while (lastCommand != Command.Interact || lastCommand != Command.Exit);
        }
        protected override void DrawScene()
        {
            // 테이블 초기화
            int row = 3;
            for (int i = 0; i < 20; i++)
                Renderer.ClearLine(row + i);

            // 씬 그리기
            row = Renderer.Print(row + 1, "아이템을 구매하거나 판매할 수 있습니다.");
            row = Renderer.Print(row, $"[아이템 {(shopModeToggle ? "구매" : "판매")}]");
            row = Renderer.Print(row, $"현재 골드 : {Game.Player.Gold:#,##0} G");
            row = Renderer.DrawTable(++row,
                shopModeToggle ? shopSaleItem : playerSaleItem,
                shopModeToggle ? formattersBuy : formattersSale,
                selectOptionIndex);
            Renderer.Print(row + 1, message);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓ : 선택지 이동] [방향키 ← → : 구매/판매 모드 변경] [Enter : 선택] [ESC : 뒤로가기]");
        }

        void UpdateSaleListOptions()
        {
            saleModeOptions.Clear();
            for (int i = 0; i < playerSaleItem.Count; i++)
            {
                Item saleItem = playerSaleItem[i];
                saleModeOptions.Add(new("", "", () =>
                {
                    if (saleItem.StackCount > 1)
                        saleItem.StackCount--;
                    else
                    {
                        Game.Player.Inventory.Remove(saleItem);
                        UpdateSaleListOptions();
                    }
                    Game.Player.ChangeGold((int)(saleItem.Price * 0.8));
                    message = $"{saleItem.Name}을(를) 팔았습니다.";
                }));
            }
            if (selectOptionIndex > 0)
                selectOptionIndex--;
        }

        #region Input
        protected override void OnCommandMoveLeft()
        {
            shopModeToggle = !shopModeToggle;
            UpdateSaleListOptions();
            selectOptionIndex = 0;
        }
        protected override void OnCommandMoveRight()
        {
            shopModeToggle = !shopModeToggle;
            UpdateSaleListOptions();
            selectOptionIndex = 0;
        }
        #endregion
    }
}
