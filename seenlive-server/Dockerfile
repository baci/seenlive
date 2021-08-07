# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env-prod
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Copy csprojs and restore as distinct layers
COPY SeenLive.Server.sln ./
COPY SeenLive.DataAccess/*.csproj ./SeenLive.DataAccess/
COPY SeenLive.Server/*.csproj ./SeenLive.Server/
RUN dotnet restore ./SeenLive.Server.sln

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env-prod /app/out .
ENTRYPOINT ["dotnet", "SeenLive.Server.dll"]


# TO SAVE PERFORMANCE IN CI PIPELINE LATER ON, BUILD THE SOLUTION FIRST AND THEN REPLACE ALL THE ABOVE WITH:
#FROM mcr.microsoft.com/dotnet/aspnet:5.0
#COPY bin/Release/netcoreapp3.1/publish/ App/
#WORKDIR /App
#ENTRYPOINT ["dotnet", "SeenLive.Server.dll"]