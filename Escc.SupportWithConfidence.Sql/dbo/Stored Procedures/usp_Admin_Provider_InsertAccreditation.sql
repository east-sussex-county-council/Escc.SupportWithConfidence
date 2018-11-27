-- =============================================
-- Author:		Rick Mason, Web Team
-- Create date: 27 November 2018
-- Description:	Adds an accreditation to a provider
-- =============================================
CREATE PROCEDURE usp_Admin_Provider_InsertAccreditation
	@flareId bigint,
	@accreditationId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO ProviderAccreditation (FlareId, AccreditationId) VALUES (@flareId, @accreditationId)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Provider_InsertAccreditation] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

