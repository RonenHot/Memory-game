namespace B20_Ex02
{
    public class Program
    {
        public static void Main()
        {
            MemoryGame();
        }

        internal static void MemoryGame()
        {
            UserInterface userInterface = new UserInterface();
            userInterface.StartGame();
        }
    }
}
