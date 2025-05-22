-- Technical Stack Management Database Schema

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TechnicalStackManagement')
BEGIN
    CREATE DATABASE TechnicalStackManagement;
END
GO

USE TechnicalStackManagement;
GO

-- Categories Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL
    );
END
GO

-- Technologies Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Technologies')
BEGIN
    CREATE TABLE Technologies (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        CategoryId INT NOT NULL,
        Description NVARCHAR(500) NULL,
        Version NVARCHAR(50) NULL,
        CONSTRAINT FK_Technologies_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
    );
END
GO

-- Teams Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Teams')
BEGIN
    CREATE TABLE Teams (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        Department NVARCHAR(100) NULL
    );
END
GO

-- Status Enum Values Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StatusTypes')
BEGIN
    CREATE TABLE StatusTypes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL
    );

    -- Insert default status types
    INSERT INTO StatusTypes (Name) VALUES 
        ('Available'),
        ('In-Use'),
        ('Deprecated'),
        ('Planned');
END
GO

-- Technology Status (Junction Table with metadata)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TechnologyStatus')
BEGIN
    CREATE TABLE TechnologyStatus (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TeamId INT NOT NULL,
        TechnologyId INT NOT NULL,
        StatusId INT NOT NULL,
        UserId NVARCHAR(450) NULL,
        LastUpdated DATETIME DEFAULT GETDATE(),
        Comments NVARCHAR(1000) NULL,
        UpdatedBy NVARCHAR(100) NULL,
        CONSTRAINT FK_TechnologyStatus_Teams FOREIGN KEY (TeamId) REFERENCES Teams(Id),
        CONSTRAINT FK_TechnologyStatus_Technologies FOREIGN KEY (TechnologyId) REFERENCES Technologies(Id),
        CONSTRAINT FK_TechnologyStatus_StatusTypes FOREIGN KEY (StatusId) REFERENCES StatusTypes(Id),
        CONSTRAINT FK_TechnologyStatus_Users FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
        CONSTRAINT UQ_TeamTechnology UNIQUE (TeamId, TechnologyId)
    );
END
GO

-- Users Table for Authentication
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        Email NVARCHAR(255) NOT NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        FirstName NVARCHAR(100) NULL,
        LastName NVARCHAR(100) NULL,
        TeamId INT NULL,
        IsAdmin BIT DEFAULT 0,
        CreatedAt DATETIME DEFAULT GETDATE(),
        LastLogin DATETIME NULL,
        CONSTRAINT UQ_Users_Username UNIQUE (Username),
        CONSTRAINT UQ_Users_Email UNIQUE (Email),
        CONSTRAINT FK_Users_Teams FOREIGN KEY (TeamId) REFERENCES Teams(Id)
    );
END
GO

-- Seed Data for Categories
IF NOT EXISTS (SELECT TOP 1 * FROM Categories)
BEGIN
    INSERT INTO Categories (Name, Description) VALUES 
        ('Frameworks', 'Software frameworks used for application development'),
        ('Platforms', 'Operating systems and runtime environments'),
        ('Programming Languages', 'Development languages used'),
        ('Tools', 'Development and operational tools'),
        ('Databases', 'Database technologies and systems'),
        ('Cloud Services', 'Cloud-based services and platforms');
END
GO 