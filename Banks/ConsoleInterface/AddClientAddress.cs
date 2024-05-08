using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class AddClientAddress
    {
        private Bank _bank;

        public AddClientAddress(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the client:");
                string? clientID = Console.ReadLine();

                Console.WriteLine("Enter the address of the client:");
                string? address = Console.ReadLine();

                try
                {
                    _bank.SetClientAddress(Guid.Parse(clientID ?? string.Empty), address ?? string.Empty);
                    Console.WriteLine($"The address was added to the client with ID {clientID}.\n");
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
