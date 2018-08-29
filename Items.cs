using System;
using System.Collections.Generic;

namespace TextAdv
{
    // create an abstract class for equipable?
    public class Item
    {
        public string name;

        public int cost;

        public Item(string itemName, int itemCost)
        {
            name = itemName;
            cost = itemCost;
        }
    }

    public class Consumable : Item
    {
        public int stack;
        public int level;
        public int gainHealth;
        public int gainMana;
        public string statusCure;
        public string statusBuff;
        public string statusNerf;

        public Consumable(string itemName, int itemLevel) : base($"{itemName} Potion {itemLevel}", itemLevel * 25)
        {
            level = itemLevel;
            if(itemName.Contains("Health"))
            {
                gainHealth = level * 25;
            }
            if(itemName.Contains("Mana"))
            {
                gainMana = level * 25;
            }
            if(itemName == "Bleeding" || itemName == "Paralysis")
            {
                name = itemName;
                statusCure = itemName;
                cost = 250;
            }
        }
        public Consumable(string itemName, int itemCost, int itemHealth, int itemMana, string itemCure, string itemBuff, string itemNerf) : base(itemName, itemCost)
        {
            gainHealth = itemHealth;
            gainMana = itemMana;
            statusCure = itemCure;
            statusBuff = itemBuff;
            statusNerf = itemNerf;
        }
    }

    public class Equipable : Item
    {
        public int level;
        public bool equipped;
        public Equipable(string itemName, int itemLevel) : base($"{itemName} {itemLevel}", (int)Math.Pow(100, itemLevel))
        {
            level = itemLevel;
            equipped = false;
        }
    }

    public class Weapon : Equipable
    {
        public int strength;
        public Weapon(string itemName, int itemLevel, int itemStrength) : base(itemName, itemLevel)
        {
            strength = itemStrength;
        }
    }
}