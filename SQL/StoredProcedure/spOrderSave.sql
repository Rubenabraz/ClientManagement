IF ld.fnObjectExists('ld.spOrderSave') = 0
BEGIN
	EXEC('CREATE PROCEDURE ld.spOrderSave AS SELECT 1')
END
GO

ALTER PROCEDURE ld.spOrderSave(
	@ordID            INT OUTPUT,
	@ordClientID      INT,
	@ordName          VARCHAR (75),
	@ordStatus        VARCHAR(50) = NULL,
	@ordTotalAmount   DECIMAL (9,2),
	@ordDescription   VARCHAR(255),
	@ordDelivered     BIT = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS (SELECT 1 FROM ld.tOrders WHERE ordID = @ordID)
	BEGIN

	UPDATE ld.tOrders SET
		ordClientID      = @ordClientID,
		ordName          = @ordName,
		ordStatus        = @ordStatus,
		ordTotalAmount   = @ordTotalAmount,
		ordDescription   = @ordDescription,
		ordDelivered     = @ordDelivered,
		ordUpdatedUser   = USER_NAME(),
		ordUpdatedDate   = GETDATE()
	WHERE ordID          = @ordID

	SET @ordID = @ordID;
END
ELSE
BEGIN
	INSERT INTO ld.tOrders (
		ordClientID,
		ordName,
		ordTotalAmount,
		ordDescription
	)
	VALUES(
		@ordClientID,
		@ordName,
		@ordTotalAmount,
		@ordDescription
	)
	END

	IF @ordID IS NOT NULL
        SET @ordID = (SELECT ordID FROM ld.tOrders WHERE ordID = @ordID);

END
GO