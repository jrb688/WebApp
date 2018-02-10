CREATE TABLE [dbo].[BatchTest]
(
	[BatchId] INT NOT NULL,
	[BatchVersion] INT NOT NULL,
	[TestId] INT NOT NULL,
	[TestVersion] INT NOT NULL,
	[Passed] INT NULL,
	Primary Key (BatchId, BatchVersion, TestId, TestVersion),
	Foreign Key (BatchId) References Batch(BatchId),
	Foreign Key (BatchVersion) References Batch(BatchVersion),
	Foreign Key (TestId) References Test(TestId),
	Foreign Key (TestVersion) References Test(TestVersion)
)
