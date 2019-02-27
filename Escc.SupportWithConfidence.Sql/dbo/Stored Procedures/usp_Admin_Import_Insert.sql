CREATE PROCEDURE [dbo].[usp_Admin_Import_Insert]
(
@Id bigint,
@UniqueId varchar (100),
@Name varchar (100),
@Address1 varchar (100),
@Address2 varchar (100),
@Address3 varchar (100),
@Address4 varchar (100),
@Postcode5 varchar (100),
@Cat1 varchar (100),
@Cat2 varchar (100),
@Cat3 varchar (100),
@Cat4 varchar (100),
@Cat5 varchar (100),
@Cat6 varchar (100),
@Cat7 varchar (100),
@Cat8 varchar (100),
@PublishToWeb varchar (100),
@PublishAddress varchar (100),
@Telephone varchar (100),
@Mobile varchar (100),
@Email varchar (100),
@AddressVerfied varchar (100), 
@Website varchar (100),
@Fax varchar (100),
@Easting varchar (100),
@Northing varchar (100),
@PublishPdf varchar (100),
@Availability1 varchar (100),
@Availability2 varchar (100),
@Availability3 varchar (100),
@Coverage varchar (100),
@EmploymentStatus varchar (100),
@ContactName varchar (100),
@CrbChecked varchar (100),
@CqcChecked varchar (100),
@IsBwcMember varchar (100)
)
AS
BEGIN TRANSACTION

INSERT INTO [SupportWithConfidence].[dbo].[Import]
           ([Id],
           [uniqueid]
           ,[name]
           ,[address1]
           ,[address2]
           ,[address3]
           ,[address4]
           ,[postcode5]
           ,[cat1]
           ,[cat2]
           ,[cat3]
           ,[cat4]
           ,[cat5]
           ,[cat6]
           ,[cat7]
           ,[cat8]
           ,[pubtoweb]
           ,[pubadd]
           ,[telno]
           ,[mobno]
           ,[email]
           ,[addstat]
           ,[webadd]
           ,[faxno]
           ,[easting]
           ,[northing]
           ,[pubpdf]
           ,[availabity1]
           ,[availability2]
           ,[availability3]
           ,[coverage]
           ,[employstatus]
           ,[contactname]
           ,[crbcheckdate]
           ,[cqccheckdate]
           ,[bwcmember])
     VALUES
           (@Id,
           @UniqueId ,
@Name ,
@Address1 ,
@Address2 ,
@Address3 ,
@Address4 ,
@Postcode5 ,
@Cat1 ,
@Cat2 ,
@Cat3 ,
@Cat4 ,
@Cat5 ,
@Cat6 ,
@Cat7 ,
@Cat8 ,
@PublishToWeb ,
@PublishAddress ,
@Telephone ,
@Mobile ,
@Email ,
@AddressVerfied , 
@Website ,
@Fax ,
@Easting ,
@Northing ,
@PublishPdf ,
@Availability1 ,
@Availability2 ,
@Availability3 ,
@Coverage ,
@EmploymentStatus ,
@ContactName ,
@CrbChecked ,
@CqcChecked ,
@IsBwcMember 
)

IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Import_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

