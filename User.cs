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
        private static int _count = 0;
        private string _userId = "123456789";
        public IBankAccountRepository GetBankAccountRepository { get; set; }
        public User() { }


        //creating details for harcoded users
        public User(string firstName, string lastName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Pasword = password;

            UserId = GenerateUserId();
            UserName = GenerateUserName(lastName, UserId);

            GetBankAccountRepository = new FakeBankAccountRepository(UserId);
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
