CREATE PROCEDURE [dbo].[usp_Admin_ProviderExtra_Update_Photo]
(
@FlareId bigint,
@PhotographId int,
@Remove bit

)
AS
BEGIN TRANSACTION
IF @Remove = 0
BEGIN
UPDATE  ProviderExtra
SET PhotographId = @PhotographId
WHERE
FlareId = @FlareId
END
ELSE
BEGIN
UPDATE  ProviderExtra
SET PhotographId = NULL
WHERE
FlareId = @FlareId
END
IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

