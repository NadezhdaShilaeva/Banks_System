using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class ReplenishmentTransaction : ITransaction
    {
        public ReplenishmentTransaction(Guid id, TransactionMoney money, DateTime dateTime, IBankAccount toAccount)
        {
            Id = id;
            Money = money;
            DateTime = dateTime;
            ToAccount = toAccount;

            Comission = new Comission(decimal.Zero);
        }

        public Guid Id { get; }
        public TransactionMoney Money { get; }
        public DateTime DateTime { get; }
        public IBankAccount? FromAccount { get; }
        public IBankAccount ToAccount { get; }
        public Comission Comission { get; private set; }

        public void Execute()
        {
            if (ToAccount.Money < decimal.Zero)
            {
                Comission = ToAccount.Comission;
            }

            ToAccount.PutMoneyInTheAccount(this);
        }

        public void Cancel()
        {
            ToAccount.CancelTransactionAndWithdrawMoney(this);
        }
    }
}
