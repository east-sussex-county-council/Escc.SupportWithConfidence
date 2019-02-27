CREATE PROCEDURE [dbo].[usp_Admin_GetAllProviders]
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


SELECT pc.FlareId, pc.CategoryId, c.Description FROM Categories as c 
INNER JOIN ProviderCategory as pc ON c.CategoryId = pc.CategoryId
ORDER BY Pc.FlareId

SELECT Count(*) as TotalResults FROM Provider
