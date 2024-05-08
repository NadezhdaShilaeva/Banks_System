using Banks.Exceptions;

namespace Banks.Models
{
    public class Percent
    {
        private const decimal _minPrecentNumber = 0m;
        private const decimal _maxPrecentNumber = 100m;

        public Percent(decimal number)
        {
            if (!IsValidPrecent(number))
            {
                throw BankException.InvalidPercent(number);
            }

            Number = number;
        }

        public decimal Number { get; }

        private bool IsValidPrecent(decimal number)
        {
            return number >= _minPrecentNumber && number <= _maxPrecentNumber;
        }
    }
}
