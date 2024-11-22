# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Services/Promotions/src/Promotions.Api/Promotions.Api.csproj", "src/Services/Promotions/src/Promotions.Api/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "src/BuildingBlocks/"]
COPY ["src/Services/EmailSender/src/EmailSender.Messages/EmailSender.Messages.csproj", "src/Services/EmailSender/src/EmailSender.Messages/"]
COPY ["src/Services/Promotions/src/Promotions/Promotions.csproj", "src/Services/Promotions/src/Promotions/"]
COPY ["src/Services/Customers/src/Customers.Messages/Customers.Messages.csproj", "src/Services/Customers/src/Customers.Messages/"]
RUN dotnet restore "./src/Services/Promotions/src/Promotions.Api/Promotions.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Promotions/src/Promotions.Api"
RUN dotnet build "./Promotions.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Promotions.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Argo.MD.Promotions.Api.dll"]