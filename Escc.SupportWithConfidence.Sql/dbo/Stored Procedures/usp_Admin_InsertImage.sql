CREATE PROCEDURE [dbo].[usp_Admin_InsertImage]
(
@ProviderId bigint,
@Photograph image

)
AS
INSERT INTO Image
(ProviderId, Photograph)
VALUES
(
@ProviderId,
@Photograph
)

