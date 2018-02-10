CREATE TABLE [dbo].[Requirement]
(
	[ReqId] INT NOT NULL,
	[TestId] INT NOT NULL,
	[Description] Varchar(1024) NOT NULL,
	PRIMARY KEY (ReqId),
	Foreign Key (TestId) References Test(TestId)
)
