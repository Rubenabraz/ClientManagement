DECLARE @newOrderID INT;

EXEC ld.spOrderSave
    @ordID = @newOrderID OUTPUT,
    @ordClientID = 1,
    @ordStatus = 'processado',
    @ordTotalAmount = 200.00,
    @ordDescription = 'Encomenda atualizada',
    @ordDelivered = 1
GO

DECLARE @newClientID INT;

EXEC ld.spClientSave
    @cltID = @newClientID OUTPUT,
    @cltName = 'João',
    @cltSurname = 'Silva',
    @cltEmail = 'joao.silva@email.com',
    @cltPhoneNumber = '912345678',
    @cltGender = 'Masculino',
    @cltStatus = 'ativo',
	@cltActive = 1
GO

EXEC ld.spOrderDelete 
	@ordID = 1,
	@ordStatus = 'removido'
GO

EXEC ld.spClientDelete 
	@cltID = 1,
	@cltStatus = 'removido'
GO
