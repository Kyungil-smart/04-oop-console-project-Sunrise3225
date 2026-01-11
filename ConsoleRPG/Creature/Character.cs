
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
        public bool RandomChance(float value)
        {
            Random rand = new Random();
            float randValue = (float)rand.NextDouble(); // 0.0 ~ 1.0 사이의 난수
            return randValue < value;
        }
    }
}
