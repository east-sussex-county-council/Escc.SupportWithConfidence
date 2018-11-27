-- =============================================
-- Author:		Phil Robinson, Web Team
-- ALTER date: 28 May 2009
-- Description:	Deletes an image file data record from the database
-- =============================================
CREATE PROCEDURE [dbo].[usp_DeleteImageData]
	@FileDataID int
AS
BEGIN
	IF (@FileDataID > 0)
	BEGIN
		-- Multi-statement update so start a transaction
		BEGIN TRANSACTION

		-- Detach the file from its event if it was an attachment file
		UPDATE  dbo.ProviderExtra
		SET		PhotographId = NULL
		WHERE
			PhotographId = @FileDataID

		-- Check for errors
		IF @@ERROR != 0
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END

		-- Now remove the image file data record
		DELETE FROM dbo.ImageData
		WHERE
			FileDataID = @FileDataID	

		-- Check for errors
		IF @@ERROR != 0
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END

		-- All OK, so complete the transaction
		COMMIT TRANSACTION

	END
END

