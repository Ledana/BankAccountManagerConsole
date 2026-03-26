using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class FakeBankAccountServices : IBankAccountServices
    {
        private decimal _balance;
        public int Id { get; private set; }
        public string UserId { get; private set; }
        private List<string> _movements { get; set; } = [];
        public FakeBankAccountServices(string userId)
        {
            UserId = userId;
        }

        public bool MakeDeposit(decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));
 
            if (amount < 50)
            {
                newBalance = _balance;
                return false;
            }
            _balance += amount;
            newBalance = _balance;
            _movements.Add($"{fakeTime.GetUtcNow().DateTime:o} Deposit {amount:C2}");
            return true;
        }

        public bool MakeWithdraw(decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));

            if (amount < 50 || amount > _balance )
            {
                newBalance = _balance;
                return false;
            }
            _balance -= amount;
            newBalance = _balance;
            _movements.Add($"{fakeTime.GetUtcNow().DateTime:o} Deposit {amount:C2}");
            return true;
        }

        public bool TransferMoney(IBankAccountServices bankAccount, decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));

            if (amount < 50 || amount > _balance)
            {
                newBalance = _balance;
                return false;
            }
            else
            {
                _balance -= amount;
                newBalance = _balance;
                _movements.Add($"{fakeTime.GetUtcNow().DateTime:o} Transfer {amount:C2} to {bankAccount.UserId}");
                bankAccount.creditAmount(amount);
                bankAccount.addMovement(this, amount, fakeTime.GetUtcNow().DateTime);
                return true;
            }
        }

        public void GetMovements()
        {
            if (_movements.Count == 0)
                _movements.Add("You have no movements");
            else
            {
                foreach (var item in _movements)
                {
                    Console.WriteLine(item);
                }
            }
        }

        //changing the balance when another bank account transfers moeny to this
        public void creditAmount(decimal amount)
        {
            _balance += amount;
        }

        //adding to movements when another bank account transfers money to this
        public void addMovement(IBankAccountServices bankAccount, decimal amount, DateTime dateTime)
        {
            this._movements.Add($"{bankAccount.UserId} transfered you {amount} at {dateTime}");
        }

        public decimal GetBalance()
        {
            return _balance;
        }
    }
}
