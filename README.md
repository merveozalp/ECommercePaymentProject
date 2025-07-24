# 💳 ECommerce Payment API
---

## 📖 Table of Contents

- [🎯 Overview](#-overview)
- [✨ Features](#-features) 
- [🏗️ Architecture](#️-architecture)
- [🛠️ Technology Stack](#️-technology-stack)
- [⚡ Installation](#-installation)
- [📡 API Usage](#-api-usage)
- [🧪 Testing](#-testing)
- [🚀 Deployment](#-deployment)

---

## 🎯 Overview

This API provides **reliable payment infrastructure** for e-commerce applications. Integrated with external Balance Management service to handle:

- ✅ **Product catalog** management
- ✅ **Order creation** and processing
- ✅ **Fund reservation** and payment completion
- ✅ **Fault tolerance** and graceful degradation

## ✨ Features

### 🏗️ **Clean Architecture**
- **Domain-Driven Design** with separated layers
- **SOLID principles** implemented code structure
- **Dependency Injection** for loose coupling
- **Repository Pattern** for data access abstraction

### 🛡️ **Resilience & Fault Tolerance**

🔄 Retry Policy → 3 attempts, exponential backoff (2s, 4s, 8s)
⚡ Circuit Breaker → 5 failure threshold, 30s recovery time
⏱️ Timeout Handling → 10s per request, 30s overall
🎲 Jitter → 0-1000ms random delay (thundering herd prevention)

### 🔥 **Production-Ready Features**
- **Structured Logging** with correlation IDs
- **Health Checks** endpoint
- **Performance Monitoring** with execution time tracking
- **Global Exception Handling** with standardized responses
- **API Versioning** support
- **Rate Limiting** through circuit breaker

### 📊 **Data & Persistence**
- **Entity Framework Core** with SQL Server
- **Database Migrations** support
- **Connection Pooling** optimization
- **Transaction Management** for data consistency

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

### 🌐 **HTTP Layer**
- **Controllers/** - API Endpoints & HTTP handling

### 💼 **Application Layer**
- **Services/** - Business logic implementations
- **DTOs/** - Data transfer objects
- **Interfaces/** - Service contracts

### 🎯 **Domain Layer**
- **Entities/** - Core business entities
- **Exceptions/** - Domain-specific exceptions

### 🔧 **Infrastructure Layer**
- **Persistence/** - Database & repository implementations
- **BalanceManagement/** - External service integration
- **Resilience/** - Fault tolerance policies (Polly)

### 🔄 **Cross-Cutting**
- **Middleware/** - Exception handling & logging

---

## 🛠️ Technology Stack

### **Backend Core**
- ![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=flat&logo=dotnet) **Runtime & Framework**
- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat&logo=dotnet) **Web API Framework**
- ![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp) **Programming Language**

### **Database & ORM**
- ![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoft-sql-server) **Database Engine**
- ![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=flat) **ORM**

### **Libraries & Tools**
- **Polly** - Resilience patterns (Retry, Circuit Breaker, Timeout)
- **Swagger/OpenAPI** - API documentation
- **System.Text.Json** - JSON serialization
- **NUnit** - Unit testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library

---

## ⚡ Installation

### **Prerequisites**
- ![.NET 8](https://img.shields.io/badge/.NET-8.0-blue) **.NET 8 SDK**
- ![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-red) **SQL Server LocalDB**
- ![Visual Studio](https://img.shields.io/badge/Visual_Studio-2022-purple) **Visual Studio 2022** (optional)

### **1️⃣ Clone Repository**
```bash
git clone https://github.com/merveozalp/ECommercePayment.git
cd ECommercePayment
```

### **2️⃣ Dependencies & Database Setup**
```bash
# Package restore
dotnet restore

# Database migration
dotnet ef database update

# Build project
dotnet build
```

### **3️⃣ Run Application**
```bash
# Development mode
dotnet run --project ECommercePayment

# Or with specific profile
dotnet run --project ECommercePayment --launch-profile https
```

### **4️⃣ Access API**
- 🌐 **Swagger UI**: https://localhost:7100

---

## 📡 API Usage

### **📦 Products**

#### Get all products
```http
GET /api/products
Accept: application/json
```

**Response:**
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

### **🛒 Orders**

#### Create order
```http
POST /api/orders/create
Content-Type: application/json

{
  "productId": "prod-001",
  "quantity": 2
}
```

#### Complete order
```http
POST /api/orders/{orderId}/complete
```


**Response:**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "database": {
      "status": "Healthy"
    }
  }
}
```

---

## 🧪 Testing

### **Test Categories**
- 🧪 **Unit Tests** - Business logic with mocked dependencies
- 🔗 **Integration Tests** - End-to-end API testing  
- 🛡️ **Resilience Tests** - Network failure simulation



</div>
