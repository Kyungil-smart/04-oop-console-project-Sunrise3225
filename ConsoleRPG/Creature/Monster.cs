using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleRPG
{
    public class Monster : Creature
    {
        public override float Hp
        {
            get => hp;
            set
            {
                if (value <= 0) hp = 0;
                else hp = value;
            }
        }
        public Monster(string name, float hp, float damage, float defence, float mp, float critical, float avoid)
        {
            Name = name;
            Hp = hp;
            DefaultHpMax = hp;
            DefaultDamage = damage;
            DefaultDefence = defence;
            DefaultCritical = critical;
            DefaultAvoid = avoid;
            DefaultMpMax = mp;
        }
        public Monster(Monster other) // 새로운 몬스터 인스턴스 생성을 위한 복사 생성자 추가
        {
            Name = other.Name;
            Hp = other.Hp;
            DefaultHpMax = other.DefaultHpMax;
            DefaultDamage = other.DefaultDamage;
            DefaultDefence = other.DefaultDefence;
            DefaultCritical = other.DefaultCritical;
            DefaultAvoid = other.DefaultAvoid;
            DefaultMpMax = other.DefaultMpMax;
        }
        public override void Attack(Creature creature, int line)
        {
            int printWidthPos = Console.WindowWidth / 2;
            bool isCritical = false;
            for (int i = line; i < 26; i++)
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
                string battleText = $"{Name}이(가) {finalDamage}의 데미지를 입혔습니다!";
                if (isCritical) battleText = "치명타 발생!" + battleText;
                Renderer.Print(line++, battleText, false, textDelay, printWidthPos);
                creature.OnDamaged(finalDamage);
            }
        }
        public override void OnDamaged(int damage)
        {
            Hp -= damage;
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
    }
}
