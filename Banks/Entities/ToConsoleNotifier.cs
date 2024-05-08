using Banks.Interfaces;

namespace Banks.Entities
{
    public class ToConsoleNotifier : INotifier
    {
        public ToConsoleNotifier()
        { }

        public void SendNotification(string message)
        {
            Console.WriteLine(message);
        }
    }
}
