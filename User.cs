using Microsoft.Data.SqlClient;

namespace BankAccountManager
{
    public class User
    {
        //public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserId { get; set; }// = "123456789";
        private static int _count = 0;
        public string? UserName { get; set; }
        internal string Pasword {  get; set; }
        public IBankAccountRepository GetBankAccountRepository { get; set; }
        public User() { }

        //public User(string fullName)
        //{
        //if (string.IsNullOrWhiteSpace(fullName))
        //    throw new ArgumentException("The name can't be empty");

        //FullName = fullName;

        //int userId = Convert.ToInt32(UserId);
        //_count++;
        //UserId = (userId + _count).ToString();
        //int index = FullName.IndexOf(" ");
        //UserName = FullName.Substring(index+1) + UserId.Substring(UserId.Length - 4);

        //GetBankAccountRepository = new(this.UserId, conn);
        //}


    }
}
