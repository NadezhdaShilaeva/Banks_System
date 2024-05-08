using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class CreditBankAccount : IBankAccount
    {
        private List<ITransaction> _transactionsHystory = new List<ITransaction>();

        public CreditBankAccount(Guid id, Client client, Clock clock, DateTime endDate, Comission comission, LimitAbove limitAbove, LimitBelow limitBelow)
        {
            Id = id;
            Client = client;
            OpenDate = clock.DateTimeNow;
            EndDate = endDate;
            Comission = comission;
            LimitAbove = limitAbove;
            LimitBelow = limitBelow;
            Percent = new Percent(decimal.Zero);
        }

        public Guid Id { get; }
        public Client Client { get; }
        public DateTime OpenDate { get; }
        public DateTime EndDate { get; }
        public decimal Money { get; private set; }
        public Percent Percent { get; set; }
        public Comission Comission { get; set; }
        public LimitAbove LimitAbove { get; set; }
        public LimitBelow LimitBelow { get; set; }
        public bool IsSuspicious => Client.Address is null || Client.Passport is null;
        public IReadOnlyCollection<ITransaction> TransactionsHystory => _transactionsHystory;

        public void PutMoneyInTheAccount(ITransaction transaction)
        {
            CheckPutMoneyInTheAccount(transaction);

            Money += transaction.Money.Count;
            if (transaction is ReplenishmentTransaction)
            {
                Money -= transaction.Comission.Count;
            }

            _transactionsHystory.Add(transaction);
        }

        public void WithdrawMoneyFromTheAccount(ITransaction transaction)
        {
            CheckWithdrawMoneyFromTheAccount(transaction);

            Money -= transaction.Money.Count + transaction.Comission.Count;
            _transactionsHystory.Add(transaction);
        }

        public void CancelTransactionAndPutMoney(ITransaction transaction)
        {
            CheckCancelTransaction(transaction);

            Money += transaction.Money.Count + transaction.Comission.Count;
            _transactionsHystory.Remove(transaction);
        }

        public void CancelTransactionAndWithdrawMoney(ITransaction transaction)
        {
            CheckCancelTransaction(transaction);

            Money -= transaction.Money.Count;
            if (transaction is ReplenishmentTransaction)
            {
                Money += transaction.Comission.Count;
            }

            _transactionsHystory.Remove(transaction);
        }

        public void CheckPutMoneyInTheAccount(ITransaction transaction)
        {
            if (!IsActive(transaction.DateTime))
            {
                BankAccountException.NotActive(this);
            }

            if (IsGoingBeyondTheLimitAbove(transaction))
            {
                throw BankAccountException.GoingBeyondTheLimit(this);
            }
        }

        public void CheckWithdrawMoneyFromTheAccount(ITransaction transaction)
        {
            if (!IsActive(transaction.DateTime))
            {
                BankAccountException.NotActive(this);
            }

            if (IsGoingBeyondTheLimitBelow(transaction))
            {
                throw BankAccountException.GoingBeyondTheLimit(this);
            }
        }

        public void CheckCancelTransaction(ITransaction transaction)
        {
            if (!_transactionsHystory.Contains(transaction))
            {
                throw TransactionException.TransactionNotFound(transaction, this);
            }
        }

        private bool IsActive(DateTime dateTime)
        {
            return dateTime <= EndDate;
        }

        private bool IsGoingBeyondTheLimitAbove(ITransaction transaction)
        {
            return IsSuspicious && Money + transaction.Money.Count > LimitAbove.Count;
        }

        private bool IsGoingBeyondTheLimitBelow(ITransaction transaction)
        {
            return Money - transaction.Money.Count - transaction.Comission.Count < LimitBelow.Count;
        }
    }
}
