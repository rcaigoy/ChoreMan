USE [Choreman]
GO
/****** Object:  StoredProcedure [dbo].[Authenticate]    Script Date: 3/17/2020 6:42:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Authenticate]
	-- Add the parameters for the stored procedure here
	@pUsername nvarchar(50),
	@pPassword nvarchar(50),

	@responseMessage nvarchar(50)='' OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @Username nvarchar(50)

	IF EXISTS (SELECT Top 1 Username from dbo.Users where Username=@pUsername)
		BEGIN
			SET @Username = (SELECT Username FROM dbo.Users
								WHERE Username = @pUsername
									AND
								PasswordHash = HASHBYTES('SHA2_512', @pPassword + CAST(Salt AS NVARCHAR(36))))
			IF (@Username IS NULL)
				SET @responseMessage='Invalid Credentials'
			ELSE
				SET @responseMessage='Success'
		END
	ELSE
		SET @responseMessage='Invalid Credentials'
END
