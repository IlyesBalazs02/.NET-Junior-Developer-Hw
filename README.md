### Setup

.NET8, ASP.NET Core Web API

EF Core 8 (in-memory database)

Tests: xUnit and Moq





### Architecture

Controller -> Service -> Repository -> DbContext



Controllers: HTTP only (read request, call service, return status code)

Services: business rules, creating entities, mapping to DTOs

Repositories: data access only (queries, add, remove, save)

Models: entities that protect their own state (private setters, methods like MarkAsBorrowed and Return)

DTOs: request and response formats, with input validation

Middleware: one place that turns exceptions into HTTP status codes





### KEY DECISIONS

\- Controllers only handle HTTP, services hold the business rules, and repositories only access data. Each layer has one job, so the code is easy to change and the services can be unit tested with mocked repositories.



\- Encapsulation: Entities have private setters and can only be created through their constructor and changed through methods (for example MarkAsBorrowed). Rules that need other data are in the services, because an entity cannot query the database.



\- exception-handling middleware: Services can throw domain exceptions (NotFoundException, ConflictException, ModelValidationException). The middleware maps them to 404 / 409 / 400. Unexpected errors become a generic 500



\- Borrow and return change two entities (book and loan). The changes are made in memory first and saved with one SaveChangesAsync call, so they succeed or fail together.



\- Related data is explicitly loaded using eager loading rather than lazy loading for better database performance

