CREATE TABLE [dbo].[Categories] (
    [CategoryId]  INT            IDENTITY (1, 1) NOT NULL,
    [Sequence]    INT            NOT NULL,
    [Code]        NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [ParentId]    INT            NULL,
    [Depth]       INT            NOT NULL,
    [IsActive]    BIT            NOT NULL,
    [Summary]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);



