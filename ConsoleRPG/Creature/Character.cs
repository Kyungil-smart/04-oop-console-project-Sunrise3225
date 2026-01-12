
namespace ConsoleRPG
{
    public class Character : Creature
    {
        public string Job { get; set; }
        public float HpMax => DefaultHpMax + hpMaxModifier;
        public float Damage => DefaultDamage + damageModifier;
        public float Defence => DefaultDefence + defenceModifier;
        public float MpMax => DefaultMpMax + mpMaxModifier;
        public float Critical => DefaultCritical + criticalModifier;
        public float Avoid => DefaultAvoid + avoidModifier;

        public int NextLevelExp;
        public int TotalExp;
        public Skill PlayerSkill;
        public int Gold { get; set; }
        public override float Hp
        {
            get => hp;
            set
            {
                if (value <= 0) hp = 0;
                else if (value >= HpMax) hp = HpMax;
                else hp = value;
            }
        }
        protected float mp;
        public float Mp
        {
            get => mp;
            set
            {
                if (value <= 0) mp = 0;
                else if (value >= MpMax) mp = MpMax;
                else mp = value;
            }
        }
        public Inventory Inventory { get; set; }
        public Equipment Equipment { get; set; }

        public float hpMaxModifier;
        public float damageModifier;
        public float defenceModifier;
        public float mpMaxModifier;
        public float criticalModifier;
        public float avoidModifier;

        public Character(string name, string job, int level, float damage, float defence,
                          float hp, float mp, int gold, float critical, float avoid, Skill playerSkill)
        {
            Name = name;
            Job = job;
            Level = level;
            DefaultDamage = damage;
            DefaultDefence = defence;
            DefaultHpMax = hp;
            DefaultMpMax = mp;
            DefaultCritical = critical;
            DefaultAvoid = avoid;
            Gold = gold;

            Inventory = new Inventory(this);
            Equipment = new Equipment();
            NextLevelExp = 100;
            TotalExp = 0;
            Hp = HpMax;
            Mp = MpMax;
            PlayerSkill = playerSkill;
        }
        public override void Attack(Creature creature, int line)
        {
            int printWidthPos = Console.WindowWidth / 2;
            bool isCritical = false;
            for (int i = 0; i < 5; i++)
            {
                Renderer.Print(line + i, new string(' ', printWidthPos - 1), false, 0, printWidthPos);
            }
            if (RandomChance(creature.DefaultAvoid)) // 회피 했을 때
            {
                Renderer.Print(line++, $"{creature.Name}이(가) 회피 했습니다!", false, textDelay, printWidthPos);
            }
            else // 공격 성공 했을 때
            {
                float damage = DefaultDamage;
                if (RandomChance(DefaultCritical))
                {
                    isCritical = true;
                    damage *= 1.5f; // 기본 데미지 1.5배
                }

                int finalDamage = Math.Clamp((int)damage - (int)creature.DefaultDefence / 2, 0, (int)creature.Hp);
                string battleText = $"{creature.Name}에게 {finalDamage}의 데미지를 입혔습니다!";
                if (isCritical) battleText = "치명타 발생!" + battleText;
                Renderer.Print(line++, battleText, false, textDelay, printWidthPos);
                creature.OnDamaged(finalDamage);
            }
        }
        public void Skill(Creature creature, ref int line, float damage)
        {
            if (creature.IsDead())
                return;

            int printWidthPos = Console.WindowWidth / 2;
            int finalDamage = Math.Clamp((int)damage - (int)creature.DefaultDefence / 2, 0, (int)creature.Hp);
            string battleText = $"{creature.Name}에게 {finalDamage}의 데미지를 입혔습니다!";
            Renderer.Print(line++, battleText, false, textDelay, printWidthPos);
            creature.OnDamaged(finalDamage);
        }

        public override void OnDamaged(int damage)
        {
            Hp -= damage;
            Managers.Game.SaveGame();
        }
        public void ChangeMana(int value)
        {
            Mp += value;
            Managers.Game.SaveGame();
        }
        public bool RandomChance(float value)
        {
            Random rand = new Random();
            float randValue = (float)rand.NextDouble(); // 0.0 ~ 1.0 사이의 난수
            return randValue < value;
        }
        public override bool IsDead()
        {
            if (hp <= 0) return true;
            return false;
        }
        public void ChangeGold(int gold)
        {
            Gold += gold;
            Managers.Game.SaveGame();
        }
        public void ChangeExp(int expAmount)
        {
            int levelUpCount = 0;
            TotalExp += expAmount;
            while (TotalExp >= NextLevelExp)
            {
                TotalExp -= NextLevelExp;
                levelUpCount++;
                NextLevelExp += 50;
            }
            LevelUp(levelUpCount);
            Managers.Game.SaveGame();
        }
        public void LevelUp(int levelUpCount)
        {
            if (levelUpCount == 0)
                return;
            Level += levelUpCount;
            // 레벨업당 공1, 방어 0.5, 체력 10, 마나 5.5 증가
            DefaultDamage += 1.0f * levelUpCount;
            DefaultDefence += 0.5f * levelUpCount;
            DefaultHpMax += 10f * levelUpCount;
            DefaultMpMax += 5.5f * levelUpCount;

            Renderer.Print(Console.WindowHeight - 7, $"레벨 {Level - levelUpCount} -> {Level}");
            Renderer.Print(Console.WindowHeight - 7, $"공격력 {Damage - 1.0f * levelUpCount} -> {Damage}");
            Renderer.Print(Console.WindowHeight - 7, $"방어력 {Defence - 0.5f * levelUpCount} -> {Defence}");
            Renderer.Print(Console.WindowHeight - 7, $"체력 {Hp - 10f * levelUpCount} -> {Hp}");
            Renderer.Print(Console.WindowHeight - 7, $"마나 {Mp - 5.5f * levelUpCount} -> {Mp}");
            Managers.Game.SaveGame();
        }
        public void Healing(int value)
        {
            Hp += value;
            Managers.Game.SaveGame();
        }
    }
}
