# Technical Stack Management (TSM) Deployment Options

The TSM application has been prepared for deployment using the following options:

## 1. IIS Deployment (Windows Server)

**Files:**
- Published application in `./publish` directory
- Deployment guide in `deployment-guide.md`

**Advantages:**
- Traditional Windows hosting
- Integration with Windows authentication
- Familiar for Windows administrators

**Steps:**
1. Set up Windows Server with IIS
2. Install .NET 8.0 runtime
3. Deploy published files to IIS website
4. Configure database connection
5. Set up HTTPS

## 2. Docker Deployment

**Files:**
- `Dockerfile` - For containerizing the application
- `docker-compose.yml` - For orchestrating the application with its dependencies

**Advantages:**
- Portable across environments
- Consistent deployment
- Isolated from host system
- Easier scaling

**Steps:**
1. Install Docker and Docker Compose
2. Build and run using Docker Compose:
   ```
   docker-compose up -d
   ```
3. Access the application at http://localhost:8080

## 3. Azure App Service Deployment

**Files:**
- `deploy-to-azure.ps1` - PowerShell script for automated deployment

**Advantages:**
- Platform-as-a-Service (PaaS)
- Built-in scaling
- Integrated with other Azure services
- Easy CI/CD setup

**Steps:**
1. Install Azure CLI
2. Log in to Azure: `az login`
3. Run the deployment script: `./deploy-to-azure.ps1`
4. Access the application at the displayed URL

## Recommendation

Choose the deployment option based on your infrastructure requirements:

- **For traditional Windows environments**: Use IIS deployment
- **For containerized environments**: Use Docker deployment
- **For cloud-native approach**: Use Azure App Service deployment

All deployment options include proper configuration for the database, API endpoints, and authentication settings. 