CREATE TABLE [dbo].[Simulator]
(
	[SimId] INT NOT NULL, 
	[EcuId] INT NOT NULL,
	Primary Key (SimId),
	Foreign Key (EcuId) References ECU(EcuId)
)
