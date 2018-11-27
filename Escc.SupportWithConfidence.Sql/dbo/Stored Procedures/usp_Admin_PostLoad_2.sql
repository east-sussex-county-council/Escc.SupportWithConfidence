
CREATE PROCEDURE [dbo].[usp_Admin_PostLoad_2]
AS
BEGIN TRANSACTION
UPDATE Category SET IsActive = 1
WHERE Category.Id IN (
SELECT DISTINCT Category.ParentId from Category WHERE Category.Id IN
(SELECT ProviderCategory.CategoryId FROM Provider
INNER JOIN ProviderCategory ON ProviderCategory.FlareId = Provider.FlareId
INNER JOIN Category ON Category.Id = ProviderCategory.CategoryId
WHERE
Provider.PublishToWeb = 1
))

UPDATE Category SET IsActive = 1
WHERE Category.Id IN (
SELECT ProviderCategory.CategoryId FROM Provider
INNER JOIN ProviderCategory ON ProviderCategory.FlareId = Provider.FlareId
INNER JOIN Category ON Category.Id = ProviderCategory.CategoryId
WHERE
Provider.PublishToWeb = 1
)
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

BEGIN TRANSACTION
INSERT INTO ProviderExtra
(
FlareId
)
SELECT p.FlareId FROM Provider as p
LEFT JOIN ProviderExtra as pe ON pe.FlareId = p.FlareId
WHERE pe.FlareId IS NULL
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_PostLoad_2] TO [SupportWithConfidenceAdminRole]
    AS [dbo];


