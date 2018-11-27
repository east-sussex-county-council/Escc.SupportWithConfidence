CREATE TABLE [dbo].[ProviderExtra] (
    [Id]            BIGINT IDENTITY (1, 1) NOT NULL,
    [FlareId]       BIGINT NOT NULL,
    [PhotographId]  INT    NULL,
    [Experience]    TEXT   NULL,
    [Background]    TEXT   NULL,
    [Expertise]     TEXT   NULL,
    [Accreditation] TEXT   NULL,
    [Services]      TEXT   NULL,
    [Costs]         TEXT   NULL,
    [Crb]           TEXT   NULL,
    [IsDeleted]     BIT    CONSTRAINT [DF__ProviderE__IsDel__3D5E1FD2] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ProviderExtra] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ProviderExtra_ImageData] FOREIGN KEY ([PhotographId]) REFERENCES [dbo].[ImageData] ([FileDataID])
);

