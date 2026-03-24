using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public  interface IUsersRepository
    {
        public List<User> GetAllUsers();
        public bool ValidatePassword(string userId, string password);
        public User? FindUserById(string id);
        public User? FindUserByUsername(string userName);
    }
}
