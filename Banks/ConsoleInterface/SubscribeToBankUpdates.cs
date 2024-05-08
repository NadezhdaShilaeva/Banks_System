using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class SubscribeToBankUpdates
    {
        private Bank _bank;

        public SubscribeToBankUpdates(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the client:");
                string? clientID = Console.ReadLine();

                try
                {
                    Client client = _bank.SubscribeToBankUpdates(Guid.Parse(clientID ?? string.Empty));
                    client.Notifier = new ToConsoleNotifier();
                    Console.WriteLine($"The clint with ID: {clientID} was subscribed to updates of bank {_bank.Name}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Do you want to try again?");
                    Console.WriteLine("Enter 'yes' if you want.");

                    string? answer = Console.ReadLine();
                    if (answer is not null && answer.ToLower().Equals("yes"))
                        continue;
                }

                break;
            }

            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new BankHandler(_bank).Handle();
        }
    }
}
