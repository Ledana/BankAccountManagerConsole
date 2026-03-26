using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager
{
    public class BankAccountServices : IBankAccountServices
    {
        private decimal _balance;
        public int Id { get; private set; }
        public string UserId { get; private set; }
        private List<string> _movements = [];

        private readonly SqlConnection _conn;

        public BankAccountServices(string userId, int id, decimal balance, SqlConnection conn)
        {
            UserId = userId;
            Id = id;
            _balance = balance;
            _conn = conn;
        }

        public bool MakeDeposit(decimal amount, out decimal newBalance)
        {
            if (amount < 50)
            {
                newBalance = _balance;
                return false;
            }
            else
            {
                string insertQuery = @"INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Deposit', @Date)";
                int MovementId;
                string insertIntoDeposit = @"INSERT INTO Deposit (MovementId, Amount) VALUES (@MovementId, @Amount)";
                string updateQuery = @"UPDATE BankAccount SET Balance = Balance + @Amount WHERE Id = @BankAccountId";

                using (SqlCommand insertCmd = new(insertQuery, _conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    MovementId = (int)insertCmd.ExecuteScalar();

                }
                using (SqlCommand cmd = new(insertIntoDeposit, _conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", MovementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, _conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }
                _balance += amount;
                newBalance = _balance;
                return true;

            }            
        }
        public bool MakeWithdraw(decimal amount, out decimal newBalance)
        {
            if (amount < 50 || amount > _balance)
            {
                newBalance = _balance;
                return false;
            }
            else
            {
                string insertQuery = @" INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Withdrawal', @Date)";

                string insertIntoWithdraw = @" INSERT INTO Withdraw (MovementId, Amount) VALUES (@MovementId, @Amount)";
                int movementId;

                string updateQuery = @" UPDATE BankAccount SET Balance = Balance - @Amount WHERE Id = @BankAccountId";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, _conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    movementId = (int)insertCmd.ExecuteScalar();
                }

                using (SqlCommand cmd = new(insertIntoWithdraw, _conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", movementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, _conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }
                _balance -= amount;
                newBalance = _balance;
                return true;
            }
        }

        public void TransferMoney(IBankAccountServices bankAccount, decimal amount)
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
            else if(amount < 50)
                Console.WriteLine("You cannot transfer less then 50.00");

            if (bankAccount == null)
                Console.WriteLine("The userId is not valid");
            else if (bankAccount.UserId == UserId)
            {
                Console.WriteLine("You can not transfer money to yourself");
                return;
            }

            // perform DB changes (existing code)...
            int movementId;
                int targetMovementId;
                //insert into table movements for this bank account
                string insertIntoMovementQuery = @" INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id
                VALUES (@BankAccountId, 'Transfer', @Date)";
                using (SqlCommand insertCmd = new SqlCommand(insertIntoMovementQuery, _conn))
                {
                    insertCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    insertCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    movementId = (int)insertCmd.ExecuteScalar();
                }

                //change the balance of this bank account
                string updateBalanceQuery = @" UPDATE BankAccount SET Balance = Balance - @Amount WHERE Id = @BankAccountId";
                using (SqlCommand updateCmd = new SqlCommand(updateBalanceQuery, _conn))
                {
                    updateCmd.Parameters.AddWithValue("@BankAccountId", Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }

                //insert into table transfer for this bank account
                string insertIntoTransfer = @"INSERT INTO [Transfer] (MovementId, Amount, ToBankAccountId) VALUES (@MovementId, @Amount, @BankAccountId)";
                using (SqlCommand cmd = new(insertIntoTransfer, _conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", movementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@BankAccountId", bankAccount.Id);
                    cmd.ExecuteNonQuery();
                }

                //insert into table movements for the target bank account
                string insertIntoMovementTarget = @"INSERT INTO Movement (BankAccountId, Title, [Date]) OUTPUT INSERTED.Id VALUES (@BankAccountId, 'Transfer', @Date)";
                using (SqlCommand cmd = new(insertIntoMovementTarget, _conn))
                {
                    cmd.Parameters.AddWithValue("@BankAccountId", bankAccount.Id);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    targetMovementId = (int)cmd.ExecuteScalar();
                }

                //insert into table trasnfer for the target bank account
                string insertIntoTransferTarget = @"INSERT INTO [Transfer] (MovementId, Amount, FromBankAccountId) VALUES (@MovementId, @Amount, @BankAccountId)";
                using (SqlCommand cmd = new(insertIntoTransferTarget, _conn))
                {
                    cmd.Parameters.AddWithValue("@MovementId", targetMovementId);
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@BankAccountId", Id);
                    cmd.ExecuteNonQuery();
                }

                //change the balance of the target bank account
                string updateBalanceTargetQuery = "UPDATE BankAccount SET Balance = Balance + @Amount WHERE Id = @Id";
                using (SqlCommand updateCmd = new SqlCommand(updateBalanceTargetQuery, _conn))
                {
                    updateCmd.Parameters.AddWithValue("@Id", bankAccount.Id);
                    updateCmd.Parameters.AddWithValue("@Amount", amount);
                    updateCmd.ExecuteNonQuery();
                }

                _balance -= amount;
                bankAccount.creditAmount(amount);
            _movements.Add($"You transfered {amount} to {bankAccount.UserId}");
                Console.WriteLine($"You transfered {amount} to {bankAccount.UserId}");
                Console.WriteLine($"Your balance is now {_balance - amount}");
        }

        public void GetMovements()
        {
            ArgumentNullException.ThrowIfNull(_conn);
            if (Id == 0) throw new InvalidOperationException("BankAccount.Id must be set before loading movements.");
            if (_conn.State != System.Data.ConnectionState.Open)
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

            using (var cmd = new SqlCommand(sql, _conn))
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
            this._balance += amount;
        }

        //adding the movement when another bank account transfered money to this
        public void addMovement(IBankAccountServices bankAccount, decimal amount, DateTime dateTime)
        {
            this._movements.Add($"{bankAccount.UserId} transfered you {amount} at {dateTime}");
        }

        public decimal GetBalance()
        {
            return _balance;
        }

    }
}
