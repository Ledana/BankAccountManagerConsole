
namespace BankAccountManager
{
    public class FakeUsersRepository : User, IUsersRepository
    {
        private List<User> _users = [];
        public FakeUsersRepository()
        {
            _users = [
                new User("Bob", "Dylan", "Bob123"),
                //{
                //    UserName = "Dylan6790",
                //    FirstName = "Bob", LastName = "Dylan", UserId = "123456790", Pasword = "Bob123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456790", Id = 101})
                //},
                new User("Amelia", "Aster", "Amelia123"),
                //{
                //    UserName = "Aster6791",
                //    FirstName = "Amelia", LastName = "Aster",UserId = "123456791", Pasword = "Amelia123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456791", Id = 102})
                //},
                new User("Vivian", "Scott", "Vivian123"),
                //{
                 //   UserName = "Scott6792",
                 //   FirstName = "Vivian", LastName = "Scott", UserId = "123456792", Pasword = "Vivian123",
                    //GetBankAccountRepository = (
                    //new FakeBankAccountRepository() { UserId = "123456792", Id = 103})
                //},
                new User("Luiza", "Griffin", "Luiza123"),
                //{
                //    UserName = "Griffin6793",
                //    FirstName = "Luiza", LastName = "Griffin", UserId = "123456793", Pasword = "Luiza123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456793", Id = 104})
                //},
                new User("Franceska", "DeMarti", "Francesca123"),
                //{
                //    UserName = "DeMarti6794",
                //    FirstName = "Franceska", LastName = "DeMarti", UserId = "123456794", Pasword = "Francesca123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456794", Id = 105})
                //},
                new User("Lory", "Marti", "Lory123"),
                //{
                //    UserName = "Marti6795",
                //    FirstName = "Lory", LastName = "Marti", UserId = "123456795", Pasword = "Lory123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456795", Id = 106 })
                //},
                new User("Laila", "Martini", "Laila123"),
                //{
                //    UserName = "Martini6796",
                //    FirstName = "Laila", LastName = "Martini", UserId = "123456796", Pasword = "Laila123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456796", Id = 107})
                //},
                new User("Violet", "Vi", "Violet123")
                //{
                //    UserName = "Vi6797",
                //    FirstName = "Violet", LastName = "Vi", UserId = "123456797", Pasword = "Violet123",
                //    //GetBankAccountRepository = (
                //    //new FakeBankAccountRepository() { UserId = "123456797", Id = 108 })
                //}
                ];
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public bool ValidatePassword(string userId, string password)
        {
            User? user = _users.FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return false;
            return user.Pasword == password;
        }

        public User? FindUserById(string id)
        {
            return _users.FirstOrDefault(u =>
            u.UserId == id);
        }

        public User? FindUserByUsername(string userName)
        {
            return _users.FirstOrDefault(a => a.UserName == userName);
        }
    }
}
