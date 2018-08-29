using System;
using System.Collections.Generic;

namespace TextAdv
{
    public class Human
    {
        public string name;
        public string charClass;
        public int level;
        public int experience;
        public int maxHealth;
        public int health;
        public int maxMana;
        public int mana;
        public int strength;
        public int intelligence;
        public int defense;
        public int dexterity;
        public string status;
        public string buff;
        public int gold;
        public Human target;
        public Weapon equipWeapon;
        // public Armor equipArmor;
        public List<Item> bag = new List<Item>();
        public List<Spell> spells = new List<Spell>();

        public Human(string valName, string valClass, int valLevel)
        {
            name = valName;
            charClass = valClass;
            level = valLevel;
            health = 100;
            mana = 100;
            strength = 1;
            defense = 1;
            intelligence = 1;
            dexterity = 1;
            status = null;
            gold = 0;
            switch(charClass)
            {
                case "Warrior":
                    maxHealth = 115 + level * 5;
                    maxMana = 78 + level * 2;
                    strength = level * 5;
                    defense = level * 2;
                    intelligence = level * 2;
                    dexterity = level * 2;
                    break;
                case "Wizard":
                    maxHealth = 78 + level * 2;
                    maxMana = 115 + level * 5;
                    strength = level * 2;
                    defense = level * 2;
                    intelligence = level * 5;
                    dexterity = level * 2;
                    break;
                default:
                    maxHealth = 98 + level * 2;
                    maxMana = 98 + level * 2;
                    strength = level * 2;
                    defense = level * 2;
                    intelligence = level * 2;
                    dexterity = level * 2;
                    break;
            }
            health = maxHealth;
            mana = maxMana;
        }
        public Human(string val, string humanClass, int humanHp, int humanMan, int humanStr, int humanInt, int humanDex, int humanGold)
        {
            name = val;
            charClass = humanClass;
            health = humanHp;
            mana = humanMan;
            strength = humanStr;
            intelligence = humanInt;
            dexterity = humanDex;
            gold = humanGold;
        }
        public void Use(Consumable toConsume)
        {
            this.health = (this.health + toConsume.gainHealth > this.maxHealth) ? this.maxHealth : this.health + toConsume.gainHealth;
            this.mana = (this.mana + toConsume.gainMana > this.maxMana) ? this.maxMana : this.mana + toConsume.gainMana;
            Messages.msgs.Add($"{this.name} drank a {toConsume.name}.");
            this.bag.Remove(toConsume);
        }
        public void Attack(bool instigated = true)
        {
            Human toAttack = this.target;
            if(toAttack == null)
            {
                Messages.msgs.Add("You fail to find an enemy to attack.");
            }
            else
            {
                int damage = 0;
                if(this.equipWeapon != null)
                {
                    damage = this.strength + (this.equipWeapon.strength * this.equipWeapon.level) - toAttack.defense;
                }
                else
                {
                    damage = this.strength - toAttack.defense;
                }
                if(damage < 0)
                {
                    damage = 0;
                }
                toAttack.health -= damage;
                string msg = $"{this.name} attacked {toAttack.name}, dealing {damage} damage.";
                if(toAttack.health <= 0)
                {
                    msg += $" {toAttack.name} has died. You gained {toAttack.level * 5} experience.";
                    this.experience += toAttack.level * 5;
                    this.target = null;
                }
                Messages.msgs.Add(msg);
                if(this.status == "Burning")
                {
                    int burnDamage = this.maxHealth / 10;
                    this.health -= burnDamage;
                    msg = $"{this.name} took {burnDamage} burn damage.";
                    if(this.health <= 0)
                    {
                        msg += $" {this.name} succumed to the flames and died.";
                        toAttack.experience += this.level * 5;
                        toAttack.target = null;
                        this.target = null;
                    }
                    Messages.msgs.Add(msg);
                }
                if(instigated && toAttack.health > 0)
                {
                    toAttack.Attack(false);
                }
            }
        }
    }
}