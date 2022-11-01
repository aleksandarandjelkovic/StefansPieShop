CREATE TABLE [dbo].[Pies] (
    [PieId]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX)  NOT NULL,
    [ShortDescription]  NVARCHAR (MAX)  NULL,
    [LongDescription]   NVARCHAR (MAX)  NULL,
    [AllergyInformation] NVARCHAR (MAX)  NULL,
    [Price]             DECIMAL (18, 2) NOT NULL,
    [ImageUrl]          NVARCHAR (MAX)  NULL,
    [ImageThumbnailUrl] NVARCHAR (MAX)  NULL,
    [IsPieOfTheWeek]    BIT             NOT NULL,
    [InStock]           BIT             NOT NULL,
    [CategoryId]        INT             NOT NULL,
    CONSTRAINT [PK_Pies] PRIMARY KEY CLUSTERED ([PieId] ASC),
    CONSTRAINT [FK_Pies_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Pies_CategoryId]
    ON [dbo].[Pies]([CategoryId] ASC);

