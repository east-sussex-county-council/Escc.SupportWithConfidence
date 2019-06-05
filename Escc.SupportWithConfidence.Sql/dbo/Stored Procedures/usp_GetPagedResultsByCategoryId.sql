CREATE PROCEDURE [dbo].[usp_GetPagedResultsByCategoryId]
(

@PageIndex int,
@PageSize int,
@Easting  int,
@Northing int,
@CategoryId int 


)
AS

DECLARE 	@firstRecord int, @lastRecord int, @ParentCategoryId int

DECLARE  @ChildCategories TABLE (ChildId int)

SELECT @ParentCategoryId = c.ParentId FROM dbo.Categories AS c WHERE c.CategoryId = @CategoryId
IF @ParentCategoryId IS NULL 
BEGIN

CREATE TABLE #tempProviders  
    (
        tId bigint IDENTITY PRIMARY KEY,
		Id bigint NOT NULL,
        FlareId bigint NOT NULL,
		[Name] varchar (500) NOT NULL,
		[Paon] varchar (500),
		[StreetName] varchar (500),
		[Town] varchar (500),
		[AdministrativeArea] varchar (500),
		[Postcode] varchar (500),
		[PublishAddress] bit,
		[Based in] varchar (500),
        Telephone varchar (500),
        Mobile varchar (500),
        Email varchar (500),
        [Distance from me] int ,
        Availability1 varchar (500),
        Availability2 varchar (500),
        Availability3 varchar (500),
        Coverage varchar (500),
        Coverage2 varchar (500)

    ) 


		INSERT INTO @ChildCategories (ChildId)
		SELECT  DISTINCT c.CategoryId FROM dbo.Categories AS c 
		INNER JOIN dbo.ProviderCategory AS pc ON c.CategoryId = pc.CategoryId
		INNER JOIN dbo.Provider AS p ON pc.FlareId = p.FlareId	
		WHERE c.IsActive = 1 AND p.PublishToWeb = 1 AND (c.ParentId  = @CategoryId AND c.ParentId IS NOT NULL)
		ORDER BY c.CategoryId

		SET NOCOUNT ON
	INSERT INTO #tempProviders
(
		Id ,
        FlareId ,
        [Name] ,
		[Paon] ,
		[StreetName] ,
		[Town] ,
		[AdministrativeArea],
		[Postcode],
		[PublishAddress],
        [Based in] ,
        Telephone ,
        Mobile,
        Email ,
        [Distance from me] ,
		Availability1,
		Availability2,
		Availability3,
        Coverage,
        Coverage2 
)
		SELECT DISTINCT  
			p.Id, 
			p.FlareId, 
			p.ProviderName as [Name],
			p.Address1 as [Paon],
			p.Address2 as [StreetName],
			p.Address3 as [Town],
			p.Address4 as [AdminstrativeArea],
			p.Postcode,
			p.PublishAddress,
			p.address3 as[Based in], 
			p.TelephoneNumber as Telephone,
			p.MobileNumber as Mobile, 
			p.EmailAddress as Email, 
			CASE
				WHEN @Easting IS NULL OR @Northing IS NULL THEN 0
					ELSE ROUND(.00062137119 * (SQRT(square(CAST(Easting AS int) - @Easting) + square(CAST(Northing AS int) - @Northing))),0) 
				END AS [Distance from me],
			p.Availability1, p.Availability2, p.Availability3,
			p.Coverage, p.Coverage2
		 
		FROM dbo.Provider AS p
		INNER JOIN dbo.ProviderCategory AS pc ON p.FlareId = pc.FlareId
		 WHERE pc.CategoryId IN (SELECT ChildId  FROM @ChildCategories)
			AND (p.PublishToWeb = 1)
	ORDER BY [Distance from me], [Name] ASC

SELECT @firstRecord = (@PageIndex - 1) * @PageSize
SELECT @lastRecord = (@PageIndex * @PageSize + 1)
SELECT * FROM #tempProviders
WHERE
	#tempProviders.tId > @firstRecord AND  #tempProviders.tId  < @lastRecord 
	



