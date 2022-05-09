# In Touch

A correspondence tracker for people who need a simple way to see who owes them a follow up and who they owe the same.

This project doesn't aim to compete with any large CRM. It's just a simple app driven primarily by my immediate needs. Take a look at the [future plans](./future.md) and feel free to send your own suggestions.

# Important

  * Set your database location in appSettings.js
  * Run the API with `dotnet run --project api` and use `https://localhost:<port>/swagger`
  * Run the UI with `dotnet run --project presentation`. Note that it expects to find the API at `https://localhost:7022`, as specified in `Program.cs`
  * Learn how to [create the database](./create-db.md)

# Tech stack

  * Presentation layer / user interface
  
    * [Bulma CSS framework](https://bulma.io)
    * [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (SDK 6.0.202)

      * [Microsoft Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)

  * Web API

    * [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (SDK 6.0.202)

      * [Swagger](https://swagger.io)

    * ORM: [Dapper](https://github.com/DapperLib/Dapper)

  * Database

    * [sqlite](https://www.sqlite.com/index.html)

  * Editors used to create this project

    * [Visual Studio Code ](https://code.visualstudio.com)
    * [SQLPro for SQLite](https://www.sqlitepro.com)
