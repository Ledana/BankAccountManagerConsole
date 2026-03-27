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

