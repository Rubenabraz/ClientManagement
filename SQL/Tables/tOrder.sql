IF ld.fnObjectExists ('ld.tOrders') = 0
BEGIN
	CREATE TABLE ld.tOrders (
		ordID            INT IDENTITY (1,1),
		ordClientID      INT           NOT NULL,
		ordName          VARCHAR(75)   NOT NULL,
		ordStatus        VARCHAR(50)   NOT NULL CONSTRAINT DF_tOrders_ordStatus    DEFAULT('em processamento'),
		ordIsDeleted     BIT           NOT NULL CONSTRAINT DF_tOrders_ordIsDeleted DEFAULT (0),
		ordTotalAmount   DECIMAL (9,2) NOT NULL,
		ordDescription   VARCHAR(255)  NULL,
		ordDelivered     BIT           NOT NULL CONSTRAINT DF_tOrders_ordDelivered   DEFAULT (0),
		ordCreatedUser   VARCHAR(50)   NOT NULL CONSTRAINT DF_tOrders_ordCreatedUser DEFAULT (USER_NAME()),
		ordCreatedDate   DATETIME      NOT NULL CONSTRAINT DF_tOrders_ordCreatedDate DEFAULT (GETDATE()),
		ordUpdatedUser   VARCHAR(50)   NULL,
		ordUpdatedDate   DATETIME      NULL
		CONSTRAINT PK_ordID PRIMARY KEY (ordID)
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('ld.tOrders') AND name = 'IDX_tOrders01')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_tOrders01 ON ld.tOrders(
		ordClientID ASC
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('ld.tOrders') AND name = 'IDX_tOrders02')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_tOrders02 ON ld.tOrders(
		ordStatus ASC
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE NAME = 'FK_ordClientID')
BEGIN
	ALTER TABLE ld.tOrders ADD CONSTRAINT FK_ordClientID FOREIGN KEY (ordClientID) REFERENCES ld.tClients (cltID)
END
GO