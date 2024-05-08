using Banks.ConsoleInterface;

namespace Banks
{
    public class Program
    {
        public static void Main()
        {
            new CentralBankHandler().Handle();
        }
    }
}