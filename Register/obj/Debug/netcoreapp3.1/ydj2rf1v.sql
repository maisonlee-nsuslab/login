IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Info] (
    [Id] int NOT NULL IDENTITY,
    [userId] nvarchar(max) NULL,
    [userName] nvarchar(max) NULL,
    [userPassword] nvarchar(max) NULL,
    CONSTRAINT [PK_Info] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200715022732_info', N'3.1.6');

GO

ALTER TABLE [Info] ADD [checkPassword] nvarchar(8) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200721031947_infoCheck', N'3.1.6');

GO

