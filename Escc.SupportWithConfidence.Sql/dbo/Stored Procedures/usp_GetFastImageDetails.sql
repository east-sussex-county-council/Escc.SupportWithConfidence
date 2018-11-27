-- =============================================
-- Author:		Phil Robinson, Web Team
-- ALTER date: 28 May 2009
-- Description:	Efficiently gets all the details about an image file from a database record
--              without getting the BLOB data itself.
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetFastImageDetails]
	@FileDataID int
AS
BEGIN
	IF (@FileDataID > 0)
	BEGIN
		SELECT 
			FileOriginalName,
			FileDescription,
			MIMEContentType,
			DATALENGTH(FileData) AS FileSize,
			AddedBy,
			AddedDate,
			ModifiedBy,
			ModifiedDate
		FROM dbo.ImageData
		WHERE FileDataID = @FileDataID
	END
END