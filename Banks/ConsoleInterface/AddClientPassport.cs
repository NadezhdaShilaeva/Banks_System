using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class AddClientPassport
    {
        private Bank _bank;

        public AddClientPassport(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the client:");
                string? clientID = Console.ReadLine();

                Console.WriteLine("Enter the passport number of the client:");
                string? passport = Console.ReadLine();

                try
                {
                    _bank.SetClientPassport(Guid.Parse(clientID ?? string.Empty), new Models.PassportData(int.Parse(passport ?? string.Empty)));
                    Console.WriteLine($"The passport number was added to the client with ID {clientID}.");
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
