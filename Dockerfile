FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Simpli.SearchPortal.Api/Simpli.SearchPortal.Api.csproj", "Services/Simpli.SearchPortal.Api/"]

RUN dotnet restore "./Services/Simpli.SearchPortal.Api/Simpli.SearchPortal.Api.csproj"

COPY . .
WORKDIR "/src/Services/Simpli.SearchPortal.Api"
RUN pwd
RUN ls
RUN dotnet build "./Simpli.SearchPortal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN ls -l /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
run pwd
run ls
RUN dotnet publish "./Simpli.SearchPortal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN ls -l /app/publish
# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT ["dotnet", "Simpli.SearchPortal.Api.dll"]