using System.Collections.Generic;
using System;

namespace B20_Ex02_Ronen_316273655_Chen_312608417
{
    public class GameManagement
    {
        private const string k_Computer = "- computer -";
        private GameBoard m_GameBoard;
        private User m_MainPlayer;
        private User m_OpponentPlayer;
        private Pc m_Computer = null;
        private User m_CurrentPlayer;
        private List<char> m_DataOfCells;
        private ePlayerTurn m_CurrentPlayerTurn;
        private Random m_RandomMove = new Random();

        public GameManagement(int i_Height, int i_Width, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            m_GameBoard = new GameBoard(i_Height, i_Width);
            m_MainPlayer = new User(i_FirstPlayerName, "MainPlayer");
            m_OpponentPlayer = new User(i_SecondPlayerName, "OpponentPlayer");
            m_CurrentPlayer = new User(i_FirstPlayerName, "MainPlayer");

            if (i_SecondPlayerName == k_Computer)
            {
                m_Computer = new Pc(m_OpponentPlayer);
            }

            dataCellPlacement();
        }

        private void dataCellPlacement()
        {
            int gameBoardSize = m_GameBoard.GameBoardSize;
            char data = 'A';

            m_DataOfCells = new List<char>(gameBoardSize);

            for (int i = 0; i < gameBoardSize / 2; i++)
            {
                m_DataOfCells.Add(data);
                m_DataOfCells.Add(data);
                data++;
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

        public User CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
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

        private bool isAlreadyExposed(int i_RowNumber, int i_ColumnNumber)
        {
            return m_GameBoard.Cells[i_RowNumber, i_ColumnNumber].IsExposed;
        }

        public void RevealTheCell(GameBoard.Cell i_Cell)
        {
            m_GameBoard.Cells[i_Cell.Row, i_Cell.Column].IsExposed = true;
        }

        private void hideTheCell(GameBoard.Cell i_Cell)
        {
            m_GameBoard.Cells[i_Cell.Row, i_Cell.Column].IsExposed = false;
        }

        public bool CheckPairOfCells(
            GameBoard.Cell i_FirstCell,
            GameBoard.Cell i_SecondCell)
        {
            bool isPair = isPairOfMatchingCells(i_FirstCell, i_SecondCell);

            if (isPair)
            {
                updateSequenceCounter();
            }
            else
            {
                isNotPairOfCells(i_FirstCell, i_SecondCell);
            }

            return isPair;
        }

        private bool isPairOfMatchingCells(GameBoard.Cell i_FirstCell, GameBoard.Cell i_SecondCell)
        {
            int indexFirstCell = i_FirstCell.IndexValue;
            int indexSecondCell = i_SecondCell.IndexValue;

            return m_DataOfCells[indexFirstCell] == m_DataOfCells[indexSecondCell];
        }

        private void isNotPairOfCells(GameBoard.Cell i_FirstCell, GameBoard.Cell i_SecondCell)
        {
            updateComputerAIArrays(i_FirstCell, i_SecondCell);
            turnExchange();
            hideTheCell(i_FirstCell);
            hideTheCell(i_SecondCell);
        }

        private void updateComputerAIArrays(GameBoard.Cell i_FirstCell, GameBoard.Cell i_SecondCell)
        {
            if (m_CurrentPlayer.Name == k_Computer)
            {
                Computer.ExposeCellArrayForAI.Remove(i_FirstCell.IndexValue);
                Computer.ExposeCellArrayForAI.Remove(i_SecondCell.IndexValue);
            }
            else if (m_Computer != null)
            {
                Computer.RememberLastMovesForAI.Clear();
                Computer.RememberLastMovesForAI.Add(m_DataOfCells[i_FirstCell.IndexValue], i_FirstCell);
                Computer.RememberLastMovesForAI.Add(m_DataOfCells[i_SecondCell.IndexValue], i_SecondCell);
            }
        }

        private void turnExchange()
        {
            if (m_CurrentPlayerTurn == ePlayerTurn.MainPlayer)
            {
                m_CurrentPlayerTurn = ePlayerTurn.OpponentPlayer;
                m_CurrentPlayer.Title = OpponentPlayer.Title;
                m_CurrentPlayer.Name = OpponentPlayer.Name;
            }
            else
            {
                m_CurrentPlayerTurn = ePlayerTurn.MainPlayer;
                m_CurrentPlayer.Title = MainPlayer.Title;
                m_CurrentPlayer.Name = MainPlayer.Name;
            }
        }

        private void updateSequenceCounter()
        {
            if (m_CurrentPlayerTurn == ePlayerTurn.MainPlayer)
            {
                MainPlayer.SequenceCounter++;
            }
            else
            {
                OpponentPlayer.SequenceCounter++;
            }
        }

        public void ComputerFirstMove(ref GameBoard.Cell io_FirstCell)
        {
            int firstRowNumber, firstColumnNumber;

            receiveComputerMove(out firstRowNumber, out firstColumnNumber);
            io_FirstCell = GameBoard.Cells[firstRowNumber, firstColumnNumber];
        }

        public bool ComputerSecondMove(ref GameBoard.Cell io_FirstCell, ref GameBoard.Cell io_SecondCell)
        {
            bool isPair = false;
            int secondRowNumber, secondColumnNumber;
            bool isRememberCells = checkIfPcRememberCells(io_FirstCell);

            if (!isRememberCells)
            {
                isPair = true;
                Computer.RememberLastMovesForAI.Clear();
            }
            else
            {
                receiveComputerMove(out secondRowNumber, out secondColumnNumber);
                io_SecondCell = GameBoard.Cells[secondRowNumber, secondColumnNumber];
            }

            return isPair;
        }

        private bool checkIfPcRememberCells(GameBoard.Cell i_ComputerCell)
        {
            bool isNotEmpty = true;
            int countRememberCells = Computer.RememberLastMovesForAI.Count;

            if (countRememberCells > 0)
            {
                isNotEmpty = isTherePairInPcMemory(i_ComputerCell);
            }

            return isNotEmpty;
        }

        private bool isTherePairInPcMemory(GameBoard.Cell i_ComputerCell)
        {
            bool isSame = true;
            char isEqualCharacter = m_DataOfCells[i_ComputerCell.IndexValue];
            GameBoard.Cell cell;

            if (Computer.RememberLastMovesForAI.TryGetValue(isEqualCharacter, out cell))
            {
                isSame = isNotTheSameCell(cell, i_ComputerCell.IndexValue);
                if (!isSame)
                {
                    addToExpose(cell);
                }
            }

            return isSame;
        }

        private bool isNotTheSameCell(GameBoard.Cell i_Value, int i_IndexValue)
        {
            return m_DataOfCells[i_Value.IndexValue] == m_DataOfCells[i_IndexValue];
        }

        private void receiveComputerMove(out int o_RowNumber, out int o_ColumnNumber)
        {
            getTheCellPosition(out o_ColumnNumber, out o_RowNumber, m_RandomMove);

            while (isAlreadyExposed(o_RowNumber, o_ColumnNumber))
            {
                getTheCellPosition(out o_ColumnNumber, out o_RowNumber, m_RandomMove);
            }

            GameBoard.Cell cellToExpose = GameBoard.Cells[o_RowNumber, o_ColumnNumber];
            addToExpose(cellToExpose);
        }

        private void getTheCellPosition(out int o_ColumnNumber, out int o_RowNumber, Random i_RandomMove)
        {
            o_RowNumber = i_RandomMove.Next(0, GameBoard.Width);
            o_ColumnNumber = i_RandomMove.Next(0, GameBoard.Height);
        }

        private void addToExpose(GameBoard.Cell i_CellToExpose)
        {
            Computer.ExposeCellArrayForAI.Add(i_CellToExpose.IndexValue);
            RevealTheCell(i_CellToExpose);
        }

        public string TheWinner()
        {
            string winner = string.Empty;

            if (m_Computer == null)
            {
                if (m_MainPlayer.SequenceCounter > m_OpponentPlayer.SequenceCounter)
                {
                    winner = m_MainPlayer.Name;
                }
                else if (m_MainPlayer.SequenceCounter < m_OpponentPlayer.SequenceCounter)
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
                if (m_MainPlayer.SequenceCounter > this.m_Computer.UserData.SequenceCounter)
                {
                    winner = m_MainPlayer.Name;
                }
                else if (m_MainPlayer.SequenceCounter < this.m_Computer.UserData.SequenceCounter)
                {
                    winner = m_OpponentPlayer.Name;
                }
                else
                {
                    winner = m_MainPlayer.Name + " and " + this.m_Computer.UserData.Name;
                }
            }

            return winner;
        }

        public enum ePlayerTurn
        {
            MainPlayer,
            OpponentPlayer
        }
    }
}