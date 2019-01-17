CREATE PROCEDURE [dbo].[usp_GetProviderById]
(
@FlareId BIGINT,
@IsApproved bit
)
AS
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
      ,pe.[PhotographId]
      ,pe.[Experience]
      ,pe.[Background]
      ,pe.[Expertise]
	  ,pe.[Services]
	  ,pe.[Costs]
	  ,pe.[Crb],
	  Accreditations.AccreditationId,
	  Accreditations.Name,
	  Accreditations.Website
 FROM dbo.Provider as p
INNER JOIN ProviderExtra as pe ON pe.FlareId =  p.FlareId
LEFT JOIN ProviderAccreditation ON p.FlareId = ProviderAccreditation.FlareId
LEFT JOIN Accreditations ON ProviderAccreditation.AccreditationId = Accreditations.AccreditationId
WHERE pe.FlareId = @FlareId
AND
PublishToWeb = @IsApproved


SELECT pc.FlareId, pc.CategoryId, c.Description FROM Category as c 
INNER JOIN ProviderCategory as pc ON c.Id = pc.CategoryId
WHERE pc.FlareId  = @FlareId
ORDER BY Pc.FlareId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_GetProviderById] TO [SupportWithConfidenceUserRole]
    AS [dbo];

