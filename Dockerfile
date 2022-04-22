FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /
COPY [".", "."]
RUN dotnet restore "./DockerDemo.API/DockerDemo.API.csproj"
COPY . /build
WORKDIR /build
RUN dotnet build "./DockerDemo.API/DockerDemo.API.csproj" -c Release -o /app

# Publish 
FROM build AS publish
RUN dotnet publish "./DockerDemo.API/DockerDemo.API.csproj" -c Release -o /app

# Deployment
FROM build AS final
WORKDIR /app

COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerDemo.API.dll"]
