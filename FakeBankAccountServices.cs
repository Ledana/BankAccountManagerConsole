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
        //private decimal _balance;
        //public int Id { get; private set; }
        //public string UserId { get; private set; }
        //private List<string> _movements { get; set; } = [];
        //public IReadOnlyList<string> Movements => _movements.AsReadOnly();
        private BankAccount _account;
        public FakeBankAccountServices(BankAccount account)
        {
            //UserId = userId;
            _account = account;
        }

        public bool MakeDeposit(decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));
 
            if (amount < 50)
            {
                newBalance = _account.Balance;
                return false;
            }
            _account.ApplyCredit(amount);
            newBalance = _account.Balance;
            _account.AddMovement($"{fakeTime.GetUtcNow().DateTime:o} Deposit {amount:C2}");
            return true;
        }

        public bool MakeWithdraw(decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));

            if (amount < 50 || amount > _account.Balance)
            {
                newBalance = _account.Balance;
                return false;
            }
            _account.ApplyDebit(amount);
            newBalance = _account.Balance;
            _account.AddMovement($"{fakeTime.GetUtcNow().DateTime:o} Withdraw {amount:C2}");
            return true;
        }

        public bool TransferMoney(IBankAccountServices bankAccountServices, decimal amount, out decimal newBalance)
        {
            //adding a fake time for testing
            var fakeTime = new FakeTimeProvider(DateTimeOffset.Parse("2024-01-01T00:00:00Z"));

            if (amount < 50 || amount > _account.Balance)
            {
                newBalance = _account.Balance;
                return false;
            }
            else
            {
                _account.ApplyDebit(amount);
                newBalance = _account.Balance;
                _account.AddMovement($"{fakeTime.GetUtcNow().DateTime:o} Transfer {amount:C2} to {_account.UserId}");

                bankAccountServices.creditAmount(amount);
                bankAccountServices.addMovement(this, amount, fakeTime.GetUtcNow().DateTime);
                return true;
            }
        }

        //changing the balance when another bank account transfers moeny to this
        public void creditAmount(decimal amount)
        {
            _account.ApplyCredit(amount);
        }
        public string GetUserId()
        {
            return _account.UserId;
        }
        public int GetId()
        {
            return _account.Id;
        }
        //adding to movements when another bank account transfers money to this
        public void addMovement(IBankAccountServices bankAccountServices, decimal amount, DateTime dateTime)
        {
            _account.AddMovement($"{bankAccountServices.GetUserId()} transfered you {amount} at {dateTime}");
        }

        public decimal GetBalance()
        {
            return _account.Balance;
        }

        public IReadOnlyList<string> GetMovements()
        {
            return _account.Movements;
        }

        

        
    }
}
