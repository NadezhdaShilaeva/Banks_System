using Banks.Entities;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface ICentralBank
    {
        void RegisterBank(Bank bank);
        ITransaction PutMoneyToTheAccount(Guid toAccountId, TransactionMoney money);
        ITransaction WithdrawMoneyFromTheAccount(Guid fromAccountId, TransactionMoney money);
        ITransaction TransferMoney(Guid fromAccountId, Guid toAccountId, TransactionMoney money);
        void CancelTransaction(Guid transactionId);
    }
}
