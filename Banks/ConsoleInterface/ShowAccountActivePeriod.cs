using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class ShowAccountActivePeriod
    {
        private Bank _bank;

        public ShowAccountActivePeriod(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            Console.WriteLine($"Account active period: {_bank.AccountActivePeriod.Days} days\n");
            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();

            new BankHandler(_bank).Handle();
        }
    }
}
