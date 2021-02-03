using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace B20_Ex02
{
    internal class UserInterface
    {
        private const int k_MinimumBoundary = 4;
        private const int k_MaximumBoundary = 6;
        private GameManagement m_GameManagement;
        private string m_CurrentPlayer;
        private List<char> m_DataOfCells;
        private GameManagement.eUserDecision m_UserDecision;

        internal void StartGame()
        {
            getDataFromUser();
            dataCellPlacement();
            playGame();
        }

        private void getDataFromUser()
        {
            Console.WriteLine(
                @"Hello, welcome to a memory game card.
Please enter your name: ");

            string mainPlayerName;

            while(!checkNameValidation(out mainPlayerName))
            {
                Console.WriteLine(
                    @"The name 'Computer' is reserved for the PC.
Please choose another name:");
            }

            Console.WriteLine(
                @"Against who would you like to play?
1. Another player
2. Computer system
(Choose a number 1 or 2):");

            m_UserDecision = checkUserOptionChoiceValidity();

            string opponentPlayerName;

            if(m_UserDecision == GameManagement.eUserDecision.AnotherPlayer)
            {
                Console.WriteLine("Please enter the name of the second player:");

                while(!checkNameValidation(out opponentPlayerName) || mainPlayerName == opponentPlayerName)
                {
                    Console.WriteLine(
                        @"This name is already taken.
Please choose another name:");
                }
            }
            else
            {
                opponentPlayerName = "Computer";
            }

            int height, width;

            enterDimensionsMatrix(out height, out width);
            m_GameManagement = new GameManagement(height, width, mainPlayerName, opponentPlayerName);
        }

        private void dataCellPlacement()
        {
            int gameBoardSize = m_GameManagement.GameBoard.GameBoardSize;
            char data = 'A';

            m_DataOfCells = new List<char>(gameBoardSize);

            for(int i = 0; i < gameBoardSize / 2; i++)
            {
                m_DataOfCells.Add(data);
                m_DataOfCells.Add(data);
                data++;
            }
        }

        private bool checkNameValidation(out string o_PlayerName)
        {
            o_PlayerName = Console.ReadLine();

            return o_PlayerName != "Computer";
        }

        private void enterDimensionsMatrix(out int o_Height, out int o_Width)
        {
            Console.WriteLine("Please enter the height for the game board: ");

            o_Height = checkHeightAndWidthValidity();

            Console.WriteLine("Please enter the width for the game board: ");

            o_Width = checkHeightAndWidthValidity();

            if(!GameBoard.IsEvenMatrix(o_Height, o_Width))
            {
                Console.WriteLine("Error - The dimensions of the matrix (height x width) must be even.");

                // enter the dimensions of the matrix again
                enterDimensionsMatrix(out o_Height, out o_Width);
            }
        }

        private GameManagement.eUserDecision checkUserOptionChoiceValidity()
        {
            int userChoice;

            while(!int.TryParse(Console.ReadLine(), out userChoice) || !isLegalChoice(userChoice))
            {
                Console.WriteLine(
                    @"Invalid input.
Please choose 1 or 2.");
            }

            return (GameManagement.eUserDecision)userChoice;
        }

        private bool isLegalBoundaryInput(int i_UserInput)
        {
            bool isInBoundaries = i_UserInput >= k_MinimumBoundary && i_UserInput <= k_MaximumBoundary;

            if(!isInBoundaries)
            {
                printErrorBoundaryMsg();
            }

            return isInBoundaries;
        }

        private bool isLegalChoice(int i_Choice)
        {
            return i_Choice == (int)GameManagement.eUserDecision.AnotherPlayer
                   || i_Choice == (int)GameManagement.eUserDecision.ComputerSystem;
        }

        private void printErrorBoundaryMsg()
        {
            string msg = string.Format(
                @"Out of boundaries !
minimum input : {0}
maximum input : {1}.
please try again : ",
                k_MinimumBoundary,
                k_MaximumBoundary);

            Console.WriteLine(msg);
        }

        private int checkHeightAndWidthValidity()
        {
            int userInput;
            bool isNumber = int.TryParse(Console.ReadLine(), out userInput);

            while(!isNumber || !isLegalBoundaryInput(userInput))
            {
                if(!isNumber)
                {
                    Console.WriteLine("Wrong Input , please enter only positive digit for the boundary:");
                }

                isNumber = int.TryParse(Console.ReadLine(), out userInput);
            }

            return userInput;
        }

        private void playGame()
        {
            bool gameProgress = true;
            GameManagement.ePlayerTurn currentPlayerTurn = GameManagement.ePlayerTurn.MainPlayer;

            while(gameProgress)
            {
                if(currentPlayerTurn == GameManagement.ePlayerTurn.MainPlayer)
                {
                    m_CurrentPlayer = m_GameManagement.MainPlayer.Name;
                    humanPlayerMove(ref currentPlayerTurn);
                }
                else
                {
                    m_CurrentPlayer = m_GameManagement.OpponentPlayer.Name;
                    if(m_UserDecision == GameManagement.eUserDecision.AnotherPlayer)
                    {
                        humanPlayerMove(ref currentPlayerTurn);
                    }
                    else
                    {
                        computerSystemMove(ref currentPlayerTurn);
                    }
                }

                gameProgress = !m_GameManagement.IsGameOver();
            }

            printTheWinner();
            endOfGame();
        }

        // The purpose of artificial intelligence is to cause the computer not to make the wrong choices
        // such as choosing a cell that is already exposed or choosing a cell outside the boundaries.
        // AI is expressed through the memory of one previous turn that the player performed.
        // Following the operation of memorizing the previous turn,
        // The computer "remembers" the cell location that contains the pairing to the cell that was picked up
        private void computerSystemMove(ref GameManagement.ePlayerTurn io_CurrentPlayerTurn)
        {
            int firstRowNumber, firstColumnNumber;
            int secondRowNumber, secondColumnNumber;

            receiveComputerMove(out firstRowNumber, out firstColumnNumber);
            GameBoard.Cell firstComputerCell = m_GameManagement.GameBoard.Cells[firstRowNumber, firstColumnNumber];
            bool isRememberCells = checkIfPcRememberCells(firstComputerCell);

            if(!isRememberCells)
            {
                updatePointsAndPrintMsg(ref io_CurrentPlayerTurn);
                m_GameManagement.Computer.RememberLastMovesForAI.Clear();
            }
            else
            {
                receiveComputerMove(out secondRowNumber, out secondColumnNumber);
                GameBoard.Cell secondComputerCell =
                    m_GameManagement.GameBoard.Cells[secondRowNumber, secondColumnNumber];

                if(!checkPairOfCells(ref io_CurrentPlayerTurn, firstComputerCell, secondComputerCell))
                {
                    isNotPairOfCells(ref io_CurrentPlayerTurn, firstComputerCell, secondComputerCell);
                }
            }
        }

        private bool checkIfPcRememberCells(GameBoard.Cell i_ComputerCell)
        {
            bool isNotEmpty = true;
            int countRememberCells = m_GameManagement.Computer.RememberLastMovesForAI.Count;

            if(countRememberCells > 0)
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

            if(m_GameManagement.Computer.RememberLastMovesForAI.TryGetValue(isEqualCharacter, out cell))
            {
                isSame = isNotTheSameCell(cell, i_ComputerCell.IndexValue);
                if(!isSame)
                {
                    addToExposeAndPrint(cell);
                }
            }

            return isSame;
        }

        private bool isNotTheSameCell(GameBoard.Cell i_Value, int i_IndexValue)
        {
            return i_Value.IndexValue == i_IndexValue;
        }

        private void receiveComputerMove(out int o_RowNumber, out int o_ColumnNumber)
        {
            Ex02.ConsoleUtils.Screen.Clear(); // clear the console
            boardPrinter();
            Console.WriteLine("PC turn");
            Thread.Sleep(1000);

            Random randomMove = new Random();
            getTheCellPosition(out o_ColumnNumber, out o_RowNumber, randomMove);

            while(theSelectedCellIsExposed(o_RowNumber, o_ColumnNumber))
            {
                getTheCellPosition(out o_ColumnNumber, out o_RowNumber, randomMove);
            }

            GameBoard.Cell cellToExpose = m_GameManagement.GameBoard.Cells[o_RowNumber, o_ColumnNumber];
            addToExposeAndPrint(cellToExpose);
        }

        private void addToExposeAndPrint(GameBoard.Cell i_CellToExpose)
        {
            m_GameManagement.Computer.ExposeCellArrayForAI.Add(i_CellToExpose.IndexValue);
            revealAndPrintCell(i_CellToExpose);
        }

        private void revealAndPrintCell(GameBoard.Cell i_CellToExpose)
        {
            m_GameManagement.RevealTheCell(i_CellToExpose);
            Ex02.ConsoleUtils.Screen.Clear(); // clear the console
            boardPrinter();
            Thread.Sleep(500);
        }

        private bool theSelectedCellIsExposed(int i_RowNumber, int i_ColumnNumber)
        {
            bool isExposeCell = true;
            int currentCellValue = getCurrentCellValue(i_RowNumber, i_ColumnNumber);
            bool isNotEmptyArray = m_GameManagement.Computer.ExposeCellArrayForAI.Count > 0;

            if(isNotEmptyArray)
            {
                foreach(int cellIndexValue in m_GameManagement.Computer.ExposeCellArrayForAI)
                {
                    isExposeCell = currentCellValue == cellIndexValue;
                    if(isExposeCell)
                    {
                        break;
                    }
                }
            }

            return isExposeCell && isNotEmptyArray;
        }

        private int getCurrentCellValue(int i_RowNumber, int i_ColumnNumber)
        {
            return m_GameManagement.GameBoard.Cells[i_RowNumber, i_ColumnNumber].IndexValue;
        }

        private void getTheCellPosition(out int o_ColumnNumber, out int o_RowNumber, Random i_RandomMove)
        {
            o_RowNumber = i_RandomMove.Next(0, m_GameManagement.GameBoard.Width);
            o_ColumnNumber = i_RandomMove.Next(0, m_GameManagement.GameBoard.Height);
        }

        // User Turn:
        private void humanPlayerMove(ref GameManagement.ePlayerTurn io_CurrentPlayerTurn)
        {
            Ex02.ConsoleUtils.Screen.Clear(); // clear the console
            boardPrinter();

            int firstRowNumber, firstColumnNumber;
            int secondRowNumber, secondColumnNumber;

            chooseInput(out firstColumnNumber, out firstRowNumber);
            GameBoard.Cell firstCell = m_GameManagement.GameBoard.Cells[firstRowNumber, firstColumnNumber];

            revealAndPrintCell(firstCell);
            chooseInput(out secondColumnNumber, out secondRowNumber);

            GameBoard.Cell secondCell = m_GameManagement.GameBoard.Cells[secondRowNumber, secondColumnNumber];

            revealAndPrintCell(secondCell);

            if(checkPairOfCells(ref io_CurrentPlayerTurn, firstCell, secondCell))
            {
                if(m_UserDecision == GameManagement.eUserDecision.ComputerSystem)
                {
                    m_GameManagement.Computer.ExposeCellArrayForAI.Add(firstCell.IndexValue);
                    m_GameManagement.Computer.ExposeCellArrayForAI.Add(secondCell.IndexValue);
                }
            }
            else
            {
                isNotPairOfCells(ref io_CurrentPlayerTurn, firstCell, secondCell);
            }
        }

        private bool checkPairOfCells(
            ref GameManagement.ePlayerTurn io_CurrentPlayerTurn,
            GameBoard.Cell i_FirstCell,
            GameBoard.Cell i_SecondCell)
        {
            bool isPair = isPairOfMatchingCells(i_FirstCell, i_SecondCell);

            if(isPair)
            {
                updatePointsAndPrintMsg(ref io_CurrentPlayerTurn);
            }

            return isPair;
        }

        private void updatePointsAndPrintMsg(ref GameManagement.ePlayerTurn io_CurrentPlayerTurn)
        {
            m_GameManagement.UpdateSequenceCounter(ref io_CurrentPlayerTurn);
            Console.WriteLine("Well done! you've exposed a pair");
            Thread.Sleep(1000);
        }

        private void isNotPairOfCells(
            ref GameManagement.ePlayerTurn io_CurrentPlayerTurn,
            GameBoard.Cell i_FirstCell,
            GameBoard.Cell i_SecondCell)
        {
            updateComputerAIArrays(i_FirstCell, i_SecondCell);

            Console.WriteLine("Not too bad, maybe next turn");
            Thread.Sleep(2000);
            m_GameManagement.HideTheCell(i_FirstCell);
            m_GameManagement.HideTheCell(i_SecondCell);
            m_GameManagement.TurnExchange(ref io_CurrentPlayerTurn);
        }

        private void updateComputerAIArrays(GameBoard.Cell i_FirstCell, GameBoard.Cell i_SecondCell)
        {
            if(m_CurrentPlayer == "Computer")
            {
                m_GameManagement.Computer.ExposeCellArrayForAI.Remove(i_FirstCell.IndexValue);
                m_GameManagement.Computer.ExposeCellArrayForAI.Remove(i_SecondCell.IndexValue);
            }
            else if(m_UserDecision == GameManagement.eUserDecision.ComputerSystem)
            {
                m_GameManagement.Computer.RememberLastMovesForAI.Clear();
                m_GameManagement.Computer.RememberLastMovesForAI.Add(
                    m_DataOfCells[i_FirstCell.IndexValue],
                    i_FirstCell);
                m_GameManagement.Computer.RememberLastMovesForAI.Add(
                    m_DataOfCells[i_SecondCell.IndexValue],
                    i_SecondCell);
            }
        }

        private bool isPairOfMatchingCells(GameBoard.Cell i_FirstCell, GameBoard.Cell i_SecondCell)
        {
            int indexFirstCell = i_FirstCell.IndexValue;
            int indexSecondCell = i_SecondCell.IndexValue;

            return m_DataOfCells[indexFirstCell] == m_DataOfCells[indexSecondCell];
        }

        private void chooseInput(out int o_ColumnNumber, out int o_RowNumber)
        {
            string msg = string.Format(
                @"{0}, Please choose the cell you want to reveal: 
(choose letter (row) and then digit (column), for example: A1",
                m_CurrentPlayer);

            Console.WriteLine(msg);

            string userCellChoice = Console.ReadLine();

            if(m_GameManagement.IsQuit(userCellChoice))
            {
                Console.WriteLine(string.Format("{0},You decided to quit the game, goodbye!", m_CurrentPlayer));
                Thread.Sleep(2000);
                Environment.Exit(0);
            }

            checkTheCellInput(ref userCellChoice);
            convertsLetterToNumber(out o_RowNumber, out o_ColumnNumber, userCellChoice);
        }

        private void checkTheCellInput(ref string io_CellInput)
        {
            while(isNotValidCellInput(io_CellInput))
            {
                Console.WriteLine("Please try to choose the cell again:");
                io_CellInput = Console.ReadLine();
            }
        }

        private bool isNotValidCellInput(string i_CellInput)
        {
            bool isNotValidCellInput = true;

            if(i_CellInput.Length != 2)
            {
                Console.WriteLine("Wrong input! too many values , cell format input: [row][column], for example: A1");
            }
            else if(!(char.IsLetter(i_CellInput[0]) && isUpperCaseLetter(i_CellInput[0])))
            {
                Console.WriteLine("Wrong input! Row should be Upper-case: for example: A");
            }
            else if(!char.IsDigit(i_CellInput[1]))
            {
                Console.WriteLine("Wrong input! Column should be a digit, for example: 1");
            }
            else
            {
                isNotValidCellInput = checkCellInput(i_CellInput);
            }

            return isNotValidCellInput;
        }

        private bool isUpperCaseLetter(char i_CellInput)
        {
            return i_CellInput >= 'A' && i_CellInput <= 'Z';
        }

        private bool checkCellInput(string i_CellInput)
        {
            bool isExpose = true;
            int rowNumber, columnNumber;

            convertsLetterToNumber(out rowNumber, out columnNumber, i_CellInput);

            bool isAtBoundary = m_GameManagement.CheckBoundary(rowNumber, columnNumber);

            if(!isAtBoundary)
            {
                Console.WriteLine("Out of boundary.");
            }
            else
            {
                isExpose = m_GameManagement.IsAlreadyExposed(rowNumber, columnNumber);
                if(isExpose)
                {
                    Console.WriteLine("The cell you selected is already exposed.");
                }
            }

            return !isAtBoundary || isExpose;
        }

        private void convertsLetterToNumber(out int o_RowNumber, out int o_ColumnNumber, string i_CellToCheck)
        {
            o_ColumnNumber = i_CellToCheck[0] - 'A'; // converts a letter to a number
            o_RowNumber = i_CellToCheck[1] - '1'; // converts char digit to an integer digit
        }

        private void boardPrinter()
        {
            const int k_NumberOfSpacesFromBeginning = 2;
            const int k_NumberOfEqualSign = 4;
            const int k_ExtraEqualSign = 1;
            const int k_ExtraSpace = 1;

            StringBuilder gameBoardFormat = new StringBuilder();

            gameBoardFormat.Append(' ', k_NumberOfSpacesFromBeginning + k_ExtraSpace);
            printFirstLine(ref gameBoardFormat);

            gameBoardFormat.AppendLine();
            gameBoardFormat.Append(' ', k_NumberOfSpacesFromBeginning);
            gameBoardFormat.Append('=', (m_GameManagement.GameBoard.Height * k_NumberOfEqualSign) + k_ExtraEqualSign);

            int numericCounter = 1;

            for(int i = 0; i < m_GameManagement.GameBoard.Width; i++)
            {
                gameBoardFormat.AppendLine();
                gameBoardFormat.Append(numericCounter.ToString());
                numericCounter++;
                gameBoardFormat.Append(" |");

                printTheBoardCell(ref gameBoardFormat, i);

                gameBoardFormat.AppendLine();
                gameBoardFormat.Append(' ', k_NumberOfSpacesFromBeginning);
                gameBoardFormat.Append(
                    '=',
                    (m_GameManagement.GameBoard.Height * k_NumberOfEqualSign) + k_ExtraEqualSign);
            }

            Console.WriteLine(gameBoardFormat.ToString());
        }

        private void printTheBoardCell(ref StringBuilder io_GameBoardFormat, int i_Index)
        {
            const char k_EmptyCell = ' ';

            for(int j = 0; j < m_GameManagement.GameBoard.Height; j++)
            {
                // print the content in the cell:
                if(m_GameManagement.IsAlreadyExposed(i_Index, j))
                {
                    io_GameBoardFormat.Append(string.Format(" {0} |", exposeCellData(i_Index, j)));
                }
                else
                {
                    io_GameBoardFormat.Append(string.Format(" {0} |", k_EmptyCell));
                }
            }
        }

        private void printFirstLine(ref StringBuilder io_GameBoardFormat)
        {
            const int k_NumberOfSpacesBeforeLetter = 1;
            const int k_NumberOfSpacesAfterLetter = 2;
            char alphabeticCounter = 'A';

            for(int j = 0; j < m_GameManagement.GameBoard.Height; j++)
            {
                io_GameBoardFormat.Append(' ', k_NumberOfSpacesBeforeLetter);
                io_GameBoardFormat.Append(alphabeticCounter);
                alphabeticCounter++;
                io_GameBoardFormat.Append(' ', k_NumberOfSpacesAfterLetter);
            }
        }

        private char exposeCellData(int i_RowIndex, int i_ColIndex)
        {
            return m_DataOfCells[m_GameManagement.GameBoard.Cells[i_RowIndex, i_ColIndex].IndexValue];
        }

        private void printPlayersScore()
        {
            User mainPlayer = m_GameManagement.MainPlayer;
            User opponentPlayer;

            if(m_UserDecision == GameManagement.eUserDecision.AnotherPlayer)
            {
                opponentPlayer = m_GameManagement.Computer.UserData;
            }
            else
            {
                opponentPlayer = m_GameManagement.OpponentPlayer;
            }

            string msg = string.Format(
                @"{0}'s score is: {1}
{2}'s score is: {3}",
                mainPlayer.Name,
                mainPlayer.SequenceCounter,
                opponentPlayer.Name,
                opponentPlayer.SequenceCounter);

            Console.WriteLine(msg);
        }

        private void printTheWinner()
        {
            printPlayersScore();
            string winner = m_GameManagement.TheWinner(m_UserDecision);

            string msg = string.Format(@"The Winner: {0}", winner);

            Console.WriteLine(msg);
        }

        private void endOfGame()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Would you like one more round? Yes/No");
            string answer = Console.ReadLine();

            if(answer == "yes" || answer == "Yes" || answer == "YES")
            {
                UserInterface userInterface = new UserInterface();
                Ex02.ConsoleUtils.Screen.Clear(); // clear the console
                userInterface.StartGame();
            }
            else if(answer == "no" || answer == "No" || answer == "NO")
            {
                Console.WriteLine(
                    @"Thank you for playing !
Hope you enjoyed. See you next time.");
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("Invalid input. Choose only Yes/No");
                endOfGame();
            }
        }
    }
}