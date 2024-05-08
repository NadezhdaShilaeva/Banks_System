using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities
{
    public class Client
    {
        private Client(Guid id, string name, string surname, string? address, PassportData? passport)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Address = address;
            Passport = passport;
        }

        public static ClientBuilder Builder => new ClientBuilder();

        public Guid Id { get; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Address { get; set; }
        public PassportData? Passport { get; set; }
        public bool GetNotifications { get; set; } = false;
        public INotifier? Notifier { get; set; }

        public void GetNotification(string message)
        {
            if (GetNotifications && Notifier is not null)
            {
                Notifier.SendNotification(message);
            }
        }

        public class ClientBuilder
        {
            private Guid _id;
            private string? _name;
            private string? _surname;
            private string? _address;
            private PassportData? _passport;

            public ClientBuilder()
            {
                _id = Guid.NewGuid();
                _name = null;
                _surname = null;
                _address = null;
                _passport = null;
            }

            public ClientBuilder SetName(string name)
            {
                if (name.Equals(string.Empty))
                {
                    throw ClientException.InvalidName(name);
                }

                _name = name;

                return this;
            }

            public ClientBuilder SetSurname(string surname)
            {
                if (surname.Equals(string.Empty))
                {
                    throw ClientException.InvalidSurname(surname);
                }

                _surname = surname;

                return this;
            }

            public ClientBuilder SetAddress(string address)
            {
                if (address.Equals(string.Empty))
                {
                    throw ClientException.InvalidAddress(address);
                }

                _address = address;

                return this;
            }

            public ClientBuilder SetPassport(int number)
            {
                _passport = new PassportData(number);

                return this;
            }

            public Client Build()
            {
                return new Client(
                    _id,
                    _name ?? throw ClientException.NoRequiredData(_id),
                    _surname ?? throw ClientException.NoRequiredData(_id),
                    _address,
                    _passport);
            }
        }
    }
}
