namespace ConsoleRPG
{
    public class Stage
    {
        public int StageLevel { get; set; }
        public int MonsterMinCount;
        public int MonsterMaxCount;
        public int MonsterCount;

        public Stage()
        {
            StageLevel = 1;
            MonsterMinCount = 1;
            MonsterMaxCount = 4;
        }

        public List<Creature> MonsterSpawn()
        {
            const int MAX_MONSTER_CAP = 6;
            Random rand = new Random();

            int inc = StageLevel / 10;
            // 기본값 + 보정
            int min = 1 + inc;
            int max = 4 + inc;
            // 최대 6마리로 고정
            if (max > MAX_MONSTER_CAP) max = MAX_MONSTER_CAP;
            if (min > max) min = max;


            MonsterCount = rand.Next(min, max + 1);
            // 스테이지별 몬스터 소환
            List<Creature> monsters = new List<Creature>();
            // 몬스터 생성
            for (int i = 0; i < MonsterCount; i++)
            {
                Monster randMonster = Game.Monsters[rand.Next(0, Game.Monsters.Length)];
                Creature monster = new Monster(randMonster); // 새로운 몬스터 인스턴스 생성
                monsters.Add(monster);
            }
            return monsters;
        }
        public void StageClearReward()
        {
            Game.StageReward.isClear = true;
            Game.StageReward.stageNumber = StageLevel;
            // 스테이지 보상
            Game.StageReward.exp = StageLevel * (MonsterCount * 20);
            Game.StageReward.gold = StageLevel * 300;
            // 스테이지 1 증가
            StageLevel++;
            Managers.Game.SaveGame();
        }
        public void StageFailReward()
        {
            // 던전 클리어 실패 시
            Game.StageReward.isClear = false;
            Game.StageReward.stageNumber = StageLevel;
            Game.StageReward.gold = 100;
            Game.StageReward.exp = StageLevel * 2;
            Managers.Game.SaveGame();
        }
    }
}
