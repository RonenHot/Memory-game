using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using B20_Ex02_Ronen_316273655_Chen_312608417;

namespace MemoryGame
{
    public partial class FormGame : Form
    {
        private const string k_Computer = "- computer -";
        private readonly Color r_ControlColor = Color.DarkSlateGray;
        private Control[,] m_Controls;
        private List<PictureBox> m_DataOfCells;
        private eClickIndex m_ClickIndex = eClickIndex.FirstChoice;
        private GameManagement m_GameManagement = null;
        private GameBoard.Cell m_FirstCellChoice = null;
        private GameBoard.Cell m_SecondCellChoice = null;
        private GameBoard.Cell m_CurrentCellChoice = null;
        private int m_CellsRows;
        private int m_CellsColumns;

        public FormGame(int i_Rows, int i_Columns, string i_MainPlayer, string i_OpponentPlayer)
        {
            m_CellsRows = i_Rows;
            m_CellsColumns = i_Columns;
            m_GameManagement = new GameManagement(i_Columns, i_Rows, i_MainPlayer, i_OpponentPlayer);
            m_Controls = new Control[m_CellsRows, m_CellsColumns];
            InitializeComponent();
            dataCellPlacement();
            refreshLabelsState();
            ShowDialog();
        }

        private void memoryGame_Start(object sender, EventArgs e)
        {
            int height = 20;
            int width;

            // add all pictureBox to the form
            for(int i = 0; i < m_CellsRows; i++)
            {
                width = 10;
                for(int j = 0; j < m_CellsColumns; j++)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.BackColor = r_ControlColor;
                    pictureBox.Size = new Size(90, 90);
                    pictureBox.Location = new Point(width, height);
                    pictureBox.AutoSize = true;
                    m_Controls[i, j] = pictureBox;
                    this.Controls.Add(pictureBox);

                    pictureBox.Click += new EventHandler(pictureBox_Click);
                    width += 10 + pictureBox.Width;
                }

                height += 10 + m_Controls[0, 0].Height;
            }

            // add names and score
            width = 10;
            refreshLabelsState();
        }

        private void dataCellPlacement()
        {
            int gameBoardSize = m_GameManagement.GameBoard.GameBoardSize;
            m_DataOfCells = new List<PictureBox>(gameBoardSize);

            for (int i = 0; i < gameBoardSize / 2; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(80, 80);
                pictureBox.AutoSize = true;
                pictureBox.TabIndex = i;
                pictureBox.Load(@"https://picsum.photos/80");
                m_DataOfCells.Add(pictureBox);
                m_DataOfCells.Add(pictureBox);
            }
        }

        private void refreshLabelsState()
        {
            if(m_GameManagement.CurrentPlayer.Title == m_GameManagement.MainPlayer.Title)
            {
                m_CurrentPlayer.BackColor = m_MainPlayer.BackColor;
            }
            else
            {
                m_CurrentPlayer.BackColor = m_OpponentPlayer.BackColor;
            }

            m_CurrentPlayer.Text = string.Format("Current Player: {0}", m_GameManagement.CurrentPlayer.Name);
            m_MainPlayer.Text = string.Format(
                "{0} : {1} Pairs",
                m_GameManagement.MainPlayer.Name,
                m_GameManagement.MainPlayer.SequenceCounter);
            m_OpponentPlayer.Text = string.Format(
                "{0} : {1} Pairs",
                m_GameManagement.OpponentPlayer.Name,
                m_GameManagement.OpponentPlayer.SequenceCounter);
            m_MainPlayer.Update();
            m_OpponentPlayer.Update();
            m_CurrentPlayer.Update();
        }
        // $G$ DSN-003 (-3) This method is too long. Should be split into several methods.
        private void pictureBox_Click(object sender, EventArgs e)
        {
            bool isGameOver = false;

            if(!isGameOver)
            {
                bool isPair = false;

                switch(m_ClickIndex)
                {
                    case eClickIndex.FirstChoice:

                        getClickedCell((Control)sender);
                        m_FirstCellChoice = m_CurrentCellChoice;
                        firstChoice();
                        m_ClickIndex = eClickIndex.SecondChoice;
                        break;

                    case eClickIndex.SecondChoice:

                        getClickedCell((Control)sender);
                        m_SecondCellChoice = m_CurrentCellChoice;
                        secondChoice(ref isPair);
                        Thread.Sleep(600);
                        m_ClickIndex = eClickIndex.CheckPair;
                        break;
                }

                if(m_ClickIndex == eClickIndex.CheckPair)
                {
                    if(!isPair)
                    {
                        turnOffButton();
                        refreshLabelsState();
                        while(m_GameManagement.CurrentPlayer.Name == k_Computer && !m_GameManagement.IsGameOver())
                        {
                            computerMoves(ref isPair);
                            Thread.Sleep(300);
                            refreshLabelsState();
                        }
                    }

                    m_ClickIndex = eClickIndex.FirstChoice;
                }

                refreshLabelsState();
                isGameOver = m_GameManagement.IsGameOver();
                if(isGameOver)
                {
                    this.Hide();
                    string theWinner = m_GameManagement.TheWinner();
                    displayEndDialog(theWinner);
                }
            }
        }

