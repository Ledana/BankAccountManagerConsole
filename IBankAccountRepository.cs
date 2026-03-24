using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public interface IBankAccountRepository
    {
        public decimal Balance
        {
            get; 
        }
        public int Id { get; }
        public string UserId { get;}

        //public List<string> _movements { get; set; }
        

        public void MakeDeposit(decimal amount, SqlConnection conn);
        public void MakeWithdraw(decimal amount, SqlConnection conn);

        public void TransferMoney(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn);
        public void GetMovements(SqlConnection conn);
        //a method to change the balance of another bank account
        public void creditAmount(decimal amount);
        //a method to add to movements of another bank account
        public void addMovement(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn, DateTime dateTime);
    }
}
