using Banks.Entities;
using Banks.Models;

namespace Banks.ConsoleInterface
{
    public class ShowDepositAccountRates
    {
        private Bank _bank;

        public ShowDepositAccountRates(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            Console.WriteLine("{0, 22}{1, 10}", "Minimum count of money", "Percent");
            foreach (DepositAccountRate rate in _bank.DepositAccountRates.Rates)
            {
                Console.WriteLine("{0, 22}{1, 10}", rate.MinMoneyCount, rate.Percent.Number);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new BankHandler(_bank).Handle();
        }
    }
}
