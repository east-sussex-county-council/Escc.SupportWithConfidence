CREATE PROCEDURE [dbo].[usp_Admin_ProviderExtra_Update]
(
@FlareId bigint,
@Experience text,
@Expertise text,
@Background text,
@Services text,
@Costs text,
@Crb text,
@PublishToWeb bit
)
AS


BEGIN TRANSACTION
UPDATE ProviderExtra
SET
Experience = @Experience,
Background = @Background,
Expertise = @Expertise,
Services = @Services,
Costs = @Costs,
Crb = @Crb
WHERE
FlareId = @FlareId
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;


BEGIN
BEGIN TRANSACTION
UPDATE Provider
SET PublishToWeb = @PublishToWeb
WHERE FlareId = @FlareId
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_ProviderExtra_Update] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

