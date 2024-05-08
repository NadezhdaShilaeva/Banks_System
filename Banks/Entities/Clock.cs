using Banks.Exceptions;

namespace Banks.Entities
{
    public class Clock
    {
        private static Clock? _instance;
        private DateTime _dateTimeNow;

        private Clock(DateTime dateTimeNow)
        {
            DateTimeNow = dateTimeNow;
        }

        public delegate void ClockChangedHandler(DateTime newDateTime);
        public event ClockChangedHandler? ClockChanged;

        public DateTime DateTimeNow
        {
            get => _dateTimeNow;
            private set
            {
                if (value <= DateTimeNow)
                {
                    throw ClockException.InvalidDateTime(value, DateTimeNow);
                }

                _dateTimeNow = value;
                ClockChanged?.Invoke(value);
            }
        }

        public static Clock GetInstance()
        {
            return _instance ?? (_instance = new Clock(DateTime.Now));
        }

        public void AddDay()
        {
            DateTimeNow = DateTimeNow.AddDays(1);
        }

        public void AddMonth()
        {
            DateTime newDateTime = DateTimeNow.AddMonths(1);

            while (!DateTimeNow.Equals(newDateTime))
            {
                AddDay();
            }
        }

        public void AddYear()
        {
            DateTime newDateTime = DateTimeNow.AddYears(1);

            while (!DateTimeNow.Equals(newDateTime))
            {
                AddDay();
            }
        }
    }
}
