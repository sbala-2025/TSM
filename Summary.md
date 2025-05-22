# Technical Stack Management (TSM) - Project Summary

## Overview
TSM is a full-stack web application that enables organizations to manage and monitor the availability and usage of various technologies, tools, platforms, and frameworks across different teams or departments.

## Components Developed

### 1. Database
- Created SQL Server database schema in `DataBase/TSM_DatabaseSchema.sql`
- Implemented seed data script in `DataBase/SeedData.sql`
- Designed tables for:
  - Categories
  - Technologies
  - Teams
  - Technology Status
  - Users (Authentication)
  - Status Types

### 2. Backend (ASP.NET Core Web API)
- Implemented a clean architecture with:
  - Core project for domain models and interfaces
  - Infrastructure project for data access
  - API project for controllers and services
- Created controllers for:
  - Authentication
  - Categories
  - Technologies
  - Teams
  - Technology Status
  - Users
- Implemented JWT authentication
- Set up dependency injection
- Added database initialization and seeding
- Configured CORS for cross-origin requests

### 3. Frontend (Angular)
- Created core models matching the API DTOs
- Implemented services for API communication
- Designed components for:
  - Navigation bar
  - Login
  - Dashboard
- Added JWT interceptor for authentication
- Set up error handling
- Configured routing with authentication guards
- Applied Bootstrap styling

## Features Implemented

### Admin Features
- Add/Edit/Delete technologies
- Define categories
- Manage team/department lists
- User management

### Team/Department Features
- Mark technologies as available, in-use, or deprecated
- Add comments or provide feedback on usage

### Monitoring and Reporting
- Dashboard with filters (team, category, status)
- Availability matrix (teams × technologies)

## How to Run

1. Database Setup:
   - Execute the SQL scripts in the `DataBase` folder

2. Backend Setup:
   - Navigate to the `BackEnd` folder
   - Run `dotnet run --project TSM.API`

3. Frontend Setup:
   - Navigate to the `FrontEnd/tsm-app` folder
   - Run `npm install`
   - Run `ng serve`

4. Access the application at http://localhost:4200
   - Login with the default admin account:
     - Username: admin
     - Password: Admin123!

## Next Steps for Completion

1. Complete the Angular components for:
   - Technology management
   - Team management
   - Category management
   - User profile and management
   - Technology status detail view

2. Add unit tests for both the backend and frontend

3. Implement export functionality for reports

4. Add more robust error handling and validation

5. Enhance the UI with additional visualizations 