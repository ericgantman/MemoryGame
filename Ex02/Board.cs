using System;
using System.Collections.Generic;

namespace Ex02
{
    public class Board<T>
    {
        private readonly int r_WidthSize;
        private readonly int r_HeightSize;
        private Card<T>[,] m_GameBoard;

        public Board(int i_Width, int i_Height)
        {
            r_WidthSize = i_Width;
            r_HeightSize = i_Height;
            m_GameBoard = new Card<T>[r_HeightSize, r_WidthSize];
        }

        public int Width
        {
            get
            {
                return r_WidthSize;
            }
        }

        public int Height
        {
            get
            {
                return r_HeightSize;
            }
        }

        public Card<T> this[int i_Row, int i_Column]
        {
            get
            {
                return m_GameBoard[i_Row, i_Column];
            }
            set
            {
                m_GameBoard[i_Row, i_Column] = value;
            }
        }

        public bool IsValidMove(int i_Row, int i_Column)
        {
            bool o_IsValid = true, isRevealed = false, isOutOfBounds = false;

            isOutOfBounds = i_Row < 0 || i_Row >= r_HeightSize || i_Column < 0 || i_Column >= r_WidthSize;
            isRevealed = m_GameBoard[i_Row, i_Column].IsRevealed || m_GameBoard[i_Row, i_Column].IsMatched;
            o_IsValid = !isOutOfBounds && !isRevealed;

            return o_IsValid;
        }

        public bool IsBoardFull()
        {
            bool o_BoardIsFull = true;

            for (int i = 0; i < r_HeightSize; i++)
            {
                for (int j = 0; j < r_WidthSize; j++)
                {
                    if (!m_GameBoard[i, j].IsMatched)
                    {
                        o_BoardIsFull = false;
                        break;
                    }
                }
            }

            return o_BoardIsFull;
        }

        public void InitializeBoard(List<T> i_AllCardValues)
        {
            T selectedValue;

            for (int row = 0; row < r_HeightSize; row++)
            {
                for (int column = 0; column < r_WidthSize; column++)
                {
                    selectedValue = i_AllCardValues[0];
                    m_GameBoard[row, column] = new Card<T>(selectedValue);
                    i_AllCardValues.RemoveAt(0);
                }
            }

            ShuffelBoard();
        }

        public void ShuffelBoard()
        {
            Random random = new Random();
            int numberOfCards, randomIndex;
            List<Card<T>> cardList = new List<Card<T>>();
            Card<T> tempCardValueToSwap;

            for (int row = 0; row < r_HeightSize; row++)
            {
                for (int col = 0; col < r_WidthSize; col++)
                {
                    cardList.Add(m_GameBoard[row, col]);
                }
            }

            numberOfCards = cardList.Count;
            while (numberOfCards > 1)
            {
                numberOfCards--;
                randomIndex = random.Next(numberOfCards + 1);
                tempCardValueToSwap = cardList[randomIndex];
                cardList[randomIndex] = cardList[numberOfCards];
                cardList[numberOfCards] = tempCardValueToSwap;
            }

            for (int row = 0; row < r_HeightSize; row++)
            {
                for (int column = 0; column < r_WidthSize; column++)
                {
                    m_GameBoard[row, column] = cardList[row * r_WidthSize + column];
                }
            }
        }

        public void FlipCard(int i_Row, int i_Column)
        {
            m_GameBoard[i_Row, i_Column].IsRevealed = true;
        }

        public void FlipBackCard(int i_Row, int i_Column)
        {
            m_GameBoard[i_Row, i_Column].IsRevealed = false;
        }

        public void CardIsMatch(int i_Row, int i_Column)
        {
            m_GameBoard[i_Row, i_Column].IsMatched = true;
        }
    }
}