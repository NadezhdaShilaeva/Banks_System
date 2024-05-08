using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class WithdrawalTransaction : ITransaction
    {
        public WithdrawalTransaction(Guid id, TransactionMoney money, DateTime dateTime, IBankAccount fromAccount)
        {
            Id = id;
            Money = money;
            DateTime = dateTime;
            FromAccount = fromAccount;

            Comission = new Comission(decimal.Zero);
        }

        public Guid Id { get; }
        public TransactionMoney Money { get; }
        public DateTime DateTime { get; }
        public IBankAccount FromAccount { get; }
        public IBankAccount? ToAccount { get; }
        public Comission Comission { get; private set; }

        public void Execute()
        {
            if (FromAccount.Money < decimal.Zero)
            {
                Comission = FromAccount.Comission;
            }

            FromAccount.WithdrawMoneyFromTheAccount(this);
        }

        public void Cancel()
        {
            FromAccount.CancelTransactionAndPutMoney(this);
        }
    }
}
