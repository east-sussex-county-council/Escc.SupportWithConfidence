CREATE TABLE [dbo].[PostcodeUsage] (
    [Count]    INT      NOT NULL,
    [DateUsed] DATETIME DEFAULT (getdate()) NOT NULL
);

