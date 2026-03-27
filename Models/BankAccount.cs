using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Models
{
    public class BankAccount
    {
        public  decimal Balance { get; private set; }
        public int Id { get; init; }
        public string UserId { get; init; } = string.Empty;

        public IReadOnlyList<string> Movements => _movements.AsReadOnly();

        private List<string> _movements = [];

        //ctor for users from database
        public BankAccount(string userId, int id, decimal balance = 0m)
        {
            UserId = userId;
            Id = id;
            Balance = balance;
        }
        //ctor for harcoded users
        public BankAccount(string userId, decimal balance = 0m)
        {
            UserId = userId;
            Balance = balance;
        }
        internal void ApplyCredit(decimal amount)
        {
            Balance += amount;
        }
        internal void ApplyDebit(decimal amount)
        {
            Balance -= amount;
        }
        internal void AddMovement(string movement)
        {
            _movements.Add(movement);
        }
    }
}
