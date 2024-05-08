using Banks.Entities;

namespace Banks.ConsoleInterface
{
    public class BankHandler
    {
        private Bank _bank;

        public BankHandler(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            Console.WriteLine($"Hello! Welcome to the Bank {_bank.Name}!");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Register new client.");
            Console.WriteLine("2. Add client address.");
            Console.WriteLine("3. Add client passport.");
            Console.WriteLine("4. Create debit bank account.");
            Console.WriteLine("5. Create deposit bank account.");
            Console.WriteLine("6. Create credit bank account.");
            Console.WriteLine("7. Subscribe client to bank updates.");
            Console.WriteLine("8. Show deposit account rates.");
            Console.WriteLine("9. Show active period of the bank account.");
            Console.WriteLine("10. Show transaction limit.");
            Console.WriteLine("11. Show debit percent.");
            Console.WriteLine("12. Show comission.");
            Console.WriteLine("13. Show credit account limit.");
            Console.WriteLine("14. Show account limit above.");
            Console.WriteLine("15. Exit the bank.");
            Console.WriteLine("Choose one number of the menu.");

            string? answer = Console.ReadLine();

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
                    new RegisterClient(_bank).Handle();
                    break;
                case 2:
                    new AddClientAddress(_bank).Handle();
                    break;
                case 3:
                    new AddClientPassport(_bank).Handle();
                    break;
                case 4:
                    new CreateDebitBankAccount(_bank).Handle();
                    break;
                case 5:
                    new CreateDepositBankAccount(_bank).Handle();
                    break;
                case 6:
                    new CreateCreditBankAccount(_bank).Handle();
                    break;
                case 7:
                    new SubscribeToBankUpdates(_bank).Handle();
                    break;
                case 8:
                    new ShowDepositAccountRates(_bank).Handle();
                    break;
                case 9:
                    new ShowAccountActivePeriod(_bank).Handle();
                    break;
                case 10:
                    new ShowTransactionLimit(_bank).Handle();
                    break;
                case 11:
                    new ShowDebitPercent(_bank).Handle();
                    break;
                case 12:
                    new ShowComission(_bank).Handle();
                    break;
                case 13:
                    new ShowCreditAccountLimit(_bank).Handle();
                    break;
                case 14:
                    new ShowAccountLimitAbove(_bank).Handle();
                    break;
                default:
                    Console.WriteLine($"Thank you for your visit to the Bank {_bank.Name}!\n");
                    new CentralBankHandler().Handle();
                    break;
            }
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

            if (number >= 1 && number <= 15)
            {
                return true;
            }

            return false;
        }
    }
}
