CREATE PROCEDURE [dbo].[usp_Admin_Provider_Insert]
(
@Id bigint,  
@FlareId bigint,
@ProviderName VarChar (500) , 
@Address1 VarChar (500) , 
@Address2 VarChar (500) , 
@Address3 VarChar (500),  
@Address4 VarChar (500) ,  
@Postcode VarChar (500),  
@PublishAddress Bit , 
@Telephone VarChar (500), 
@Mobile VarChar (500) ,
@Email VarChar (500) ,
@Website VarChar (500) , 
@Fax VarChar (500) ,
@Easting VarChar (500) , 
@Northing VarChar(500) , 
@PublishToWeb Bit , 
@Availability1 VarChar (500), 
@Availability2 VarChar (500),  
@Availability3 VarChar (500), 
@Coverage VarChar (500) ,
@Coverage2 VarChar (500) ,
@ContactName VarChar (500) , 
@CRBCheckDate VarChar (500) ,
@CQCCheckDate VarChar (500) ,
@BWCFlag Bit
)
AS
BEGIN TRANSACTION
INSERT INTO [SupportWithConfidence].[dbo].[Provider]
(			[Id]
           ,[FlareId]
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
           ,[BWCFlag])
     VALUES
           (
           @Id ,
           @FlareId ,
           @ProviderName ,
           @Address1 ,
           @Address2 ,
           @Address3 ,
           @Address4 ,
           @Postcode ,
           @PublishAddress, 
           @Telephone,
           @Mobile ,
           @Email ,
           @Website ,
           @Fax,
           @Easting ,
           @Northing,
           @PublishToWeb, 
           @Availability1 ,
           @Availability2 ,
           @Availability3 ,
           @Coverage ,
           @Coverage2 ,
           @ContactName ,
           @CRBCheckDate ,
           @CQCCheckDate ,
           @BWCFlag 
           )
		   IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Provider_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

