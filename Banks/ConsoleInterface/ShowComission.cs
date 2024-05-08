using Banks.Entities;
using Banks.Models;

namespace Banks.ConsoleInterface
{
    public class ShowComission
    {
        private Bank _bank;

        public ShowComission(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            Console.WriteLine($"Comission: {_bank.Comission.Count}");
            Console.WriteLine("Do you want to change comission? (Enter yes/no)");
            string? answer = Console.ReadLine();

            while (answer is null || (!answer.ToLower().Equals("yes") && !answer.ToLower().Equals("no")))
            {
                Console.WriteLine("The answer is not valid. Enter the answer again:");
                answer = Console.ReadLine();
            }

            if (answer.ToLower().Equals("yes"))
            {
                ChangeData();
            }

            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new BankHandler(_bank).Handle();
        }

        private void ChangeData()
        {
            while (true)
            {
                Console.WriteLine("Enter new comission for credit accounts:");
                string? answer = Console.ReadLine();
                try
                {
                    _bank.Comission = new Comission(decimal.Parse(answer ?? string.Empty));
                    Console.WriteLine("The comission changed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Do you want to try again?");
                    Console.WriteLine("Enter 'yes' if you want.");

                    answer = Console.ReadLine();
                    if (answer is not null && answer.ToLower().Equals("yes"))
                        continue;
                }

                break;
            }
        }
    }
}
