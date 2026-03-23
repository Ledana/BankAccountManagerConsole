
namespace BankAccountManager
{
    public class FakeUsersRepository : User, IUsersRepository
    {
        private List<User> _users = [];
        public FakeUsersRepository()
        {
            _users = [
                new User()
                {
                    UserName = "Dylan6790",
                    FirstName = "Bob", LastName = "Dylan", UserId = "123456790", Pasword = "Bob123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456790", Balance = 200.00m , Id = 101})
                },
                new User()
                {
                    UserName = "Aster6791",
                    FirstName = "Amelia", LastName = "Aster",UserId = "123456791", Pasword = "Amelia123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456791", Balance = 200.00m , Id = 102})
                },
                new User()
                {
                    UserName = "Scott6792",
                    FirstName = "Vivian", LastName = "Scott", UserId = "123456792", Pasword = "Vivian123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456792", Balance = 200.00m , Id = 103})
                },
                new User()
                {
                    UserName = "Griffin6793",
                    FirstName = "Luiza", LastName = "Griffin", UserId = "123456793", Pasword = "Luiza123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456793", Balance = 200.00m , Id = 104})
                },
                new User()
                {
                    UserName = "DeMarti6794",
                    FirstName = "Franceska", LastName = "DeMarti", UserId = "123456794", Pasword = "Francesca123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456794", Balance = 200.00m , Id = 105})
                },
                new User()
                {
                    UserName = "Marti6795",
                    FirstName = "Lory", LastName = "Marti", UserId = "123456795", Pasword = "Lory123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456795", Balance = 200.00m, Id = 106 })
                },
                new User()
                {
                    UserName = "Martini6796",
                    FirstName = "Laila", LastName = "Martini", UserId = "123456796", Pasword = "Laila123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456796", Balance = 200.00m , Id = 107})
                },
                new User()
                {
                    UserName = "Vi6797",
                    FirstName = "Violet", LastName = "Vi", UserId = "123456797", Pasword = "Violet123",
                    GetBankAccountRepository = (
                    new FakeBankAccountRepository() { UserId = "123456797", Balance = 200.00m, Id = 108 })
                }
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
