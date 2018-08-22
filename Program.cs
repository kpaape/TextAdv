using System;

namespace TextAdv
{
    class Program
    {
        static void Main(string[] args)
        {
            Human dude = new Human("Bandit");
            Human hero = Commands.StartGame();

            Console.WriteLine($"You are now {hero.name}.");
            Console.WriteLine($"type \"help\" for a list of commands");
            Console.WriteLine($"{dude.name} has appeared...");
            Commands.TypeCommand(hero, dude);

            Console.Write("Press <Enter> to exit... ");
            while(Console.ReadKey().Key != ConsoleKey.Enter) {}
            Console.Clear();
        }
    }
}
