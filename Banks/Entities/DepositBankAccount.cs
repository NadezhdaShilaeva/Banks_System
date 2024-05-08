using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class DepositBankAccount : IBankAccount
    {
        private decimal _currentPercentIncome = decimal.Zero;
        private DateTime _lastDateUpdate;
        private List<ITransaction> _transactionsHystory = new List<ITransaction>();

        public DepositBankAccount(Guid id, Client client, Clock clock,  DateTime endDate, Percent percent, decimal money, LimitAbove limitAbove)
        {
            Id = id;
            Client = client;
            OpenDate = clock.DateTimeNow;
            EndDate = endDate;
            Percent = percent;
            Money = money;
            LimitAbove = limitAbove;
            Comission = new Comission(decimal.Zero);
            LimitBelow = new LimitBelow(decimal.Zero);
            _lastDateUpdate = OpenDate;

            clock.ClockChanged += AddPercentIncome;
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
            _transactionsHystory.Add(transaction);
        }

        public void WithdrawMoneyFromTheAccount(ITransaction transaction)
        {
            CheckWithdrawMoneyFromTheAccount(transaction);

            Money -= transaction.Money.Count;
            _transactionsHystory.Add(transaction);
        }

        public void CancelTransactionAndPutMoney(ITransaction transaction)
        {
            CheckCancelTransaction(transaction);

            Money += transaction.Money.Count;
            _transactionsHystory.Remove(transaction);
        }

        public void CancelTransactionAndWithdrawMoney(ITransaction transaction)
        {
            CheckCancelTransaction(transaction);

            Money -= transaction.Money.Count;
            _transactionsHystory.Remove(transaction);
        }

        public void CheckPutMoneyInTheAccount(ITransaction transaction)
        {
            if (IsPeriodEnded(transaction.DateTime))
            {
                throw BankAccountException.NotActive(this);
            }

            if (IsGoingBeyondTheLimitAbove(transaction))
            {
                throw BankAccountException.GoingBeyondTheLimit(this);
            }
        }

        public void CheckWithdrawMoneyFromTheAccount(ITransaction transaction)
        {
            if (!IsPeriodEnded(transaction.DateTime))
            {
                throw BankAccountException.PeriodNotEnded(this);
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

        private void AddPercentIncome(DateTime newDateTime)
        {
            var timeToAddPercent = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, 0, 0, 0);

            if (!IsPeriodEnded(timeToAddPercent))
            {
                _currentPercentIncome += Money * (Percent.Number / 365);

                if (timeToAddPercent.Day == 1)
                {
                    AddPercentToAccount(timeToAddPercent);
                }

                _lastDateUpdate = newDateTime;
            }
            else
            {
                if (_lastDateUpdate < EndDate)
                {
                    AddPercentToAccount(timeToAddPercent);
                    _lastDateUpdate = newDateTime;
                }
            }
        }

        private bool IsPeriodEnded(DateTime dateTime)
        {
            return dateTime > EndDate;
        }

        private bool IsGoingBeyondTheLimitAbove(ITransaction transaction)
        {
            return IsSuspicious && Money + transaction.Money.Count > LimitAbove.Count;
        }

        private bool IsGoingBeyondTheLimitBelow(ITransaction transaction)
        {
            return Money < transaction.Money.Count;
        }

        private void AddPercentToAccount(DateTime dateTime)
        {
            var transaction = new ReplenishmentTransaction(Guid.NewGuid(), new TransactionMoney(_currentPercentIncome), dateTime, this);
            transaction.Execute();

            _currentPercentIncome = 0;
        }
    }
}
