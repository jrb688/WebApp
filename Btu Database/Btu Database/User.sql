CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL IDENTITY(1,1),
	[FirstName] varchar(32) NOT NULL,
	[LastName] varchar(32) NOT NULL,
	[Password] varchar(32) NOT NULL,
	[Email] varchar(32) NOT NULL,
	[Privilege] varchar(32) NOT NULL,
	Primary Key (UserId)
)
