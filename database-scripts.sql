-- Technical Stack Management Database Tables

-- Users table for authentication and user management
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);

-- Technologies table to store tech stack components
CREATE TABLE Technologies (
    TechnologyId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Category NVARCHAR(50) NOT NULL, -- e.g., Frontend, Backend, Database, DevOps
    Description NVARCHAR(MAX),
    Version NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Projects table
-- CREATE TABLE Projects (
--     ProjectId INT IDENTITY(1,1) PRIMARY KEY,
--     Name NVARCHAR(100) NOT NULL,
--     Description NVARCHAR(MAX),
--     StartDate DATETIME,
--     EndDate DATETIME NULL,
--     Status NVARCHAR(20) DEFAULT 'Active', -- Active, Completed, On Hold, Canceled
--     CreatedAt DATETIME DEFAULT GETDATE(),
--     UpdatedAt DATETIME DEFAULT GETDATE()
-- );

-- ProjectTechnologies junction table for many-to-many relationship
CREATE TABLE ProjectTechnologies (
    ProjectId INT NOT NULL,
    TechnologyId INT NOT NULL,
    UsageDetails NVARCHAR(MAX),
    AddedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (ProjectId, TechnologyId),
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
    FOREIGN KEY (TechnologyId) REFERENCES Technologies(TechnologyId)
);

-- TeamMembers table
CREATE TABLE TeamMembers (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    Name NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100),
    Email NVARCHAR(100),
    IsExternal BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- ProjectTeamMembers junction table
CREATE TABLE ProjectTeamMembers (
    ProjectId INT NOT NULL,
    MemberId INT NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    JoinedAt DATETIME DEFAULT GETDATE(),
    LeftAt DATETIME NULL,
    PRIMARY KEY (ProjectId, MemberId),
    FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
    FOREIGN KEY (MemberId) REFERENCES TeamMembers(MemberId)
);

-- TechnologySkills table to track team member skills
CREATE TABLE TechnologySkills (
    SkillId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    TechnologyId INT NOT NULL,
    ProficiencyLevel INT NOT NULL, -- 1-5 scale
    YearsOfExperience DECIMAL(5,2),
    LastUsed DATE,
    FOREIGN KEY (MemberId) REFERENCES TeamMembers(MemberId),
    FOREIGN KEY (TechnologyId) REFERENCES Technologies(TechnologyId)
);

-- Initial seed data for Technologies
INSERT INTO Technologies (Name, Category, Description, Version)
VALUES 
('React', 'Frontend', 'A JavaScript library for building user interfaces', '18.2.0'),
('Angular', 'Frontend', 'Platform for building mobile and desktop web applications', '16.0.0'),
('ASP.NET Core', 'Backend', 'Cross-platform, high-performance framework for building modern, cloud-based applications', '7.0'),
('Node.js', 'Backend', 'JavaScript runtime built on Chrome''s V8 JavaScript engine', '18.12.0'),
('SQL Server', 'Database', 'Microsoft''s relational database management system', '2022'),
('MongoDB', 'Database', 'NoSQL database program', '6.0'),
('Docker', 'DevOps', 'Platform for developing, shipping, and running applications', '24.0.2'),
('Kubernetes', 'DevOps', 'Container orchestration system', '1.26.0'); 