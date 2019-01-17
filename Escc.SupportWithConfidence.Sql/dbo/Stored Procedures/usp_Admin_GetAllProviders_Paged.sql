CREATE PROCEDURE [dbo].[usp_Admin_GetAllProviders_Paged]
(
	@PageIndex int,
	@PageSize int
)
AS
DECLARE
@firstRecord int, 
@lastRecord int




CREATE TABLE #tempProviders
(
	tId bigint IDENTITY PRIMARY KEY,
	[Id] [bigint] NOT NULL,
	[FlareId] [bigint] NOT NULL,
	[ProviderName] [varchar](500) NOT NULL,
	[Address1] [varchar](500) NULL,
	[Address2] [varchar](500) NULL,
	[Address3] [varchar](500) NULL,
	[Address4] [varchar](500) NULL,
	[Postcode] [varchar](10) NULL,
	[PublishAddress] [bit] NOT NULL,
	[TelephoneNumber] [varchar](50) NULL,
	[MobileNumber] [varchar](50) NULL,
	[EmailAddress] [varchar](200) NULL,
	[WebsiteAddress] [varchar](200) NULL,
	[FaxNumber] [varchar](50) NULL,
	[Easting] [varchar](20) NULL,
	[Northing] [varchar](20) NULL,
	[PublishToWeb] [bit] NOT NULL,
	[Availability1] [varchar](500) NULL,
	[Availability2] [varchar](500) NULL,
	[Availability3] [varchar](500) NULL,
	[Coverage] [varchar](500) NULL,
	[Coverage2] [varchar](500) NULL,
	[ContactName] [varchar](500) NULL,
	[CRBCheckDate] [varchar](100) NULL,
	[CQCCheckDate] [varchar](100) NULL,
	[BWCFlag] [bit] NOT NULL,
	[PhotographId] [int] NULL,
	[Experience] [text] NULL,
	[Background] [text] NULL,
	[Expertise] [text] NULL,
	[Services] [text] NULL,
	[Costs] [text] NULL,
	[Crb] [text] NULL
)

INSERT INTO #tempProviders
(
	[Id],
	[FlareId],
	[ProviderName],
	[Address1],
	[Address2],
	[Address3],
	[Address4],
	[Postcode],
	[PublishAddress],
	[TelephoneNumber],
	[MobileNumber],
	[EmailAddress],
	[WebsiteAddress],
	[FaxNumber],
	[Easting],
	[Northing],
	[PublishToWeb],
	[Availability1],
	[Availability2],
	[Availability3],
	[Coverage],
	[Coverage2],
	[ContactName],
	[CRBCheckDate],
	[CQCCheckDate],
	[BWCFlag],
	[PhotographId],
	[Experience],
	[Background],
	[Expertise],
	[Services],
	[Costs],
	[Crb]
)
SELECT 
	   p.[Id]
	  ,p.[FlareId]
	  ,[ProviderName]
	  ,[Address1]
	  ,[Address2]
	  ,[Address3]
	  ,[Address4]
	  ,[Postcode]
	  ,[PublishAddress]
	  ,[TelephoneNumber]
	  ,[MobileNumber]
	  ,[EmailAddress]
	  ,[WebsiteAddress]
	  ,[FaxNumber]
	  ,[Easting]
	  ,[Northing]
	  ,[PublishToWeb]
	  ,[Availability1]
	  ,[Availability2]
	  ,[Availability3]
	  ,[Coverage]
	  ,[Coverage2]
	  ,[ContactName]
	  ,[CRBCheckDate]
	  ,[CQCCheckDate]
	  ,[BWCFlag]
	  ,pe.PhotographId
	  ,pe.Experience
	  ,pe.Background
	  ,pe.Expertise
	  ,pe.Services
	  ,pe.Costs
	  ,pe.Crb
	  
	  
	
 FROM Provider as p
 INNER JOIN ProviderExtra as pe ON pe.FlareId =  p.FlareId
ORDER BY ProviderName ASC




SELECT @firstRecord =  (@PageIndex - 1) * @PageSize
SELECT @lastRecord =  (@PageIndex * @PageSize + 1)

SELECT * FROM #tempProviders
WHERE
	#tempProviders.tId > @firstRecord AND  #tempProviders.tId  < @lastRecord 
ORDER BY tId ASC

SELECT pc.FlareId, pc.CategoryId, c.Description FROM Category as c 
INNER JOIN ProviderCategory as pc ON c.Id = pc.CategoryId
ORDER BY Pc.FlareId

SELECT Count(*) as TotalResults FROM Provider


DROP TABLE #tempProviders

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_GetAllProviders_Paged] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

