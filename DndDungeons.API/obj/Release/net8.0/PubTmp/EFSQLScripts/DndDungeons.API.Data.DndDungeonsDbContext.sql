IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE TABLE [Difficulties] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Difficulties] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE TABLE [Images] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [FileDescription] nvarchar(max) NULL,
        [FileExtension] nvarchar(max) NOT NULL,
        [FileSizeInBytes] bigint NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Images] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE TABLE [Regions] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [RegionImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE TABLE [Dungeons] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [LengthInKm] float NOT NULL,
        [DungeonImageUrl] nvarchar(max) NULL,
        [DifficultyId] uniqueidentifier NOT NULL,
        [RegionId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Dungeons] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Dungeons_Difficulties_DifficultyId] FOREIGN KEY ([DifficultyId]) REFERENCES [Difficulties] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Dungeons_Regions_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [Regions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] ON;
    EXEC(N'INSERT INTO [Difficulties] ([Id], [Name])
    VALUES (''29f006ee-4961-4de1-867d-0797eb080e91'', N''Medium''),
    (''56f83232-0bf4-447d-b7c6-2bf5a4c818a6'', N''Easy''),
    (''f28c19c1-ef2b-48f5-8741-e259080d0640'', N''Hard'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] ON;
    EXEC(N'INSERT INTO [Regions] ([Id], [Code], [Name], [RegionImageUrl])
    VALUES (''54e9293e-b843-457d-9be3-8cb95d5e86ed'', N''ETB'', N''Eltabbar'', N''https://preview.redd.it/the-city-of-eltabbar-capital-city-of-thay-at-night-with-a-v0-46wdg9cm83fa1.png?auto=webp&s=28d424c1c7e6854d87f9d7e22e652d75b6217a2a''),
    (''83b06436-c95c-400f-a8df-03037f407b42'', N''ATK'', N''Athkatla'', N''https://www.worldanvil.com/media/cache/cover/uploads/images/ac86f132ef5cffd6486891449e00a89e.webp''),
    (''9b5aaaca-4280-4fb0-a72a-2a2e460f2c79'', N''UND'', N''Menzoberranzan'', N''https://db4sgowjqfwig.cloudfront.net/campaigns/97510/assets/1064433/The_City_of_Menzoberranzan.jpg?1586500934''),
    (''a836c716-acdb-4961-b141-75f66b4703cc'', N''CLP'', N''Calimport'', N''https://db4sgowjqfwig.cloudfront.net/campaigns/230022/assets/1289938/Calimport.webp?1667532906''),
    (''ed8a75d6-28c2-409d-b77a-47a4fa72e94b'', N''BG'', N''Baldur''''s Gate'', N''https://static.wikia.nocookie.net/forgottenrealms/images/c/c4/Baldur%27s_Gate_overview_BG3.png/revision/latest?cb=20190606171350'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE INDEX [IX_Dungeons_DifficultyId] ON [Dungeons] ([DifficultyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    CREATE INDEX [IX_Dungeons_RegionId] ON [Dungeons] ([RegionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231209135622_Second Migration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231209135622_Second Migration', N'8.0.0');
END;
GO

COMMIT;
GO

