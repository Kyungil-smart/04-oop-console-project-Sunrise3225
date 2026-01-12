namespace ConsoleRPG
{
    public enum Command
    {
        None,
        MoveTop,
        MoveBottom,
        MoveLeft,
        MoveRight,
        Interact,
        Exit,
    }
    public enum BattleAction // 전투 씬 내부에서 현재 상태를 나타내는 상태값
    {
        SelectAction,
        SelectSkill,
        SelectAttack,
        Attack,
        UsePotion,
    }
    public class SceneManager
    {
        public BaseScene? CurrentScene { get; protected set; }  // 현재 씬 EnterScene 호출 때마다 바뀐다.
        public BaseScene? PrevScene { get; protected set; }     // 현재 씬을 PrevScene으로 저장

        private Dictionary<string, BaseScene> Scene = new Dictionary<string, BaseScene>();
        private Dictionary<string, ActionOption> Options = new Dictionary<string, ActionOption>(); // 메뉴 행동들을 key로 저장

        public void Init()
        {
            // 씬 정보 초기화
            DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Scene");
            foreach (FileInfo info in directoryInfo.GetFiles())
            {
                string sceneName = Path.GetFileNameWithoutExtension(info.FullName);
                Type? type = Type.GetType($"ConsoleRPG.{sceneName}");
                if (type != null)
                {
                    BaseScene? scene = Activator.CreateInstance(type) as BaseScene;
                    Scene.Add(sceneName, scene);
                }
            }
            // 선택지 정보 초기화
            Options.Add("NewGame", new("NewGame", "새로 시작", () => EnterScene<CreateCharacterScene>()));
            Options.Add("LoadGame", new("LoadGame", "불러 오기", LoadGame));
            Options.Add("Back", new("Back", "뒤로 가기", () => EnterScene<BaseScene>(PrevScene.GetType().Name)));
            Options.Add("ShowInfo", new("ShowInfo", "상태 보기", () => EnterScene<CharacterInfoScene>()));
            Options.Add("Inventory", new("Inventory", "인벤 토리", () => EnterScene<InventoryInfoScene>()));
            Options.Add("Equipment", new("Equipment", "장비 관리", () => EnterScene<EquipmentScene>()));
            Options.Add("Shop", new("Shop", "상  점", () => EnterScene<ShopScene>()));
            Options.Add("Dungeon", new("Dungeon", "던전 입구", () => EnterScene<DungeonGateScene>()));
            Options.Add("DungeonEnter", new("DungeonEnter", "던전 입장", () => EnterScene<BattleScene>()));
            Options.Add("Main", new("Main", "메인 으로", () => EnterScene<MainScene>()));
            //Options.Add("Rest", new("Rest", "휴식 하기", () => EnterScene<RestScene>()));

            Options.Add("UseInn", new("UseInn", "여관 이용하기", UseInn));
        }

        void UseInn()
        {
            if (Game.Player.Hp == Game.Player.HpMax && Game.Player.Mp == Game.Player.MpMax)
                Renderer.Print(12, "지금 휴식할 필요는 없을 것 같다.", clear: true);
            else if (Game.Player.Gold <= 100)
                Renderer.Print(12, "돈이 부족합니다..", clear: true);
            else
            {
                Renderer.Print(12, "휴 식 중 . . .", false, 2500, 2);
                // 회복 처리
                Game.Player.Hp = Game.Player.HpMax;
                Game.Player.Mp = Game.Player.MpMax;
                Game.Player.ChangeGold(-100);
                Renderer.Print(5, $"당신의 체력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
                Renderer.Print(6, $"당신의 마나 : {Game.Player.Mp} / {Game.Player.DefaultMpMax}");
                Renderer.Print(8, $"보유 골드 : {Game.Player.Gold} G");
                Renderer.Print(12, "휴식을 끝내니 힘이 솟아오른다.", clear: true);
            }
        }
        public ActionOption GetOption(string key) => Options[key];

        // 씬을 불러온다.
        public BaseScene GetScene<T>(string? sceneKey = null) where T : BaseScene
        {
            if (string.IsNullOrEmpty(sceneKey))
                sceneKey = typeof(T).Name;
            if (!Scene.TryGetValue(sceneKey, out BaseScene? scene))
                return null;

            return scene;
        }
        // 씬에 진입한다.
        public void EnterScene<T>(string? sceneKey = null) where T : BaseScene
        {
            if (string.IsNullOrEmpty(sceneKey))
                sceneKey = typeof(T).Name;
            if (!Scene.TryGetValue(sceneKey, out BaseScene? scene))
                return;
            if (scene == null || scene == CurrentScene) // 이미 같은 씬이면 재진입 불가
                return;

            // 이전 씬 설정
            SetPrevScene();
            // 현재 씬 진입
            CurrentScene = scene;
            scene.EnterScene();  // 진입 시 초기화
            scene.NextScene();   // 입력 루프.화면 루프를 돌면서 실제 진행을 수행
        }
        void SetPrevScene() // 씬 전환 직전에 현재 씬을 백업
        {
            PrevScene = CurrentScene;
        }
        void LoadGame() // 데이터 불러오기
        {
            Game.Player = Managers.Game.data.character;
            Game.Stage = Managers.Game.data.stage;
            //EnterScene<MainScene>();
        }
    }
    public class ActionOption
    {
        public string Key { get; private set; }          // 옵션 식별자
        public string Description { get; private set; }  // 화면에 보여줄 설명
        public Action Action { get; private set; }       // 실제 실행될 행동 
        public ActionOption(string key, string description, Action action)
        {
            Key = key;
            Description = description;
            Action = action;
        }
        public void Excute() => Action?.Invoke(); // 등록된 Action을 호출
    }
}
