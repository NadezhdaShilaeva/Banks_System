using Banks.Exceptions;

namespace Banks.Models
{
    public class DepositAccountRates
    {
        private DepositAccountRates(List<DepositAccountRate> rates)
        {
            Rates = rates;
        }

        public static DepositAccountRatesBuilder Builder => new DepositAccountRatesBuilder();

        public IReadOnlyCollection<DepositAccountRate> Rates { get; }

        private static bool IsRateRelevant(IReadOnlyList<DepositAccountRate> rates, DepositAccountRate otherRate)
        {
            if (rates.LastOrDefault() is null)
                return true;

            return rates.Last().MinMoneyCount < otherRate.MinMoneyCount;
        }

        public class DepositAccountRatesBuilder
        {
            private readonly List<DepositAccountRate> _rates;

            public DepositAccountRatesBuilder()
            {
                _rates = new List<DepositAccountRate>();
            }

            public DepositAccountRatesBuilder AddRate(DepositAccountRate rate)
            {
                if (rate is null)
                {
                    throw new ArgumentNullException(nameof(rate));
                }

                if (!IsRateRelevant(_rates, rate))
                {
                    throw BankException.InvalidRate(rate.MinMoneyCount);
                }

                _rates.Add(rate);

                return this;
            }

            public DepositAccountRates Build()
            {
                return new DepositAccountRates(_rates);
            }
        }
    }
}
