namespace Banks.ConsoleInterface
{
    public class CentralBankHandler
    {
        public void Handle()
        {
            Console.WriteLine("Hello! Welcome to the Central Bank!");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Register new bank.");
            Console.WriteLine("2. Choose the bank.");
            Console.WriteLine("3. Put money to the account.");
            Console.WriteLine("4. Withdraw money from the account.");
            Console.WriteLine("5. Transfer money.");
            Console.WriteLine("6. Cancel transaction.");
            Console.WriteLine("7. Show information about the bank account.");
            Console.WriteLine("8. Exit the central bank.");
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
                    new RegisterBank().Handle();
                    break;
                case 2:
                    new ChooseBank().Handle();
                    break;
                case 3:
                    new PutMoneyToTheAccount().Handle();
                    break;
                case 4:
                    new WithdrawMoneyFromTheAccount().Handle();
                    break;
                case 5:
                    new TransferMoney().Handle();
                    break;
                case 6:
                    new CancelTransaction().Handle();
                    break;
                case 7:
                    new ShowBankAccount().Handle();
                    break;
                default:
                    Console.WriteLine("Thank you for your visit to the Central Bank!");
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

            if (number >= 1 && number <= 8)
            {
                return true;
            }

            return false;
        }
    }
}
