CREATE PROCEDURE [dbo].[usp_Admin_PostLoad]
AS
BEGIN TRANSACTION
-- Make all categories Live and providers
UPDATE Category SET IsActive = 1
WHERE Category.Id IN (SELECT c.Id from provider as p
inner join providercategory as pc on  pc.FlareId = p.FlareId
inner join category as c on c.Id = pc.CategoryId GROUP BY c.Id)



UPDATE Category SET IsActive = 1
WHERE Category.Id IN (
SELECT DISTINCT c.ParentId from  dbo.Category AS c
WHERE c.Id IN (SELECT c.Id from  dbo.Category AS c
inner join dbo.ProviderCategory AS pc on pc.CategoryId = c.Id
INNER JOIN dbo.Provider AS p ON p.FlareId = pc.FlareId WHERE p.PublishToWeb = 1))
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

BEGIN TRANSACTION
UPDATE ProviderExtra
SET
IsDeleted = 0
WHERE FlareId IN
(SELECT p.FlareId FROM Provider as p
RIGHT JOIN ProviderExtra as pe ON pe.FlareId = p.FlareId)
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

