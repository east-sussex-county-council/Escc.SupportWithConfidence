-- =============================================
-- Author:		Phil Robinson, Web Team
-- ALTER date: 28 May 2009
-- Description:	Gets an image file from a database record
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetImageData]
	@FileDataID int
AS
BEGIN
	IF (@FileDataID > 0)
	BEGIN
		SELECT *, DATALENGTH(FileData) AS FileSize
		FROM dbo.ImageData
		WHERE FileDataID = @FileDataID
	END
END

