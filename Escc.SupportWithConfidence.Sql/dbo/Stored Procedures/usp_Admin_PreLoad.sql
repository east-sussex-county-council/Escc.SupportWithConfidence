CREATE PROCEDURE [dbo].[usp_Admin_PreLoad]
AS
BEGIN TRANSACTION

DELETE FROM Provider

IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_PreLoad] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

