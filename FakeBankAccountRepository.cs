using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class FakeBankAccountRepository : IBankAccountRepository
    {
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }

            set { _balance = value; }
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        private List<string> _movements { get; set; } = [];

        public FakeBankAccountRepository()
        {
            
        }

        public void MakeDeposit(decimal amount, SqlConnection conn)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));
            if (amount < 0)
                Console.WriteLine("The amount should be positive");

            else if (amount == 0)
                return;
            else
            {
                _balance += amount;
                Console.WriteLine($"You deposidet {amount} and your balance now is {_balance}");
                _movements.Add($"You deposited {amount} in {fakeTime.GetUtcNow().DateTime}");
            }
        }

        public void MakeWithdraw(decimal amount, SqlConnection conn)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));
            if (amount <= 0)
            {
                Console.WriteLine("The amount should be positive");
                return;
            }

            if (amount > _balance)
                Console.WriteLine("The amount to withdraw should be less then the balance");

            else
            {
                _balance -= amount;
                Console.WriteLine($"You withdrawed {amount} and your balance now is {_balance}");
                _movements.Add($"You withdrawed {amount} at {fakeTime.GetUtcNow().DateTime}");
            }
        }

        public void TransferMoney(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));

            if (amount <= 0)
            {
                Console.WriteLine("The amount should be positive");
                return;
            }
            else if (amount >= _balance)
            {
                Console.WriteLine("The amount is bigger than your balance");
                return;
            }

            if (bankAccount == null)
                Console.WriteLine("The userId is not valid");
            else if (bankAccount.UserId == UserId)
            {
                Console.WriteLine("You can not transfer money to yourself");
                return;
            }
            else
            {
                _balance -= amount;
                bankAccount.creditAmount(amount);
                bankAccount.addMovement(this, amount, conn, fakeTime.GetUtcNow().DateTime);

                Console.WriteLine($"You transfered {amount} to {bankAccount.UserId}");
                Console.WriteLine($"Your balance is now {_balance}");
                _movements.Add($"You transfered {amount} to {bankAccount.UserId} at {fakeTime.GetUtcNow().DateTime}");
            }
        }

        public void GetMovements(SqlConnection conn)
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
        public void addMovement(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn, DateTime dateTime)
        {
            this._movements.Add($"{bankAccount.UserId} transfered you {amount} at {dateTime}");
        }
    }
}
