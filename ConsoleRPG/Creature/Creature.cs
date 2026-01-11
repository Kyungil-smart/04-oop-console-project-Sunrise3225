using System;

namespace ConsoleRPG
{
    public class Creature
    {
        // 변하지 않는 정보
        public string Name { get; set; } = "123133";
        public float DefaultHpMax { get; set; }
        public float DefaultDamage { get; set; }
        public float DefaultDefence { get; set; }
        public float DefaultMpMax { get; set; }
        public float DefaultCritical { get; set; }
        public float DefaultAvoid { get; set; }

        // 게임중에 변하는 정보
        public int Level { get; set; }
        public virtual float Hp
        {
            get => hp;
            set
            {
                if (value <= 0)
                    hp = 0;
                else if (value >= DefaultHpMax)
                    hp = DefaultHpMax;
                else
                    hp = value;
            }
        }
        public float hp;
        public int textDelay = 200; // 0.2초에 한 줄 출력
        public virtual void Attack(Creature creature, int line) { }
        public virtual void OnDamaged(int damage) { }
        public virtual bool IsDead()
        {
            return false;
        }
    }

}
