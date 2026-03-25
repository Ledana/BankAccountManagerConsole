using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class BankAccount
    {
            private decimal _balance;
            private readonly List<string> _movements = [];

            public int Id { get; }
            public string UserId { get; }
            public decimal Balance => _balance;
            public IReadOnlyList<string> Movements => _movements.AsReadOnly();

            public BankAccount(int id, string userId, decimal initial = 0m)
            {
                Id = id;
                UserId = userId;
                _balance = initial;
            }

            public bool TryDeposit(decimal amount)
            {
                if (amount <= 50) return false;
                _balance += amount;
                _movements.Add($"{DateTime.UtcNow:o} Deposit {amount:C2}");
                return true;
            }

            public bool TryWithdraw(decimal amount)
            {
                if (amount <= 50 || amount > _balance) return false;
                _balance -= amount;
                _movements.Add($"{DateTime.UtcNow:o} Withdraw {amount:C2}");
                return true;
            }

    }
}
