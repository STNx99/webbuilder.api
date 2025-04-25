#!/bin/bash

# Exit on error
set -e

# Configuration
IMAGE_NAME="webbuilder-api"
CONTAINER_NAME="webbuilder-api-container"
HOST_HTTP_PORT=5000
CONTAINER_HTTP_PORT=80

# Navigate to project directory
cd "$(dirname "$0")/.."
PROJECT_DIR=$(pwd)

echo "Building .NET application..."

# Optional: Restore and build locally first
# dotnet restore
# dotnet build --configuration Release

echo "Building Docker image: $IMAGE_NAME"
docker build -t $IMAGE_NAME .

# Stop and remove existing container if it exists
if [ "$(docker ps -aq -f name=$CONTAINER_NAME)" ]; then
    echo "Stopping and removing existing container..."
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
fi

echo "Running new container..."
docker run -d \
    -p $HOST_HTTP_PORT:$CONTAINER_HTTP_PORT \
    -e "ASPNETCORE_ENVIRONMENT=Development" \
    -e "ASPNETCORE_URLS=http://+:80" \
    -e "ConnectionStrings__DefaultConnection=Host=ep-broad-breeze-a1rz6l8d-pooler.ap-southeast-1.aws.neon.tech;Database=websitebuilder;Username=websitebuilder_owner;Password=npg_WRQ1nYC4yIoP" \
    --name $CONTAINER_NAME \
    $IMAGE_NAME

echo "Container is running!"
echo "API is available at: http://localhost:$HOST_HTTP_PORT"
echo "To view logs: docker logs $CONTAINER_NAME"
