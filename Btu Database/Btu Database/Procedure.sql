CREATE TABLE [dbo].[Procedure]
(
	[ProcId] INT NOT NULL,
	[Name] Varchar(32) NOT NULL,
	[Description] varchar(1024) NOT NULL,
	[Script] varchar(32) NOT NULL,
	PRIMARY KEY (ProcId)
)
