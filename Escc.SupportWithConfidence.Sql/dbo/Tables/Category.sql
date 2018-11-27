CREATE TABLE [dbo].[Category] (
    [Id]             BIGINT        NOT NULL,
    [Sequence]       BIGINT        NOT NULL,
    [Code]           CHAR (10)     NOT NULL,
    [Description]    NVARCHAR (70) NOT NULL,
    [ParentId]       BIGINT        NULL,
    [Depth]          INT           NOT NULL,
    [ProviderTypeId] BIGINT        NOT NULL,
    [IsActive]       BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Category_ProviderType] FOREIGN KEY ([ProviderTypeId]) REFERENCES [dbo].[ProviderType] ([Id])
);

