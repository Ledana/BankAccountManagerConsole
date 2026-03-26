using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public interface IBankAccountServices
    {
        public bool MakeDeposit(decimal amount,  out decimal newBalance);
        public bool MakeWithdraw(decimal amount, out decimal newBalance);
        public decimal GetBalance();
        public IReadOnlyList<string> GetMovements();
        public bool TransferMoney(IBankAccountServices bankAccount, decimal amount, out decimal newBalance);
        //a method to change the balance of another bank account
        public void creditAmount(decimal amount);
        public string GetUserId();
        public int GetId();
        //a method to add to movements of another bank account
        public void addMovement(IBankAccountServices bankAccount, decimal amount, DateTime dateTime);
    }
}
