namespace Ex02
{
    public struct Card<T>
    {
        private readonly T r_CardValue;
        private bool m_IsRevealed;
        private bool m_IsMatched;

        public Card(T i_CardValue)
        {
            r_CardValue = i_CardValue;
            m_IsRevealed = false;
            m_IsMatched = false;
        }

        public T CardValue
        {
            get
            {
                return r_CardValue;
            }
        }

        public bool IsRevealed
        {
            get
            {
                return m_IsRevealed;
            }
            set
            {
                m_IsRevealed = value;
            }
        }

        public bool IsMatched
        {
            get
            {
                return m_IsMatched;
            }
            set
            {
                m_IsMatched = value;
            }
        }
    }
}
