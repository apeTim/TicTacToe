using TicTacToeLib;

public class Program
{
    public static void Main()
    {
        Console.WriteLine($"Welcome to {Crayon.Output.Blue("Tic-Tac-Toe!")}\nPress {Crayon.Output.Bold("Enter")} to start the game:");
        
        while (Console.ReadKey().Key.Equals(ConsoleKey.Enter))
        {
            int size;
            
            Console.WriteLine("Enter field size:");
            while (!int.TryParse(Console.ReadLine(), out size))
            {
                Console.WriteLine(Crayon.Output.Yellow("Incorrect field size. Try again:"));
            }

            Game game = new Game(size);
            game.Run();
            
            Console.WriteLine($"Press {Crayon.Output.Bold("Enter")} to start new game or any key to exit:");
        }
        
        Console.WriteLine("\nThanks for playing!");
    }
}