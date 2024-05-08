using Banks.Exceptions;

namespace Banks.Models
{
    public class PassportData
    {
        private const int _minPassportNumber = 1;

        public PassportData(int number)
        {
            if (!IsValidNumber(number))
            {
                throw ClientException.InvalidPassportData(number);
            }

            Number = number;
        }

        public int Number { get; }

        private bool IsValidNumber(int number)
        {
            return number >= _minPassportNumber;
        }
    }
}
