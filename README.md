🏦 Bank Account Manager
📌 Overview
This is a C#/.NET console application that simulates a simple banking system.
Users can log in, deposit, withdraw, transfer money, and view their transaction history.
I’m a beginner developer, and this project was my first step into SQL. I chose ADO.NET deliberately to see database operations in “raw mode” and understand how things work under the hood.
👉 In my GitHub account, there’s also a WPF version of this project (without database, just hardcoded users) that shows my progression from UI design to database integration.

⚙️ Features
- User login with username and password.
- Deposit and withdraw money.
- Transfer money between accounts with transaction safety.
- View all account movements.
- Option to use a real SQL repository or a fake repository with hardcoded users.

🛠️ Technologies
- C# / .NET
- SQL Server
- ADO.NET (SqlConnection, SqlCommand, SqlTransaction)

🚀 How to Run
- Clone the repository.
- Create a SQL Server database named Users.
- Run the script in schema.sql to create tables and seed initial users.
- Update the connection string in Program.cs.
- Run the console app and log in with a username. (Inforamtion about users are saved in UserDetails.txt)

## 📖 Example Flow
Wellcome to our bank app
Please input your username or type 'exit'
Aster6791
Please put you password:
Amelia123
Wellcome Amelia Aster
You balance is: 0,00

What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
1
Put the amount you want to deposit
500
You deposited 500 and your balance now is 500
What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
3
Put the userId you want to transfer to
123456792
Put the amount you want to transfer
150
You transfer 150 to 123456792 and you balance now is 350
What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
4
2024-01-01T00:00:00 Deposit 500,00 Lekë
2024-01-01T00:00:00 Transfer 150,00 Lekë to 123456791
What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
5
Please input your username or type 'exit'
Scott6792
Please put you password:
Vivian123
Wellcome Vivian Scott
You balance is: 150,00

What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
2
Put the amount you want to withdraw
100
You withdrawed 100 and your balance now is 50
What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
4
123456791 transfered you 150 at 1.1.2024 12:00:00?p.d.
2024-01-01T00:00:00 Withdraw 100,00 Lekë
What do you want to do?
1. Make a deposit.
2. Make a withdraw
3. Make a transfer
4. See all movements
5. Log out
5
Please input your username or type 'exit'
exit
The user could not be found try again
Thank you for using our bank!
