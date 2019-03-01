CREATE PROCEDURE [dbo].[usp_Admin_Category_Insert]
(
@Id bigint,
@Sequence bigint,
@Description nvarchar(70),
@ParentId bigint,
@Depth int,
@IsActive bit
)
AS
SET IDENTITY_INSERT [SupportWithConfidence].[dbo].[Categories] ON

BEGIN TRANSACTION

INSERT INTO [SupportWithConfidence].[dbo].[Categories]
           ([CategoryId],
           [Sequence]
           ,[Description]
           ,[ParentId]
           ,[Depth]
           ,[IsActive])
     VALUES
           (
           @Id,
			@Sequence,
			@Description,
			@ParentId,
			@Depth,
			@IsActive
           )

IF @@ERROR <> 0 ROLLBACK TRANSACTION;
ELSE COMMIT TRANSACTION;

SET IDENTITY_INSERT [SupportWithConfidence].[dbo].[Categories] OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_Admin_Category_Insert] TO [SupportWithConfidenceAdminRole]
    AS [dbo];

