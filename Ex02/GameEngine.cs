using System;

namespace Ex02
{
    public class GameEngine<T>
    {
        private Board<T> m_GameBoard;
        private HumanPlayer<T> m_HumanPlayerOne;
        private HumanPlayer<T> m_HumanPlayerTwo;
        private ComputerPlayer<T> m_ComputerPlayer;
        private readonly bool m_IsMultiplayer;
        private bool m_FirstPlayerTurn;

        public GameEngine(int i_Width, int i_Height, bool i_IsMultiplayer, string i_PlayerNameNumberOne, string i_PlayerNameNumberTwo = "")
        {
            m_GameBoard = new Board<T>(i_Width, i_Height);
            m_IsMultiplayer = i_IsMultiplayer;
            m_HumanPlayerOne = new HumanPlayer<T>(i_PlayerNameNumberOne);
            m_FirstPlayerTurn = true;
            if (i_IsMultiplayer)
            {
                m_HumanPlayerTwo = new HumanPlayer<T>(i_PlayerNameNumberTwo);
            }
            else
            {
                m_ComputerPlayer = new ComputerPlayer<T>();
            }
        }

        public Board<T> Board
        {
            get
            {
                return m_GameBoard;
            }
            set
            {
                m_GameBoard = value;
            }
        }

        public HumanPlayer<T>? HumanPlayerOne
        {
            get
            {
                return m_HumanPlayerOne;
            }
        }

        public HumanPlayer<T>? HumanPlayerTwo
        {
            get
            {
                return m_HumanPlayerTwo;
            }
        }

        public ComputerPlayer<T> ComputerPlayer
        {
            get
            {
                return m_ComputerPlayer;
            }
        }

        public bool IsMultiplayer
        {
            get
            {
                return m_IsMultiplayer;
            }
        }

        public bool FirstPlayerTurn
        {
            get
            {
                return m_FirstPlayerTurn;
            }
            set
            {
                m_FirstPlayerTurn = value;
            }
        }

        public bool IsGameOver()
        {
            return m_GameBoard.IsBoardFull();
        }

        public bool CheckForMatch(int i_FirstRowMove, int i_FirstColumnMove, int i_SecondRowMove, int i_SecondColumnMove)
        {
            bool isMatch = false;

            if (m_GameBoard[i_FirstRowMove, i_FirstColumnMove].CardValue.Equals(m_GameBoard[i_SecondRowMove, i_SecondColumnMove].CardValue))
            {
                m_GameBoard.CardIsMatch(i_FirstRowMove, i_FirstColumnMove);
                m_GameBoard.CardIsMatch(i_SecondRowMove, i_SecondColumnMove);
                isMatch = true;
                if (m_FirstPlayerTurn)
                {
                    m_HumanPlayerOne.Score++;
                }
                else
                {
                    if (m_IsMultiplayer)
                    {
                        m_HumanPlayerTwo.Score++;
                    }
                    else
                    {
                        m_ComputerPlayer.Score++;
                    }
                }
            }
            else
            {
                m_GameBoard.FlipBackCard(i_FirstRowMove, i_FirstColumnMove);
                m_GameBoard.FlipBackCard(i_SecondRowMove, i_SecondColumnMove);
                m_FirstPlayerTurn = !m_FirstPlayerTurn;
                isMatch = false;
            }

            return isMatch;
        }

        public (int, int) MakeMove(int i_RowMove, int i_ColumnMove)
        {
            (int, int) o_DesiredMove;

            if (m_FirstPlayerTurn)
            {
                o_DesiredMove = m_HumanPlayerOne.MakeMove(i_RowMove, i_ColumnMove, m_GameBoard);
            }
            else
            {
                if (m_IsMultiplayer)
                {
                    o_DesiredMove = m_HumanPlayerTwo.MakeMove(i_RowMove, i_ColumnMove, m_GameBoard);
                }
                else
                {
                    o_DesiredMove = m_ComputerPlayer.MakeMove(m_GameBoard);
                }
            }

            return o_DesiredMove;
        }

        public bool IsATie()
        {
            bool o_IsTie = false;

            if (m_IsMultiplayer)
            {
                o_IsTie = m_HumanPlayerOne.Score == m_HumanPlayerTwo.Score;
                Console.WriteLine(m_HumanPlayerOne.Score);
            }
            else
            {
                o_IsTie = m_HumanPlayerOne.Score == m_ComputerPlayer.Score;
            }

            return o_IsTie;
        }

        public (string, int) DetermineWinner()
        {
            string o_WinnerName = string.Empty;
            int o_MaxScore = 0;
            o_MaxScore = m_HumanPlayerOne.Score;
            o_WinnerName = m_HumanPlayerOne.Name;

            if (m_IsMultiplayer && m_HumanPlayerTwo.Score > o_MaxScore)
            {
                o_MaxScore = m_HumanPlayerTwo.Score;
                o_WinnerName = m_HumanPlayerTwo.Name;
            }
            if (!m_IsMultiplayer && m_ComputerPlayer.Score > o_MaxScore)
            {
                o_MaxScore = m_ComputerPlayer.Score;
                o_WinnerName = m_ComputerPlayer.Name;
            }

            return (o_WinnerName, o_MaxScore);
        }
    }
}