using Banks.Services;

namespace Banks.ConsoleInterface
{
    public class CancelTransaction
    {
        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the transaction to cancel:");
                string? transactionId = Console.ReadLine();

                try
                {
                    CentralBank.GetInstance().CancelTransaction(Guid.Parse(transactionId ?? string.Empty));
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