SELECT pc.FlareId,  pc.CategoryId, c.Description FROM Categories as c 
INNER JOIN ProviderCategory as pc ON c.CategoryId = pc.CategoryId
WHERE pc.FlareId  IN (Select FlareId FROM #tempProviders)
ORDER BY Pc.FlareId

SELECT COUNT(*) as TotalResults FROM #tempProviders
SELECT c.Description, c.Summary FROM dbo.Categories AS c
WHERE c.CategoryId = @CategoryId

DROP TABLE #tempProviders
END

ELSE
BEGIN
CREATE TABLE #tempProviders2  
    (
         tId bigint IDENTITY PRIMARY KEY,
		Id bigint NOT NULL,
        FlareId bigint NOT NULL,
		[Name] varchar (500) NOT NULL,
		[Paon] varchar (500),
		[StreetName] varchar (500),
		[Town] varchar (500),
		[AdministrativeArea] varchar (500),
		[Postcode] varchar (500),
		[PublishAddress] bit,
		[Based in] varchar (500),
        Telephone varchar (500),
        Mobile varchar (500),
        Email varchar (500),
        [Distance from me] int ,
		Availability1 varchar (500),
        Availability2 varchar (500),
        Availability3 varchar (500),
        Coverage varchar (500),
        Coverage2 varchar (500)
    ) 
		
	SET NOCOUNT ON
	INSERT INTO #tempProviders2
(
		Id ,
        FlareId ,
        [Name] ,
		[Paon] ,
		[StreetName] ,
		[Town] ,
		[AdministrativeArea],
		[Postcode],
		[PublishAddress],
        [Based in] ,
        Telephone ,
        Mobile,
        Email ,
        [Distance from me] ,
		Availability1,
		Availability2,
		Availability3,
        Coverage,
        Coverage2
)
SELECT DISTINCT
	p.Id, 
	p.FlareId, 
	p.ProviderName as [Name],
	p.Address1 as [Paon],
	p.Address2 as [StreetName],
	p.Address3 as [Town],
	p.Address4 as [AdminstrativeArea],
	p.Postcode,
	p.PublishAddress,
	p.address3 as[Based in], 
	p.TelephoneNumber as Telephone,
	p.MobileNumber as Mobile, 
	p.EmailAddress as Email, 
	CASE
		WHEN @Easting IS NULL OR @Northing IS NULL THEN 0
			ELSE ROUND(.00062137119 * (SQRT(square(CAST(Easting AS int) - @Easting) + square(CAST(Northing AS int) - @Northing))),0) 
		END AS [Distance from me],
		p.Availability1, p.Availability2, p.Availability3,
		p.Coverage, p.Coverage2
FROM 
	Provider as p
INNER JOIN ProviderCategory as pc 
	ON p.FlareId = pc.FlareId
INNER JOIN Categories as c 
	ON pc.CategoryId = c.CategoryId

WHERE  pc.CategoryId IN  (SELECT  pc.CategoryId FROM dbo.Categories AS c 
		INNER JOIN dbo.ProviderCategory AS pc ON c.CategoryId = pc.CategoryId
		INNER JOIN dbo.Provider AS p ON pc.FlareId = p.FlareId	
		WHERE c.IsActive = 1 AND p.PublishToWeb = 1 AND (pc.CategoryId = @CategoryId))
		AND p.PublishToWeb = 1
ORDER BY [Distance from me], [Name] ASC


SELECT @firstRecord = (@PageIndex - 1) * @PageSize
SELECT @lastRecord = (@PageIndex * @PageSize + 1)
SELECT * FROM #tempProviders2
WHERE
	#tempProviders2.tId > @firstRecord AND  #tempProviders2.tId  < @lastRecord 



SELECT pc.FlareId,  pc.CategoryId, c.Description FROM Categories as c 
INNER JOIN ProviderCategory as pc ON c.CategoryId = pc.CategoryId
WHERE pc.FlareId  IN (Select FlareId FROM #tempProviders2)
ORDER BY Pc.FlareId

SELECT COUNT(*) as TotalResults  FROM #tempProviders2
SELECT c.Description, c.Summary FROM dbo.Categories AS c
WHERE c.CategoryId = @CategoryId


DROP TABLE #tempProviders2

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_GetPagedResultsByCategoryId] TO [SupportWithConfidenceUserRole]
    AS [dbo];

