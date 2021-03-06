USE [Choreman]
GO
/****** Object:  StoredProcedure [dbo].[UpdatePassword]    Script Date: 3/17/2020 6:44:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[UpdatePassword]
	-- Add the parameters for the stored procedure here
	@pUsername NVARCHAR(50),
	@pPassword NVARCHAR(50),

	@responseMessage NVARCHAR(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @salt UNIQUEIDENTIFIER = NEWID()

	BEGIN TRY
		Update dbo.Users
		SET
			PasswordHash = HASHBYTES('SHA2_512', @pPassword + CAST(@salt AS NVARCHAR(36))),
			Salt = @salt
		WHERE
			Username = @pUsername

		SET @responseMessage='Success'
	END TRY

	BEGIN CATCH
		SET @responseMessage=ERROR_MESSAGE()
	END CATCH

END
