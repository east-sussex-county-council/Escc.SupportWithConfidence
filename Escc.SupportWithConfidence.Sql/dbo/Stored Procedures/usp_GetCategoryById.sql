CREATE PROCEDURE [dbo].[usp_GetCategoryById]
(
@CategoryId INT
)
AS

DECLARE @ParentId INT

SELECT @ParentId =  dbo.Category.ParentId FROM dbo.Category WHERE dbo.Category.Id = @CategoryId

   SELECT 	
		c.Id ,
		Sequence,
        Code ,
        Description ,
        ParentId ,
        Depth ,
        ProviderTypeId ,
        IsActive
FROM dbo.Category AS c
 WHERE   ParentId  = @ParentId OR Id = @ParentId OR Id = @CategoryId  OR ParentId = @CategoryId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_GetCategoryById] TO [SupportWithConfidenceUserRole]
    AS [dbo];

