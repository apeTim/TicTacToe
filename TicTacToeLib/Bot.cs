namespace TicTacToeLib;

public class Bot
{
    private static Bot _instance;
    private Random _generator = new Random();

    public Bot Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Bot();
            return _instance;
        }
    }

    public void MakeMove(Board board)
    {
        Freeze();
        
        int emptyCellId = _generator.Next(1, board.EmptyCells + 1);

        for (int i = 0; i < board.Size; i++)
        {
            for (int j = 0; j < board.Size; j++)
            {
                if (board.IsCellEmpty(i, j))
                    emptyCellId--;

                if (emptyCellId == 0)
                {
                    board.Place(i, j);
                    return;
                }
            }
        }
    }

    private void Freeze()
    {
        Console.WriteLine("Bot is thinking...");
        Thread.Sleep(1000);
    }
}