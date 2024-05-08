using Banks.Exceptions;

namespace Banks.Models
{
    public class DepositAccountRate
    {
        private const decimal _minCountOfMoney = decimal.Zero;

        public DepositAccountRate(decimal minMoneyCount, decimal percent)
        {
            if (!IsValidMoney(minMoneyCount))
            {
                BankException.InvalidMoney(minMoneyCount);
            }

            MinMoneyCount = minMoneyCount;
            Percent = new Percent(percent);
        }

        public decimal MinMoneyCount { get; set; }
        public Percent Percent { get; set; }

        private bool IsValidMoney(decimal money)
        {
            return money >= _minCountOfMoney;
        }
    }
}
