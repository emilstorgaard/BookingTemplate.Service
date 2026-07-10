# Base stage - Use .NET 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copy .csproj file/s
COPY ["src/BookingTemplate.Service.Api.csproj", "src/BookingTemplate.Service.Api"]

# 2. Restore
RUN dotnet restore "./BookingTemplate.Service.Api.csproj"

# 3. Copy the rest of the code
COPY . .

# 4. Build the API
WORKDIR "/src/src/BookingTemplate.Service.Api"
RUN dotnet build "BookingTemplate.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookingTemplate.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
# Copy the published files
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BookingTemplate.Service.Api.dll"]