using System;
using System.Collections.Generic;

namespace Ex02
{
    public class ComputerPlayer<T>
    {
        private const string r_PlayerName = "Computer";
        private int m_Score;

        public ComputerPlayer()
        {
            m_Score = 0;
        }

        public string Name
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public (int row, int column) MakeMove(Board<T> i_Board)
        {
            Random m_Random = new Random();
            var unrevealedCards = new List<(int row, int column)>();

            for (int row = 0; row < i_Board.Height; row++)
            {
                for (int column = 0; column < i_Board.Width; column++)
                {
                    if (!i_Board[row, column].IsRevealed)
                    {
                        unrevealedCards.Add((row, column));
                    }
                }
            }

            (int row, int column) o_DesiredMove = unrevealedCards[m_Random.Next(unrevealedCards.Count)];
            i_Board.FlipCard(o_DesiredMove.row, o_DesiredMove.column);
            return o_DesiredMove;
        }
    }
}