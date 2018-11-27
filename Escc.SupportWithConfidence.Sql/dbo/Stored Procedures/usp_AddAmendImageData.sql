-- =============================================
-- Author:		Phil Robinson, Web Team
-- ALTER date: 28 May 2009
-- Description:	Adds or amends an image file as a database record
-- =============================================
CREATE PROCEDURE [dbo].[usp_AddAmendImageData]
	@FileDataID int,
	@FileOriginalName varchar(255),
	@FileDescription varchar(255),
	@MIMEContentType varchar(25),
	@FileData image,
	@Username varchar(50)
AS
BEGIN
	IF (@FileDataID = 0)
	BEGIN
		-- We need to add a brand new file data record
		INSERT INTO dbo.ImageData
			(FileOriginalName,
			FileDescription,
			MIMEContentType,
			FileData,
			AddedBy,
			AddedDate)
		VALUES		
			(@FileOriginalName,
			LTRIM(RTRIM(@FileDescription)),
			@MIMEContentType,
			@FileData,
			@Username,
			GETDATE())

		-- Get the new id value
		SET @FileDataID = @@IDENTITY
	END
	ELSE
	BEGIN
		-- We need to update an existing file data record
		UPDATE dbo.ImageData
		SET
			FileOriginalName = @FileOriginalName,
			FileDescription = LTRIM(RTRIM(@FileDescription)),
			MIMEContentType = @MIMEContentType,
			FileData = @FileData,
			ModifiedBy = @Username,
			ModifiedDate = GETDATE()
		WHERE
			FileDataID = @FileDataID	
	END

	SELECT @FileDataID
END