        private void computerMoves(ref bool io_IsPair)
        {
            m_GameManagement.ComputerFirstMove(ref m_FirstCellChoice);
            firstChoice();
            Thread.Sleep(300);
            io_IsPair = m_GameManagement.ComputerSecondMove(ref m_FirstCellChoice, ref m_SecondCellChoice);
            secondChoice(ref io_IsPair);
            Thread.Sleep(400);
            if(!io_IsPair)
            {
                turnOffButton();
            }
        }

        private void firstChoice()
        {
            PictureBox pictureBoxTemp = m_DataOfCells[m_FirstCellChoice.IndexValue];

            m_GameManagement.RevealTheCell(m_FirstCellChoice);
            this.m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].BackgroundImage = pictureBoxTemp.Image;
            this.m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].BackgroundImageLayout
                = ImageLayout.Center;
            this.m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].BackColor = m_CurrentPlayer.BackColor;
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].Enabled = false;
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].Update();
        }

        private void secondChoice(ref bool io_IsPair)
        {
            PictureBox pictureBoxTemp = m_DataOfCells[m_SecondCellChoice.IndexValue];

            m_GameManagement.RevealTheCell(m_SecondCellChoice);
            this.m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].BackgroundImage = pictureBoxTemp.Image;
            this.m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].BackgroundImageLayout
                = ImageLayout.Center;
            this.m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].BackColor = m_CurrentPlayer.BackColor;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].Enabled = false;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].Update();
            io_IsPair = m_GameManagement.CheckPairOfCells(m_FirstCellChoice, m_SecondCellChoice);
        }

        private void turnOffButton()
        {
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].BackgroundImage = null;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].BackgroundImage = null;
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].BackColor = r_ControlColor;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].BackColor = r_ControlColor;
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].Enabled = true;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].Enabled = true;
            m_Controls[m_SecondCellChoice.Row, m_SecondCellChoice.Column].Update();
            m_Controls[m_FirstCellChoice.Row, m_FirstCellChoice.Column].Update();
        }

        private void getClickedCell(Control i_ClickedCell)
        {
            for(int row = 0; row < m_GameManagement.GameBoard.Width; row++)
            {
                for(int column = 0; column < m_GameManagement.GameBoard.Height; column++)
                {
                    if(m_Controls[row, column].Equals(i_ClickedCell))
                    {
                        m_CurrentCellChoice = m_GameManagement.GameBoard.Cells[row, column];
                    }
                }
            }
        }

        private void displayEndDialog(string i_TheWinner)
        {
            string caption = "The game is over";
            string message = string.Format(
                @"The winner is : {0}
            
Would you like one more round ? ",
                i_TheWinner);
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Display the messageBox:
            result = MessageBox.Show(message, caption, buttons);
            if(result == DialogResult.Yes)
            {
                this.Close();
                FormGame startNewGame = new FormGame(
                    this.m_CellsRows,
                    this.m_CellsColumns,
                    m_GameManagement.MainPlayer.Name,
                    m_GameManagement.OpponentPlayer.Name);
            }
            else
            {
                string goodBye = string.Format(
                    @"Thank you for playing!
Hope you enjoyed. 
See you next time.");
                MessageBox.Show(goodBye, caption);
                this.Close();
                Environment.Exit(0); 
            }
        }

        public enum eClickIndex
        {
            FirstChoice,
            SecondChoice,
            CheckPair
        }
    }
}