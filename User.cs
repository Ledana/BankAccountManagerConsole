using Microsoft.Data.SqlClient;
using System.Globalization;

namespace BankAccountManager
{
    public class User
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? UserId { get; private set; }
        public string? UserName { get; private set; }
        internal string Password {  get; private set; }
        private static int _count = 0;
        private string _userId = "123456789";
        public IBankAccountServices GetBankAccountServices { get; set; }
        public User(string userId, string firstName, string lastName, string userName, IBankAccountServices bankAccountRepository)
        {
            FirstName = firstName;
            LastName = lastName;
            UserId = userId;
            UserName = userName;
            GetBankAccountServices = bankAccountRepository;
        }

        //creating details for harcoded users
        public User(string firstName, string lastName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;

            UserId = GenerateUserId();
            UserName = GenerateUserName(lastName, UserId);

            GetBankAccountServices = new FakeBankAccountServices(new BankAccount(UserId));
        }

        private string GenerateUserId()
        {
            int Id = Convert.ToInt32(_userId);
            _count++;
            _userId = (Id + _count).ToString();
            return _userId;
        }
        private string GenerateUserName(string lastName, string userId)
        {
            return lastName + userId[^4..];
        }
    }
}
