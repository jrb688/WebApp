CREATE TABLE [dbo].[Simulator]
(
	[SimId] INT NOT NULL, 
	[EcuId] INT NOT NULL,
	Primary Key (SimId),
	Foreign Key (EcuID) References ECU(EcuId)
)
