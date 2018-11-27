CREATE TABLE [dbo].[ProviderType] (
    [Id]   BIGINT        NOT NULL,
    [Name] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_ProviderType] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

