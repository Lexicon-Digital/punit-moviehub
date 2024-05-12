# MovieHub API

## Overview
MovieHub API provides detailed information on movies and the cinemas where they are shown. This API can retrieve movies by their attributes and provide detailed or general information based on user requests.

## Requirements

- **.NET 8.0 SDK** or later
- The project was built using [JetBrains Rider](https://www.jetbrains.com/rider/).
- **Visual Studio 2019** or later, or another compatible IDE that supports .NET development (like VS Code with C# plugin)

## Setup
### Clone the repository
Start by cloning the repository to your local machine. To do this, run:

```shell
git clone https://github.com/PunitDharmadhikariLexicon/MovieHub
cd MovieHub
```

### Install dependencies
Ensure that all the necessary packages are restored:

```shell
dotnet restore
```

### Configure the database
The API uses SQLite database, which will be automatically generated upon running migrations.

### Run migrations
To run migrations in the `MovieHub/Migrations` folder, use the command:

```shell
dotnet ef database update
```

### Seeding the database
There is no need to manually seed the database. The database will be automatically seeded using the script `./MovieHub/Scripts/moviehub-db-data-seed.sql` when the application is run the first time.

### Run the application
Run the application using the command:
```shell
dotnet run
```

### Testing with Swagger
Go to `https://localhost:7190/swagger` (port number specified in `./MovieHub/Properties/launchSettings.json`) and test each endpoint.

### Testing with Postman
Download and import the `./MovieHub.postman_collection.json` file into Postman. It contains all the different requests for the controllers.