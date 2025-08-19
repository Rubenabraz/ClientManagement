IF ld.fnObjectExists ('ld.tClients') = 0
BEGIN
	CREATE TABLE ld.tClients (
		cltID               INT IDENTITY (1,1),
		cltName             VARCHAR(25)   NOT NULL,
		cltSurname          VARCHAR(50)   NOT NULL,
		cltFullName         AS (cltName + ' ' + cltSurname) PERSISTED NOT NULL,
		cltEmail            NVARCHAR(150) NOT NULL,
		cltPhoneNumber      NVARCHAR(20)  NOT NULL,
		cltGender           VARCHAR(25)   NOT NULL,
		cltActive           BIT           NOT NULL CONSTRAINT DF_ld_tClients_cltActive      DEFAULT (1),
		cltStatus           VARCHAR(25)   NOT NULL CONSTRAINT DF_ld_tClients_cltStatus      DEFAULT ('novo'),
		cltIsDeleted        BIT           NOT NULL CONSTRAINT DF_ld_tClients_cltIsDeleted   DEFAULT (0),
		cltCreatedUser      VARCHAR(50)   NOT NULL CONSTRAINT DF_ld_tClients_clCreatedBy    DEFAULT (USER_NAME()),
		cltCreatedDate      DATETIME      NOT NULL CONSTRAINT DF_ld_tClients_cltCreatedDate DEFAULT (GETDATE()),
		cltUpdatedUser      VARCHAR(50)   NULL,
		cltUpdatedDate      DATETIME      NULL,
		CONSTRAINT PK_cltID PRIMARY KEY (cltID)
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('ld.tClients') AND name = 'IDX_tClients01')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IDX_tClients01 ON ld.tClients(
		cltEmail       ASC,
		cltPhoneNumber ASC
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('ld.tClients') AND name = 'IDX_tClients02')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_tClients02 ON ld.tClients(
		cltActive       ASC,
		cltStatus       ASC
	)
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('ld.tClients') AND name = 'IDX_tClients03')
BEGIN
	CREATE NONCLUSTERED INDEX IDX_tClients03 ON ld.tClients(
		cltFullName
	)
END
GO