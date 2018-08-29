using System;

namespace TextAdv
{
    class Program
    {
        static void Main(string[] args)
        {
            Human dude = new Human("Bandit", "Warrior", 1);
            Human hero = Commands.StartGame();

            hero.target = dude;
            dude.target = hero;
            Console.WriteLine($"You are now {hero.name}.");
            Console.WriteLine($"type \"help\" for a list of commands");
            Console.WriteLine($"{dude.name} has appeared...");
            Console.Write("Press <Enter> to continue... ");
            while(Console.ReadKey().Key != ConsoleKey.Enter) {}
            Console.Clear();
            Commands.ShowStats(hero);
            Commands.TypeCommand(hero);

            Console.Write("Press <Enter> to exit... ");
            while(Console.ReadKey().Key != ConsoleKey.Enter) {}
            Console.Clear();
        }
    }
}
