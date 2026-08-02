# LoanStream Refactoring

## Overview
This workspace now contains a simple landing page, an ASP.NET Core Web API, a RabbitMQ-backed lead worker, SQL Server schema scripts, and a lightweight admin dashboard.

## Structure
- frontend: landing page and client-side JavaScript
- backend: ASP.NET Core API, repositories, services, workers, SQL scripts
- admin: plain HTML/CSS/JS dashboard

## Running the app
1. Start SQL Server and create a database named LoanStreamDb.
2. Run the script in backend/Database/schema.sql.
3. Start RabbitMQ locally on port 5672.
4. Update backend/appsettings.json with your connection strings if needed.
5. Run the API:
   - dotnet run --project backend/LoanStream.Api.csproj
6. Open http://localhost:5086/ for the frontend.
7. Open http://localhost:5086/admin/index.html for the admin dashboard.

## API endpoints
- POST /api/leads
- POST /api/contact
- GET /api/admin/leads
- GET /api/admin/export/excel
- GET /api/admin/export/csv

## Notes
- The lead API validates the request, publishes a LeadCreated event to RabbitMQ, and returns success.
- The worker consumes the RabbitMQ queue and inserts the lead into SQL Server.
- The partner redirect URL is configured in appsettings.json and is preserved with the original UTM values.
