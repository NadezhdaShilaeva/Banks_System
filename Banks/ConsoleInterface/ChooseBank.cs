using Banks.Entities;
using Banks.Services;

namespace Banks.ConsoleInterface
{
    public class ChooseBank
    {
        public void Handle()
        {
            if (CentralBank.GetInstance().Banks.Count == 0)
            {
                Console.WriteLine("There are no registered banks.\n");
                new CentralBankHandler().Handle();

                return;
            }

            Console.WriteLine("List of banks registered in the Central Bank:");
            int bankIndex = 1;
            foreach (Bank bank in CentralBank.GetInstance().Banks)
            {
                Console.WriteLine($"{bankIndex}. {bank.Name}.");
                bankIndex++;
            }

            Console.WriteLine("Choose one number of the banks.");
            string? answer = Console.ReadLine();

            while (answer is null || !IsValidAnswer(answer))
            {
                Console.WriteLine("The number is incorrect.");
                Console.WriteLine("Choose one number of the banks.");
                answer = Console.ReadLine();
            }

            int number = int.Parse(answer);

            new BankHandler(CentralBank.GetInstance().Banks.ElementAt(number - 1)).Handle();
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

            if (number >= 1 && number <= CentralBank.GetInstance().Banks.Count())
            {
                return true;
            }

            return false;
        }
    }
}
