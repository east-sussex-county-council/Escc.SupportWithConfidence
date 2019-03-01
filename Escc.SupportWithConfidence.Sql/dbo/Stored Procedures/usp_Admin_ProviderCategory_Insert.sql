CREATE PROCEDURE [dbo].[usp_Admin_ProviderCategory_Insert]
(
@FlareId bigint,
@CategoryId bigint
)
AS

-- Prevent duplicates
SELECT Id FROM ProviderCategory WHERE FlareId = @FlareId AND CategoryId = @CategoryId
IF (@@ROWCOUNT = 0)
BEGIN
	INSERT INTO ProviderCategory
	(
	FlareId,
	CategoryId
	)
	VALUES
	(
	@FlareId,
	@CategoryId
	)

	-- We may now have a case to set a previously inactive category to active,
	-- but only if at least one of its providers (including this one) is published
	UPDATE Categories SET IsActive = 1 WHERE CategoryId IN (
		SELECT CategoryId FROM ProviderCategory 
		LEFT JOIN Provider ON ProviderCategory.FlareId = Provider.FlareId 
		WHERE Provider.PublishToWeb = 1
		AND 
		CategoryId = @CategoryId)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_ProviderCategory_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

