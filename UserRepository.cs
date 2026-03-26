using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class UserRepository : IUsersRepository
    {
        private readonly SqlConnection _conn;

        public UserRepository(SqlConnection conn)
        {
            _conn = conn;
        }
        private List<User> _users = [];
        public List<User> GetAllUsers()
        {
            _users.Clear();
            string query = "SELECT ac.UserId, ac.UserName, ud.FirstName, ud.LastName, ba.Id AS BankAccountId, " +
                "ba.Balance FROM AccountCredentials ac JOIN UserDetails ud ON ud.UserId = " +
                "ac.UserId JOIN BankAccount ba ON ba.UserId = ac.UserId";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    _users.Add(new User
                    (
                        reader["UserId"].ToString(),
                        reader["FirstName"].ToString(),
                        reader["LastName"].ToString(),
                        reader["UserName"].ToString(),
                        new BankAccountServices(new BankAccount(reader["UserId"].ToString() ?? "Not found", Convert.ToInt32(reader["BankAccountId"]), Convert.ToDecimal(reader["Balance"])), _conn))
                        );
                        
                }
            }
            return _users;
        }
        public void CreateUser(string userId, string username, string password)
        {
            string hashedPassword = PasswordHasher.HashPassword(password);
            string query = "INSERT INTO AccountCredentials (UserId, UseName, Password)" +
                "VALUES (@userId, @username, @password)";
            using (SqlCommand cmd = new(query, _conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.ExecuteNonQuery();
            }
        }
        public bool ValidatePassword(string userId, string password)
        {
            //string query = "SELECT Password FROM AccountCredentials WHERE UserId = @userId";
            string query = "SELECT COUNT(*) FROM AccountCredentials WHERE UserId = @userId AND Password = @password";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                //var storedHash = cmd.ExecuteScalar()?.ToString();

                //if (string.IsNullOrEmpty(storedHash))
                    //return false;
                //return PasswordHasher.VerifyPassword(password, storedHash);
                
                cmd.Parameters.AddWithValue("@password", password);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
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
