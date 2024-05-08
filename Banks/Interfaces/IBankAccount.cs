using Banks.Entities;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankAccount
    {
        Guid Id { get; }
        DateTime OpenDate { get; }
        DateTime EndDate { get; }
        Client Client { get; }
        decimal Money { get; }
        Percent Percent { get; set; }
        Comission Comission { get; set; }
        LimitAbove LimitAbove { get; set; }
        LimitBelow LimitBelow { get; set; }
        bool IsSuspicious { get; }
        IReadOnlyCollection<ITransaction> TransactionsHystory { get; }

        void PutMoneyInTheAccount(ITransaction transaction);
        void WithdrawMoneyFromTheAccount(ITransaction transaction);
        void CancelTransactionAndPutMoney(ITransaction transaction);
        void CancelTransactionAndWithdrawMoney(ITransaction transaction);
        void CheckPutMoneyInTheAccount(ITransaction transaction);
        void CheckWithdrawMoneyFromTheAccount(ITransaction transaction);
        void CheckCancelTransaction(ITransaction transaction);
    }
}
