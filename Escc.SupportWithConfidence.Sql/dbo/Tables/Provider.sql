﻿CREATE TABLE [dbo].[Provider] (
    [Id]              BIGINT        NOT NULL,
    [FlareId]         BIGINT        NOT NULL,
    [ProviderName]    VARCHAR (500) NOT NULL,
    [Address1]        VARCHAR (500) NULL,
    [Address2]        VARCHAR (500) NULL,
    [Address3]        VARCHAR (500) NULL,
    [Address4]        VARCHAR (500) NULL,
    [Postcode]        VARCHAR (500) NULL,
    [PublishAddress]  BIT           CONSTRAINT [DF__Provider__Publis__76CBA758] DEFAULT ((0)) NOT NULL,
    [TelephoneNumber] VARCHAR (500) NULL,
    [MobileNumber]    VARCHAR (500) NULL,
    [EmailAddress]    VARCHAR (500) NULL,
    [WebsiteAddress]  VARCHAR (500) NULL,
    [FaxNumber]       VARCHAR (500) NULL,
    [Easting]         VARCHAR (500) NULL,
    [Northing]        VARCHAR (500) NULL,
    [PublishToWeb]    BIT           CONSTRAINT [DF__Provider__Publis__77BFCB91] DEFAULT ((0)) NOT NULL,
    [Availability1]   VARCHAR (500) NULL,
    [Availability2]   VARCHAR (500) NULL,
    [Availability3]   VARCHAR (500) NULL,
    [Coverage]        VARCHAR (500) NULL,
    [Coverage2]       VARCHAR (500) NULL,
    [ContactName]     VARCHAR (500) NULL,
    [CRBCheckDate]    VARCHAR (500) NULL,
    [CQCCheckDate]    VARCHAR (500) NULL,
    [BWCFlag]         BIT           CONSTRAINT [DF__Provider__BWCFla__78B3EFCA] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Provider] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

