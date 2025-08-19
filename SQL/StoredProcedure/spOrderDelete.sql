IF ld.fnObjectExists ('ld.spOrderDelete') = 0
BEGIN
	EXEC('CREATE PROCEDURE ld.spOrderDelete AS SELECT 1')
END
GO

ALTER PROCEDURE ld.spOrderDelete (
	@ordID INT 
)
AS
BEGIN

	IF @ordID IS NULL
	BEGIN
		RAISERROR('Order não encontrada', 16,1)
		RETURN
	END

	UPDATE ld.tOrders SET
		ordStatus      = 'removido',
		ordIsDeleted   = 1,
		ordUpdatedUser = USER_NAME(),
		ordUpdatedDate = GETDATE()
	WHERE ordID        = @ordID
END
GO