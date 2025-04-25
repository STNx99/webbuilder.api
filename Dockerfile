# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["*.csproj", "./"]
RUN dotnet restore

# Copy everything else and build
COPY . .
# Perform explicit restore with the full source
RUN dotnet restore
# Build and publish without the --no-restore flag
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables to disable HTTPS requirement
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser
USER appuser

# Copy from build stage
COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD wget -qO- http://localhost/api/diagnostics/ping || exit 1

ENTRYPOINT ["dotnet", "webbuilder.api.dll"]
