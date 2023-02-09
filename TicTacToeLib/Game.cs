namespace TicTacToeLib;

public class Game
{
    private const int DEFAULT_SIZE = 3;
    
    private GameMode _gameMode;
    private Board _board;
    private Bot _bot;
    private Move _botTurn;

    public Game(int size=DEFAULT_SIZE)
    {
        _board = new Board(size);
    }

    public void Run()
    {
        SetGameMode();
        
        Console.WriteLine(_board);
        
        while (_board.IsActive)
        {
            if (_gameMode == GameMode.Multiplayer)
            {
                Console.WriteLine("Player " + (_board.NextTurn == Move.Circle ? 'O' : 'X') + ", make a move:");
                MakeMove();
            }
            else
            {
                if (_board.NextTurn == _botTurn)
                {
                    _bot.MakeMove(_board);
                }
                else
                {
                    Console.WriteLine("Player " + (_board.NextTurn == Move.Circle ? 'O' : 'X') + ", make a move:");
                    MakeMove();
                }
            }

            Console.WriteLine(_board);
        }

        if (_board.Winner != Move.Empty)
            Console.WriteLine("Game is finished. Winner: " + Crayon.Output.Bold(_board.Winner == Move.Circle ? "Player O!" : "Player X!"));
        else
            Console.WriteLine($"Game is finished. {Crayon.Output.Bold("Draw!")}");
    }

    private void SetGameMode()
    {
        Console.WriteLine("Choose game mode. 1 - Computer. 2 - MultiPlayer.");
        ConsoleKey key = Console.ReadKey().Key;

        while (!key.Equals(ConsoleKey.D1) && !key.Equals(ConsoleKey.D2))
        {
            Console.WriteLine("Incorrect game mode. 1 - Computer. 2 - MultiPlayer.");
            key = Console.ReadKey().Key;
        }

        if (key.Equals(ConsoleKey.D1))
        {
            _gameMode = GameMode.Bot;
            _bot = new Bot();
            _botTurn = (Move)(new Random().Next(1, 3));
        }
        else
        {
            _gameMode = GameMode.Multiplayer;
        }
    }

    public void MakeMove()
    {
        bool placed = false;
        
        while (!placed)
        {
            int row;
            int column;
            
            string[] data = Console.ReadLine().Split();

            try
            {
                if (!(data.Length == 2 && data[0].Length == 1 && int.TryParse(data[1], out row)))
                    throw new InvalidDataException("Move is not in correct format");

                if (!('a' <= data[0][0] && data[0][0] < 'a' + _board.Size))
                    throw new InvalidDataException("Move is not possible");
                
                row = row - 1;
                column = data[0][0] % 'a';
                
                _board.Place(row, column);
                placed = true;
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine(Crayon.Output.Yellow($"{e.Message}. Try again:"));
            }
        }
    }
}