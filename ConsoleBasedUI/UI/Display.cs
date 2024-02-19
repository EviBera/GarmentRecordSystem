namespace ConsoleBasedUI.UI
{
    public class Display
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void PrintColorMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
