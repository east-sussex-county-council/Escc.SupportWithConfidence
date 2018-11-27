CREATE TABLE [dbo].[Accreditations] (
    [AccreditationId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NOT NULL,
    [Website]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Accreditations] PRIMARY KEY CLUSTERED ([AccreditationId] ASC)
);

