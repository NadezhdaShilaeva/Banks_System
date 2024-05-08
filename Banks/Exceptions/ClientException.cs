namespace Banks.Exceptions
{
    public class ClientException : Exception
    {
        private ClientException(string message)
            : base(message) { }

        public static ClientException NoRequiredData(Guid id)
        {
            return new ClientException($"Error: the name or surname of client (ID:{id}) is not specified.");
        }

        public static ClientException InvalidName(string name)
        {
            return new ClientException($"Error: the name {name} of client is not valid.");
        }

        public static ClientException InvalidSurname(string surname)
        {
            return new ClientException($"Error: the surname {surname} of client is not valid.");
        }

        public static ClientException InvalidAddress(string address)
        {
            return new ClientException($"Error: the address {address} of client is not valid.");
        }

        public static ClientException InvalidPassportData(int number)
        {
            return new ClientException($"Error: passport data {number} is invalid.");
        }
    }
}
