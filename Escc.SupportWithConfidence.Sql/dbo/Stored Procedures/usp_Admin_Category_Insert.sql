CREATE PROCEDURE [dbo].[usp_Admin_Category_Insert]
(
@Id bigint,
@Sequence bigint,
@Code char(10),
@Description nvarchar(70),
@ParentId bigint,
@Depth int,
@ProviderTypeId bigint,
@IsActive bit
)
AS
BEGIN TRANSACTION

INSERT INTO [SupportWithConfidence].[dbo].[Category]
           ([Id],
           [Sequence]
           ,[Code]
           ,[Description]
           ,[ParentId]
           ,[Depth]
           ,[ProviderTypeId]
           ,[IsActive])
     VALUES
           (
           @Id,
			@Sequence,
			@Code,
			@Description,
			@ParentId,
			@Depth,
			@ProviderTypeId,
			@IsActive
           )

IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Category_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];
