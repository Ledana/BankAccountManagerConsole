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
        public int Id { get; }
        public string UserId { get; }

        public bool MakeDeposit(decimal amount,  out decimal newBalance);
        public void MakeWithdraw(decimal amount);
        public decimal GetBalance();

        public void TransferMoney(IBankAccountServices bankAccount, decimal amount);
        public void GetMovements();
        //a method to change the balance of another bank account
        public void creditAmount(decimal amount);
        //a method to add to movements of another bank account
        public void addMovement(IBankAccountServices bankAccount, decimal amount, DateTime dateTime);
    }
}
