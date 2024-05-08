using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class TransferTransaction : ITransaction
    {
        public TransferTransaction(Guid id, TransactionMoney money, DateTime dateTime, IBankAccount fromAccount, IBankAccount toAccount)
        {
            Id = id;
            Money = money;
            DateTime = dateTime;
            FromAccount = fromAccount;
            ToAccount = toAccount;

            Comission = new Comission(decimal.Zero);
        }

        public Guid Id { get; }
        public TransactionMoney Money { get; }
        public DateTime DateTime { get; }
        public IBankAccount FromAccount { get; }
        public IBankAccount ToAccount { get; }
        public Comission Comission { get; private set; }

        public void Execute()
        {
            if (FromAccount.Money < decimal.Zero)
            {
                Comission = FromAccount.Comission;
            }

            FromAccount.CheckWithdrawMoneyFromTheAccount(this);
            ToAccount.CheckPutMoneyInTheAccount(this);

            FromAccount.WithdrawMoneyFromTheAccount(this);
            ToAccount.PutMoneyInTheAccount(this);
        }

        public void Cancel()
        {
            FromAccount.CheckCancelTransaction(this);
            ToAccount.CheckCancelTransaction(this);

            ToAccount.CancelTransactionAndWithdrawMoney(this);
            FromAccount.CancelTransactionAndPutMoney(this);
        }
    }
}
