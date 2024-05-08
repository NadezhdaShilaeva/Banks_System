using Banks.Entities;
using Banks.Interfaces;

namespace Banks.Exceptions
{
    public class BankAccountException : Exception
    {
        private BankAccountException(string message)
            : base(message) { }

        public static BankAccountException GoingBeyondTheLimit(IBankAccount bankAccount)
        {
            return new BankAccountException($"Error: the operation cannot be performed because it is beyond the limit of the account {bankAccount.Id}.");
        }

        public static BankAccountException NotActive(IBankAccount bankAccount)
        {
            return new BankAccountException($"Error: the operation cannot be perfomed because the bank account {bankAccount.Id} is not active." +
                $" The active period ended on {bankAccount.EndDate}.");
        }

        public static BankAccountException PeriodNotEnded(DepositBankAccount bankAccount)
        {
            return new BankAccountException($"Error: the operation cannot be performed because the period of bank account {bankAccount.Id} has not ended yet." +
                $" It will end on {bankAccount.EndDate}.");
        }
    }
}
