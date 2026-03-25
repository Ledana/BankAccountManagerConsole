
namespace BankAccountManager
{
    public class FakeUsersRepository : IUsersRepository
    {
        private List<User> _users = [];
        public FakeUsersRepository()
        {
            _users = [
                new User("Bob", "Dylan", "Bob123"),
                new User("Amelia", "Aster", "Amelia123"),
                new User("Vivian", "Scott", "Vivian123"),
                new User("Luiza", "Griffin", "Luiza123"),
                new User("Franceska", "DeMarti", "Francesca123"),
                new User("Lory", "Marti", "Lory123"),
                new User("Laila", "Martini", "Laila123"),
                new User("Violet", "Vi", "Violet123")
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
            return user.Password == password;
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
