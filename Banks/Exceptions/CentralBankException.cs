using Banks.Entities;

namespace Banks.Exceptions
{
    public class CentralBankException : Exception
    {
        private CentralBankException(string message)
            : base(message) { }

        public static CentralBankException BankIsAlreadyExist(Bank bank)
        {
            return new CentralBankException($"Error: the bank with ID {bank.Id} is already registered in the central bank.");
        }
    }
}
