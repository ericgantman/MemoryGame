namespace Ex02
{
    public struct HumanPlayer<T>
    {
        private readonly string r_PlayerName;
        private int m_ScoreOfThePlayer;

        public HumanPlayer(string i_Name)
        {
            r_PlayerName = i_Name;
            m_ScoreOfThePlayer = 0;
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
                return m_ScoreOfThePlayer;
            }

            set
            {
                m_ScoreOfThePlayer = value;
            }
        }

        public (int, int) MakeMove(int i_MoveRow, int i_MoveColumn, Board<T> i_Board)
        {
            (int, int) o_DesiredCardToFlip = (i_MoveRow, i_MoveColumn);

            i_Board.FlipCard(i_MoveRow, i_MoveColumn);

            return o_DesiredCardToFlip;
        }

        public void IncrementHumanPlayerScore()
        {
            m_ScoreOfThePlayer++;
        }
    }
}