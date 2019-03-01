-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 28 February 2019
-- Description:	Clears all categories for a provider, ready to re-insert
-- =============================================
CREATE PROCEDURE [dbo].[usp_Admin_Provider_ClearCategories]
	@flareId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	-- Note the categories that were assigned, before we clear them
	DECLARE @ClearedCategories TABLE (CategoryId bigint)

	INSERT INTO @ClearedCategories
	SELECT CategoryId FROM ProviderCategory WHERE FlareId = @FlareId
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	-- Remove the categories from the provider
	DELETE FROM ProviderCategory WHERE FlareId = @flareId
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	-- Set any category which now has 0 providers to be not active
	UPDATE Categories SET IsActive = 0 WHERE CategoryId IN (
		SELECT Categories.CategoryId FROM Categories 
		LEFT JOIN ProviderCategory ON Categories.CategoryId = ProviderCategory.CategoryId
		LEFT JOIN Provider ON ProviderCategory.FlareId = Provider.FlareId 
		WHERE (Provider.PublishToWeb = 1 OR Provider.PublishToWeb IS NULL)
		AND 
		Categories.CategoryId IN (SELECT CategoryId FROM @ClearedCategories)
		GROUP BY Categories.CategoryId
		HAVING COUNT(Provider.FlareId) = 0
	)
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	-- Set any parent categories which now have no active child categories to be not active themselves
	UPDATE Categories SET IsActive = 0 WHERE Depth = 1 AND CategoryId NOT IN (
		SELECT DISTINCT Parent.CategoryId FROM Categories AS Parent 
		INNER JOIN Categories AS Child ON Parent.CategoryId = Child.ParentId
		WHERE Parent.Depth = 1 AND Child.IsActive = 1
	)
	IF @@ERROR <> 0 ROLLBACK TRANSACTION

	COMMIT TRANSACTION
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Provider_ClearCategories] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

