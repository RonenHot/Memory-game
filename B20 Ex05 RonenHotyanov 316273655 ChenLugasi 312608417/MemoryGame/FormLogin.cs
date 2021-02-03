using System;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class FormLogin : Form
    {
        private const int k_MinimumBoardBorder = 4;
        private const int k_MaximumBoardBorder = 6;
        private int m_CurrentWidthBoardBorder = 4;
        private int m_CurrentHeightBoardBorder = 4;

        public FormLogin()
        {
            InitializeComponent();
            this.ShowDialog();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            boardBorder();
        }

        private void boardBorder()
        {
            if(m_CurrentHeightBoardBorder < k_MaximumBoardBorder)
            {
                m_CurrentHeightBoardBorder++;

                if(!isEvenBoardBorder())
                {
                    m_CurrentHeightBoardBorder++;
                }
            }
            else if(m_CurrentWidthBoardBorder < k_MaximumBoardBorder)
            {
                m_CurrentWidthBoardBorder++;
                m_CurrentHeightBoardBorder = k_MinimumBoardBorder;
            }
            else
            {
                m_CurrentWidthBoardBorder = k_MinimumBoardBorder;
                m_CurrentHeightBoardBorder = k_MinimumBoardBorder;
            }

            string changeText = string.Format(@"{0}x{1}", m_CurrentWidthBoardBorder, m_CurrentHeightBoardBorder);
            m_ButtonBoardSize.Text = changeText;
        }

        private bool isEvenBoardBorder()
        {
            return m_CurrentWidthBoardBorder % 2 == 0 || m_CurrentHeightBoardBorder % 2 == 0;
        }

        private void buttonAgainstAFriend_Click(object sender, EventArgs e)
        {
            if(m_TextBoxSecondPlayer.Text == "- computer -")
            {
                m_TextBoxSecondPlayer.Enabled = true;
                m_TextBoxSecondPlayer.Text = string.Empty;
                m_ButtonAgainstAFriend.Text = "Against Computer";
            }
            else
            {
                m_ButtonAgainstAFriend.Text = "Against A Friend";
                m_TextBoxSecondPlayer.Text = "- computer -";
                m_TextBoxSecondPlayer.Enabled = false;
            }

            this.DialogResult = sender == m_ButtonAgainstAFriend ? DialogResult.None : DialogResult.Cancel;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            int rows = m_CurrentWidthBoardBorder;
            int columns = m_CurrentHeightBoardBorder;
            FormGame startGame = new FormGame(rows, columns, m_TextBoxMainPlayer.Text, m_TextBoxSecondPlayer.Text);
            this.Show();
        }
    }
}
