CREATE TABLE [dbo].[Test]
(
	[TestId] INT NOT NULL,
	[TestVersion] INT NOT NULL,
	[UserId] INT NOT NULL,
	[EcuId] INT NOT NULL,
	[Name] Varchar(32) NULL,
	[DateCreated] datetime NOT NULL,
	[DateRun] datetime NULL,
	PRIMARY KEY (TestId, TestVersion),
	Foreign Key (UserID) References [User](UserId),
	Foreign Key (EcuId) References [ECU](EcuId)
)
