![Frontend CI](https://github.com/baci/seenlive/workflows/Node.js%20CI/badge.svg?branch=master) ![Backend CI](https://github.com/baci/seenlive/workflows/.NET-Backend/badge.svg) [![CC BY-NC-SA 4.0][cc-by-nc-sa-shield]][cc-by-nc-sa]

# seenlive
seenlive is single-page application that allows the user to track bands and concerts that they have been to. 

The Frontend is written in React / Typescript, the backend in ASP.NET Core and C#, using MongoDB for data storage.

# Installation / Execution

## Requirements
The following tools are required to compile and run seenlive locally:
* NodeJS, NPM and Yarn (additional dependencies loaded from package definitions)
* .NET 5.0
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


This work is licensed under a
[Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License][cc-by-nc-sa].

[![CC BY-NC-SA 4.0][cc-by-nc-sa-image]][cc-by-nc-sa]

[cc-by-nc-sa]: http://creativecommons.org/licenses/by-nc-sa/4.0/
[cc-by-nc-sa-image]: https://licensebuttons.net/l/by-nc-sa/4.0/88x31.png
[cc-by-nc-sa-shield]: https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg
