# Docker Deployment Guide for .NET Applications

This guide covers deployment of your webbuilder.api application to Docker.

## Prerequisites

- Docker installed on your development machine
- .NET SDK for local development
- Access to a container registry (Docker Hub, Azure Container Registry, etc.) for production deployments

## 1. Build Your Docker Image

Navigate to your project directory and build the Docker image:

```bash
cd /home/ghug/Code/csharp/webbuilder.api
docker build -t webbuilder-api .
```

## 2. Run Locally

Run your container locally to verify everything works:

```bash
docker run -d -p 5000:80 -p 5001:443 --name webbuilder-api-container webbuilder-api
```

Test that your API is accessible:

```bash
curl http://localhost:5000/api/v1.0/projects
```

## 3. Using Docker Compose

For a more complete environment with multiple services:

```bash
docker compose up -d
```

## 4. Push to a Container Registry

### Docker Hub

```bash
# Login to Docker Hub
docker login

# Tag your image
docker tag webbuilder-api yourusername/webbuilder-api:latest

# Push to Docker Hub
docker push yourusername/webbuilder-api:latest
```

### Azure Container Registry

```bash
# Login to Azure
az login

# Login to your ACR
az acr login --name YourRegistryName

# Tag your image
docker tag webbuilder-api yourregistry.azurecr.io/webbuilder-api:latest

# Push to ACR
docker push yourregistry.azurecr.io/webbuilder-api:latest
```

## 5. Production Deployment Options

### Azure App Service

```bash
# Deploy to Azure App Service
az webapp config container set --name YourAppName --resource-group YourResourceGroup --docker-custom-image-name yourregistry.azurecr.io/webbuilder-api:latest
```

### Azure Container Instances

```bash
az container create \
  --resource-group YourResourceGroup \
  --name webbuilder-container \
  --image yourregistry.azurecr.io/webbuilder-api:latest \
  --dns-name-label webbuilder-api \
  --ports 80
```

### Kubernetes Deployment

Create a `deployment.yaml` file:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: webbuilder-api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: webbuilder-api
  template:
    metadata:
      labels:
        app: webbuilder-api
    spec:
      containers:
        - name: webbuilder-api
          image: yourregistry.azurecr.io/webbuilder-api:latest
          ports:
            - containerPort: 80
---
apiVersion: v1
kind: Service
metadata:
  name: webbuilder-api-service
spec:
  selector:
    app: webbuilder-api
  ports:
    - port: 80
      targetPort: 80
  type: LoadBalancer
```

Apply to your cluster:

```bash
kubectl apply -f deployment.yaml
```

## 6. Environment Configuration

For different environments, consider:

- Using environment variables for configuration
- Using Docker secrets for sensitive data
- Mounting configuration files as volumes

## 7. Monitoring and Maintenance

### View container logs

```bash
docker logs webbuilder-api-container
```

### Enter the container for troubleshooting

```bash
docker exec -it webbuilder-api-container bash
```

### Update your container

```bash
# Pull the latest image
docker pull yourregistry.azurecr.io/webbuilder-api:latest

# Stop and remove the old container
docker stop webbuilder-api-container
docker rm webbuilder-api-container

# Run a new container with the updated image
docker run -d -p 5000:80 --name webbuilder-api-container yourregistry.azurecr.io/webbuilder-api:latest
```
