# NetPCTask

End-to-end contact management demo delivered as part of a recruitment task. The solution exposes a .NET 9 Web API backed by MS SQL Server and a React single-page application for the user interface.

## Prerequisites

- .NET 9 SDK (includes the `dotnet` CLI)
- MS SQL Server instance reachable from the backend
- Node.js 18+ with npm

## Getting Started

1. Clone the repository and switch into the project directory.
2. Provision a new SQL Server database for the application.
3. Update `BackendApp/ContactList/appsettings.json` and `BackendApp/ContactList/appsettings.Development.json` with your database connection string and a secure JWT signing key (replace the `[FillHere]` placeholders).

### Backend setup (`BackendApp/ContactList`)

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run -lp https
```

The commands above install dependencies, compile the application, apply EF Core migrations, and start the API. The service listens on the ports configured in `launchSettings.json` and publishes OpenAPI/Swagger documentation via NSwag.

### Frontend setup (`FrontEndApp/contacts-app`)

```bash
npm install
npm start
```

The React development server proxies API calls to the backend and automatically reloads on code changes.

## Solution Overview

### Backend architecture

The backend follows a domain-driven design (DDD) layering scheme while remaining within a single project. Folders and files mark the boundaries instead of separate assemblies:

- **Domain layer (`Entities/`)** – Entity classes (`Category`, `Contact`, `Subcategory`, `User`) representing the core model persisted in the database.
- **Application layer (`ApplicationServices/`, `Interfaces/`, `DTOs/`)** – Coordinates use cases via handlers, contracts, and data transfer objects that abstract domain logic from infrastructure concerns.
- **Infrastructure layer (`DbContext.cs`, `Repositories/`, `Migrations/`)** – Entity Framework Core context, database migrations, and repository implementations that realise the interfaces defined in the application layer.
- **API (presentation) layer (`Controllers/`, `Program.cs`, `Properties/launchSettings.json`)** – ASP.NET Core controllers and startup configuration responsible for request validation, routing, and surfacing API metadata. NSwag and Swagger expose the OpenAPI definition, while MiniValidation enforces request rules at the edge.

### Frontend architecture

- **api** – Abstractions for communicating with the backend API in a consistent manner.
- **components** – Reusable UI elements shared across pages.
- **context** – Currently contains `AuthContext` for managing authentication state and propagating tokens.
- **helpers** – Constants and utilities (including `ValidationError`) for uniform 400-response handling.
- **pages** – Route-specific views that include shared components while tailoring data requirements.
- **routes** – Wrapper utilities such as `PrivateRoute` to enforce authenticated access.

## Roadmap Ideas

- **Cross-cutting**
  - Introduce structured logging and distributed tracing (e.g. Microsoft.Extensions.Logging + OpenTelemetry) to improve observability.
  - Automate quality gates with CI/CD workflows, incorporating tests, and automated database migrations.
  - Containerize the stack (Docker Compose) to streamline onboarding and deployment.
- **Backend**
  - Add unit and integration tests around domain services, repositories, and API endpoints.
  - Implement role-based access control, and rate limiting to tighten security.
  - Cache frequently read data (e.g., categories) via Redis or in-memory caching to reduce database load.
- **Frontend**
  - Adopt TypeScript typings across components to improve developer ergonomics (e.g. type hinting) and tooling support.
  - Adopt a styling system (e.g., MUI, Tailwind, or SCSS modules) and establish a shared design language.
  - Add Storybook to document and exercise components in isolation.
  - Expand automated tests coverage with React Testing Library, Cypress, or Playwright end-to-end flows.
