CREATE TABLE [dbo].[Audit] (
    [Id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [Task]    VARCHAR (500) NOT NULL,
    [Success] BIT           NOT NULL,
    [Date]    DATETIME      DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Audit] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

