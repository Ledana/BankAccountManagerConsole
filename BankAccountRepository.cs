using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }
             set { _balance = value; }           
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        private List<string> _movements = [];

        public BankAccountRepository() { }

        public void MakeDeposit(decimal amount, SqlConnection conn)
        {
            if (amount < 0)
                Console.WriteLine("The amount should be positive");

            else if (amount == 0)
                return;
            else
            {
                string insertQuery = @"INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Deposit', @Date)";
                int MovementId;
                string insertIntoDeposit = @"INSERT INTO Deposit (MovementId, Amount) VALUES (@MovementId, @Amount)";
                string updateQuery = @"UPDATE BankAccount SET Balance = Balance + @Amount WHERE Id = @BankAccountId";

                using (SqlCommand insertCmd = new (insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    MovementId = (int)insertCmd.ExecuteScalar();

                }
                using (SqlCommand cmd = new(insertIntoDeposit, conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", MovementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }

                _balance += amount;
                Console.WriteLine($"You deposidet {amount} and your balance now is {_balance}");
                
            }
        }
        public void MakeWithdraw(decimal amount, SqlConnection conn)
        {
            if (amount <= 0)
            {
                Console.WriteLine("The amount should be positive");
                return;
            }

            if (amount > _balance)
                Console.WriteLine("The amount to withdraw should be less then the balance");

            else
            {
                string insertQuery = @" INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Withdrawal', @Date)";

                string insertIntoWithdraw = @" INSERT INTO Withdraw (MovementId, Amount) VALUES (@MovementId, @Amount)";
                int movementId;

                string updateQuery = @" UPDATE BankAccount SET Balance = Balance - @Amount WHERE Id = @BankAccountId";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    movementId = (int)insertCmd.ExecuteScalar();
                }
  
                using (SqlCommand cmd = new(insertIntoWithdraw, conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", movementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }
                _balance -= amount;
                Console.WriteLine($"You withdrawed {amount} and your balance now is {_balance}");
            }
        }

        public void TransferMoney(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn)
        { 
            if (amount <= 0)
            {
                Console.WriteLine("The amount should be positive");
                return;
            }
            else if(amount >= _balance)
            {
                Console.WriteLine("The amount is bigger than your balance");
                return;
            }

            if (bankAccount == null)
                Console.WriteLine("The userId is not valid");
            else if (bankAccount.UserId == UserId)
            {
                Console.WriteLine("You can not transfer money to yourself");
                return;
            }
            else
            {
                int movementId;

                string insertIntoMovementQuery = @" INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Transfer', @Date)";
                using (SqlCommand insertCmd = new SqlCommand(insertIntoMovementQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    movementId = (int)insertCmd.ExecuteScalar();
                }

                string updateBalanceQuery = @" UPDATE BankAccount SET Balance = Balance - @Amount WHERE Id = @BankAccountId";
                using (SqlCommand updateCmd = new SqlCommand(updateBalanceQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }

                string insertIntoTransfer = @"INSERT INTO [Transfer] (MovementId, Amount, ToBankAccountId) VALUES (@MovementId, @Amount, @BankAccountId)";
                using (SqlCommand cmd = new(insertIntoTransfer, conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", movementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@BankAccountId", bankAccount.Id);
                    cmd.ExecuteNonQuery();
                }

                string insertIntoMovementTarget = @"INSERT INTO Movement (BankAccountId, Title, [Date]) VALUES (@BankAccountId, 'Transfer', @Date)";
                using (SqlCommand cmd = new(insertIntoMovementTarget, conn))
                {
                    cmd.Parameters.AddWithValue("@BankAccountId", bankAccount.Id);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                string insertIntoTransferTarget = @"INSERT INTO [Transfer] (MovementId, Amount, FromBankAccountId) VALUES (@MovementId, @Amount, @BankAccountId)";
                using (SqlCommand cmd = new(insertIntoTransferTarget, conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", movementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@BankAccountId", Id);
                    cmd.ExecuteNonQuery();
                }

                string updateBalanceTargetQuery = "UPDATE BankAccount SET Balance = Balance + @Amount WHERE Id = @Id";
                using (SqlCommand updateCmd = new SqlCommand(updateBalanceTargetQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@Id", bankAccount.Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }


                _balance -= amount;
                bankAccount.creditAmount(amount);
                Console.WriteLine($"You transfered {amount} to {bankAccount.UserId}");
                Console.WriteLine($"Your balance is now {_balance}");
            }
        }

        public void GetMovements(SqlConnection conn)
        {
            ArgumentNullException.ThrowIfNull(conn);
            if (Id == 0) throw new InvalidOperationException("BankAccount.Id must be set before loading movements.");
            if (conn.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("The supplied SqlConnection must be open.");

            _movements.Clear();

            const string sql = @"SELECT m.Id, m.Title, m.[Date] AS MovementDate,
            d.Amount AS DepositAmount,
            w.Amount AS WithdrawAmount,
            t.Amount AS TransferAmount,
            t.ToBankAccountId AS ToBankAccountId,
            t.FromBankAccountId AS FromBankAccountId
            FROM Movement m
            LEFT JOIN Deposit d ON d.MovementId = m.Id
            LEFT JOIN Withdraw w ON w.MovementId = m.Id
            LEFT JOIN [Transfer] t ON t.MovementId = m.Id
            WHERE m.BankAccountId = @BankAccountId
            ORDER BY m.[Date] DESC";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@BankAccountId", Id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var movementId = reader.GetInt32(reader.GetOrdinal("Id"));
                        var title = reader.IsDBNull(reader.GetOrdinal("Title")) ? string.Empty : reader.GetString(reader.GetOrdinal("Title"));
                        var date = reader.IsDBNull(reader.GetOrdinal("MovementDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MovementDate"));

                        string formatted = "";
                        if (!reader.IsDBNull(reader.GetOrdinal("DepositAmount")))
                        {
                            var amount = reader.GetDecimal(reader.GetOrdinal("DepositAmount"));
                            formatted = $"{date:yyyy-MM-dd HH:mm} - {title} - Deposit {amount:C2} (Id:{movementId})";
                        }
                        else if (!reader.IsDBNull(reader.GetOrdinal("WithdrawAmount")))
                        {
                            var amount = reader.GetDecimal(reader.GetOrdinal("WithdrawAmount"));
                            formatted = $"{date:yyyy-MM-dd HH:mm} - {title} - Withdrawal {amount:C2} (Id:{movementId})";
                        }
                        else if (!reader.IsDBNull(reader.GetOrdinal("TransferAmount")))
                        {
                            var amount = reader.GetDecimal(reader.GetOrdinal("TransferAmount"));
                            if (!reader.IsDBNull(reader.GetOrdinal("ToBankAccountId")))
                            {
                                var toId = reader.IsDBNull(reader.GetOrdinal("ToBankAccountId")) ? "?" : reader["ToBankAccountId"].ToString();
                                formatted = $"{date:yyyy-MM-dd HH:mm} - {title} - Transfer {amount:C2} to {toId} (Id:{movementId})";
                            }
                            else if (!reader.IsDBNull(reader.GetOrdinal("FromBankAccountId")))
                            {
                                var fromId = reader.IsDBNull(reader.GetOrdinal("FromBankAccountId")) ? "?" : reader["FromBankAccountId"].ToString();
                                formatted = $"{date:yyyy-MM-dd HH:mm} - {title} - Transfer {amount:C2} from {fromId} (Id:{movementId})";
                            }
                        }
                        else
                        {
                            // fallback when no specific amount column is present
                            formatted = $"{date:yyyy-MM-dd HH:mm} - {title} (Id:{movementId})";
                        }
                        _movements.Add(formatted);
                    }
                }
            }
            if (_movements.Count == 0)
                Console.WriteLine("You have no movements");
            else
            {
                foreach (var item in _movements)
                {
                    Console.WriteLine(item);
                }
            }
        }
        //changing the balance when another bank account transfered money to this
        public void creditAmount(decimal amount)
        {
            
        }

        //adding the movement when another bank account transfered money to this
        public void addMovement(IBankAccountRepository bankAccount, decimal amount, SqlConnection conn, DateTime dateTime)
        {
            
        }
    }
}
