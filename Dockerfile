# Imagen base para .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 50051

# Imagen base para .NET SDK (para construir la aplicación)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["users-service.csproj", "./"]
RUN dotnet restore "./users-service.csproj"
COPY . .
RUN dotnet publish "./users-service.csproj" -c Release -o /app/publish

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "users-service.dll"]
