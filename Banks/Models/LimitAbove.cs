using Banks.Exceptions;

namespace Banks.Models
{
    public class LimitAbove
    {
        private const decimal _minLimitAbove = decimal.Zero;
        public LimitAbove(decimal count)
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
            return count >= _minLimitAbove;
        }
    }
}
