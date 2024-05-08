using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class Bank
    {
        private List<Client> _clients;
        private List<IBankAccount> _accounts;
        private Percent _debitPercent;
        private Comission _comission;
        private LimitBelow _creditAccountLimit;
        private LimitAbove _accountLimitAbove;
        private LimitAbove _transactionLimit;

        public Bank(
            Guid id,
            string name,
            Percent debitPercent,
            Comission comission,
            LimitBelow creditAccountLimit,
            LimitAbove accountLimitAbove,
            LimitAbove transactionLimit,
            TimeSpan accounActivePeriod,
            DepositAccountRates depositAccountRates)
        {
            Id = id;
            Name = name;
            _debitPercent = debitPercent;
            _comission = comission;
            _creditAccountLimit = creditAccountLimit;
            _accountLimitAbove = accountLimitAbove;
            _transactionLimit = transactionLimit;
            AccountActivePeriod = accounActivePeriod;
            DepositAccountRates = depositAccountRates;

            _clients = new List<Client>();
            _accounts = new List<IBankAccount>();
        }

        public static BankBuilder Builder => new BankBuilder();

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<Client> Clients => _clients;
        public IReadOnlyCollection<IBankAccount> Accounts => _accounts;
        public DepositAccountRates DepositAccountRates { get; }
        public TimeSpan AccountActivePeriod { get; }
        public LimitAbove TransactionLimit
        {
            get => _transactionLimit;
            set
            {
                _transactionLimit = value;

                _accounts.ForEach(account =>
                {
                    account.Client.GetNotification($"The limit above of transactions changed to {value.Count}.");
                });
            }
        }

        public Percent DebitPercent
        {
            get => _debitPercent;
            set
            {
                _debitPercent = value;

                _accounts.ForEach(account =>
                {
                    if (account is DebitBankAccount)
                    {
                        account.Percent = value;
                        account.Client.GetNotification($"The debit bank account percent changed to {value.Number}.");
                    }
                });
            }
        }

        public Comission Comission
        {
            get => _comission;
            set
            {
                _comission = value;

                _accounts.ForEach(account =>
                {
                    if (account is DebitBankAccount)
                    {
                        account.Comission = value;
                        account.Client.GetNotification($"The comission changed to {value.Count}.");
                    }
                });
            }
        }

        public LimitBelow CreditAccountLimit
        {
            get => _creditAccountLimit;
            set
            {
                _creditAccountLimit = value;

                _accounts.ForEach(account =>
                {
                    if (account is CreditBankAccount)
                    {
                        account.LimitBelow = value;
                        account.Client.GetNotification($"The limit below for the credit bank accounts changed to {value.Count}.");
                    }
                });
            }
        }

        public LimitAbove AccountLimitAbove
        {
            get => _accountLimitAbove;
            set
            {
                _accountLimitAbove = value;

                _accounts.ForEach(account =>
                {
                    account.LimitAbove = value;
                    account.Client.GetNotification($"The limit above for the bank account money for suspecious clients changed to {value.Count}.");
                });
            }
        }

        public Client RegisterClient(Client client)
        {
            if (_clients.Contains(client))
            {
                throw BankException.ClientIsAlreadyExist(client);
            }

            _clients.Add(client);

            return client;
        }

        public void SetClientAddress(Guid clientId, string address)
        {
            Client client = GetClient(clientId);
            client.Address = address;
        }

        public void SetClientPassport(Guid clientId, PassportData passport)
        {
            Client client = GetClient(clientId);
            client.Passport = passport;
        }

        public DebitBankAccount CreateDebitBankAccount(Guid clientId)
        {
            Client client = GetClient(clientId);
            var id = Guid.NewGuid();

            DateTime endDate = Clock.GetInstance().DateTimeNow + AccountActivePeriod;
            var newDebitAccount = new DebitBankAccount(id, client, Clock.GetInstance(), endDate, DebitPercent, AccountLimitAbove);
            _accounts.Add(newDebitAccount);

            return newDebitAccount;
        }

        public DepositBankAccount CreateDepositBankAccount(Guid clientId, decimal money, int daysNumber)
        {
            Client client = GetClient(clientId);

            decimal minMoney = decimal.Zero;
            int minDaysNumber = 1;

            if (money < minMoney)
            {
                throw BankException.InvalidMoney(money);
            }

            if (daysNumber < minDaysNumber)
            {
                throw BankException.InvalidDaysToOpen(daysNumber);
            }

            var id = Guid.NewGuid();

            DepositAccountRate? relevantRate = DepositAccountRates.Rates.LastOrDefault(rate => rate.MinMoneyCount <= money);
            if (relevantRate is null)
            {
                throw BankException.RateNotFound(money);
            }

            Percent relevantPercent = relevantRate.Percent;
            DateTime endDate = Clock.GetInstance().DateTimeNow + new TimeSpan(daysNumber, 0, 0, 0);
            var newDepositAccount = new DepositBankAccount(id, client, Clock.GetInstance(), endDate, relevantPercent, money, AccountLimitAbove);
            _accounts.Add(newDepositAccount);

            return newDepositAccount;
        }

        public CreditBankAccount CreateCreditBankAccount(Guid clientId)
        {
            Client client = GetClient(clientId);
            var id = Guid.NewGuid();

            DateTime endDate = Clock.GetInstance().DateTimeNow + AccountActivePeriod;
            var newCreditAccount = new CreditBankAccount(id, client, Clock.GetInstance(), endDate, Comission, AccountLimitAbove, CreditAccountLimit);
            _accounts.Add(newCreditAccount);

            return newCreditAccount;
        }

        public Client SubscribeToBankUpdates(Guid clientId)
        {
            Client client = GetClient(clientId);
            client.GetNotifications = true;

            return client;
        }

        private Client GetClient(Guid id)
        {
            return _clients.FirstOrDefault(client => client.Id.Equals(id)) ?? throw BankException.ClientNotFound(id);
        }

        public class BankBuilder
        {
            private Guid _id;
            private string? _name;
            private Percent? _debitPercent;
            private Comission? _comission;
            private LimitBelow? _creditAccountLimit;
            private LimitAbove? _accountLimitAbove;
            private LimitAbove? _transactionLimit;
            private TimeSpan? _accounActivePeriod;
            private DepositAccountRates? _depositAccountRates;

            public BankBuilder()
            {
                _id = Guid.NewGuid();
                _name = null;
                _debitPercent = null;
                _comission = null;
                _creditAccountLimit = null;
                _accountLimitAbove = null;
                _transactionLimit = null;
                _accounActivePeriod = null;
                _depositAccountRates = null;
            }

            public BankBuilder SetName(string name)
            {
                _name = name;

                return this;
            }

            public BankBuilder SetDebitPercent(decimal debitPercent)
            {
                _debitPercent = new Percent(debitPercent);

                return this;
            }

            public BankBuilder SetComission(decimal comission)
            {
                _comission = new Comission(comission);

                return this;
            }

            public BankBuilder SetCreditAccountLimit(decimal creditAccountLimit)
            {
                _creditAccountLimit = new LimitBelow(creditAccountLimit);

                return this;
            }

            public BankBuilder SetAccountLimitAbove(decimal accountLimitAbove)
            {
                _accountLimitAbove = new LimitAbove(accountLimitAbove);

                return this;
            }

            public BankBuilder SetTransactionLimit(decimal transactionLimit)
            {
                _transactionLimit = new LimitAbove(transactionLimit);

                return this;
            }

            public BankBuilder SetAccounActivePeriod(int accounActiveDaysPeriod)
            {
                _accounActivePeriod = new TimeSpan(accounActiveDaysPeriod, 0, 0, 0);

                return this;
            }

            public BankBuilder SetDepositAccountRates(DepositAccountRates depositAccountRates)
            {
                _depositAccountRates = depositAccountRates;

                return this;
            }

            public Bank Build()
            {
                return new Bank(
                    _id,
                    _name ?? throw BankException.NoRequiredData(_id),
                    _debitPercent ?? throw BankException.NoRequiredData(_id),
                    _comission ?? throw BankException.NoRequiredData(_id),
                    _creditAccountLimit ?? throw BankException.NoRequiredData(_id),
                    _accountLimitAbove ?? throw BankException.NoRequiredData(_id),
                    _transactionLimit ?? throw BankException.NoRequiredData(_id),
                    _accounActivePeriod ?? throw BankException.NoRequiredData(_id),
                    _depositAccountRates ?? throw BankException.NoRequiredData(_id));
            }
        }
    }
}
