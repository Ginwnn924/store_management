# Store Management System

A .NET 8 Web API application for managing store operations including inventory, orders, customers, and user authentication.

## Prerequisites

Before running this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- [Redis Server](https://redis.io/downloads/) (optional, for caching)
- [Git](https://git-scm.com/downloads)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Ginwnn924/store_management.git
cd store_management
```

### 2. Configuration Setup

1. **Configure your database connection:**
   
   Duplicate `appsettings.Sample.json`  then rename it to `appsettings.Development.json` and update the connection strings according to your local environment

   **Important Configuration Notes:**
   - Replace `your_username` and `your_password` with your MySQL credentials
   - Generate a secure secret key for JWT authentication (minimum 32 characters)
   - Ensure your MySQL server is running on the specified port (default: 3306)
   - Redis connection is optional but recommended for performance


### 3. Access the API Documentation

Once the application is running, you can access the Swagger API documentation at:
- `http://localhost:5163/index.html` (HTTP)
