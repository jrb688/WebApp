CREATE TABLE [dbo].[TestProc]
(
	[BatchId] INT NOT NULL, 
    [BatchVersion] INT NOT NULL, 
	[TestId] INT NOT NULL,
	[TestVersion] INT NOT NULL,
	[ProcId] INT NOT NULL,
	[ReqId] INT NULL,
	[Parameters] varchar(256) NOT NULL,
	[Passed] INT NULL,
	[Order] INT NOT NULL,
    Primary Key (TestId, TestVersion, ProcId, BatchId, BatchVersion),
	Foreign Key (TestId, TestVersion) references Test(TestId, TestVersion),
	Foreign Key (ProcId) references [Procedure](ProcId),
	Foreign Key (ReqId) references Requirement(ReqId),
	Foreign Key (BatchId, BatchVersion) references Batch(BatchId, BatchVersion)
)
