
CREATE PROCEDURE [dbo].[usp_Admin_PostLoad_2]
AS
BEGIN TRANSACTION
UPDATE Categories SET IsActive = 0
IF @@ERROR <> 0 ROLLBACK TRANSACTION

UPDATE Categories SET IsActive = 1
WHERE Categories.CategoryId IN (
SELECT DISTINCT Categories.ParentId from Categories WHERE Categories.CategoryId IN
(SELECT ProviderCategory.CategoryId FROM Provider
INNER JOIN ProviderCategory ON ProviderCategory.FlareId = Provider.FlareId
INNER JOIN Categories ON Categories.CategoryId = ProviderCategory.CategoryId
WHERE
Provider.PublishToWeb = 1
))
IF @@ERROR <> 0 ROLLBACK TRANSACTION

UPDATE Categories SET IsActive = 1
WHERE Categories.CategoryId IN (
SELECT ProviderCategory.CategoryId FROM Provider
INNER JOIN ProviderCategory ON ProviderCategory.FlareId = Provider.FlareId
INNER JOIN Categories ON Categories.CategoryId = ProviderCategory.CategoryId
WHERE
Provider.PublishToWeb = 1
)
IF @@ERROR <> 0 ROLLBACK TRANSACTION

INSERT INTO ProviderExtra
(
FlareId
)
SELECT p.FlareId FROM Provider as p
LEFT JOIN ProviderExtra as pe ON pe.FlareId = p.FlareId
WHERE pe.FlareId IS NULL
IF @@ERROR <> 0 ROLLBACK TRANSACTION

COMMIT TRANSACTION



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_PostLoad_2] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

