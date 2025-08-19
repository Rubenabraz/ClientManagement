IF ld.fnObjectExists ('ld.spClientSave') = 0
BEGIN
	EXEC('CREATE PROCEDURE ld.spClientSave AS SELECT 1')
END
GO

ALTER PROCEDURE ld.spClientSave (
	@cltID               INT OUTPUT,
	@cltName             VARCHAR(25),
	@cltSurname          VARCHAR(50),
	@cltEmail            NVARCHAR(150),
	@cltPhoneNumber      NVARCHAR(20),
	@cltGender           VARCHAR(25),
	@cltActive           BIT           = NULL,
	@cltStatus           VARCHAR(25)   = NULL
)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM ld.tClients WHERE cltID = @cltID)
	BEGIN

	SET NOCOUNT ON

	UPDATE ld.tClients SET
		cltName             = @cltName,
		cltSurname          = @cltSurname,
		cltEmail            = @cltEmail,
		cltPhoneNumber      = @cltPhoneNumber,
		cltGender           = @cltGender,
		cltActive           = @cltActive,
		cltStatus           = @cltStatus,
		cltUpdatedUser      = USER_NAME(),
		cltUpdatedDate      = GETDATE()
	WHERE cltID             = @cltID

	SET @cltID = @cltID
END
ELSE
	BEGIN

	INSERT INTO ld.tClients(
		cltName,
		cltSurname,
		cltEmail,
		cltPhoneNumber,
		cltGender
	)
	VALUES(
		@cltName,
		@cltSurname,
		@cltEmail,
		@cltPhoneNumber,
		@cltGender
	)

	SET @cltID = SCOPE_IDENTITY();

	END
END
GO