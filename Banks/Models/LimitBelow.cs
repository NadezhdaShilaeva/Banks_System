using Banks.Exceptions;

namespace Banks.Models
{
    public class LimitBelow
    {
        private const decimal _maxLimitBelow = decimal.Zero;
        public LimitBelow(decimal count)
        {
            if (!IsValidLimit(count))
            {
                throw BankException.InvalidLimit(count);
            }

            Count = count;
        }

        public decimal Count { get; }

        private bool IsValidLimit(decimal count)
        {
            return count <= _maxLimitBelow;
        }
    }
}
