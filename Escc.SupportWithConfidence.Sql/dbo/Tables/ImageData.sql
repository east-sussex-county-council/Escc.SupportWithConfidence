CREATE TABLE [dbo].[ImageData] (
    [FileDataID]       INT           IDENTITY (1, 1) NOT NULL,
    [FileOriginalName] VARCHAR (255) NOT NULL,
    [FileDescription]  VARCHAR (255) NULL,
    [MIMEContentType]  VARCHAR (25)  NOT NULL,
    [FileData]         IMAGE         NOT NULL,
    [AddedBy]          VARCHAR (50)  NOT NULL,
    [AddedDate]        DATETIME      NOT NULL,
    [ModifiedBy]       VARCHAR (50)  NULL,
    [ModifiedDate]     DATETIME      NULL,
    CONSTRAINT [PK_ImageData] PRIMARY KEY CLUSTERED ([FileDataID] ASC) WITH (FILLFACTOR = 90)
);

