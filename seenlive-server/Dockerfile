# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env-prod
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS run-env-prod

WORKDIR /app
COPY --from=build-env-prod /app/out .
ENTRYPOINT ["dotnet", "SeenLive.Web.dll"]


# TO SAVE PERFORMANCE IN CI PIPELINE LATER ON, BUILD THE SOLUTION FIRST AND THEN REPLACE ALL THE ABOVE WITH:
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#COPY bin/Release/netcoreapp3.1/publish/ App/
#WORKDIR /App
#ENTRYPOINT ["dotnet", "SeenLive.Web.dll"]