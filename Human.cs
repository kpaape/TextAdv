using System;

namespace TextAdv
{
    public class Human
    {
        public string name;
        public int health;
        public int mana;
        public int strength;
        public int intelligence;
        public int dexterity;
        public int gold;

        public Human(string val)
        {
            name = val;
            health = 100;
            mana = 100;
            strength = 3;
            intelligence = 3;
            dexterity = 3;
            gold = 0;
        }
        public Human(string val, int humanHp, int humanMan, int humanStr, int humanInt, int humanDex, int humanGold)
        {
            name = val;
            health = humanHp;
            mana = humanMan;
            strength = humanStr;
            intelligence = humanInt;
            dexterity = humanDex;
            gold = humanGold;
        }
        public void Attack(object obj, bool instigated = true)
        {
            Human toAttack = obj as Human;
            if(toAttack == null)
            {
                Console.WriteLine("You fail to find an enemy to attack.");
            }
            else
            {
                toAttack.health -= this.strength * 5;
                string msg = $"{this.name} attacked {toAttack.name}, dealing {this.strength * 5} damage.";
                if(toAttack.health <= 0)
                {
                    msg += $" {toAttack.name} has died.";
                }
                Console.WriteLine($"{msg}");
                if(instigated && toAttack.health > 0)
                {
                    toAttack.Attack(this, false);
                }
            }
        }
    }

    public class Wizard : Human
    {
        public Wizard(string val) : base(val)
        {
            health = 50;
            intelligence = 25;
        }
        public void Heal()
        {
            this.health = (this.intelligence * 10 > 50) ? 50 : this.intelligence * 10;
            // 50 should be replaced by a max health property
            Console.WriteLine($"{this.name} healed to {this.health}");
            // when healing, make it so the enemy has a chance to attack based on its dexterity
        }
        public void FireBall(object obj, bool instigated = true)
        {
            Human toAttack = obj as Human;
            Random rand = new Random();
            if(toAttack == null)
            {
                Console.WriteLine("You fail to find an enemy to attack.");
            }
            else
            {
                int damage = rand.Next(20, 50);
                // should include intelligence in this equation
                toAttack.health -= damage;
                this.mana -= 10;
                string msg = $"{this.name} used a fireball on {toAttack.name}, dealing {damage} damage.";
                if(toAttack.health <= 0)
                {
                    msg += $" {toAttack.name} has died.";
                }
                Console.WriteLine($"{msg}");
                if(instigated && toAttack.health > 0)
                {
                    toAttack.Attack(this, false);
                }
            }
        }
    }

    public class Ninja : Human
    {
        public Ninja(string val) : base(val)
        {
            dexterity = 175;
        }
        public void Steal(object obj)
        {
            this.Attack(obj);
            if(this.health > 0)
            {
                this.health += 10;
                Console.WriteLine($"{this.name} stole 10 health.");
            }
            // steal should not run attack and should increase money instead. also, have a chance to get attacked based on dexterity (if you get detected)
        }
        public void GetAway()
        {
            this.health -= 15;
            Console.WriteLine($"{this.name} used \"get away\" but tripped over his own feet, doing 15 damage to himself.");
            // this should instead have a chance to exit the fight based on dexterity and should be a human method so all types can access it
        }
    }

    public class Samurai : Human
    {
        public Samurai(string val) : base(val)
        {
            health = 200;
        }
        public void DeathBlow(object obj)
        {
            Human toAttack = obj as Human;
            if(toAttack == null)
            {
                Console.WriteLine("You fail to find an enemy to attack.");
            }
            else
            {
                if(toAttack.health <= 50)
                {
                    toAttack.health = 0;
                    Console.WriteLine($"{this.name} dealt a death blow to {toAttack.name}");
                }
                else
                {
                    int damage = this.strength;
                    toAttack.health -= damage;
                    Console.WriteLine($"{this.name} failed to kill {toAttack.name} with a death blow, only dealing {damage} damage.");
                    toAttack.Attack(this, false);
                }
            }
        }
        public void Meditate()
        {
            this.health = 200;
            // health should be set to a max health property instead
            Console.WriteLine($"{this.name} used meditation to heal all wounds.");
        }
    }
}