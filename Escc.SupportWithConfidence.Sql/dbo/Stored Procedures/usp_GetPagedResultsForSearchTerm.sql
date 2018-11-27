CREATE PROCEDURE [dbo].[usp_GetPagedResultsForSearchTerm]
     (
    @PageIndex int,
	@PageSize int,
	@Easting  int,
	@Northing int,
	@Name varchar (500)
    )
AS 
DECLARE @firstRecord int, @lastRecord int  
SET @Name = '%' + @Name + '%'

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
       Mobile varchar(500),
       Email varchar (500),
        [Address] varchar (500),
        [Distance from me] int  ,
        Availability1 varchar (500),
        Availability2 varchar (500),
        Availability3 varchar (500),
        Coverage varchar (500),
        Coverage2 varchar (500)

    )

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
        [Address] ,
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
	CASE When COALESCE ([Address1], '')= '' Then '' Else [Address1] + ', ' END 
			+ CASE WHEN COALESCE ([Address2], '')= '' THEN '' ELSE [Address2] + ', ' END 
			+ CASE WHEN COALESCE ([Address3], '')= '' THEN '' ELSE [Address3] + ', ' END 
			+ CASE WHEN COALESCE ([Address4], '')= '' THEN '' ELSE [Address4] + ', ' END
		 	+ CASE WHEN COALESCE ([Postcode], '')= '' THEN '' ELSE [Postcode]  END AS Address,
	CASE
		WHEN @Easting IS NULL OR @Northing IS NULL THEN 0
			ELSE ROUND(.00062137119 * (SQRT(square(CAST(Easting AS int) - @Easting) + square(CAST(Northing AS int) - @Northing))),0) 
		END AS [Distance from me],
		p.Availability1, p.Availability2, p.Availability3,
		p.Coverage, p.Coverage2
		

FROM 
	Provider as p
INNER JOIN dbo.ProviderCategory AS pc ON pc.FlareId = p.FlareId
INNER JOIN dbo.Category AS c ON c.id = pc.CategoryId
WHERE  
c.IsActive = 1 AND p.PublishToWeb = 1
AND
(p.ProviderName LIKE @Name) OR (p.ProviderName LIKE '') OR p.ContactName LIKE @Name

ORDER BY [Distance from me], [Name] ASC



SELECT @firstRecord =  (@PageIndex - 1) * @PageSize
SELECT @lastRecord =  (@PageIndex * @PageSize + 1)

SELECT * FROM #tempProviders
WHERE
	#tempProviders.tId > @firstRecord AND  #tempProviders.tId  < @lastRecord 




SELECT pc.FlareId, pc.CategoryId, c.Description FROM Category as c 
INNER JOIN ProviderCategory as pc ON c.Id = pc.CategoryId
WHERE pc.FlareId  IN (Select FlareId FROM #tempProviders)
ORDER BY pc.FlareId

--SELECT @TotalResults  = COUNT(*) FROM #tempProviders
SELECT COUNT(*) as TotalResults  FROM #tempProviders

DROP TABLE #tempProviders
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_GetPagedResultsForSearchTerm] TO [SupportWithConfidenceUserRole]
    AS [dbo];

