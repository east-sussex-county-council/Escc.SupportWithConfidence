CREATE TABLE [dbo].[ProviderCategory] (
    [Id]         BIGINT NOT NULL,
    [FlareId]    BIGINT NOT NULL,
    [CategoryId] BIGINT NOT NULL,
    CONSTRAINT [PK_ProviderCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

