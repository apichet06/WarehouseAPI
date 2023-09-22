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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [Approvals] (
        [ID] int NOT NULL IDENTITY,
        [ApprovalRequestID] int NOT NULL,
        [UserID] nvarchar(10) NULL,
        [ProductID] int NOT NULL,
        [Quantity] int NOT NULL,
        [RequestDate] datetime2 NOT NULL,
        [IsApproved] bit NOT NULL,
        CONSTRAINT [PK_Approvals] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [Divisions] (
        [ID] int NOT NULL IDENTITY,
        [DV_ID] nvarchar(10) NULL,
        [DV_Name] nvarchar(150) NULL,
        CONSTRAINT [PK_Divisions] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [IncomingStocks] (
        [ID] int NOT NULL IDENTITY,
        [IncomingStockID] nvarchar(10) NULL,
        [ProductID] nvarchar(10) NULL,
        [QtyReceived] int NOT NULL,
        [ReceivedDate] datetime2 NOT NULL,
        [ReceivedBy] nvarchar(10) NULL,
        CONSTRAINT [PK_IncomingStocks] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [OutgoingStocks] (
        [ID] int NOT NULL IDENTITY,
        [OutgoingStockID] nvarchar(10) NULL,
        [ProductID] nvarchar(10) NULL,
        [QTYWithdrawn] int NOT NULL,
        [UnitPrice] decimal(18,4) NOT NULL,
        [WithdrawnDate] datetime2 NOT NULL,
        [WithdrawnBy] nvarchar(10) NULL,
        CONSTRAINT [PK_OutgoingStocks] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [Positions] (
        [ID] int NOT NULL IDENTITY,
        [P_ID] nvarchar(10) NULL,
        [P_Name] nvarchar(150) NULL,
        [DV_ID] nvarchar(10) NULL,
        CONSTRAINT [PK_Positions] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [Products] (
        [ID] int NOT NULL IDENTITY,
        [ProductID] nvarchar(10) NULL,
        [ProductName] nvarchar(200) NULL,
        [ProductDescription] nvarchar(500) NULL,
        [PImages] nvarchar(500) NULL,
        [QtyMinimumStock] int NOT NULL,
        [QtyInStock] int NOT NULL,
        [UnitPrice] decimal(18,4) NOT NULL,
        [UnitOfMeasure] nvarchar(10) NULL,
        [ReceiveAt] datetime2 NULL,
        [lastAt] datetime2 NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    CREATE TABLE [Users] (
        [ID] int NOT NULL IDENTITY,
        [UserID] nvarchar(10) NULL,
        [Username] nvarchar(100) NULL,
        [Password] nvarchar(100) NULL,
        [FirstName] nvarchar(100) NULL,
        [LastName] nvarchar(100) NULL,
        [DV_ID] nvarchar(10) NULL,
        [P_ID] nvarchar(10) NULL,
        [Status] nvarchar(50) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230911021611_AddTableWHDB')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230911021611_AddTableWHDB', N'7.0.10');
END;
GO

COMMIT;
GO

