CREATE PROCEDURE [dbo].[usp_Admin_ProviderExtra_Select_Photo]
(
@FlareId bigint
)
AS
SELECT PhotographId From ProviderExtra
WHERE FlareId = @FlareId

