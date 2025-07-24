# ECommerce Payment API

A robust, production-ready e-commerce payment API built with **Clean Architecture** principles in **.NET 8**, integrating with an external Balance Management service.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Resilience Patterns](#resilience-patterns)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Error Handling](#error-handling)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)

## 🎯 Overview

This API serves as a payment gateway for e-commerce applications, providing product management and order processing capabilities while maintaining high availability and fault tolerance through integration with an external Balance Management service.

### Key Responsibilities
- ✅ Product catalog management
- ✅ Order creation and processing  
- ✅ Fund reservation and payment completion
- ✅ Robust error handling for external service failures
- ✅ Clean API documentation with Swagger

## 🏗️ Architecture

```
ECommercePayment/
├── Controllers/          # API Controllers (HTTP layer)
├── Application/          # Business logic and DTOs
│   ├── DTOs/            # Data Transfer Objects
│   ├── Enums/           # Application enumerations
│   ├── Interfaces/      # Service interfaces
│   └── Services/        # Business logic implementation
├── Domain/              # Core domain entities and exceptions
│   ├── Entities/        # Domain entities
│   └── Exceptions/      # Custom exceptions
├── Infrastructure/      # External concerns
│   ├── BalanceManagement/  # External service integration
│   ├── Persistence/        # Data persistence
│   └── Resilience/         # Resilience policies
└── Middleware/          # Cross-cutting concerns
```

## ✨ Features

### Core Features
- **Clean Architecture** implementation with SOLID principles
- **Swagger/OpenAPI** documentation with detailed examples
- **Global Exception Handling** with structured error responses
- **Dependency Injection** throughout the application
- **Production-ready logging** with request tracing

### Resilience & Fault Tolerance
- **Retry Policy** with exponential backoff (2s, 4s, 8s)
- **Circuit Breaker** pattern (5 failures → 30s break)
- **Timeout Handling** (10s per request, 30s overall)
- **Jitter** to prevent thundering herd problems
- **Health Monitoring** and service availability tracking

### Data Management
- **SQL Server database** with Entity Framework Core
- **Repository pattern** for data access abstraction
- **Type-safe DTOs** with validation attributes
- **Enum-based status management**
- **Structured error responses** with trace IDs

## 🚀 API Endpoints

### Products

#### GET /api/products
Retrieves all available products from Balance Management service.

**Response Example:**
```json
[
  {
    "id": "prod-001",
    "name": "Premium Smartphone",
    "description": "Latest model with advanced features",
    "price": 19.99,
    "currency": "USD",
    "category": "Electronics",
    "stock": 42
  }
]
```

**Status Codes:**
- `200 OK` - Products retrieved successfully
- `503 Service Unavailable` - Balance Management service unavailable

### Orders

#### POST /api/orders/create
Creates a new order and reserves funds through Balance Management.

**Request Body:**
```json
{
  "productId": "prod-001",
  "quantity": 2
}
```

**Response Example:**
```json
{
  "id": 1234,
  "productId": "prod-001", 
  "quantity": 2,
  "totalAmount": 39.98,
  "status": "Pending"
}
```

**Status Codes:**
- `200 OK` - Order created successfully
- `400 Bad Request` - Invalid request data
- `402 Payment Required` - Insufficient balance
- `404 Not Found` - Product not found
- `503 Service Unavailable` - Balance Management service unavailable

#### POST /api/orders/{id}/complete
Completes an existing order and finalizes payment.

**Response Example:**
```json
{
  "id": 1234,
  "productId": "prod-001",
  "quantity": 2, 
  "totalAmount": 39.98,
  "status": "Completed"
}
```

**Status Codes:**
- `200 OK` - Order completed successfully
- `400 Bad Request` - Invalid order ID
- `404 Not Found` - Order not found
- `503 Service Unavailable` - Balance Management service unavailable

## 🛡️ Resilience Patterns

Our API implements production-ready resilience patterns to handle external service failures gracefully.

### Retry Policy with Exponential Backoff

**Configuration:**
- **Retry Count:** 3 attempts
- **Backoff Strategy:** Exponential (2s, 4s, 8s)
- **Jitter:** 0-1000ms random delay
- **Triggers:** Network errors, 5XX responses, timeouts

**Example Flow:**
```
Request → Network Error → Wait 2s → Retry
Still Error → Wait 4s → Retry  
Still Error → Wait 8s → Final Retry
Success → Return Response
```

### Circuit Breaker Pattern

**Configuration:**
- **Failure Threshold:** 5 consecutive failures
- **Break Duration:** 30 seconds
- **Recovery:** Automatic reset on success

**States:**
- **Closed:** Normal operation
- **Open:** Failing fast, no requests sent
- **Half-Open:** Testing with limited requests

### Timeout Management

**Configuration:**
- **Per-Request Timeout:** 10 seconds
- **Overall Timeout:** 30 seconds
- **Behavior:** Timeout triggers retry mechanism

### Benefits

✅ **Network Failures:** Automatic retry with smart backoff  
✅ **Service Downtime:** Circuit breaker prevents cascading failures  
✅ **Slow Responses:** Timeout handling maintains responsiveness  
✅ **High Load:** Jitter prevents thundering herd problems  

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server LocalDB (included with Visual Studio)
- Internet connection (for Balance Management service)

### Running the Application

1. **Clone the repository:**
```bash
git clone <repository-url>
cd ECommercePayment
```

2. **Restore dependencies:**
```bash
dotnet restore
```

3. **Build the project:**
```bash
dotnet build
```

4. **Update database:**
```bash
dotnet ef database update
```

5. **Run the application:**
```bash
dotnet run
```

6. **Access Swagger UI:**
- HTTP: `http://localhost:5215`
- HTTPS: `https://localhost:7100`

### Visual Studio

1. Open `ECommercePayment.sln`
2. Select **"https"** profile
3. Press **F5** to start debugging
4. Browser will open at `https://localhost:7100`

## ⚙️ Configuration

### External Services

The application integrates with Balance Management service. Configuration is handled in code:

- **Base URL**: `https://balance-management-pi44.onrender.com`
- **Timeout**: 30 seconds overall, 10 seconds per request
- **Retry Count**: 3 attempts with exponential backoff
- **Circuit Breaker**: 5 consecutive failures trigger 30-second break

Database configuration in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommercePaymentDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Resilience Policies

Policies are configured in `Infrastructure/Resilience/ResiliencePolicies.cs`:

- **Retry:** Exponential backoff with jitter
- **Circuit Breaker:** Fail-fast protection  
- **Timeout:** Request-level timeouts
- **Combined Policy:** All patterns working together

## 🔥 Error Handling

### Global Exception Middleware

All exceptions are handled by `ExceptionHandlingMiddleware` providing:

- **Structured JSON responses**
- **Appropriate HTTP status codes**  
- **Request trace IDs for debugging**
- **Detailed logging**

### Error Response Format

```json
{
  "status": 503,
  "error": "Failed to fetch products: Connection timeout",
  "traceId": "0HNE8E0FPA7TM:0000000B", 
  "timestamp": "2025-07-21T16:04:33.5446851Z"
}
```

### Custom Exceptions

- **`ExternalServiceException`** - Balance Management failures
- **`OrderNotFoundException`** - Order not found scenarios  
- **`InsufficientBalanceException`** - Payment failures
- **`ValidationException`** - Input validation errors

## 🧪 Testing

### Test Structure

```
ECommercePayment.Tests/
├── Controllers/          # Controller integration tests
├── Services/            # Business logic unit tests  
└── Infrastructure/      # External service tests
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

### Test Categories

- **Unit Tests:** Business logic with mocked dependencies
- **Integration Tests:** End-to-end API testing
- **Resilience Tests:** Network failure simulation

## 📁 Project Structure

### Core Layers

**Controllers** (`Controllers/`)
- `ProductsController` - Product catalog endpoints
- `OrdersController` - Order management endpoints

**Application** (`Application/`)
- `DTOs/` - Data transfer objects with validation
- `Services/` - Business logic implementation
- `Interfaces/` - Service contracts
- `Enums/` - Application enumerations

**Domain** (`Domain/`)
- `Entities/` - Core business entities (Product, Order)
- `Exceptions/` - Domain-specific exceptions

**Infrastructure** (`Infrastructure/`)
- `BalanceManagement/` - External service integration
- `Persistence/` - Entity Framework Core and Repository implementations
- `Resilience/` - Fault tolerance policies (Polly)

## 🛠️ Technology Stack

### Core Technologies
- **.NET 8** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C#** - Programming language

### Libraries & Packages
- **Swashbuckle.AspNetCore** - OpenAPI/Swagger documentation
- **Microsoft.Extensions.Http.Polly** - Resilience patterns
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server data access
- **Microsoft.EntityFrameworkCore.Tools** - EF Core CLI tools
- **System.Text.Json** - JSON serialization

### Architecture Patterns
- **Clean Architecture** - Separation of concerns
- **Dependency Injection** - Inversion of control
- **Repository Pattern** - Data access abstraction
- **Retry Pattern** - Fault tolerance
- **Circuit Breaker** - Service protection

### Development Tools
- **Visual Studio 2022** - IDE
- **Swagger UI** - API testing
- **HTTP Client** - External service integration

## 📊 Monitoring & Observability

### Logging
- **Structured logging** with request correlation
- **Retry attempt tracking** 
- **Circuit breaker state changes**
- **Performance metrics**

### Database & Persistence
- **Entity Framework Core** with SQL Server
- **Migration support** for database schema changes
- **Repository pattern** implementation
- **Connection string** configuration in appsettings

## 🔒 Security Considerations

- **Input validation** on all endpoints
- **Exception sanitization** in responses
- **Timeout protection** against slowloris attacks
- **Rate limiting** through circuit breaker

## 🚀 Production Readiness

This API is designed for production deployment with:

✅ **Fault tolerance** for external dependencies  
✅ **Graceful degradation** during service outages  
✅ **Monitoring and observability** built-in  
✅ **Clean error handling** and user experience  
✅ **Scalable architecture** supporting future growth 