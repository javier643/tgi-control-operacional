-- TGI Control Operacional - Schema Inicial
-- Base de datos: TgiControl

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TgiControl')
BEGIN
    CREATE DATABASE TgiControl;
END
GO

USE TgiControl;
GO

-- Tabla de Usuarios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Username] [nvarchar](100) NOT NULL UNIQUE,
        [Email] [nvarchar](256) NOT NULL UNIQUE,
        [FirstName] [nvarchar](100) NOT NULL,
        [LastName] [nvarchar](100) NOT NULL,
        [Role] [nvarchar](50) NOT NULL,
        [Company] [nvarchar](200) NOT NULL,
        [OperationalCenter] [nvarchar](200) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2] NULL
    );
    CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email]);
    CREATE INDEX [IX_Users_Role] ON [dbo].[Users] ([Role]);
END
GO

-- Tabla de Permisos de Trabajo
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permits]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Permits] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Number] [nvarchar](50) NOT NULL UNIQUE,
        [Description] [nvarchar](1000),
        [StartDate] [datetime2] NOT NULL,
        [EndDate] [datetime2] NOT NULL,
        [Status] [nvarchar](50) NOT NULL DEFAULT 'Draft',
        [RequestedBy] [nvarchar](200) NOT NULL,
        [ApprovedBy] [nvarchar](200),
        [CreatedAt] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2] NULL,
        [Center] [nvarchar](200) NOT NULL,
        [Area] [nvarchar](200) NOT NULL,
        [Activity] [nvarchar](500),
        [ResidualRisk] [int] DEFAULT 0,
        [HotWork] [bit] DEFAULT 0,
        [LineBreak] [bit] DEFAULT 0,
        [ConfinedSpace] [bit] DEFAULT 0,
        [Electrical] [bit] DEFAULT 0,
        [Lifting] [bit] DEFAULT 0,
        [Excavation] [bit] DEFAULT 0,
        [GasTestValid] [bit] DEFAULT 0,
        [SSTReviewComplete] [bit] DEFAULT 0,
        [SupervisorApprovalComplete] [bit] DEFAULT 0,
        [FieldValidationComplete] [bit] DEFAULT 0
    );
    CREATE INDEX [IX_Permits_Status] ON [dbo].[Permits] ([Status]);
    CREATE INDEX [IX_Permits_Center] ON [dbo].[Permits] ([Center]);
    CREATE INDEX [IX_Permits_StartDate] ON [dbo].[Permits] ([StartDate]);
END
GO

-- Tabla de Turnos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Shifts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Shifts] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Date] [date] NOT NULL,
        [ShiftType] [nvarchar](50) NOT NULL,
        [OperationalCenter] [nvarchar](100) NOT NULL,
        [Company] [nvarchar](100) NOT NULL,
        [HeadCount] [int],
        [HandoverNotes] [nvarchar](2000),
        [DeliveredBy] [nvarchar](200) NOT NULL,
        [ReceivedBy] [nvarchar](200),
        [DeliveryTime] [datetime2],
        [ReceiptTime] [datetime2],
        [CreatedAt] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] [datetime2] NULL
    );
    CREATE INDEX [IX_Shifts_Date] ON [dbo].[Shifts] ([Date]);
    CREATE INDEX [IX_Shifts_Center] ON [dbo].[Shifts] ([OperationalCenter]);
END
GO

-- Tabla de Documentos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Documents]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Documents] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [EntityType] [nvarchar](50) NOT NULL,
        [EntityId] [nvarchar](50) NOT NULL,
        [FileName] [nvarchar](500) NOT NULL,
        [ContentType] [nvarchar](100),
        [StorageUri] [nvarchar](max) NOT NULL,
        [UploadedBy] [nvarchar](256) NOT NULL,
        [UploadedAtUtc] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [Center] [nvarchar](200) NOT NULL
    );
    CREATE INDEX [IX_Documents_Entity] ON [dbo].[Documents] ([EntityType], [EntityId]);
END
GO

-- Tabla de Auditoría
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLog] (
        [Id] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Timestamp] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [User] [nvarchar](256) NOT NULL,
        [Action] [nvarchar](100) NOT NULL,
        [EntityType] [nvarchar](100) NOT NULL,
        [EntityId] [nvarchar](100) NOT NULL,
        [Details] [nvarchar](max),
        [IpAddress] [nvarchar](50)
    );
    CREATE INDEX [IX_AuditLog_Timestamp] ON [dbo].[AuditLog] ([Timestamp]);
    CREATE INDEX [IX_AuditLog_User] ON [dbo].[AuditLog] ([User]);
END
GO

-- Tabla de Condiciones Operacionales
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OperationalConditions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[OperationalConditions] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Center] [nvarchar](200) NOT NULL,
        [Area] [nvarchar](200) NOT NULL,
        [Unit] [nvarchar](200) NOT NULL,
        [Status] [nvarchar](100) NOT NULL,
        [LastUpdated] [datetime2] NOT NULL DEFAULT GETUTCDATE(),
        [Inhibitions] [nvarchar](max),
        [Isolations] [nvarchar](max),
        [UnavailableEquipment] [nvarchar](max),
        [Alarms] [nvarchar](max)
    );
    CREATE INDEX [IX_OperationalConditions_Center_Area] ON [dbo].[OperationalConditions] ([Center], [Area]);
END
GO

PRINT 'Schema TgiControl creado exitosamente';
GO