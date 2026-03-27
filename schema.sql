CREATE TABLE [dbo].[AccountCredentials](
	[UserId] [int] IDENTITY(123456790,1) NOT NULL PRIMARY KEY,
	[UserName] [nvarchar](25) NOT NULL,
	[Password] [nvarchar](25) NOT NULL);

CREATE TABLE [dbo].[BankAccount](
	[UserId] [int] NOT NULL,
	[Id] [int] IDENTITY(100,1) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
  FOREIGN KEY([UserId]) REFERENCES [dbo].[AccountCredentials] ([UserId]));

CREATE TABLE [dbo].[UserDetails](
	[FirstName] [nvarchar](25) NOT NULL,
	[LastName] [nvarchar](25) NOT NULL,
	[PhoneNumber] [nvarchar](25) NULL,
	[Address] [nvarchar](25) NULL,
	[UserId] [int] NOT NULL,
  FOREIGN KEY([UserId]) REFERENCES [dbo].[AccountCredentials] ([UserId]));

CREATE TABLE [dbo].[Movement](
	[BankAccountId] [int] NOT NULL,
	[Title] [nvarchar](25) NOT NULL,
	[Date] [datetime2](0) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
  FOREIGN KEY([BankAccountId]) REFERENCES [dbo].[BankAccount] ([Id]));

CREATE TABLE [dbo].[Deposit](
	[MovementId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
  FOREIGN KEY([MovementId]) REFERENCES [dbo].[Movement] ([Id]));

CREATE TABLE [dbo].[Withdraw](
	[MovementId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
  FOREIGN KEY([MovementId]) REFERENCES [dbo].[Movement] ([Id]));

CREATE TABLE [dbo].[Transfer](
	[MovementId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[ToBankAccountId] [int] NULL,
	[FromBankAccountId] [int] NULL,
  FOREIGN KEY([MovementId]) REFERENCES [dbo].[Movement] ([Id]));

ALTER TABLE [dbo].[Transfer]  WITH CHECK 
  ADD  CONSTRAINT [CK_ToBankAccountId_FromBankAccountId_MutualNull] 
  CHECK  (([ToBankAccountId] IS NULL AND [FromBankAccountId] IS NOT NULL 
  OR [ToBankAccountId] IS NOT NULL AND [FromBankAccountId] IS NULL));

INSERT INTO AccountCredentials (UserName, Password)
VALUES ('Dylan6790', 'Bob123'),
('Aster6791', 'Amelia123'),
('Scott6792', 'Vivian123'),
('Griffin6793', 'Luiza123'),
('DeMarti6794', 'Francesca123'),
('Marti6795', 'Lory123'),
('Martini6796', 'Laila123'),
('Vi6797', 'Violet123');

INSERT INTO UserDetails (FirstName, LastName, UserId)
VALUES ('Bob', 'Dylan', 123456790),
('Amelia', 'Aster', 123456791),
('Vivian', 'Scott', 123456792),
('Luiza', 'Griffin', 123456793),
('Franceska', 'DeMarti', 123456794),
('Lory', 'Marti', 123456795),
('Laila', 'Martini', 123456796),
('Violet', 'Vi', 123456797);

INSERT INTO BankAccount (UserId, Balance)
VALUES(123456790, 400.00),
(123456791, 520.00),
(123456792, 100.00),
(123456793, 900.00),
(123456794, 410.00),
(123456795, 0.00),
(123456796, 570.00),
(123456797, 490.00);


  
  
  
