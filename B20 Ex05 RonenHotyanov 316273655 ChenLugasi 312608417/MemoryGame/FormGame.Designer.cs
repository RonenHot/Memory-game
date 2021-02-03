using System.Windows.Forms;
using System.Drawing;

namespace MemoryGame
{
    public partial class FormGame
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

            m_CurrentPlayer = new Label();
            m_CurrentPlayer.AutoSize = true;
            m_CurrentPlayer.Location = new Point(10, m_CellsRows * 100 + 15);
            this.Controls.Add(m_CurrentPlayer);

            m_MainPlayer = new Label();
            m_MainPlayer.AutoSize = true;
            m_MainPlayer.BackColor = Color.DarkOrchid;
            m_MainPlayer.Location = new Point(10, m_CellsRows * 100 + 35);
            this.Controls.Add(m_MainPlayer);

            m_OpponentPlayer = new Label();
            m_OpponentPlayer.AutoSize = true;
            m_OpponentPlayer.BackColor = Color.SpringGreen;
            m_OpponentPlayer.Location = new Point(10, m_CellsRows * 100 + 55);
            this.Controls.Add(m_OpponentPlayer);

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(m_CellsColumns * 100 + 20 , m_CellsRows * 110 + 40);
            this.Text = "Memory Game";
            this.Load += new System.EventHandler(this.memoryGame_Start);
        }

        #endregion

        private Label m_MainPlayer;
        private Label m_OpponentPlayer;
        private Label m_CurrentPlayer;
    }
}