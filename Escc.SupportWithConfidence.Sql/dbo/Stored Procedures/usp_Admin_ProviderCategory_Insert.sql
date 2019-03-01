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
	BEGIN TRANSACTION
	
	INSERT INTO ProviderCategory (FlareId, CategoryId) VALUES (@FlareId, @CategoryId)
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	-- We may now have a case to set a previously inactive category to active,
	-- but only if at least one of its providers (including this one) is published
	UPDATE Categories SET IsActive = 1 WHERE CategoryId IN (
		SELECT CategoryId FROM ProviderCategory 
		LEFT JOIN Provider ON ProviderCategory.FlareId = Provider.FlareId 
		WHERE Provider.PublishToWeb = 1
		AND CategoryId = @CategoryId
	)
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	-- This in turn means we may need to set the parent category to active
	UPDATE Categories SET IsActive = 1 WHERE Categories.CategoryId IN (
		SELECT ParentId FROM Categories 
		INNER JOIN ProviderCategory ON Categories.CategoryId = ProviderCategory.CategoryId
		INNER JOIN Provider ON ProviderCategory.FlareId = Provider.FlareId 
		WHERE Provider.PublishToWeb = 1
		AND Categories.CategoryId = @CategoryId
	)
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	COMMIT TRANSACTION
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_ProviderCategory_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

