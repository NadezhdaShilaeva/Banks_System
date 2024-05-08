using Banks.Interfaces;
using Banks.Models;

namespace Banks.Exceptions
{
    public class TransactionException : Exception
    {
        private TransactionException(string message)
            : base(message) { }

        public static TransactionException InvalidTransactionMoney(decimal money)
        {
            return new TransactionException($"Error: the sum of transaction money {money} can not be less than 0.");
        }

        public static TransactionException LimitExceeding(TransactionMoney money, LimitAbove limit)
        {
            return new TransactionException($"Error: the sum of transaction money {money.Count} is exceed the limit {limit.Count}.");
        }

        public static TransactionException TransactionNotFound(ITransaction transaction, IBankAccount bankAccount)
        {
            return new TransactionException($"Error: the transaction {transaction.Id} not found in the banck acccount {bankAccount.Id}.");
        }
    }
}
