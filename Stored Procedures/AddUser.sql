USE [Choreman]
GO
/****** Object:  StoredProcedure [dbo].[dbo.AddUser]    Script Date: 3/17/2020 6:44:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[dbo.AddUser]
	-- Add the parameters for the stored procedure here
	@pUsername nvarchar(50),
	@pEmail nvarchar(50),
	@pPhone nchar(10),
	@pFirstName nvarchar(50),
	@pLastName nvarchar(50),
	@pPassword nvarchar(50),

	@responseMessage NVARCHAR(250) OUTPUT

AS	

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
		--create salt
		DECLARE @salt UNIQUEIDENTIFIER = NEWID()
		INSERT INTO dbo.Users(Username, Email, Phone, PasswordHash, Salt, FirstName, LastName, IsVerified, IsActive, NumberOfLoginAttempts, AccountTypeId)
		Values(@pUsername, @pEmail, @pPhone, 
			HASHBYTES('SHA2_512', @pPassword + CAST(@salt AS NVARCHAR(36))), @salt,
			@pFirstName, @pLastName, 0, 1, 0, 1)

		SET @responseMessage='Success'
	END TRY

	BEGIN CATCH
		SET @responseMessage=ERROR_MESSAGE()
	END CATCH
END

