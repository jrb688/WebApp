CREATE TABLE [dbo].[BatchTest]
(
	[BatchId] INT NOT NULL,
	[BatchVersion] INT NOT NULL,
	[TestId] INT NOT NULL,
	[TestVersion] INT NOT NULL,
	[Passed] INT NULL,
	Primary Key (BatchId, BatchVersion, TestId, TestVersion),
	Foreign Key (BatchId, BatchVersion) References Batch(BatchId, BatchVersion),
	Foreign Key (TestId, TestVersion) References Test(TestId, TestVersion)
)
