#!/bin/bash

# Exit on error
set -e

# Configuration - replace with your values
ACR_NAME="yourregistryname"
IMAGE_NAME="webbuilder-api"
APP_NAME="webbuilder-api"
RESOURCE_GROUP="your-resource-group"
LOCATION="eastus"
APP_SERVICE_PLAN="webbuilder-plan"

# Log in to Azure (comment out if already logged in)
# az login

# Navigate to project directory
cd "$(dirname "$0")/.."

# Build the Docker image
docker build -t $IMAGE_NAME:latest .

# Log in to Azure Container Registry
az acr login --name $ACR_NAME

# Tag the image for ACR
docker tag $IMAGE_NAME:latest $ACR_NAME.azurecr.io/$IMAGE_NAME:latest

# Push the image to ACR
docker push $ACR_NAME.azurecr.io/$IMAGE_NAME:latest

# Create resource group if it doesn't exist
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create App Service Plan if it doesn't exist
az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --is-linux --sku B1

# Create or update the Web App
if az webapp show --name $APP_NAME --resource-group $RESOURCE_GROUP &>/dev/null; then
    echo "Updating existing Web App..."
else
    echo "Creating new Web App..."
    az webapp create --name $APP_NAME \
                     --resource-group $RESOURCE_GROUP \
                     --plan $APP_SERVICE_PLAN \
                     --deployment-container-image-name $ACR_NAME.azurecr.io/$IMAGE_NAME:latest
fi

# Configure the Web App to use the container image
az webapp config container set --name $APP_NAME \
                               --resource-group $RESOURCE_GROUP \
                               --docker-custom-image-name $ACR_NAME.azurecr.io/$IMAGE_NAME:latest \
                               --docker-registry-server-url https://$ACR_NAME.azurecr.io

# Configure environment variables
az webapp config appsettings set --name $APP_NAME \
                                --resource-group $RESOURCE_GROUP \
                                --settings ASPNETCORE_ENVIRONMENT=Production

echo "Deployment complete!"
echo "Your API is available at: https://$APP_NAME.azurewebsites.net"
