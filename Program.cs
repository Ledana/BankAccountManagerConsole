using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;

namespace BankAccountManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=LAPTOP-VIIQV46I;Database=Users;Trusted_Connection=True;Encrypt=False;";

           

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                   string ? username = "";
                    //var allUsers = GetRepository(new UserRepository(conn));
                    var allUsers = GetRepository(new FakeUsersRepository());
                    var users = allUsers.GetAllUsers();

                    Console.WriteLine("Wellcome to our bank app");
                    while (username != "exit")
                    {
                        Console.WriteLine("Please input your username or type 'exit'");
                        username = Console.ReadLine();
                        if (string.IsNullOrEmpty(username))
                        {
                            Console.WriteLine("The username is not in the right format");
                            continue;
                        }
                        //allusers choose if you want to work with the real repository which has access
                        //in the database or the fake repository with hard coded users
                        //everytime a user logs out and the user is asked for username to log in, the list
                        //of users is updated with the latest change from the database
                        
                        users = allUsers.GetAllUsers();

                        var user = allUsers.FindUserByUsername(username);

                        if (user != null)
                        {
                            while (true)
                            {
                                Console.WriteLine("Please put you password: ");
                                //putting the pasword input in the username variable so if 'exit' is typed to close the app
                                username = Console.ReadLine();
                                if (username != null)
                                {
                                    if (allUsers.ValidatePassword(user.UserId, username))
                                    {
                                        Console.WriteLine($"Wellcome {user.FirstName} {user.LastName}");
                                        Console.WriteLine($"You balance is: {user.GetBankAccountRepository.GetBalance():F2}\n");
                                        break;
                                    }
                                    //if the user doesnt remember password can exit the app
                                    else if (username == "exit")
                                        break;
                                    else
                                    {   
                                        Console.WriteLine("The password is incorrect, try again or type 'exit' to log out");
                                    }  
                                }
                                
                            }
                        }
                        else
                        {
                            Console.WriteLine("The username is not in the right format");
                            continue;
                        }

                        if (username == "exit") break;

                        if (user == null)
                        {
                            Console.WriteLine("Account could not be found, try another username");
                            continue;
                        }

                        string? input = "";
                        while (input != "5")
                        {
                            ChooseAction();
                            input = Console.ReadLine();
                            if (input == null)
                            {
                                Console.WriteLine("The input is not in the right format");
                                continue;
                            }
                            if (input == "1")
                            {
                                Deposit(user, conn);
                            }
                            if (input == "2")
                            {
                                Withdraw(user, conn);
                            }
                            if (input == "3")
                            {
                                Transfer(allUsers, user, conn);
                            }
                            if (input == "4")
                            {
                                user.GetBankAccountRepository.GetMovements();
                            }
                            if (input == "5")
                                break;
                            else
                                input = "0";
                        }
                    }
                    Console.WriteLine("Thnak you for using our bank!");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetType() + e.StackTrace);
                }
            }
        }
        //getting the repository which can be the real one or the mock
        public static IUsersRepository GetRepository(IUsersRepository repository)
        {
            return repository;
        }
        public static void Deposit(User user, SqlConnection conn)
        {
            
            Console.WriteLine("Put the amount you want to deposit");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if(user.GetBankAccountRepository.MakeDeposit(amount, out decimal newBalance))
                    Console.WriteLine($"You deposidet {amount} and your balance now is {newBalance}");
                else
                    Console.WriteLine("The amount can not be deposited");
            }
            else
            {
                Console.WriteLine("The amount is not in the right format");
            }
           
        }
        public static void Withdraw(User user, SqlConnection conn)
        {
            Console.WriteLine("Put the amount you want to withdraw");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if(user.GetBankAccountRepository.MakeWithdraw(amount, out decimal newBalance))
                    Console.WriteLine($"You deposidet {amount} and your balance now is {newBalance}");
                else
                    Console.WriteLine("The amount can not be withdrawed");
            }
            else
                Console.WriteLine("The amount is not in the right format");
        }
        public static void Transfer(IUsersRepository allUsers, User user, SqlConnection conn)
        {
            var users = allUsers.GetAllUsers();
            Console.WriteLine("Put the userId you want to transfer to");
            string? targetId = Console.ReadLine();
            if (targetId == null)
            {
                Console.WriteLine("The userId is not in the right format");
                return;
            }

            User target = allUsers.FindUserById(targetId);

            if (target == null)
            {
                Console.WriteLine("The target id could not be found");
                return;
            }
            Console.WriteLine("Put the amount you want to transfer");

            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("The amount is not in the right format");
                return;
            }
                
            user.GetBankAccountRepository.TransferMoney(target.GetBankAccountRepository, amount);
        
        }
        public static void ChooseAction()
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Make a deposit.");
            Console.WriteLine("2. Make a withdraw");
            Console.WriteLine("3. Make a transfer");
            Console.WriteLine("4. See all movements");
            Console.WriteLine("5. Log out");
        }
    }
}
