-- Seed Data Script for Technical Stack Management
USE TechnicalStackManagement;
GO

-- Seed Teams
IF NOT EXISTS (SELECT TOP 1 * FROM Teams)
BEGIN
    INSERT INTO Teams (Name, Description, Department) VALUES 
        ('Frontend Team', 'Web UI development team', 'Engineering'),
        ('Backend Team', 'API and service development team', 'Engineering'),
        ('DevOps Team', 'Infrastructure and deployment team', 'Operations'),
        ('Mobile Team', 'Mobile app development team', 'Engineering'),
        ('Data Science Team', 'Analytics and ML team', 'Data');
END
GO

-- Seed Technologies
IF NOT EXISTS (SELECT TOP 1 * FROM Technologies)
BEGIN
    -- Get Category IDs
    DECLARE @FrameworksId INT = (SELECT Id FROM Categories WHERE Name = 'Frameworks');
    DECLARE @PlatformsId INT = (SELECT Id FROM Categories WHERE Name = 'Platforms');
    DECLARE @LanguagesId INT = (SELECT Id FROM Categories WHERE Name = 'Programming Languages');
    DECLARE @ToolsId INT = (SELECT Id FROM Categories WHERE Name = 'Tools');
    DECLARE @DatabasesId INT = (SELECT Id FROM Categories WHERE Name = 'Databases');
    DECLARE @CloudId INT = (SELECT Id FROM Categories WHERE Name = 'Cloud Services');

    -- Insert Technologies
    INSERT INTO Technologies (Name, CategoryId, Description, Version) VALUES 
        -- Frameworks
        ('Angular', @FrameworksId, 'Frontend JavaScript framework by Google', '16.x'),
        ('React', @FrameworksId, 'Frontend JavaScript library by Facebook', '18.x'),
        ('ASP.NET Core', @FrameworksId, 'Web framework by Microsoft', '8.0'),
        ('Django', @FrameworksId, 'Python web framework', '4.2'),
        ('Flutter', @FrameworksId, 'UI toolkit for building cross-platform apps', '3.0'),
        
        -- Platforms
        ('Windows Server', @PlatformsId, 'Microsoft server OS', '2022'),
        ('Linux', @PlatformsId, 'Open source operating system', 'Ubuntu 22.04'),
        ('Docker', @PlatformsId, 'Containerization platform', 'Latest'),
        ('Kubernetes', @PlatformsId, 'Container orchestration platform', 'Latest'),
        
        -- Programming Languages
        ('C#', @LanguagesId, '.NET programming language', '12.0'),
        ('TypeScript', @LanguagesId, 'Typed JavaScript', '5.0'),
        ('Python', @LanguagesId, 'General purpose programming language', '3.12'),
        ('JavaScript', @LanguagesId, 'Web scripting language', 'ES2022'),
        ('Java', @LanguagesId, 'Object-oriented programming language', '21'),
        
        -- Tools
        ('Visual Studio', @ToolsId, 'Microsoft IDE', '2022'),
        ('VS Code', @ToolsId, 'Lightweight code editor', 'Latest'),
        ('Git', @ToolsId, 'Version control system', 'Latest'),
        ('Jenkins', @ToolsId, 'CI/CD tool', 'Latest'),
        ('GitHub Actions', @ToolsId, 'CI/CD platform', 'Latest'),
        
        -- Databases
        ('SQL Server', @DatabasesId, 'Microsoft relational database', '2022'),
        ('PostgreSQL', @DatabasesId, 'Open source relational database', '15'),
        ('MongoDB', @DatabasesId, 'NoSQL document database', '6.0'),
        ('Redis', @DatabasesId, 'In-memory data store', 'Latest'),
        
        -- Cloud Services
        ('Azure', @CloudId, 'Microsoft cloud platform', 'Latest'),
        ('AWS', @CloudId, 'Amazon cloud platform', 'Latest'),
        ('Google Cloud', @CloudId, 'Google cloud platform', 'Latest'),
        ('Azure DevOps', @CloudId, 'Microsoft DevOps platform', 'Latest');
END
GO

