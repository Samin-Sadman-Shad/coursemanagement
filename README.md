# University Course Management System

A **University Course Management System** built with **.NET 9, ASP.NET Core Web API, Entity Framework Core, PostgreSQL, ASP.NET Core Identity, JWT authentication, and Docker Compose**.

The application follows a **modular monolithic architecture** with a Clean Architecture approach.

## Project Structure

```text
CourseManagement/
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── UniversityCourseManagement.slnx
│
└── src/
|    ├── University.API/
|    ├── University.Application/
|    ├── University.Domain/
|    ├── University.Persistance/
|    ├── University.Identity/
|
└── University.Tests/
```

### Projects

* **University.API** — Web API/controllers and application entry point.
* **University.Application** — Commands, queries, handlers, DTOs, and application logic.
* **University.Domain** — Domain entities and business concepts.
* **University.Persistance** — EF Core, PostgreSQL, repositories, Unit of Work, and migrations.
* **University.Identity** — ASP.NET Core Identity, users, roles, and JWT authentication.
* **University.Tests** — Automated tests.

The application is a **modular monolith**, not a collection of independently deployed microservices.

---

## Prerequisites

To run the application with Docker:

* Git
* Docker Desktop

No local PostgreSQL or .NET SDK installation is required when using Docker Compose.

---

## Clone and Run

Clone the repository:

```bash
git clone https://github.com/Samin-Sadman-Shad/coursemanagement.git
cd CourseManagement
```

Make sure Docker Desktop is running, then execute:

```bash
docker compose up --build -d
```

This builds the Web API image and starts both containers:

```text
university-api-compose
university-postgres-compose
```

Check their status:

```bash
docker compose ps
```

Stop the environment:

```bash
docker compose down
```

---

## Docker Environment

Docker Compose creates an internal network connecting the API and PostgreSQL containers.

```text
Host
 │
 │ http://localhost:8080
 ▼
.NET Web API
 │
 │ postgres:5432
 ▼
PostgreSQL
```

The API is exposed to the host through:

```text
http://localhost:8080
```

PostgreSQL is **not exposed to the host** when run in Docker Environment. It is accessible only through the internal Docker network.

The API connects to PostgreSQL using:

```text
Host=postgres;Port=5432;Database=universitydb;Username=postgres;Password=devpassword
```

---

## PostgreSQL Persistence

PostgreSQL uses a **bind mount** so database data is stored on the local machine and survives container recreation.

The Compose configuration contains:

```yaml
volumes:
  - type: bind
    source: E:\Samin\Projects\university_postgres
    target: /var/lib/postgresql/data
```

Change the `source` path to choose a different location on the local machine:

```yaml
source: D:\DockerData\university_postgres
```

The container path should remain:

```text
/var/lib/postgresql/data
```

---

## Authentication Flow

The current authentication design follows this workflow:

### 1. Staff Registration

Staff members are registered through:

```text
POST /api/Auth/register
```
(No token is required for this endpoint)

After registration, the staff member logs in and receives a JWT.

The JWT is sent with authenticated requests using:

```http
Authorization: Bearer <JWT>
```

### 2. Staff Creates Student

An authenticated staff member creates a student using:

```text
POST /api/Student
```

When a student is created, a password reset/setup token is generated.

The token must be shared with the student.

### 3. Student Sets Password

The student uses the token with:

```text
POST /api/Auth/set-password
```

This establishes the student's password.

The mechanism is similar to a password-reset token normally delivered through email.

### 4. Student Login

The student can then log in using the newly established password and receive a JWT.

The JWT is subsequently used as a Bearer token to access the operations allowed for the student ( GET student/me/peers).

---

## Swagger / API Testing

After starting Docker Compose, Swagger UI is available at:

```text
http://localhost:8080/swagger/index.html
```

All available API endpoints can be explored and tested directly from Swagger.

The OpenAPI specification is available at:

```text
http://localhost:8080/swagger/v1/swagger.json
```

The Swagger JSON was also used to generate the **Postman collection** by importing the OpenAPI specification into Postman.

Please set the following environment variables in the **Postman Collection**
baseUrl : http://localhost:8080

Also set the jwt token as {{bearerToken}} with Auth type 'Bearer Token' selected

---

## Application Configuration

The project uses environment-specific configuration.

For local development:

```text
appsettings.json
```

uses PostgreSQL through `localhost`.

Local development configuration is only for the convenience of the devloper. 

For Docker:

```text
appsettings.Docker.json
```

uses:

```text
Host=postgres
Port=5432
```

Docker Compose sets:

```yaml
environment:
  ASPNETCORE_ENVIRONMENT: Docker
```
Docker environment will be automatically set when the application is run via docker compose.
---

## Tests

The `University.Tests` project contains the automated tests.

Tests are **not included in the Docker runtime container**. The Docker image contains only what is required to run the Web API.

Run tests from the development environment with:

```bash
dotnet test
```

---

## Quick Start

```bash
git clone https://github.com/Samin-Sadman-Shad/coursemanagement.git
cd CourseManagement
docker compose up --build -d (--build tag allows to generate the image before running the containers)
```

Visit the following swagger endpoints for quick tests:

**Swagger:**
`http://localhost:8080/swagger/index.html`

**OpenAPI JSON:**
`http://localhost:8080/swagger/v1/swagger.json`
