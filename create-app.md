# How was this app created?

The following commands were used for initializing the codebase.

For a list of dotnet commands, see the [MS documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet)

## Create the solution folder
Create the folder, some config files, and initialize a new git repository.

`mkdir intouch`

`cd intouch`

`dotnet new gitignore`

`dotnet new editorconfig`

`git init`


## Create a project to act as our repository layer
Create the folder, create the project, then add any required project references before returning to the solution folder.

`mkdir data`

`cd data`

`dotnet new classlib`

`cd ..`

## Create a project to contain shared objects or methods
Create the folder, create the project, then add any required project references before returning to the solution folder.

`mkdir application`

`cd application`

`dotnet new classlib`

`cd ..`

## Create a project for our user interface
Create the folder, create the project, then add any required project references before returning to the solution folder.

This is a Blazor web assembly project.

`mkdir presentation`

`cd presentation`

`dotnet new blazorwasm`

`dotnet add reference ../data/data.csproj`

`dotnet add reference ../application/application.csproj`

`cd ..`

## Create a project to feed data to our UI
Create the folder, create the project, then add any required project references before returning to the solution folder.

`mkdir api`

`cd api`

`dotnet new webapi`

`dotnet add reference ../application/application.csproj`
`dotnet add reference ../data/data.csproj`

dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File

`cd ..`

## Create a solution file 
Add references to the projects.

`dotnet new sln`

`dotnet sln add data/data.csproj`

`dotnet sln add application/application.csproj`

`dotnet sln add api/api.csproj`

`dotnet sln add presentation/presentation.csproj`

# Add nuget package for Dapper to SQLite

`cd data`

`dotnet add package Dapper --version 2.0.123`
`dotnet add package Microsoft.Data.Sqlite --version 6.0.4`
`dotnet add package Microsoft.Extensions.Options --version 6.0.0`

`cd ..`