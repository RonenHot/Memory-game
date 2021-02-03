namespace B20_Ex02
{
    public class GameManagement
    {
        private GameBoard m_GameBoard;
        private User m_MainPlayer;
        private User m_OpponentPlayer;
        private Pc m_Computer = null;

        public GameManagement(int i_Height, int i_Width, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            m_GameBoard = new GameBoard(i_Height, i_Width);
            m_MainPlayer = new User(i_FirstPlayerName);
            m_OpponentPlayer = new User(i_SecondPlayerName);

            if(i_SecondPlayerName == "Computer")
            {
                m_Computer = new Pc(m_OpponentPlayer);
            }
        }

        public GameBoard GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public User MainPlayer
        {
            get
            {
                return m_MainPlayer;
            }
        }

        public User OpponentPlayer
        {
            get
            {
                return m_OpponentPlayer;
            }
        }

        public Pc Computer
        {
            get
            {
                return m_Computer;
            }
        }

        public bool IsGameOver()
        {
            int sumOfPoints = m_MainPlayer.SequenceCounter + m_OpponentPlayer.SequenceCounter;

            return sumOfPoints == m_GameBoard.GameBoardSize / 2;
        }

        public bool CheckBoundary(int i_RowNumber, int i_ColumnNumber)
        {
            return (i_RowNumber >= 0 && i_RowNumber < m_GameBoard.Width)
                   && (i_ColumnNumber >= 0 && i_ColumnNumber < m_GameBoard.Height);
        }

        public bool IsAlreadyExposed(int i_RowNumber, int i_ColumnNumber)
        {
            return m_GameBoard.Cells[i_RowNumber, i_ColumnNumber].IsExposed;
        }

        public void RevealTheCell(GameBoard.Cell i_Cell)
        {
            m_GameBoard.Cells[i_Cell.Row, i_Cell.Column].IsExposed = true;
        }

        public void HideTheCell(GameBoard.Cell i_Cell)
        {
            m_GameBoard.Cells[i_Cell.Row, i_Cell.Column].IsExposed = false;
        }

        public void TurnExchange(ref ePlayerTurn io_CurrentPlayerTurn)
        {
            io_CurrentPlayerTurn = io_CurrentPlayerTurn == ePlayerTurn.MainPlayer
                                       ? ePlayerTurn.OpponentPlayer
                                       : ePlayerTurn.MainPlayer;
        }

        public void UpdateSequenceCounter(ref ePlayerTurn io_CurrentPlayerTurn)
        {
            if (io_CurrentPlayerTurn == ePlayerTurn.MainPlayer)
            {
                MainPlayer.SequenceCounter++;
            }
            else
            {
                OpponentPlayer.SequenceCounter++;
            }
        }

        public string TheWinner(eUserDecision i_UserDecision)
        {
            string winner = string.Empty;

            if(i_UserDecision == eUserDecision.AnotherPlayer)
            {
                if(m_MainPlayer.SequenceCounter > m_OpponentPlayer.SequenceCounter)
                {
                    winner = m_MainPlayer.Name;
                }
                else if(m_MainPlayer.SequenceCounter < m_OpponentPlayer.SequenceCounter)
                {
                    winner = m_OpponentPlayer.Name;
                }
                else
                {
                    winner = m_MainPlayer.Name + " and " + m_OpponentPlayer.Name;
                }
            }
            else
            {
                if(m_MainPlayer.SequenceCounter > m_Computer.UserData.SequenceCounter)
                {
                    winner = m_MainPlayer.Name;
                }
                else if(m_MainPlayer.SequenceCounter < m_Computer.UserData.SequenceCounter)
                {
                    winner = m_OpponentPlayer.Name;
                }
                else
                {
                    winner = m_MainPlayer.Name + " and " + m_Computer.UserData.Name;
                }
            }

            return winner;
        }

        public bool IsQuit(string i_IsQuit)
        {
            return i_IsQuit == "Q";
        }

        public enum ePlayerTurn
        {
            MainPlayer,
            OpponentPlayer
        }

        public enum eUserDecision
        {
            AnotherPlayer = 1,
            ComputerSystem = 2
        }
    }
}
