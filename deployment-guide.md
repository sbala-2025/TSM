# Technical Stack Management (TSM) Application Deployment Guide

This guide outlines the steps to deploy the TSM application to a production environment.

## Prerequisites

- Windows Server with IIS installed
- .NET 8.0 Runtime installed
- SQL Server instance
- URL/domain for the application

## Deployment Steps

### 1. Database Setup

1. Ensure your SQL Server instance is accessible from the deployment server
2. Update the connection string in `appsettings.json` file with production database details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_PRODUCTION_SERVER;Database=TechnicalStackManagement;User ID=YOUR_PROD_USER;Password=YOUR_PROD_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

3. If needed, run database migrations to create schema in the production database

### 2. Publish the Application

The application has been published to the `./publish` directory. This contains all the files needed to run the application.

### 3. IIS Setup

1. Install the .NET Core Hosting Bundle on the server
2. Open IIS Manager and create a new website or application
3. Set the physical path to the published application folder
4. Configure the application pool:
   - Set .NET CLR version to "No Managed Code"
   - Enable 32-bit applications: False
5. Configure binding with your domain/hostname
6. Set appropriate permissions for the application folder:
   - IIS_IUSRS needs read & execute permissions
   - Application Pool identity needs read & execute permissions

### 4. Configure HTTPS

1. Obtain an SSL certificate for your domain
2. Bind the certificate to your IIS site
3. Enable HTTPS redirects in the application

### 5. Environment Variables

Set the following environment variables on the server:
- ASPNETCORE_ENVIRONMENT=Production

### 6. Application Settings

Ensure the following are properly configured in production:
- JWT token settings (secret, issuer, audience)
- CORS policies
- Logging settings

### 7. Testing the Deployment

1. Access the application URL
2. Verify Swagger works at /swagger
3. Test API endpoints using Swagger or Postman
4. Check authentication and authorization

## Troubleshooting

- Check application logs at `/logs` directory
- Verify database connectivity
- Ensure all required services are running
- Check IIS logs for any HTTP errors

## Maintenance

- Set up regular database backups
- Monitor application performance
- Update SSL certificates before expiry
- Apply security patches when available 