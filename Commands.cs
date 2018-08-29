using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace TextAdv
{
    public class Commands
    {
        static public Human StartGame()
        {
            bool pass = false;
            Human hero = new Human("Player", "Warrior", 1);
            while(pass == false)
            {
                Console.WriteLine($"Load game or enter your class type; warrior, wizard...");
                string[] enterClass = Console.ReadLine().Split(" ");
                switch(enterClass[0])
                {
                    case "load":
                        if(enterClass.Length == 1)
                        {
                            Console.WriteLine("Enter the old save file name");
                        }
                        else
                        {
                            string line;
                            try 
                            {
                                string[] statArr = {"name", "class", "health", "mana", "strength", "intelligence", "dexterity", "gold"};
                                int i = 0;
                                string[] newStats = new string[10];
                                StreamReader sr = new StreamReader($"{enterClass[1]}.txt");
                                line = sr.ReadLine();
                                while (line != null) 
                                {
                                    newStats[i] = line;
                                    i++;
                                    line = sr.ReadLine();
                                }
                                string loadedStats = $"{newStats[0]}|{newStats[1]}|{newStats[2]}|{newStats[3]}|{newStats[4]}|{newStats[5]}|{newStats[6]}|{newStats[7]}|{newStats[8]}";
                                string loadedHash = newStats[9];
                                
                                string hashedStats = "";
                                using(MD5 md5Hash = MD5.Create())
                                {
                                    hashedStats = $"{GetMd5Hash(md5Hash, loadedStats)}";
                                }
                                if(hashedStats != loadedHash)
                                {
                                    Console.WriteLine("Save file has been tampered with... canceling");
                                    break;
                                }
                                hero.name = newStats[0];
                                hero.charClass = newStats[1];
                                int statInt;
                                if(Int32.TryParse(newStats[2], out statInt))
                                {
                                    hero.health = statInt;
                                }
                                if(Int32.TryParse(newStats[3], out statInt))
                                {
                                    hero.mana = statInt;
                                }
                                if(Int32.TryParse(newStats[4], out statInt))
                                {
                                    hero.strength = statInt;
                                }
                                if(Int32.TryParse(newStats[5], out statInt))
                                {
                                    hero.intelligence = statInt;
                                }
                                if(Int32.TryParse(newStats[6], out statInt))
                                {
                                    hero.dexterity = statInt;
                                }
                                if(Int32.TryParse(newStats[7], out statInt))
                                {
                                    hero.gold = statInt;
                                }
                                // need to load the inventory
                                sr.Close();
                                pass = true;
                                Console.WriteLine($"Successfully loaded {enterClass[1]}.txt");
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message);
                            }
                        }
                        break;
                    case "warrior":
                        // add enter name proc here
                        hero = new Human("The Warrior", "Warrior", 1);
                        pass = true;
                        break;
                    case "wizard":
                        hero = new Human("The Wizard", "Wizard", 1);
                        pass = true;
                        Spell fireBall = new Spell("Flames", 1, "Burn");// by default spells have a target of true
                        Spell spark = new Spell("Spark", 3);
                        Spell heal = new Spell("Heal", 5, false);// when set to false the spell will be cast on the user
                        hero.spells.Add(fireBall);
                        hero.spells.Add(heal);
                        hero.spells.Add(spark);
                        break;
                    default:
                        Console.WriteLine($"\"{enterClass}\" is not acceptable. Please type; warrior, or wizard");
                        break;
                }
            }
            ShowStats(hero);
            Weapon sword = new Weapon("Sword", 1, 5);
            Weapon dagger = new Weapon("Dagger", 1, 3);
            hero.bag.Add(sword);
            hero.bag.Add(dagger);
            Consumable healthPotion = new Consumable("Health", 1); // (potion type, potion level) types can be health, mana, "status", buffs, and nerfs
            hero.bag.Add(healthPotion);
            Consumable manaPotion = new Consumable("Mana", 1);
            hero.bag.Add(manaPotion);
            return hero;
        }
        static public void TypeCommand(Human hero)
        {
            string[] enterCommand;
            bool quitGame = false;
            while(hero.health > 0 && !quitGame)
            {
                Messages.msgs.Clear();
                enterCommand = Console.ReadLine().Split(" ");
                switch(enterCommand[0])
                {
                    case "quit":
                        quitGame = true;
                        break;
                    case "save":
                        if(enterCommand.Length == 1)
                        {
                            Console.WriteLine("Enter the new save file name");
                        }
                        else
                        {
                            try 
                            {
                                StreamWriter sw = new StreamWriter($"{enterCommand[1]}.txt");
                                sw.WriteLine(hero.name);
                                sw.WriteLine(hero.charClass);
                                sw.WriteLine(hero.health);
                                sw.WriteLine(hero.mana);
                                sw.WriteLine(hero.strength);
                                sw.WriteLine(hero.intelligence);
                                sw.WriteLine(hero.dexterity);
                                sw.WriteLine(hero.gold);
                                sw.WriteLine(hero.bag);
                                string saveFileHash = $"{hero.name}|{hero.charClass}|{hero.health}|{hero.mana}|{hero.strength}|{hero.intelligence}|{hero.dexterity}|{hero.gold}|{hero.bag}";
                                using(MD5 md5Hash = MD5.Create())
                                {
                                    sw.WriteLine($"{GetMd5Hash(md5Hash, saveFileHash)}");
                                }

                                sw.Close();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message);
                            }
                            Console.WriteLine($"Successfully saved {enterCommand[1]}");
                        }
                        break;
                    case "help":
                        Messages.msgs.Add("check... - inspect something (check bag to view inventory, check spells to view your spells, check stats will show your stats in detail)");
                        Messages.msgs.Add("use... - you will attempt to use the specified item in your bag");
                        Messages.msgs.Add("equip... - you will attempt to equip the specified item in your bag");
                        Messages.msgs.Add("attack - you will attack the current enemy");
                        Messages.msgs.Add("cast... - you will cast the specified spell from your spells on the current enemy");
                        Messages.msgs.Add("quit - will end the game");
                        break;
                    case "attack":
                        hero.Attack();
                        break;
                    case "cast":
                        if(enterCommand.Length == 1)
                        {
                            Messages.msgs.Add("Please enter what spell you would like to use... \"cast *spell*\"");
                        }
                        else
                        {
                            string castSpell = enterCommand[1];
                            for(int i = 2; i <= enterCommand.Length - 1; i++)
                            {
                                castSpell += " " + enterCommand[i];
                            }
                            switch(castSpell)
                            {
                                default:
                                    bool spellFound = false;
                                    Spell toCast = new Spell("", 0);
                                    foreach(Spell obj in hero.spells)
                                    {
                                        if(obj.name == castSpell)
                                        {
                                            toCast = obj;
                                            spellFound = true;
                                        }
                                    }
                                    if(spellFound == false)
                                    {
                                        Messages.msgs.Add($"Could not find \"{castSpell}\" to use");
                                    }
                                    else
                                    {
                                        if(toCast.target == true && hero.target == null)
                                        {
                                            Messages.msgs.Add($"There is no enemy to cast {toCast.name} on");
                                            break;
                                        }
                                        if(toCast.cost <= hero.mana)
                                        {
                                            toCast.Cast(hero);  // (who is casting the spell)
                                        }
                                        else
                                        {
                                            Messages.msgs.Add($"You do not have enough mana to cast {toCast.name}");
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case "use":
                        if(enterCommand.Length == 1)
                        {
                            Messages.msgs.Add("Please enter what you would like to use... \"use *item*\"");
                        }
                        else
                        {
                            string useItem = enterCommand[1];
                            for(int i = 2; i < enterCommand.Length; i++)
                            {
                                useItem += " " + enterCommand[i];
                            }
                            switch(useItem)
                            {
                                default:
                                    bool itemFound = false;
                                    Item toUse = new Item("", 0);
                                    foreach(Item obj in hero.bag)
                                    {
                                        if(obj.name == useItem)
                                        {
                                            toUse = obj;
                                            itemFound = true;
                                        }
                                    }
                                    if(itemFound == false)
                                    {
                                        Messages.msgs.Add($"Could not find \"{useItem}\" to use.");
                                    }
                                    else
                                    {
                                        if(toUse.GetType() == typeof(Consumable))
                                        {
                                            hero.Use((Consumable)toUse);
                                        }
                                        else
                                        {
                                            Messages.msgs.Add($"{useItem} does not have a use.");
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case "equip":
                        if(enterCommand.Length == 1)
                        {
                            Messages.msgs.Add("Please enter what you would like to equip... \"equip *item*\"");
                        }
                        else
                        {
                            string equipItem = enterCommand[1];
                            for(int i = 2; i < enterCommand.Length; i++)
                            {
                                equipItem += " " + enterCommand[i];
                            }
                            switch(equipItem)
                            {
                                default:
                                    bool itemFound = false;
                                    Item toEquip = new Item("", 0);
                                    foreach(Item obj in hero.bag)
                                    {
                                        Console.WriteLine("* * * Checking bag!");
                                        if(obj.name == equipItem)
                                        {
                                            toEquip = obj;
                                            itemFound = true;
                                        }
                                    }
                                    if(itemFound == false)
                                    {
                                        Messages.msgs.Add($"Could not find \"{equipItem}\" to equip.");
                                    }
                                    else
                                    {
                                        if(toEquip.GetType() == typeof(Weapon))
                                        {
                                            if(hero.equipWeapon != null)
                                            {
                                                hero.equipWeapon.equipped = false;
                                                hero.equipWeapon = null;
                                            }
                                            hero.equipWeapon = (Weapon)toEquip;
                                            hero.equipWeapon.equipped = true;
                                            Messages.msgs.Add($"You equipped your {toEquip.name}.");
                                        }
                                        // ***********************************************************************************************      Equip Armor
                                        // else
                                        // {
                                        //     if(toEquip.GetType() == typeof(Armor))
                                        //     {
                                        //         if(hero.equipArmor != null)
                                        //         {
                                        //             hero.equipArmor.equipped = false;
                                        //             hero.equipArmor = null;
                                        //         }
                                        //         hero.equipArmor = (Armor)toEquip;
                                        //         hero.equipWeapon.equipped = true;
                                        //         Messages.msgs.Add($"You equipped your {toEquip.name}.");
                                        //     }
                                        // }
                                        else
                                        {
                                            Messages.msgs.Add($"Could not equip \"{equipItem}\".");
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case "unequip":
                        if(enterCommand.Length == 1)
                        {
                            Messages.msgs.Add("Please enter what you would like to unequip... \"unequip *weapon/armor*\".");
                        }
                        else
                        {
                            string equipItem = enterCommand[1];
                            for(int i = 2; i < enterCommand.Length; i++)
                            {
                                equipItem += " " + enterCommand[i];
                            }
                            switch(equipItem)
                            {
                                case "weapon":
                                    if(hero.equipWeapon == null)
                                    {
                                        Messages.msgs.Add("You do not have a weapon equipped.");
                                        break;
                                    }
                                    hero.equipWeapon.equipped = false;
                                    hero.equipWeapon = null;
                                    Messages.msgs.Add("You have unequipped your weapon.");
                                    break;
                                default:
                                    Messages.msgs.Add("Please enter what you would like to unequip... \"unequip *weapon/armor*\".");
                                    break;
                            }
                        }
                        break;
                    case "check":
                        if(enterCommand.Length == 1)
                        {
                            Messages.msgs.Add("Please enter what you would like to check... \"check *item*\"");
                        }
                        else
                        {
                            switch(enterCommand[1])
                            {
                                case "bag":
                                    Messages.msgs.Add($"Gold: {hero.gold}");
                                    foreach(Item obj in hero.bag)
                                    {
                                        Messages.msgs.Add($"{obj.name}");
                                    }
                                    break;
                                case "spells":
                                    foreach(Spell obj in hero.spells)
                                    {
                                        string hasEffect = (obj.effect != null) ? obj.effect : "None";
                                        Messages.msgs.Add($"{obj.name} -  Damage: {obj.damage * hero.intelligence}, Effect: {hasEffect}, Mana Cost: {obj.cost}");
                                    }
                                    break;
                                case "stats":
                                    Messages.msgs.Add($"{hero.name} - {hero.level}");
                                    Messages.msgs.Add($"Experience: {hero.experience}");
                                    if(hero.equipWeapon != null)
                                    {
                                        Messages.msgs.Add($"Weapon: {hero.equipWeapon.name}");
                                    }
                                    else
                                    {
                                        Messages.msgs.Add($"Weapon:");
                                    }
                                    Messages.msgs.Add($"Status: {hero.status}");
                                    Messages.msgs.Add($"Health: {hero.health}/{hero.maxHealth}");
                                    Messages.msgs.Add($"Mana: {hero.mana}/{hero.maxMana}");
                                    Messages.msgs.Add($"Strength: {hero.strength}");
                                    Messages.msgs.Add($"Defense: {hero.defense}");
                                    Messages.msgs.Add($"Intelligence: {hero.intelligence}");
                                    Messages.msgs.Add($"Dexterity: {hero.dexterity}");
                                    break;
                                default:
                                    Messages.msgs.Add($"Could not find \"{enterCommand[1]}\" to check");
                                    break;
                            }
                        }
                        break;
                    default:
                        Messages.msgs.Add("You stand around looking off at nothing...");
                        break;
                }
                ShowStats(hero);
            }
        }

        public static void ShowStats(Human hero)
        {
            Console.Clear();
            if(hero.target != null)
            {
                Console.WriteLine("----------------ENEMY-------------------");
                Console.WriteLine($"{hero.target.name} - {hero.target.charClass}");
                Console.WriteLine($"Health: {hero.target.health} / {hero.target.maxHealth}");
                Console.WriteLine($"Mana: {hero.target.mana} / {hero.target.maxMana}");
                Console.WriteLine($"Status: {hero.target.status}");
                Console.WriteLine("----------------------------------------");
            }
            Console.WriteLine("----------------PLAYER------------------");
            Console.WriteLine($"{hero.name} - {hero.charClass}");
            Console.WriteLine($"Health: {hero.health} / {hero.maxHealth}");
            Console.WriteLine($"Mana: {hero.mana} / {hero.maxMana}");
            Console.WriteLine($"Status: {hero.status}");
            Console.WriteLine("----------------------------------------");
            Messages.ShowMsgs();
        }
        
        // below is mostly copy paste of microsoft's md5 section with minor edits
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));// this used to evaluate as an if else and return t/f, now it just returns the evaluation which is either t/f
        }
    }
}