CREATE TABLE [dbo].[TestProc]
(
	[TestId] INT NOT NULL,
	[TestVersion] INT NOT NULL,
	[ProcId] INT NOT NULL,
	[ReqId] INT NULL,
	[Parameters] varchar(256) NOT NULL,
	[Passed] INT NULL,
	Primary Key (TestId, TestVersion, ProcId),
	Foreign Key (TestId) references Test(TestId),
	Foreign Key (TestVersion) references Test(TestVersion),
	Foreign Key (ProcId) references [Procedure](ProcId),
	Foreign Key (ReqId) references Requirement(ReqId)

)
