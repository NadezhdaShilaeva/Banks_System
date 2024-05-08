﻿using Banks.Entities;
using Banks.Interfaces;

namespace Banks.ConsoleInterface
{
    public class CreateCreditBankAccount
    {
        private Bank _bank;

        public CreateCreditBankAccount(Bank bank)
        {
            _bank = bank;
        }

        public void Handle()
        {
            while (true)
            {
                Console.WriteLine("Enter the ID of the client:");
                string? clientID = Console.ReadLine();

                try
                {
                    IBankAccount bankAccount = _bank.CreateCreditBankAccount(Guid.Parse(clientID ?? string.Empty));
                    Console.WriteLine($"The credit bank account with ID: {bankAccount.Id} was created for the client with ID {clientID}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Do you want to try again?");
                    Console.WriteLine("Enter 'yes' if you want.");

                    string? answer = Console.ReadLine();
                    if (answer is not null && answer.ToLower().Equals("yes"))
                        continue;
                }

                break;
            }

            Console.WriteLine("Press any key to go back.");
            Console.ReadLine();
            new BankHandler(_bank).Handle();
        }
    }
}
