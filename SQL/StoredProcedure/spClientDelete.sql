IF ld.fnObjectExists ('ld.spClientDelete') = 0
BEGIN
	EXEC('CREATE PROCEDURE ld.spClientDelete AS SELECT 1')
END
GO

ALTER PROCEDURE ld.spClientDelete (
	@cltID INT
)
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM ld.tClients WHERE cltID = @cltID)
	BEGIN
		RAISERROR('Cliente não encontrado', 16,1)
	END

	UPDATE ld.tClients SET
		cltStatus      = 'removido',
		cltIsDeleted   = 1,
		cltActive      = 0,
		cltUpdatedUser = USER_NAME(),
		cltUpdatedDate = GETDATE()
	WHERE cltID        = @cltID

END
GO