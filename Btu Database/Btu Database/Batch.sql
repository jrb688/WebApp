CREATE TABLE [dbo].[Batch]
(
	[BatchId] INT NOT NULL,
	[BatchVersion] INT NOT NULL,
	[Author_UserId] INT NOT NULL,
	[Tester_UserId] INT NULL,
	[SimId] INT NULL,
	[Name] Varchar(32) NULL,
	[Status] Varchar(8) NOT NULL,
	[DateCreated] datetime NOT NULL,
	[DateRun] datetime NULL,
	[Display] INT NOT NULL,
	Primary Key (BatchId, BatchVersion),
	Foreign Key (Author_UserId) References [User](UserId),
	Foreign Key (Tester_UserId) References [User](UserId),
	Foreign Key (SimId) References [Simulator](SimId)
)
