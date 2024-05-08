using Banks.Exceptions;

namespace Banks.Models
{
    public class TransactionMoney
    {
        private const decimal _minCountOfTransactionMoney = decimal.One;

        public TransactionMoney(decimal count)
        {
            if (!IsValidCountOfMoney(count))
            {
                throw TransactionException.InvalidTransactionMoney(count);
            }

            Count = count;
        }

        public decimal Count { get; }

        private bool IsValidCountOfMoney(decimal money)
        {
            return money > _minCountOfTransactionMoney;
        }
    }
}
