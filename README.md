![Frontend CI](https://github.com/baci/seenlive/workflows/Node.js%20CI/badge.svg?branch=master) ![Backend CI](https://github.com/baci/seenlive/workflows/.NET-Backend/badge.svg)

# seenlive
seenlive is single-page application that allows the user to track bands and concerts that they have been to. 

The Frontend is written in React / Typescript, the backend in ASP.NET Core and C#, using MongoDB for data storage.

# Installation / Execution

## Requirements
The following tools are required to compile and run seenlive locally:
* NodeJS, NPM and Yarn (additional dependencies loaded from package definitions)
* .NET Core 3.1
* a MongoDB M0 Cluster running on some server

## Web Server
* Execute from a terminal:
```
cd seenlive-server
dotnet restore
dotnet build --no-restore
```
* Set some user-specific environment variables for seenlive-db-server, seenlive-db-username and seenlive-db-password with your MongoDB credentials
* Run SeenLive.Server project

## Web Client
Assumes that the server is running. Execute from a terminal:
```
cd seenlive
yarn install
yarn start
```
