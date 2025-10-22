# NetPCTask
Repository for recruitment task

# Setup

## Prerequisites
.NET 9
MS Sql Server
npm

## How to get started
- `git clone` this repository
- Create a new database for this project
- Fill the `BackendApp/ContactList/appsettings.json` fields marked as [FillHere] - database connection string and JWT key.
- In `BackendApp/ContactList` run:
`dotnet restore`
`dotnet build`
`dotnet ef database update`
`dotnet run`
- Now backend application should be running. For frontend application run following commands in `FrontEndApp/contacts-app`:
`npm i`
`npm start`

# Documentation

## Backend
Backend is a simple CRUD application that mimics DDD structure but without splitting it into separate projects as it would be too cumbersome and time consuming for a small project like this. It uses NSwag and Swagger for API documentation, MiniValidation for validating Request models and Entity framework core as ORM.
### Domain layer
It consists of one folder - Entities. Those are the core domain objects stored in database - Category, Contact, Subcategory and User.
### Application layer
Here is the main processing centre of the application. It contains folders:
- Interfaces - list of interfaces needed for application/instrastracture layer to implement
- ApplicationServices - handlers of API calls and services that contains logic important for application to work properly
- DTOs - list of contracts for API and according controllers which passes them on to handlers.
### Infrastracture layer
Layer to process everything related with outside world - here just database. It consists of:
- Migrations - EF Core code first migrations to keep database cohesive
- Repositories - classes implementing given interfaces from application layer to enable consistent way of interacting with database
- DbContext - EF Core entry point
### API (Presentation) layer
Thin presentation layer that has controllers and all the startup files. Controllers should just contain code to receive request, pass it to handler and then return the result to the user.
### Future development
Adding logging, OpenTelemetry, unit tests. Didn't add them because of lack of time and amount of boilerplate code already in them. Splitting application into actual DDD layers.

## Frontend
React SPA application splitted into pages (route-specific view) and reusable components. Description of folders:
- api - logic to interact with API from backend in consistent way
- components - reusable react components that can be used in many pages if needed
- context - here only AuthContext to let pages have easy access to token and other authorization tools
- helpers - constants and ValidationError to have consistent way of interacting with 400 errors
- pages - url-specific pages. They can share same component underneath but if some input data is different then it will be seperate page
- routes - wrapper for a PrivateRoute to be seen only by logged user
### Future development
First of all - incorporating some visual library (like sass, bootstrap or MUI) to make layout better and to let user interact with the page in a consistent way. And of course creating CSS to beautify the page and maybe creating some storybook to ensure consistent components across whole application.
On top of that tests.