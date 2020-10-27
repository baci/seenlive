# seenlive
seenlive is single-page application that allows the user to track bands and concerts that they have been to. 

The Frontend is written in React / Typescript, the backend in ASP.NET Core and C#.

# Installation / Execution

## Requirements
The following tools are required to compile and run seenlive locally:
* NodeJS, NPM and Yarn (additional dependencies loaded from package definitions)
* .NET Core 3.1
* MongoDB Atlas M0 Cluster

## Web Server
* Compile seenlive-server\SeenLive.Server.sln
* Run SeenLive.Server project

## Web Client
Assumes that the server is running. Execute from PowerShell:
* cd seenlive
* yarn install
* yarn start
