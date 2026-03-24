using Microsoft.Data.SqlClient;

namespace BankAccountManager
{
    public class User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        internal string Pasword {  get; set; }
        public IBankAccountRepository GetBankAccountRepository { get; set; }
        public User() { }


    }
}
