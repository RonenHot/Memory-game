using System.Collections.Generic;

namespace B20_Ex02
{
    public class Pc 
    {
        private List<int> m_ExposeCellArrayForAI; 
        private Dictionary<char, GameBoard.Cell> m_RememberLastMovesForAI; 
        private User m_Computer;

        public Pc(User i_Computer)
        {
            m_Computer = i_Computer;
            m_ExposeCellArrayForAI = new List<int>();
            m_RememberLastMovesForAI = new Dictionary<char, GameBoard.Cell>();
        }

        public List<int> ExposeCellArrayForAI
        {
            get
            {
                return m_ExposeCellArrayForAI;
            }

            set
            {
                m_ExposeCellArrayForAI = value;
            }
        }

        public Dictionary<char, GameBoard.Cell> RememberLastMovesForAI
        {
            get
            {
                return m_RememberLastMovesForAI;
            }

            set
            {
                m_RememberLastMovesForAI = value;
            }
        }

        public User UserData
        {
            get
            {
                return m_Computer;
            }
        }
    }
}
