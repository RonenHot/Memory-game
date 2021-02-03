using System.Windows.Forms;
using System.Drawing;

namespace MemoryGame
{
    public partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();

            // labelMainPlayerName
            m_LabelMainPlayerName = new Label();
            m_LabelMainPlayerName.AutoSize = true;
            m_LabelMainPlayerName.Location = new Point(20, 23);
            m_LabelMainPlayerName.Size = new Size(90, 13);
            m_LabelMainPlayerName.Text = "Main Player Name:";
            this.Controls.Add(m_LabelMainPlayerName);

            // labelSecondPlayerName
            m_LabelSecondPlayerName = new Label();
            m_LabelSecondPlayerName.AutoSize = true;
            m_LabelSecondPlayerName.Location = new Point(20, 50);
            m_LabelSecondPlayerName.Size = new Size(90, 13);
            m_LabelSecondPlayerName.Text = "Second Player Name:";
            this.Controls.Add(m_LabelSecondPlayerName);

            // textBoxMainPlayer
            m_TextBoxMainPlayer = new TextBox();
            m_TextBoxMainPlayer.Location = new Point(149, 23);
            m_TextBoxMainPlayer.Size = new Size(127, 20);
            this.Controls.Add(m_TextBoxMainPlayer);

            // textBoxSecondPlayer
            m_TextBoxSecondPlayer = new TextBox();
            m_TextBoxSecondPlayer.Enabled = false;
            m_TextBoxSecondPlayer.Location = new Point(149, 50);
            m_TextBoxSecondPlayer.Size = new Size(127, 20);
            m_TextBoxSecondPlayer.Text = "- computer -";
            this.Controls.Add(m_TextBoxSecondPlayer);

            // buttonAgainstAFriend
            m_ButtonAgainstAFriend = new Button();
            m_ButtonAgainstAFriend.DialogResult = DialogResult.OK;
            m_ButtonAgainstAFriend.Location = new Point(290, 50);
            m_ButtonAgainstAFriend.Size = new Size(115, 23);
            m_ButtonAgainstAFriend.Text = "Against A Friend";
            m_ButtonAgainstAFriend.UseVisualStyleBackColor = true;
            m_ButtonAgainstAFriend.Click += new System.EventHandler(this.buttonAgainstAFriend_Click);
            this.Controls.Add(m_ButtonAgainstAFriend);

            // labelBoardSize
            m_LabelBoardSize = new Label();
            m_LabelBoardSize.AutoSize = true;
            m_LabelBoardSize.Location = new Point(19, 92);
            m_LabelBoardSize.Size = new Size(70, 20);
            m_LabelBoardSize.Text = "Board Size:";
            this.Controls.Add(m_LabelBoardSize);

            // buttonBoardSize
            m_ButtonBoardSize = new Button();
            m_ButtonBoardSize.BackColor = Color.LightSteelBlue;
            m_ButtonBoardSize.Location = new Point(110, 94);
            m_ButtonBoardSize.Size = new Size(140, 55);
            m_ButtonBoardSize.Text = "4x4";
            m_ButtonBoardSize.UseVisualStyleBackColor = false;
            m_ButtonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            this.Controls.Add(m_ButtonBoardSize);

            //  buttonStart
            m_ButtonStart = new Button();
            m_ButtonStart.BackColor = Color.ForestGreen;
            m_ButtonStart.Location = new Point(307, 139);
            m_ButtonStart.Size = new Size(98, 23);
            m_ButtonStart.Text = "Start!";
            m_ButtonStart.UseVisualStyleBackColor = false;
            m_ButtonStart.Click += new System.EventHandler(this.buttonStart_Click);
            this.Controls.Add(m_ButtonStart);

            //PropertiesForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(415, 176);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label m_LabelMainPlayerName;
        private Label m_LabelSecondPlayerName;
        private TextBox m_TextBoxMainPlayer;
        private TextBox m_TextBoxSecondPlayer;
        private Button m_ButtonAgainstAFriend;
        private Label m_LabelBoardSize;
        private Button m_ButtonBoardSize;
        private Button m_ButtonStart;
    }
}