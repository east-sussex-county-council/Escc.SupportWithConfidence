CREATE PROCEDURE [dbo].[usp_GetAllCategoriesWithProvider]
(
@HasProvider bit
)
AS
IF @HasProvider = 1
BEGIN
SELECT DISTINCT	
		c.CategoryId ,
		Sequence,
        Code ,
        Description ,
        ParentId ,
        Depth ,
        IsActive
FROM dbo.Categories AS c
LEFT JOIN dbo.ProviderCategory AS pc ON c.CategoryId = pc.CategoryId
LEFT JOIN  dbo.Provider AS p ON pc.FlareId = p.FlareId
WHERE IsActive = 1
ORDER BY Sequence , Code
END
ELSE
SELECT DISTINCT	
		c.CategoryId ,
		Sequence,
        Code ,
        Description ,
        ParentId ,
        Depth ,
        IsActive
FROM dbo.Categories AS c
LEFT JOIN dbo.ProviderCategory AS pc ON c.CategoryId = pc.CategoryId
LEFT JOIN  dbo.Provider AS p ON pc.FlareId = p.FlareId
ORDER BY Sequence , Code
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_GetAllCategoriesWithProvider] TO [SupportWithConfidenceUserRole]
    AS [dbo];

