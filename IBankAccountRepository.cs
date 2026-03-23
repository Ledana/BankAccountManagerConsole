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
            get; set;
        }
        public int Id { get; set; }
        public string UserId { get; set; }

        //public List<string> _movements { get; set; }
        

        public void MakeDeposit(decimal amount, SqlConnection conn);
        public void MakeWithdraw(decimal amount, SqlConnection conn);

        public void TransferMoney(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn);
        public void GetMovements(SqlConnection conn);
    }
}
