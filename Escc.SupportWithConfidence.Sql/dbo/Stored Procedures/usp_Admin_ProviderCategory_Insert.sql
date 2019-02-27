CREATE PROCEDURE [dbo].[usp_Admin_ProviderCategory_Insert]
(
@Id bigint,
@FlareId bigint,
@CategoryId bigint
)
AS
INSERT INTO ProviderCategory
(
Id,
FlareId,
CategoryId
)
VALUES
(
@Id,
@FlareId,
@CategoryId
)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_ProviderCategory_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

