# RWT Video Platform - Backend

Backend API for the RWT Video Platform used to manage users, authentication and video access for RITEH Web Team lectures.

## Requirements

Before running the project, make sure you have the following installed:

- .NET 8 SDK
- PostgreSQL
- pgAdmin (optional, for database management)
- Git

## How to run the project

### 1. Clone the repository

`git clone https://github.com/LVukusic0606/RWT_video_platform.git`

`cd RWT_video_platform/backend/RwtVideos.Api`

### 2. Configure application settings

Configuration files are not included in the repository because they may contain sensitive data.

Create copies of the template files:

`cp appsettings.json.init appsettings.json`

`cp appsettings.Development.json.init appsettings.Development.json`

Then fill in the required values in these files (for example):

- PostgreSQL connection string
- JWT key
- Seed admin credentials

### 3. Create PostgreSQL database

Create a PostgreSQL user and database:

`CREATE USER rwt_user WITH PASSWORD 'password';`

`CREATE DATABASE rwt_videos OWNER rwt_user;`

`GRANT ALL PRIVILEGES ON DATABASE rwt_videos TO rwt_user;`

### 4. Apply database migrations

Run the following command to create the database schema:

`dotnet ef database update`

### 5. Run the application

`dotnet watch run`

### 6. Open Swagger

After starting the application, open:

http://localhost:5080/swagger

Swagger can be used to test the API endpoints.