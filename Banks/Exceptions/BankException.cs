using Banks.Entities;

namespace Banks.Exceptions
{
    public class BankException : Exception
    {
        private BankException(string message)
            : base(message) { }
        public static BankException ClientIsAlreadyExist(Client client)
        {
            return new BankException($"Error: the client with ID {client.Id} is already registered in the bank.");
        }

        public static BankException InvalidRate(decimal moneyCount)
        {
            return new BankException($"Error: the min money count {moneyCount} of rate cannot be less than previous min money count of rate.");
        }

        public static BankException InvalidPercent(decimal number)
        {
            return new BankException($"Error: the percent {number} is not valid.");
        }

        public static BankException InvalidLimit(decimal limit)
        {
            return new BankException($"Error: the limit {limit} is not valid.");
        }

        public static BankException InvalidComission(decimal comission)
        {
            return new BankException($"Error: the comission {comission} is not valid.");
        }

        public static BankException InvalidMoney(decimal money)
        {
            return new BankException($"Error: the money {money} cannot be less than 0.");
        }

        public static BankException InvalidDaysToOpen(int daysNumber)
        {
            return new BankException($"Error: unable to open a bank account due to lack of days {daysNumber}.");
        }

        public static BankException NoRequiredData(Guid id)
        {
            return new BankException($"Error: the required data of bank (ID:{id}) is not specified.");
        }

        public static BankException ClientNotFound(Guid id)
        {
            return new BankException($"Error: the client with ID: {id} is not found.");
        }

        public static BankException AccountNotFound(Guid id)
        {
            return new BankException($"Error: the bank account with ID: {id} is not found.");
        }

        public static BankException TransactionNotFound(Guid id)
        {
            return new BankException($"Error: the transaction with ID: {id} is not found.");
        }

        public static BankException RateNotFound(decimal money)
        {
            return new BankException($"Error: no deposit rate was found for this amount of money {money}.");
        }
    }
}
