using System;
using System.Threading.Tasks;

namespace menuApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("   BIENVENUE DANS LE JEU DE PÊCHE   ");
            Console.WriteLine("======================================\n");

            // Démarrer une instance du jeu de pêche
            FishingGame jeu = new FishingGame();
            await jeu.Start();
            
            Console.WriteLine("\nMerci d'avoir joué!");
            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}