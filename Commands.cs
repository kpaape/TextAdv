using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TextAdv
{
    class Commands
    {
        static public Human StartGame()
        {
            bool pass = false;
            Human hero = new Human("Player");
            while(pass == false)
            {
                Console.WriteLine($"Load game or enter your class type; wizard, ninja, or samurai...");
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
                                string[] statArr = {"class", "health", "mana", "strength", "intelligence", "dexterity", "gold"};
                                int i = 0;
                                string[] newStats = new string[8];
                                StreamReader sr = new StreamReader($"{enterClass[1]}.txt");
                                line = sr.ReadLine();
                                while (line != null) 
                                {
                                    newStats[i] = line;
                                    i++;
                                    line = sr.ReadLine();
                                }
                                string loadedStats = $"{newStats[0]}|{newStats[1]}|{newStats[2]}|{newStats[3]}|{newStats[4]}|{newStats[5]}|{newStats[6]}";
                                string loadedHash = newStats[7];
                                
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
                                switch(newStats[0])
                                {
                                    case "Wizard":
                                        hero = new Wizard("The Hero");
                                        break;
                                    case "Ninja":
                                        hero = new Ninja("The Hero");
                                        break;
                                    case "Samurai":
                                        hero = new Samurai("The Hero");
                                        break;
                                }
                                int statInt;
                                if(Int32.TryParse(newStats[1], out statInt))
                                {
                                    hero.health = statInt;
                                }
                                if(Int32.TryParse(newStats[2], out statInt))
                                {
                                    hero.mana = statInt;
                                }
                                if(Int32.TryParse(newStats[3], out statInt))
                                {
                                    hero.strength = statInt;
                                }
                                if(Int32.TryParse(newStats[4], out statInt))
                                {
                                    hero.intelligence = statInt;
                                }
                                if(Int32.TryParse(newStats[5], out statInt))
                                {
                                    hero.dexterity = statInt;
                                }
                                if(Int32.TryParse(newStats[6], out statInt))
                                {
                                    hero.gold = statInt;
                                }
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
                    case "wizard":
                        hero = new Wizard("The Wizard");
                        pass = true;
                        break;
                    case "ninja":
                        hero = new Ninja("The Ninja");
                        pass = true;
                        break;
                    case "samurai":
                        hero = new Samurai("The Samurai");
                        pass = true;
                        break;
                    default:
                        Console.WriteLine($"\"{enterClass}\" is not acceptable. Please type; wizard, ninja, or samurai");
                        break;
                }
            }
            return hero;
        }
        static public void TypeCommand(Human hero, Human enemy)
        {
            string[] enterCommand;
            while(enemy.health > 0 && hero.health > 0)
            {
                enterCommand = Console.ReadLine().Split(" ");
                switch(enterCommand[0])
                {
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
                                sw.WriteLine(hero.GetType().ToString().Split(".")[1]);
                                sw.WriteLine(hero.health);
                                sw.WriteLine(hero.mana);
                                sw.WriteLine(hero.strength);
                                sw.WriteLine(hero.intelligence);
                                sw.WriteLine(hero.dexterity);
                                sw.WriteLine(hero.gold);
                                string saveFileHash = $"{hero.GetType().ToString().Split(".")[1]}|{hero.health}|{hero.mana}|{hero.strength}|{hero.intelligence}|{hero.dexterity}|{hero.gold}";
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
                        Console.WriteLine("check - inspect something (check bag to view inventory)");
                        Console.WriteLine("use - you will attempt to use something in your bag");
                        Console.WriteLine("attack - you will attack the current enemy");
                        Console.WriteLine("heal - wizards can use heal");
                        Console.WriteLine("fireball - wizards can use fireball");
                        Console.WriteLine("steal - ninjas can use steal");
                        Console.WriteLine("getaway - ninjas can use getaway");
                        Console.WriteLine("deathblow - samurai can use deathblow");
                        Console.WriteLine("meditate - samurai can use meditate");
                        break;
                    case "attack":
                        hero.Attack(enemy);
                        break;
                    case "fireball":
                        if(hero.GetType() == typeof(Wizard))
                        {
                            (hero as Wizard).FireBall(enemy);
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a wizard");
                        }
                        break;
                    case "heal":
                        if(hero.GetType() == typeof(Wizard))
                        {
                            (hero as Wizard).Heal();
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a wizard");
                        }
                        break;
                    case "steal":
                        if(hero.GetType() == typeof(Ninja))
                        {
                            (hero as Ninja).Steal(enemy);
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a ninja");
                        }
                        break;
                    case "getaway":
                        if(hero.GetType() == typeof(Ninja))
                        {
                            (hero as Ninja).GetAway();
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a ninja");
                        }
                        break;
                    case "deathblow":
                        if(hero.GetType() == typeof(Samurai))
                        {
                            (hero as Samurai).DeathBlow(enemy);
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a samurai");
                        }
                        break;
                    case "meditate":
                        if(hero.GetType() == typeof(Samurai))
                        {
                            (hero as Samurai).Meditate();
                        }
                        else
                        {
                            Console.WriteLine($"{hero.name} is not a samurai");
                        }
                        break;
                    case "use":
                        if(enterCommand.Length == 1)
                        {
                            Console.WriteLine("Please enter what you would like to use... \"use *item*\"");
                        }
                        else
                        {
                            switch(enterCommand[1])
                            {
                                case "potion":
                                    Console.WriteLine($"You do not currently have a {enterCommand[1]}");
                                    break;
                                default:
                                    Console.WriteLine($"Could not find \"{enterCommand[1]}\" to use");
                                    break;
                            }
                        }
                        break;
                    case "check":
                        if(enterCommand.Length == 1)
                        {
                            Console.WriteLine("Please enter what you would like to check... \"check *item*\"");
                        }
                        else
                        {
                            switch(enterCommand[1])
                            {
                                case "bag":
                                    Console.WriteLine($"Gold: {hero.gold}");
                                    break;
                                case "stats":
                                    Console.WriteLine($"Health: {hero.health}");
                                    Console.WriteLine($"Mana: {hero.mana}");
                                    Console.WriteLine($"Strength: {hero.strength}");
                                    Console.WriteLine($"Intelligence: {hero.intelligence}");
                                    Console.WriteLine($"Dexterity: {hero.dexterity}");
                                    break;
                                default:
                                    Console.WriteLine($"Could not find \"{enterCommand[1]}\" to check");
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("You stand around looking off at nothing...");
                        break;
                }
            }
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