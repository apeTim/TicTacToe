namespace TicTacToeLib;

public class Board {
        private int _size;

        private bool _isActive;
        private int _emptyCells;
        private Move[,] _matrix;
        private Move _nextTurn = Move.Circle;
        private Move _winner = Move.Empty;

        public bool IsActive
        {
            get { return _isActive; }
        }

        public int Size
        {
            get { return _size; }
        }
        
        public int EmptyCells
        {
            get { return _emptyCells; }
        }

        public Move NextTurn
        {
            get { return _nextTurn; }
        }

        public Move Winner
        {
            get { return _winner; }
        }

        public Board(int size)
        {
            _size = size;
            _emptyCells = size * size;
            _matrix = new Move[size, size];
            _isActive = true;
        }

        public bool CanPlace(int row, int cell)
        {
            if (!(0 <= row && row < _size && 0 <= cell && cell < _size && IsCellEmpty(row, cell)))
                return false;

            return true;
        }

        public void Place(int row, int cell)
        {
            if (!CanPlace(row, cell))
                throw new InvalidDataException("Move is not possible");

            _matrix[row, cell] = _nextTurn;
            _nextTurn = _nextTurn == Move.Circle ? Move.Cross : Move.Circle;
            _emptyCells--;

            CheckGame();
        }

        public bool IsCellEmpty(int row, int column) => _matrix[row, column] == Move.Empty;

        private void CheckGame()
        {
            if (!_isActive)
                return;

            int[][]? combination = GetWinCombination();

            if (combination != null)
            {
                Move winner = _matrix[combination[0][0], combination[0][1]];
                SetWinner(winner);
            }

            if (_emptyCells == 0)
                _isActive = false;
        }

        private int[][]? GetWinCombination()
        {
            int[][]? combination = null;

            for (int i = 0; i < _size; i++)
            {
                bool horizontalCombination = true;
                bool verticalCombination = true;

                if (IsCellEmpty(i, 0))
                    horizontalCombination = false;
                if (IsCellEmpty(0, i))
                    verticalCombination = false;

                if (!horizontalCombination && !verticalCombination)
                    continue;

                for (int j = 0; j < _size; j++)
                {
                    if (_matrix[i, j] != _matrix[i, 0])
                    {
                        horizontalCombination = false;
                    }

                    if (_matrix[j, i] != _matrix[0, i])
                    {
                        verticalCombination = false;
                    }
                }

                if (horizontalCombination)
                {
                    combination = new int[_size][];
                    for (int j = 0; j < _size; j++)
                    {
                        combination[j] = new int[2] { i, j };
                    }
                }
                else if (verticalCombination)
                {
                    combination = new int[_size][];
                    for (int j = 0; j < _size; j++)
                    {
                        combination[j] = new int[2] { j, i };
                    }
                }

                if (horizontalCombination || verticalCombination)
                    return combination;
            }


            bool mainDiagonalCombination = true;
            bool antiDiagonalCombination = true;

            if (IsCellEmpty(0, 0))
                mainDiagonalCombination = false;
            if (IsCellEmpty(0, _size - 1))
                antiDiagonalCombination = false;

            if (!mainDiagonalCombination && !antiDiagonalCombination)
                return combination;

            for (int i = 0; i < _size; i++)
            {
                if (_matrix[i, i] != _matrix[0, 0])
                {
                    mainDiagonalCombination = false;
                }

                if (_matrix[i, _size - 1 - i] != _matrix[0, _size - 1])
                {
                    antiDiagonalCombination = false;
                }
            }

            if (mainDiagonalCombination)
            {
                combination = new int[_size][];
                for (int j = 0; j < _size; j++)
                {
                    combination[j] = new int[2] { j, j };
                }
            }
            else if (antiDiagonalCombination)
            {
                combination = new int[_size][];
                for (int j = 0; j < _size; j++)
                {
                    combination[j] = new int[2] { j, _size - 1 - j };
                }
            }

            return combination;
        }

        private void SetWinner(Move winner)
        {
            _winner = winner;
            _isActive = false;
        }

        public string GenerateHeadOutput()
        {
            string output = "\t";

            for (int i = 0; i < _size; i++)
            {
                output += $"{Crayon.Output.Bold(((char)('a' + i)).ToString())}\t";
            }

            return output;
        }
        
        public string GenerateBoardOutput()
        {
            string output = "";
            
            for (int i = 0; i < _size; i++)
            {
                output += $"{Crayon.Output.Bold((i + 1).ToString())}\t";

                for (int j = 0; j < _size; j++)
                {
                    output += !IsCellEmpty(i, j) ? $"{_matrix[i, j]}\t" : $"{Crayon.Output.Dim(_matrix[i, j].ToString())}\t"; 
                }

                output += "\n";
            }

            return output;
        }

        public string FinaliseBoardOutput(string output)
        {
            if (IsActive)
                throw new Exception("Board is active.");

            if (_winner == Move.Empty)
                return output;

            string[] outputCells = output.Split('\t');
            int[][]? combination = GetWinCombination();

            if (combination == null)
                return output;

            foreach (int[] comb in combination)
            {
                int index = comb[0] * (_size + 1) + comb[1] + 1;
                outputCells[index] = Crayon.Output.Green(outputCells[index]);
            }

            string resultOutput = String.Join('\t', outputCells);

            return resultOutput;
        }

        public override string ToString()
        {
            string output = GenerateHeadOutput();
            output += "\n";
            
            string boardOutput = GenerateBoardOutput();

            if (!IsActive)
                boardOutput = FinaliseBoardOutput(boardOutput);

            output += boardOutput;
            
            return output;
        }
}