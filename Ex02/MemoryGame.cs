using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ex02
{
    public class MemoryGame
    {
        private GameEngine<char> m_GameEngine;
        private string firstUserName;
        private int io_BoardWidth, io_BoardHeight;
        private bool isMultiplayers;
        private string secondUserName;

        public void InitalizeMemoryGame(bool newGame = false)
        {
            Screen.Clear();
            Console.WriteLine("Welcome to Memory Game!");
            if (!newGame)
            {
                firstUserName = getUserName();
                isMultiplayers = PlayAgainstHuman();
                if (isMultiplayers)
                {
                    secondUserName = getUserName();
                    GetBoardSize(out io_BoardWidth, out io_BoardHeight);
                    m_GameEngine = new GameEngine<char>(io_BoardWidth, io_BoardHeight, isMultiplayers, firstUserName, secondUserName);
                }
                else
                {
                    GetBoardSize(out io_BoardWidth, out io_BoardHeight);
                    m_GameEngine = new GameEngine<char>(io_BoardWidth, io_BoardHeight, isMultiplayers, firstUserName);
                }
            }
            else
            {
                GetBoardSize(out io_BoardWidth, out io_BoardHeight);
                if (isMultiplayers)
                {
                    m_GameEngine = new GameEngine<char>(io_BoardWidth, io_BoardHeight, isMultiplayers, firstUserName, secondUserName);
                }
                else
                {
                    m_GameEngine = new GameEngine<char>(io_BoardWidth, io_BoardHeight, isMultiplayers, firstUserName);
                }
            }
            InitalizeBoardWithUpperCaseLetters(io_BoardWidth, io_BoardHeight);
            StartGame();
        }

        public void GetBoardSize(out int io_BoardWidth, out int io_BoardHeight)
        {
            do
            {
                io_BoardHeight = GetValidDimension("row");
                io_BoardWidth = GetValidDimension("column");
            } while (!EvenMult(io_BoardWidth, io_BoardHeight));
        }

        public void InitalizeBoardWithUpperCaseLetters(int i_BoardWidth, int i_BoardHeight)
        {
            List<char> allCardValues = new List<char>();
            int numOfPairs = i_BoardWidth * i_BoardHeight / 2;

            for (int i = 0; i < numOfPairs; i++)
            {
                allCardValues.Add((char)('A' + i));
                allCardValues.Add((char)('A' + i));
            }
            m_GameEngine.Board.InitializeBoard(allCardValues);
        }

        public void StartGame()
        {
            bool isMatched = false;
            bool gameContinues = true;
            (int rowInput, int columnInput)? firstCard, secondCard;

            while (!m_GameEngine.IsGameOver())
            {
                Screen.Clear();
                printBoard();
                firstCard = getValidPlayerMove("first", ref gameContinues);
                if (!gameContinues)
                {
                    break;
                }

                firstCard = m_GameEngine.MakeMove(firstCard.Value.rowInput, firstCard.Value.columnInput);
                Screen.Clear();
                printBoard();
                secondCard = getValidPlayerMove("second", ref gameContinues);
                if (!gameContinues)
                {
                    break;
                }

                secondCard = m_GameEngine.MakeMove(secondCard.Value.rowInput, secondCard.Value.columnInput);
                Screen.Clear();
                printBoard();
                isMatched = m_GameEngine.CheckForMatch(firstCard.Value.rowInput, firstCard.Value.columnInput, secondCard.Value.rowInput, secondCard.Value.columnInput);
                if (!isMatched)
                {
                    Thread.Sleep(2000);
                }
            }
            if (gameContinues)
            {
                displayWinner();
            }
        }

        private (int, int)? getValidPlayerMove(string i_Order, ref bool i_GameContinues)
        {
            bool isComputerTurn = !m_GameEngine.FirstPlayerTurn && !m_GameEngine.IsMultiplayer;
            int o_RowOfValidMove = -1, o_ColumnOfValidMove = -1;
            string errorMessage;
            string move;

            do
            {
                if (isComputerTurn)
                {
                    break;
                }
                else
                {
                    string playerName = m_GameEngine.FirstPlayerTurn ? m_GameEngine.HumanPlayerOne.Value.Name : m_GameEngine.HumanPlayerTwo.Value.Name;
                    Console.WriteLine($"{playerName}, enter your {i_Order} move (e.g., A1): ");
                    move = Console.ReadLine().Trim();
                    if (move.ToLower() == "q")
                    {
                        i_GameContinues = false;
                        Screen.Clear();
                        Console.WriteLine("Thank you for playing, come again");
                        Thread.Sleep(2000);
                        break;
                    }
                    if (isValidCardInput(move, out o_RowOfValidMove, out o_ColumnOfValidMove, out errorMessage))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(errorMessage);
                    }
                }
            } while (true);

            return (o_RowOfValidMove, o_ColumnOfValidMove);
        }

        private bool isValidCardInput(string i_InputFromUser, out int io_RowNumber, out int io_ColumnNumber, out string io_errorMessage)
        {
            bool o_IsValid = true;
            io_RowNumber = -1;
            io_ColumnNumber = -1;
            io_errorMessage = string.Empty;

            if (i_InputFromUser.Length != 2)
            {
                o_IsValid = false;
                io_errorMessage = "Invalid input. Wrong length, please enter a valid input like A1.";
            }
            else if (i_InputFromUser[0] < 'A' || i_InputFromUser[0] > (char)('A' + m_GameEngine.Board.Width - 1))
            {
                o_IsValid = false;
                io_errorMessage = "Invalid input. Wrong column, please enter a valid input like A1.";
            }
            else if (i_InputFromUser[1] < '1' || i_InputFromUser[1] > (char)('0' + m_GameEngine.Board.Height))
            {
                o_IsValid = false;
                io_errorMessage = "Invalid input. Wrong row, please enter a valid input like A1.";
            }
            else
            {
                io_RowNumber = int.Parse(i_InputFromUser[1].ToString()) - 1;
                io_ColumnNumber = i_InputFromUser[0] - 'A';
                if (!m_GameEngine.Board.IsValidMove(io_RowNumber, io_ColumnNumber))
                {
                    o_IsValid = false;
                    io_errorMessage = "Invalid input. You Can't reviled a fliped card";
                }
            }

            return o_IsValid;
        }

        public int GetValidDimension(string i_DimensionName)
        {
            int o_DesiredSize;

            do
            {
                Console.WriteLine($"Enter the desired {i_DimensionName} (between 4 and 6): ");
                if (!int.TryParse(Console.ReadLine(), out o_DesiredSize))
                {
                    Console.WriteLine("Invalid input, please enter a valid number.");
                    continue;
                }
                else if (o_DesiredSize < 4 || o_DesiredSize > 6)
                {
                    Console.WriteLine("Invalid size, please enter a number between 4 and 6.");
                }
                else
                {
                    return o_DesiredSize;
                }
            } while (true);
        }

        public bool EvenMult(int i_DesireRowSize, int i_DesireColumnSize)
        {
            bool o_PossibleDimensionSize = false;

            if (i_DesireRowSize * i_DesireColumnSize % 2 == 0)
            {
                o_PossibleDimensionSize = true;
            }
            else
            {
                Console.WriteLine("Invalid size, please enter a number that will result in an even number of cells.");
            }

            return o_PossibleDimensionSize;
        }

        public bool PlayAgainstHuman()
        {
            bool o_PlayingAgainstHuman = true;

            Console.Write("Do you want to play against the computer? (y/n): ");
            string input = Console.ReadLine().Trim().ToLower();

            while (input != "y" && input != "n")
            {
                Console.Write("Invalid input. Please enter 'y' for yes or 'n' for no: ");
                input = Console.ReadLine().Trim().ToLower();
            }

            if (input == "y")
            {
                o_PlayingAgainstHuman = false;
            }

            return o_PlayingAgainstHuman;
        }

        private string getUserName()
        {
            string o_DesiredUserName = string.Empty;
            do
            {
                Console.Write("Enter your name (no spaces, max 20 chars): ");
                o_DesiredUserName = Console.ReadLine();
                if (o_DesiredUserName.Length > 20)
                {
                    Console.WriteLine("Name is too long, please try again");
                }
                else if (o_DesiredUserName.Contains(' '))
                {
                    Console.WriteLine("Name cannot contain spaces, please try again");
                }
                else
                {
                    break;
                }

            } while (true);

            return o_DesiredUserName;
        }

        private void printBoard()
        {
            Board<char> gameBoard = m_GameEngine.Board;
            int width = gameBoard.Width;
            int height = gameBoard.Height;
            StringBuilder printedBoard = new StringBuilder();

            printedBoard.Append("   ");
            for (char columnHeader = 'A'; columnHeader < 'A' + width; columnHeader++)
            {
                printedBoard.Append($"  {columnHeader} ");
            }
            printedBoard.AppendLine();
            printedBoard.Append("   ");
            printedBoard.Append(new string('=', width * 4 + 1));
            printedBoard.AppendLine();
            for (int row = 0; row < height; row++)
            {
                printedBoard.Append($" {row + 1} |");

                for (int column = 0; column < width; column++)
                {
                    if (gameBoard[row, column].IsRevealed)
                    {
                        printedBoard.Append($" {gameBoard[row, column].CardValue} ");
                    }
                    else
                    {
                        printedBoard.Append("   ");
                    }

                    printedBoard.Append("|");
                }

                printedBoard.AppendLine();
                printedBoard.Append("   ");
                printedBoard.Append(new string('=', width * 4 + 1));
                printedBoard.AppendLine();
            }

            Console.WriteLine(printedBoard.ToString());
        }

        private void displayWinner()
        {
            (string name, int score) winner;

            Console.WriteLine("Game over!");
            if (m_GameEngine.IsATie())
            {
                Console.WriteLine("It's a draw!");
            }
            else
            {
                winner = m_GameEngine.DetermineWinner();
                Console.WriteLine($"Congratulations {winner.name}! You are the winner! with the score of {winner.score}");
            }

            Console.WriteLine("Do you want to play again? Press y");
            if (Console.ReadLine().ToLower() == "y")
            {
                InitalizeMemoryGame(true);
            }
            else
            {
                Console.WriteLine("Thank You for Playing");
                Thread.Sleep(2000);
            }
        }
    }
}
