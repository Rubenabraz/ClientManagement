USE master
GO

CREATE DATABASE LastDance COLLATE Latin1_General_BIN
GO

USE LastDance
GO

CREATE SCHEMA ld
GO

CREATE FUNCTION ld.fnObjectExists(
	@objectName NVARCHAR(150) 
)
RETURNS BIT
AS
BEGIN
	DECLARE @exists BIT = 0

	IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@objectName))
	BEGIN
		SET @exists = 1
	END

	RETURN @exists
END
GO