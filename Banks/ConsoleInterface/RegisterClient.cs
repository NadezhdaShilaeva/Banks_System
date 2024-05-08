using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class RegisterClient
    {
        private Bank _bank;
        public RegisterClient(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            string? answer;

            var clientBuilder = new Client.ClientBuilder();

            while (true)
            {
                Console.WriteLine("Enter the name of client:");
                answer = Console.ReadLine();
                try
                {
                    clientBuilder.SetName(answer ?? string.Empty);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.WriteLine("Enter the surname of client:");
                answer = Console.ReadLine();
                try
                {
                    clientBuilder.SetSurname(answer ?? string.Empty);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.WriteLine("Enter the address of client or press enter to skip:");
                answer = Console.ReadLine();
                if (answer is not null && answer.Equals(string.Empty))
                {
                    break;
                }

                try
                {
                    clientBuilder.SetAddress(answer ?? string.Empty);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.WriteLine("Enter the passport number of client or press enter to skip:");
                answer = Console.ReadLine();
                if (answer is not null && answer.Equals(string.Empty))
                {
                    break;
                }

                try
                {
                    clientBuilder.SetPassport(int.Parse(answer ?? string.Empty));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            Client client = clientBuilder.Build();
            _bank.RegisterClient(client);

            Console.WriteLine($"The client {client.Name} {client.Surname} with ID {client.Id} is registered in the Bank {_bank.Name}.\n");

            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new BankHandler(_bank).Handle();
        }
    }
}
