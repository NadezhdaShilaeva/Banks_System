using Banks.Entities;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;
using Banks.Services;
using Xunit;

namespace Banks.Test
{
    public class BanksTest
    {
        private static DepositAccountRates rates = DepositAccountRates.Builder
            .AddRate(new DepositAccountRate(0m, 3m))
            .AddRate(new DepositAccountRate(50000m, 3.5m))
            .AddRate(new DepositAccountRate(100000m, 4m))
            .Build();

        private ICentralBank centralBank = CentralBank.GetInstance();

        private Bank bank = Bank.Builder
            .SetName("Tinkoff")
            .SetDebitPercent(5m)
            .SetComission(100m)
            .SetCreditAccountLimit(-50000m)
            .SetAccountLimitAbove(100000m)
            .SetTransactionLimit(100000m)
            .SetAccounActivePeriod(365)
            .SetDepositAccountRates(rates)
            .Build();

        private Client client = Client.Builder
            .SetName("Nadezhda")
            .SetSurname("Shiliaeva")
            .Build();

        [Fact]
        public void OperationsWithDebitAccount_GetRelevantMoneyCount()
        {
            centralBank.RegisterBank(bank);
            bank.RegisterClient(client);

            IBankAccount debitAccount = bank.CreateDebitBankAccount(client.Id);
            centralBank.PutMoneyToTheAccount(debitAccount.Id, new TransactionMoney(5000m));
            ITransaction transaction = centralBank.WithdrawMoneyFromTheAccount(debitAccount.Id, new TransactionMoney(2000m));
            centralBank.CancelTransaction(transaction.Id);

            Assert.True(debitAccount.Money.Equals(5000m));
        }

        [Fact]
        public void OperationsWithDepositAccount_GetRelevantMoneyCount()
        {
            centralBank.RegisterBank(bank);
            bank.RegisterClient(client);

            IBankAccount depositAccount = bank.CreateDepositBankAccount(client.Id, 20000m, 30);
            centralBank.PutMoneyToTheAccount(depositAccount.Id, new TransactionMoney(5000m));
            centralBank.PutMoneyToTheAccount(depositAccount.Id, new TransactionMoney(1000m));
            centralBank.PutMoneyToTheAccount(depositAccount.Id, new TransactionMoney(500m));

            Assert.True(depositAccount.Money.Equals(26500m));
            Assert.Throws<BankAccountException>(() => centralBank.WithdrawMoneyFromTheAccount(depositAccount.Id, new TransactionMoney(5000m)));
        }

        [Fact]
        public void OperationsWithCreditAccount_GetRelevantMoneyCount()
        {
            centralBank.RegisterBank(bank);
            bank.RegisterClient(client);

            IBankAccount depositAccount = bank.CreateCreditBankAccount(client.Id);
            centralBank.WithdrawMoneyFromTheAccount(depositAccount.Id, new TransactionMoney(5000m));
            centralBank.WithdrawMoneyFromTheAccount(depositAccount.Id, new TransactionMoney(1000m));
            centralBank.PutMoneyToTheAccount(depositAccount.Id, new TransactionMoney(500m));

            Assert.True(depositAccount.Money.Equals(-5700m));
            Assert.Throws<BankAccountException>(() => centralBank.WithdrawMoneyFromTheAccount(depositAccount.Id, new TransactionMoney(50000m)));
        }

        [Fact]
        public void OperationsOfTransferTransaction_GetRelevantMoneyCount()
        {
            centralBank.RegisterBank(bank);
            bank.RegisterClient(client);

            Client client2 = Client.Builder
            .SetName("Nadezhda")
            .SetSurname("Shiliaeva")
            .Build();

            bank.RegisterClient(client2);

            IBankAccount depositAccount = bank.CreateDebitBankAccount(client.Id);
            IBankAccount depositAccount2 = bank.CreateDebitBankAccount(client2.Id);
            centralBank.PutMoneyToTheAccount(depositAccount.Id, new TransactionMoney(5000m));
            centralBank.PutMoneyToTheAccount(depositAccount2.Id, new TransactionMoney(1000m));
            centralBank.TransferMoney(depositAccount.Id, depositAccount2.Id, new TransactionMoney(2000m));

            Assert.True(depositAccount.Money.Equals(3000m));
            Assert.True(depositAccount2.Money.Equals(3000m));
        }
    }
}
