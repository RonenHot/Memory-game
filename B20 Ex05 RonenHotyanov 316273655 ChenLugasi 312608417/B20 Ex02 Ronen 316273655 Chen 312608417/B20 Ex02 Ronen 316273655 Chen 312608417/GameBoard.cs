using System;

namespace B20_Ex02_Ronen_316273655_Chen_312608417
{
    public class GameBoard
    {
        private readonly int r_Height;
        private readonly int r_Width;
        private readonly int r_GameBoardSize;
        private readonly Cell[,] r_Cells;
        private Random m_Random = new Random();

        public GameBoard(int i_Height, int i_Width)
        {
            r_Width = i_Width;
            r_Height = i_Height;
            r_GameBoardSize = r_Width * r_Height;
            r_Cells = new Cell[i_Width, i_Height];
            initializeGameBoard();
        }

        public int Height
        {
            get
            {
                return r_Height;
            }
        }

        public int Width
        {
            get
            {
                return r_Width;
            }
        }

        public int GameBoardSize
        {
            get
            {
                return r_GameBoardSize;
            }
        }

        public Cell[,] Cells
        {
            get
            {
                return r_Cells;
            }
        }

        private void initializeGameBoard()
        {
            int indexValue = 0;

            for (int i = 0; i < r_Width; i++)
            {
                for (int j = 0; j < r_Height; j++)
                {
                    r_Cells[i, j] = new Cell(i, j, indexValue);
                    indexValue++;
                }
            }

            randomPositionOfPairs();
        }

        private void randomPositionOfPairs()
        {
            for (int i = 0; i < r_Width; i++)
            {
                for (int j = 0; j < r_Height; j++)
                {
                    int row = m_Random.Next(0, r_Width);
                    int column = m_Random.Next(0, r_Height);
                    int currentValue = r_Cells[i, j].IndexValue;

                    r_Cells[i, j].IndexValue = r_Cells[row, column].IndexValue;
                    r_Cells[row, column].IndexValue = currentValue;
                }
            }
        }

        public class Cell
        {
            private int m_Row;
            private int m_Column;
            private int m_IndexValue;
            private bool m_IsExposed;

            public Cell(int i_Row, int i_Column, int i_IndexValue)
            {
                m_IndexValue = i_IndexValue;
                m_IsExposed = false;
                m_Row = i_Row;
                m_Column = i_Column;
            }

            public bool IsExposed
            {
                get
                {
                    return m_IsExposed;
                }

                set
                {
                    m_IsExposed = value;
                }
            }

            public int IndexValue
            {
                get
                {
                    return m_IndexValue;
                }

                set
                {
                    m_IndexValue = value;
                }
            }

            public int Row
            {
                get
                {
                    return m_Row;
                }

                set
                {
                    m_Row = value;
                }
            }

            public int Column
            {
                get
                {
                    return m_Column;
                }

                set
                {
                    m_Column = value;
                }
            }

            public static bool operator ==(Cell i_FirstCell, Cell i_SecondCell)
            {
                return i_FirstCell.IndexValue == i_SecondCell.IndexValue;
            }

            public static bool operator !=(Cell i_FirstCell, Cell i_SecondCell)
            {
                return !(i_FirstCell == i_SecondCell);
            }

            public override bool Equals(object i_OtherCell)
            {
                bool equals = false;

                // costs less to check from ahead null ref, than operate Exception mechanism
                if (i_OtherCell != null)
                {
                    // costs less to check from ahead casting validity, than operate Exception mechanism
                    if (i_OtherCell.GetType() == this.GetType())
                    {
                        equals = this.IndexValue == ((Cell)i_OtherCell).IndexValue;
                    }
                }

                return equals;
            }

            public override int GetHashCode()
            {
                return this.m_IndexValue;
            }
        }
    }
}