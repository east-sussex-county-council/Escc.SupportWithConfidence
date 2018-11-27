-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 27 November 2018
-- Description:	Clears all accreditations for a provider, ready to re-insert
-- =============================================
CREATE PROCEDURE usp_Admin_Provider_ClearAccreditations
	@flareId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM ProviderAccreditation WHERE FlareId = @flareId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Provider_ClearAccreditations] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

