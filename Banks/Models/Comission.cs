using Banks.Exceptions;

namespace Banks.Models
{
    public class Comission
    {
        private const decimal _minCountOfComission = decimal.Zero;
        public Comission(decimal count)
        {
            if (!IsValidCount(count))
            {
                throw BankException.InvalidComission(count);
            }

            Count = count;
        }

        public decimal Count { get; }

        private bool IsValidCount(decimal count)
        {
            return count >= _minCountOfComission;
        }
    }
}
