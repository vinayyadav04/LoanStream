USE [VINAY];
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Leads]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Leads] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(200) NULL,
        [Phone] NVARCHAR(20) NOT NULL,
        [EmploymentType] NVARCHAR(100) NULL,
        [MonthlyIncome] NVARCHAR(100) NULL,
        [LoanAmount] DECIMAL(12,2) NOT NULL,
        [City] NVARCHAR(100) NULL,
        [Source] NVARCHAR(100) NULL,
        [CreatedDate] DATETIME2 NOT NULL,
        [Status] NVARCHAR(50) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contacts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Contacts] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(200) NOT NULL,
        [Phone] NVARCHAR(20) NOT NULL,
        [Topic] NVARCHAR(150) NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [CreatedDate] DATETIME2 NOT NULL
    );
END
GO
