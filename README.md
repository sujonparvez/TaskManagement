# TaskManagement

A Task Management System API built with **.NET 8**, following **Clean Architecture** principles.

## Technologies

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (SQL Server)
- **RabbitMQ** (Message Broker)
- **SignalR** (Real-time Notifications)
- **Serilog** (Logging)
- **Swagger** (API Documentation)

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer)
- [RabbitMQ](https://www.rabbitmq.com/download.html) (Running on localhost or via Docker)

## Configuration

Update the `Api/appsettings.json` file with your local configuration if necessary.

### Database Connection
Ensure your SQL Server instance is running. The default connection string is configured for `SQLEXPRESS`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=DESKTOP-J4VVICC\\SQLEXPRESS;Database=TaskManagementDb;User Id=sa;password=open;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;"
}
```

> **Note:** The application attempts to **automatically create and seed** the database on startup using `EnsureCreated()`.

### Message Broker (RabbitMQ)
Ensure RabbitMQ is running. Default configuration:

```json
"MessageBroker": {
  "Host": "localhost",
  "UserName": "guest",
  "Password": "guest"
}
```

## Getting Started

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd TaskManagement
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Run the Application:**
    Navigate to the `Api` project and run:
    ```bash
    cd Api
    dotnet run
    ```
    
    *Alternatively, run from the root:*
    ```bash
    dotnet run --project Api
    ```

4.  **Access the API:**
    Once running, the API will be available at:
    - **Swagger UI:** `https://localhost:7192/swagger/index.html` (Port may vary, check console output)

## Project Structure

- **Api**: The entry point of the application (Controllers, Middleware, Configuration).
- **Application**: Business logic, Use cases (CQRS), Interfaces.
- **Domain**: Enterprise logic, Entities.
- **Infrastructure**: Database access, External services implementation (RabbitMQ, SignalR).