namespace B20_Ex02
{
    public class User
    {
        private string m_Name;
        private int m_SequenceCounter;

        public User(string i_Name)
        {
            m_Name = i_Name;
            m_SequenceCounter = 0;
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