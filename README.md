# Insurance Claims Handling API

## Overview

This project implements a multi-tier backend application for managing insurance claims.

The system allows users to:

- Create insurance claims
- Retrieve a list of existing claims
- Delete claims

The solution was designed with clean architecture principles, strong separation of concerns, and testability in mind.


## Architecture

The solution is structured into multiple layers:

### 1. API Layer (`Claims.API`)
- ASP.NET Core Web API
- Controllers are thin and delegate business logic to services
- Global exception handling middleware
- Swagger/OpenAPI documentation enabled

### 2. Application Layer (`Claims.Application`)
- Business logic and orchestration
- Validators for Claim and Cover
- Premium calculation logic
- Interfaces for abstraction (e.g., `IClaimStore`, `IAuditQueue`, `IPremiumCalculator`)
- No dependency on infrastructure

### 3. Domain Layer (`Claims.Domain`)
- Core domain entities
- Enums (`ClaimType`, `CoverType`)
- Domain-level logic

### 4. Infrastructure Layer (`Claims.Infrastructure`)
- In-memory implementation of `IClaimStore`
- Asynchronous audit queue
- Background service for audit processing

### 5. Test Project (`Claims.Application.Tests`)
- Unit tests for:
  - Claim validation
  - Cover validation
  - Business logic
  - Premium calculation
  - Audit behavior
- Deterministic clock implementation for reliable tests


## Validation Rules

### Claim
- `DamageCost` cannot exceed 100,000
- `IncidentDate` cannot be in the future
- `IncidentDate` must be within the related Cover period

### Cover
- `CoverageStart` cannot be in the past
- Insurance period cannot exceed 1 year
- `MaxAmount` must be greater than zero
- `PolicyNumber` and `Currency` are required


## Premium Calculation

Premium is calculated based on:

- Base daily rate: **1250**
- Multiplier depending on `CoverType`:
  - Yacht → +10%
  - PassengerShip → +20%
  - Tanker → +50%
  - Other types → +30%

Progressive discount:
- First 30 days → full price
- Next 150 days:
  - Yacht → 5% discount
  - Others → 2% discount
- Remaining days:
  - Yacht → 8% total discount
  - Others → 3% total discount

The premium calculation was refactored to ensure:
- No overlapping conditions
- Single rate applied per day
- Clear separation of multiplier and discount logic
- Dedicated unit tests


## Auditing

Auditing is implemented asynchronously:

- `IAuditQueue` is used to enqueue audit events
- `AuditBackgroundService` processes audit events in the background
- This prevents blocking HTTP request processing

This demonstrates non-blocking architecture and clean separation of concerns.


## Persistence

The current implementation uses an in-memory store (`InMemoryClaimStore`) for simplicity and deterministic unit testing.

Persistence is abstracted via `IClaimStore`.  
Switching to MongoDB or SQL Server would require only a new Infrastructure implementation without modifying business logic.


## How to Run

1. Build the solution
2. Run `Claims.API`
3. Open Swagger at: https://localhost:<port>/swagger


## Running Tests

Run all unit tests:

"dotnet test"

All tests are deterministic and do not require external dependencies.


## Design Principles Applied

- SOLID principles
- Separation of concerns
- Dependency inversion via interfaces
- Asynchronous processing
- Clean layering
- Deterministic unit testing


## Notes

The solution prioritizes clean architecture and testability.  
Infrastructure can be replaced without affecting core business logic.