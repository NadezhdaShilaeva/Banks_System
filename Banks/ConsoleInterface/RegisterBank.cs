using Banks.Entities;
using Banks.Models;
using Banks.Services;

namespace Banks.ConsoleInterface
{
    public class RegisterBank
    {
        public void Handle()
        {
            string? answer = null;

            var bankBuilder = new Bank.BankBuilder();

            while (answer is null)
            {
                Console.WriteLine("Enter the name of bank:");
                answer = Console.ReadLine();
            }

            bankBuilder.SetName(answer);

            while (true)
            {
                Console.WriteLine("Enter the percent for debit account:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetDebitPercent(decimal.Parse(answer ?? string.Empty));
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
                Console.WriteLine("Enter the comission for credit account:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetComission(decimal.Parse(answer ?? string.Empty));
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
                Console.WriteLine("Enter the credit limit for credit account:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetCreditAccountLimit(decimal.Parse(answer ?? string.Empty));
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
                Console.WriteLine("Enter the limit above for accounts:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetAccountLimitAbove(decimal.Parse(answer ?? string.Empty));
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
                Console.WriteLine("Enter the limit above for transactions with account:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetTransactionLimit(decimal.Parse(answer ?? string.Empty));
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
                Console.WriteLine("Enter the active period in days for debit and credit accounts:");
                answer = Console.ReadLine();
                try
                {
                    bankBuilder.SetAccounActivePeriod(int.Parse(answer ?? string.Empty));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            bankBuilder.SetDepositAccountRates(CreateDepositeAccountRates());

            Bank bank = bankBuilder.Build();
            CentralBank.GetInstance().RegisterBank(bank);

            Console.WriteLine($"The bank {bank.Name} with ID {bank.Id} is registered in the Central Bank.\n");

            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new CentralBankHandler().Handle();
        }

        private DepositAccountRates CreateDepositeAccountRates()
        {
            Console.WriteLine("Create rates for the deposit bank accounts.");
            Console.WriteLine("Attention: the initial amount of each next rate must be greater than the previous one.\n");

            var builder = new DepositAccountRates.DepositAccountRatesBuilder();

            string? answer = null;
            while (answer is null || !IsFinishAnswer(answer))
            {
                Console.WriteLine("1. Add rate.");
                Console.WriteLine("2. Finish.");
                Console.WriteLine("Choose one number of the menu.");
                answer = Console.ReadLine();

                while (answer is null || !IsValidAnswer(answer))
                {
                    Console.WriteLine("The number is incorrect.");
                    Console.WriteLine("Choose one number of the menu again.");
                    answer = Console.ReadLine();
                }

                int number = int.Parse(answer);
                switch (number)
                {
                    case 1:
                        try
                        {
                            builder.AddRate(CreateDepositAccountRate());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                    case 2:
                        break;
                }
            }

            return builder.Build();
        }

        private DepositAccountRate CreateDepositAccountRate()
        {
            string? answer;
            decimal minMoneyCount;
            decimal percent;

            while (true)
            {
                Console.WriteLine("Enter the minimum count of money to new rate:");
                answer = Console.ReadLine();
                try
                {
                    minMoneyCount = decimal.Parse(answer ?? string.Empty);
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
                Console.WriteLine("Enter the percent to new rate:");
                answer = Console.ReadLine();
                try
                {
                    percent = decimal.Parse(answer ?? string.Empty);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                break;
            }

            return new DepositAccountRate(minMoneyCount, percent);
        }

        private bool IsValidAnswer(string answer)
        {
            int number;

            try
            {
                number = int.Parse(answer);
            }
            catch
            {
                return false;
            }

            if (number >= 1 && number <= 2)
            {
                return true;
            }

            return false;
        }

        private bool IsFinishAnswer(string answer)
        {
            int number;

            try
            {
                number = int.Parse(answer);
            }
            catch
            {
                return false;
            }

            if (number == 2)
            {
                return true;
            }

            return false;
        }
    }
}