-- Seed TechnologyStatus
IF NOT EXISTS (SELECT TOP 1 * FROM TechnologyStatus)
BEGIN
    -- Get Team IDs
    DECLARE @FrontendTeamId INT = (SELECT Id FROM Teams WHERE Name = 'Frontend Team');
    DECLARE @BackendTeamId INT = (SELECT Id FROM Teams WHERE Name = 'Backend Team');
    DECLARE @DevOpsTeamId INT = (SELECT Id FROM Teams WHERE Name = 'DevOps Team');
    DECLARE @MobileTeamId INT = (SELECT Id FROM Teams WHERE Name = 'Mobile Team');
    DECLARE @DataScienceTeamId INT = (SELECT Id FROM Teams WHERE Name = 'Data Science Team');
    
    -- Get Status IDs
    DECLARE @AvailableId INT = (SELECT Id FROM StatusTypes WHERE Name = 'Available');
    DECLARE @InUseId INT = (SELECT Id FROM StatusTypes WHERE Name = 'In-Use');
    DECLARE @DeprecatedId INT = (SELECT Id FROM StatusTypes WHERE Name = 'Deprecated');
    DECLARE @PlannedId INT = (SELECT Id FROM StatusTypes WHERE Name = 'Planned');
    
    -- Insert TechnologyStatus
    -- Frontend Team
    INSERT INTO TechnologyStatus (TeamId, TechnologyId, StatusId, Comments, UpdatedBy) VALUES
        (@FrontendTeamId, (SELECT Id FROM Technologies WHERE Name = 'Angular'), @InUseId, 'Main framework for our applications', 'System'),
        (@FrontendTeamId, (SELECT Id FROM Technologies WHERE Name = 'React'), @AvailableId, 'Used for specific microsites', 'System'),
        (@FrontendTeamId, (SELECT Id FROM Technologies WHERE Name = 'TypeScript'), @InUseId, 'Used with Angular', 'System'),
        (@FrontendTeamId, (SELECT Id FROM Technologies WHERE Name = 'JavaScript'), @InUseId, 'Core language', 'System'),
        (@FrontendTeamId, (SELECT Id FROM Technologies WHERE Name = 'VS Code'), @InUseId, 'Main IDE', 'System');
        
    -- Backend Team
    INSERT INTO TechnologyStatus (TeamId, TechnologyId, StatusId, Comments, UpdatedBy) VALUES
        (@BackendTeamId, (SELECT Id FROM Technologies WHERE Name = 'ASP.NET Core'), @InUseId, 'Primary API framework', 'System'),
        (@BackendTeamId, (SELECT Id FROM Technologies WHERE Name = 'C#'), @InUseId, 'Primary language', 'System'),
        (@BackendTeamId, (SELECT Id FROM Technologies WHERE Name = 'SQL Server'), @InUseId, 'Primary database', 'System'),
        (@BackendTeamId, (SELECT Id FROM Technologies WHERE Name = 'MongoDB'), @PlannedId, 'Evaluating for specific use cases', 'System'),
        (@BackendTeamId, (SELECT Id FROM Technologies WHERE Name = 'Visual Studio'), @InUseId, 'Main IDE', 'System');
        
    -- DevOps Team
    INSERT INTO TechnologyStatus (TeamId, TechnologyId, StatusId, Comments, UpdatedBy) VALUES
        (@DevOpsTeamId, (SELECT Id FROM Technologies WHERE Name = 'Docker'), @InUseId, 'Container platform', 'System'),
        (@DevOpsTeamId, (SELECT Id FROM Technologies WHERE Name = 'Kubernetes'), @InUseId, 'Container orchestration', 'System'),
        (@DevOpsTeamId, (SELECT Id FROM Technologies WHERE Name = 'Jenkins'), @DeprecatedId, 'Moving to GitHub Actions', 'System'),
        (@DevOpsTeamId, (SELECT Id FROM Technologies WHERE Name = 'GitHub Actions'), @InUseId, 'Primary CI/CD', 'System'),
        (@DevOpsTeamId, (SELECT Id FROM Technologies WHERE Name = 'Azure'), @InUseId, 'Primary cloud platform', 'System');
        
    -- Mobile Team
    INSERT INTO TechnologyStatus (TeamId, TechnologyId, StatusId, Comments, UpdatedBy) VALUES
        (@MobileTeamId, (SELECT Id FROM Technologies WHERE Name = 'Flutter'), @InUseId, 'Used for cross-platform development', 'System'),
        (@MobileTeamId, (SELECT Id FROM Technologies WHERE Name = 'React'), @DeprecatedId, 'Previously used React Native', 'System');
        
    -- Data Science Team
    INSERT INTO TechnologyStatus (TeamId, TechnologyId, StatusId, Comments, UpdatedBy) VALUES
        (@DataScienceTeamId, (SELECT Id FROM Technologies WHERE Name = 'Python'), @InUseId, 'Primary language', 'System'),
        (@DataScienceTeamId, (SELECT Id FROM Technologies WHERE Name = 'PostgreSQL'), @InUseId, 'Used for data storage', 'System');
END
GO

-- Create an Admin User (password: Admin123!)
IF NOT EXISTS (SELECT TOP 1 * FROM Users)
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, IsAdmin)
    VALUES ('admin', 'admin@tsm.com', 'AQAAAAIAAYagAAAAEAzX9Mj/uw7DQJ94R/qZPKUCEXSgXKUKOv0hOG4KOZvL6TG3dtKu2WGqxZ0/8iRSQw==', 'Admin', 'User', 1);
END
GO 