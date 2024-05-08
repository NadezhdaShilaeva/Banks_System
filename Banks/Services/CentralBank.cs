using Banks.Entities;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Services
{
    public class CentralBank : ICentralBank
    {
        private static CentralBank? _instance;
        private List<Bank> _banks = new List<Bank>();

        private CentralBank() { }

        public IReadOnlyCollection<Bank> Banks => _banks;

        public static CentralBank GetInstance()
        {
            return _instance ?? (_instance = new CentralBank());
        }

        public void RegisterBank(Bank bank)
        {
            if (_banks.Contains(bank))
            {
                throw CentralBankException.BankIsAlreadyExist(bank);
            }

            _banks.Add(bank);
        }

        public ITransaction PutMoneyToTheAccount(Guid toAccountId, TransactionMoney money)
        {
            IBankAccount toAccount = GetBankAccount(toAccountId);
            CheckTransactionMoney(money, toAccount);

            var id = Guid.NewGuid();
            var transaction = new ReplenishmentTransaction(id, money, Clock.GetInstance().DateTimeNow, toAccount);
            transaction.Execute();

            return transaction;
        }

        public ITransaction WithdrawMoneyFromTheAccount(Guid fromAccountId, TransactionMoney money)
        {
            IBankAccount fromAccount = GetBankAccount(fromAccountId);
            CheckTransactionMoney(money, fromAccount);

            var id = Guid.NewGuid();
            var transaction = new WithdrawalTransaction(id, money, Clock.GetInstance().DateTimeNow, fromAccount);
            transaction.Execute();

            return transaction;
        }

        public ITransaction TransferMoney(Guid fromAccountId, Guid toAccountId, TransactionMoney money)
        {
            IBankAccount fromAccount = GetBankAccount(fromAccountId);
            IBankAccount toAccount = GetBankAccount(toAccountId);
            CheckTransactionMoney(money, fromAccount);
            CheckTransactionMoney(money, toAccount);

            var id = Guid.NewGuid();
            var transaction = new TransferTransaction(id, money, Clock.GetInstance().DateTimeNow, fromAccount, toAccount);
            transaction.Execute();

            return transaction;
        }

        public void CancelTransaction(Guid transactionId)
        {
            ITransaction transaction = GetTransaction(transactionId);

            transaction.Cancel();
        }

        public IBankAccount GetBankAccount(Guid bankAccountId)
        {
            return Banks.SelectMany(bank => bank.Accounts).FirstOrDefault(account => account.Id.Equals(bankAccountId))
                ?? throw BankException.AccountNotFound(bankAccountId);
        }

        private Bank GetBankOfAccount(IBankAccount bankAccount)
        {
            return Banks.FirstOrDefault(bank => bank.Accounts.Contains(bankAccount))
                ?? throw BankException.AccountNotFound(bankAccount.Id);
        }

        private ITransaction GetTransaction(Guid transactionId)
        {
            return Banks.SelectMany(bank => bank.Accounts).SelectMany(account => account.TransactionsHystory)
                .FirstOrDefault(transact => transact.Id.Equals(transactionId))
                ?? throw BankException.TransactionNotFound(transactionId);
        }

        private void CheckTransactionMoney(TransactionMoney money, IBankAccount account)
        {
            Bank bank = GetBankOfAccount(account);

            if (money.Count > bank.TransactionLimit.Count)
            {
                throw TransactionException.LimitExceeding(money, bank.TransactionLimit);
            }
        }
    }
}
