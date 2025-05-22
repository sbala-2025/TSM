# Technical Stack Management (TSM) System

A comprehensive solution for managing technical stacks, projects, team members, and skills across your organization.

## Overview

The Technical Stack Management (TSM) system helps organizations track and manage:

- Technologies used across projects
- Project technical stacks
- Team member skills and proficiencies
- Technology adoption status

## Architecture

The system follows a clean architecture approach:

- **TSM.Core**: Contains domain models and interfaces
- **TSM.Infrastructure**: Contains data access implementation
- **TSM.API**: API controllers and application logic

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server
- **Authentication**: JWT Token
- **API Documentation**: Swagger/OpenAPI
- **ORM**: Entity Framework Core

## Features

- Manage technologies, categories, and projects
- Track team members and their technical skills
- Associate technologies with projects
- Assign team members to projects
- Track skill proficiency levels
- JWT-based authentication and authorization

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 or Visual Studio Code

### Development Setup

1. Clone the repository
2. Update the connection string in `BackEnd/TSM.API/appsettings.json`
3. Navigate to the API project folder:
   ```
   cd BackEnd/TSM.API
   ```
4. Run the application:
   ```
   dotnet run
   ```
5. Access Swagger at: `http://localhost:5196/swagger`

### Database Setup

The database schema is created automatically when the application runs. Alternatively, you can run the SQL scripts in `database-scripts.sql`.

## API Documentation

The API is documented using Swagger/OpenAPI and is available at `/swagger` when the application is running.

Key endpoints:

- `/api/Projects` - Manage projects
- `/api/TeamMembers` - Manage team members
- `/api/Technologies` - Manage technologies

## Deployment

Multiple deployment options are available:

- **IIS Deployment**: See `deployment-guide.md`
- **Docker Deployment**: Use `Dockerfile` and `docker-compose.yml`
- **Azure App Service**: Use `deploy-to-azure.ps1`

For more details, see `deployment-options.md`.

## Roadmap

- Frontend UI development
- Enhanced reporting features
- Integration with external systems
- Mobile application

## License

This project is licensed under the MIT License - see the LICENSE file for details. 