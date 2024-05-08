using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Services;

namespace Banks.ConsoleInterface
{
    public class ShowBankAccount
    {
        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the bank account:");
                string? accountID = Console.ReadLine();

                try
                {
                    IBankAccount account = CentralBank.GetInstance().GetBankAccount(Guid.Parse(accountID ?? string.Empty));

                    PrintAccountInfo(account);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Do you want to try get information about the bank account again?");
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

        private void PrintAccountInfo(IBankAccount account)
        {
            if (account is DebitBankAccount)
                Console.WriteLine("The debit bank account.");
            if (account is DepositAccountRate)
                Console.WriteLine("The deposit bank account.");
            if (account is CreateCreditBankAccount)
                Console.WriteLine("The credit bank account.");
            Console.WriteLine($"Acoount ID:     {account.Id}");
            Console.WriteLine($"Client ID:      {account.Client.Id}");
            Console.WriteLine($"Client name:    {account.Client.Name} {account.Client.Surname}");
            Console.WriteLine($"Count of money: {account.Money}");
            Console.WriteLine($"Percent:        {account.Percent.Number}");
            Console.WriteLine($"Comission:      {account.Comission.Count}");
            Console.WriteLine($"Limit above:    {account.LimitAbove}");
            Console.WriteLine($"Limit below:    {account.LimitBelow}\n");
        }
    }
}
