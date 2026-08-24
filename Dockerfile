# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY MobileProductAPI/MobileProductAPI.csproj MobileProductAPI/

# Restore dependencies
RUN dotnet restore MobileProductAPI/MobileProductAPI.csproj

# Copy source code
COPY MobileProductAPI/ MobileProductAPI/

# Build and publish
RUN dotnet publish MobileProductAPI/MobileProductAPI.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render provides the PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "MobileProductAPI.dll"]
