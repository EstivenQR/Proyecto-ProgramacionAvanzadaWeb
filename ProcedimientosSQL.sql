USE BaseDatos_ProyectoLibreria
GO 

--CREATE OR ALTER PROCEDURE sp_Login(@User NVARCHAR(MAX), @Contrase�a NVARCHAR(MAX), @Exitos BIT OUTPUT)
--AS 
--BEGIN
--	IF( SELECT COUNT(*) FROM Usuario WHERE Username = @User AND Password = @Contrase�a) = 1
--	BEGIN
--		SET @Exitos =1 
--	END
--	ELSE
--	BEGIN
--		SET @Exitos =0
--	END
--	SELECT @Exitos
--END

GO
DECLARE @F bit
EXEC sp_Login 'leo3047','123', @F OUTPUT
GO

CREATE OR alter PROCEDURE sp_ObtenerUsuario(@User NVARCHAR(MAX), @Contrase�a NVARCHAR(MAX))
AS 
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Usuario WHERE Username = @User AND Password = @Contrase�a
END

GO
EXEC sp_ObtenerUsuario 'Zoe123','123'