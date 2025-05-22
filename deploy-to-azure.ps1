# Azure App Service Deployment Script for TSM API
# Prerequisites: Azure CLI installed and logged in

# Configuration
$resourceGroupName = "TSM-Resources"
$location = "eastus"
$appServicePlanName = "TSM-AppServicePlan"
$appName = "TSM-API"
$sqlServerName = "tsm-sqlserver"
$sqlDatabaseName = "TechnicalStackManagement"
$sqlAdminUser = "sqladmin"
$sqlAdminPassword = "YourStrongPassword123!"  # Consider using secure input in production
$publishFolder = "./publish"

# Create Resource Group
Write-Host "Creating Resource Group..."
az group create --name $resourceGroupName --location $location

# Create App Service Plan
Write-Host "Creating App Service Plan..."
az appservice plan create --name $appServicePlanName --resource-group $resourceGroupName --sku B1

# Create Web App
Write-Host "Creating Web App..."
az webapp create --name $appName --resource-group $resourceGroupName --plan $appServicePlanName --runtime "DOTNET|8.0"

# Create SQL Server
Write-Host "Creating SQL Server..."
az sql server create --name $sqlServerName --resource-group $resourceGroupName --location $location --admin-user $sqlAdminUser --admin-password $sqlAdminPassword

# Configure Firewall to allow Azure services
Write-Host "Configuring SQL Server Firewall..."
az sql server firewall-rule create --resource-group $resourceGroupName --server $sqlServerName --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Create Database
Write-Host "Creating SQL Database..."
az sql db create --resource-group $resourceGroupName --server $sqlServerName --name $sqlDatabaseName --service-objective S0

# Get Connection String
$connectionString = "Server=tcp:$sqlServerName.database.windows.net,1433;Database=$sqlDatabaseName;User ID=$sqlAdminUser;Password=$sqlAdminPassword;Encrypt=true;Connection Timeout=30;"

# Set Connection String in App
Write-Host "Setting Connection String in Web App..."
az webapp config connection-string set --resource-group $resourceGroupName --name $appName --settings DefaultConnection="$connectionString" --connection-string-type SQLAzure

# Set App Settings
Write-Host "Setting App Settings..."
az webapp config appsettings set --resource-group $resourceGroupName --name $appName --settings ASPNETCORE_ENVIRONMENT="Production"

# Deploy application
Write-Host "Deploying application..."
az webapp deployment source config-zip --resource-group $resourceGroupName --name $appName --src "$publishFolder"

Write-Host "Deployment completed successfully!"
Write-Host "Your app is available at: https://$appName.azurewebsites.net" 