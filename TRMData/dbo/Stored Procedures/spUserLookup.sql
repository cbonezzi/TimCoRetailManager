﻿CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT FirstName, LastName, EmailAddress 
	FROM [dbo].[User]
	WHERE Id = @Id;
END
