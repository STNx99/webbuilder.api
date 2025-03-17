#!/bin/bash

# This script provides an easier way to work with Docker for local development
# Use this script instead of manually typing docker commands

# Exit on error
set -e

function show_help {
  echo "Usage: ./local-dev.sh [COMMAND]"
  echo ""
  echo "Commands:"
  echo "  build       Build the Docker image"
  echo "  start       Start the container"
  echo "  stop        Stop the container"
  echo "  logs        Show container logs"
  echo "  shell       Open a shell inside the container"
  echo "  compose     Run docker-compose up"
  echo "  help        Show this help message"
}

case "$1" in
  build)
    echo "Building Docker image..."
    docker build -t webbuilder-api .
    ;;
  start)
    echo "Starting container..."
    docker run -d \
      --name webbuilder-api \
      -p 5000:80 \
      -e "ASPNETCORE_ENVIRONMENT=Development" \
      -e "ASPNETCORE_URLS=http://+:80" \
      -e "ConnectionStrings__DefaultConnection=Host=ep-broad-breeze-a1rz6l8d-pooler.ap-southeast-1.aws.neon.tech;Database=websitebuilder;Username=websitebuilder_owner;Password=npg_WRQ1nYC4yIoP" \
      webbuilder-api
    echo "Container started! API available at http://localhost:5000"
    ;;
  stop)
    echo "Stopping container..."
    docker stop webbuilder-api
    docker rm webbuilder-api
    ;;
  logs)
    docker logs -f webbuilder-api
    ;;
  shell)
    docker exec -it webbuilder-api /bin/bash
    ;;
  compose)
    docker-compose up -d
    echo "Services started with docker-compose"
    ;;
  *)
    show_help
    ;;
esac
