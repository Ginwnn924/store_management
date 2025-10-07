# Store Management System

A comprehensive store management system built with ASP.NET Core 8.0, Entity Framework Core, MySQL, and Redis.

## Features

- **Database**: MySQL database with comprehensive schema for store operations
- **ORM**: Entity Framework Core with complete model classes
- **Authentication**: JWT Bearer Token authentication with ASP.NET Core Identity
- **API Documentation**: Interactive Swagger/OpenAPI documentation
- **Containerization**: Docker Compose setup with MySQL, Redis, and .NET API services

## Technologies Used

- **Backend**: ASP.NET Core 8.0 Web API
- **ORM**: Entity Framework Core with Pomelo MySQL provider
- **Database**: MySQL 8.0
- **Cache**: Redis
- **Authentication**: JWT Bearer Tokens with BCrypt password hashing
- **API Documentation**: Swashbuckle (Swagger/OpenAPI)
- **Containerization**: Docker & Docker Compose

## Project Structure

```
store_management/
├── StoreManagement/
│   ├── StoreManagement/
│   │   ├── Controllers/        # API Controllers
│   │   │   └── AuthController.cs
│   │   ├── Models/             # Entity Models
│   │   │   ├── User.cs
│   │   │   ├── Customer.cs
│   │   │   ├── Category.cs
│   │   │   ├── Supplier.cs
│   │   │   ├── Product.cs
│   │   │   ├── Inventory.cs
│   │   │   ├── Promotion.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   └── Payment.cs
│   │   ├── Data/               # Database Context
│   │   │   └── StoreManagementDbContext.cs
│   │   ├── DTOs/               # Data Transfer Objects
│   │   │   ├── RegisterDto.cs
│   │   │   ├── LoginDto.cs
│   │   │   └── AuthResponseDto.cs
│   │   ├── Dockerfile
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── db.sql                  # Database schema and seed data
└── docker-compose.yml
```

## Database Schema

The system includes the following entities:

- **users**: User accounts with roles (admin/staff)
- **customers**: Customer information
- **categories**: Product categories
- **suppliers**: Supplier information
- **products**: Product catalog with pricing
- **inventory**: Stock management
- **promotions**: Discount and promotion management
- **orders**: Order management
- **order_items**: Order line items
- **payments**: Payment tracking

## Getting Started

### Prerequisites

- .NET 8.0 SDK (for local development)
- Docker and Docker Compose (for containerized deployment)

### Running with Docker Compose

1. Clone the repository:
```bash
git clone https://github.com/Ginwnn924/store_management.git
cd store_management
```

2. Start all services:
```bash
docker-compose up -d
```

This will start:
- MySQL database on port 3306
- Redis cache on port 6379
- .NET Web API on port 8080

3. Access the API:
- Swagger UI: http://localhost:8080
- API Base URL: http://localhost:8080/api

4. Stop all services:
```bash
docker-compose down
```

### Running Locally for Development

1. Ensure MySQL and Redis are running locally or update connection strings in `appsettings.json`

2. Navigate to the project directory:
```bash
cd StoreManagement/StoreManagement
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Run the application:
```bash
dotnet run
```

5. Access Swagger UI at http://localhost:5000

## API Endpoints

### Authentication

#### Register a New User
```
POST /api/auth/register
Content-Type: application/json

{
  "username": "string",
  "password": "string",
  "fullName": "string",
  "role": "staff" // or "admin"
}
```

#### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "username": "string",
  "password": "string"
}

Response:
{
  "token": "jwt-token-here",
  "username": "string",
  "role": "string"
}
```

## Authentication

The API uses JWT Bearer token authentication. After logging in:

1. Copy the token from the login response
2. In Swagger UI, click the "Authorize" button
3. Enter: `Bearer {your-token}`
4. Click "Authorize"

## Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "StoreManagement",
    "Audience": "StoreManagementAPI"
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db;Port=3306;Database=store_management;User=root;Password=rootpassword;"
  }
}
```

### Redis Connection
```json
{
  "Redis": {
    "ConnectionString": "cache:6379"
  }
}
```

## Docker Services

### MySQL (db)
- Image: mysql:latest
- Port: 3306
- Database: store_management
- Includes initial schema and seed data

### Redis (cache)
- Image: redis:latest
- Port: 6379

### .NET API (app)
- Built from Dockerfile
- Port: 8080
- Depends on db and cache services

## Development

### Building the Project
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

## Security Notes

- Change the default JWT secret key in production
- Use strong passwords for database users
- Keep sensitive configuration in environment variables
- Use HTTPS in production

## License

This project is licensed under the MIT License.

## Contributors

- Initial setup and configuration