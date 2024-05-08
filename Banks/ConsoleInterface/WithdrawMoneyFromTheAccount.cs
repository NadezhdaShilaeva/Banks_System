using Banks.Interfaces;
using Banks.Services;

namespace Banks.ConsoleInterface
{
    public class WithdrawMoneyFromTheAccount
    {
        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of withdrowal account:");
                string? fromAccountID = Console.ReadLine();

                Console.WriteLine("Enter the count of money to withdraw from the account:");
                string? money = Console.ReadLine();
                try
                {
                    ITransaction transaction = CentralBank.GetInstance().WithdrawMoneyFromTheAccount(
                        Guid.Parse(fromAccountID ?? string.Empty),
                        new Models.TransactionMoney(decimal.Parse(money ?? string.Empty)));

                    Console.WriteLine($"The transaction with ID {transaction.Id} executed.\n");
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
            new CentralBankHandler().Handle();
        }
    }
}
