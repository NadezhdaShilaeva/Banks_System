namespace Banks.Exceptions
{
    public class ClockException : Exception
    {
        private ClockException(string message)
            : base(message) { }

        public static ClockException InvalidDateTime(DateTime newDateTime, DateTime currentDateTime)
        {
            return new ClockException($"Error: the new date time {newDateTime} is not later then current date time {currentDateTime}.");
        }
    }
}
