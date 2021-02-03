namespace B20_Ex02_Ronen_316273655_Chen_312608417
{
    public class User
    {
        private string m_Name;
        private string m_Title;
        private int m_SequenceCounter;

        public User(string i_Name, string i_Title)
        {
            m_Name = i_Name;
            m_SequenceCounter = 0;
            m_Title = i_Title;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }

            set
            {
                m_Title = value;
            }
        }

        public int SequenceCounter
        {
            get
            {
                return m_SequenceCounter;
            }

            set
            {
                m_SequenceCounter = value;
            }
        }
    }
}