using Banks.Models;

namespace Banks.Interfaces
{
    public interface ITransaction
    {
        public Guid Id { get; }
        public TransactionMoney Money { get; }
        public DateTime DateTime { get; }
        public IBankAccount? FromAccount { get; }
        public IBankAccount? ToAccount { get; }
        public Comission Comission { get; }

        public void Execute();
        public void Cancel();
    }
}
