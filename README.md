# ECommerce Inventory

ECommerce Inventory is an ASP.NET Core MVC application for managing products and customer orders. It uses Entity Framework Core with SQL Server LocalDB and separates web controllers from database access through a Unit of Work and a generic Repository Pattern.

## Features

- Product management with create, view, edit, and delete workflows.
- Order management with create, view, edit, and delete workflows.
- Inventory fields for SKU, product name, price, and stock quantity.
- Order fields for customer email, order date, and total amount.
- Philippine peso (`PHP`) formatting throughout the user interface.
- Responsive Bootstrap-based interface with shared navigation.
- Initial Entity Framework Core migration for the complete model.

## Architecture

Requests follow this path:

```text
Browser
  -> MVC Controller
  -> IUnitOfWork / UnitOfWork
  -> IRepository<T> / Repository<T>
  -> ApplicationDbContext
  -> SQL Server
```

Controllers do not inject or call `ApplicationDbContext`. `ProductsController` uses `IUnitOfWork.Products`, and `OrdersController` uses `IUnitOfWork.Orders`. The generic repository performs entity queries and state changes. The Unit of Work owns the shared context and commits changes through `SaveAsync()`.

All database services are registered with a scoped lifetime. This gives each web request one shared `ApplicationDbContext`, allowing changes made through multiple repositories to be tracked and saved together.

## Project structure

```text
ECommerceInventory/
├── Controllers/
│   ├── HomeController.cs
│   ├── OrdersController.cs
│   └── ProductsController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Migrations/
│   ├── 20260916002803_InitialCreate.cs
│   ├── 20260916002803_InitialCreate.Designer.cs
│   └── ApplicationDbContextModelSnapshot.cs
├── Models/
│   ├── ErrorViewModel.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Product.cs
├── Repositories/
│   ├── Interfaces/IRepository.cs
│   └── Implementations/Repository.cs
├── UnitOfWork/
│   ├── Interfaces/IUnitOfWork.cs
│   └── Implementations/UnitOfWork.cs
├── Views/
│   ├── Home/
│   ├── Orders/
│   ├── Products/
│   └── Shared/
├── wwwroot/
├── appsettings.json
├── ECommerceInventory.csproj
└── Program.cs
```

## Domain model

`Product` stores a product identifier, stock keeping unit, name, price, and available stock. A product can be referenced by many order items.

`Order` stores the customer email, order date, and total amount. An order can contain many order items.

`OrderItem` connects an order to a product through `OrderId` and `ProductId`. It also stores the quantity and unit price captured for that line item. EF Core recognizes both relationships through the matching foreign-key and navigation-property names.

## Technology

- .NET 8 and ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server provider for Entity Framework Core
- SQL Server LocalDB for local development
- Bootstrap for the responsive interface

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or another .NET-compatible IDE
- SQL Server LocalDB, normally installed with Visual Studio

## Database configuration

The connection string is stored in `appsettings.json` under `ConnectionStrings:DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommerceInventoryDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

For SQL Server Express, replace `Server=(localdb)\\MSSQLLocalDB` with the appropriate instance name, such as `Server=.\\SQLEXPRESS`.

## Running the project

From the project directory:

```bash
dotnet restore
dotnet build
dotnet run
```

The default MVC route is:

```text
{controller=Home}/{action=Index}/{id?}
```

Useful pages include `/Products` for inventory and `/Orders` for orders.

## Entity Framework Core migrations

The verified initial migration is `InitialCreate`. It creates the `Products`, `Orders`, and `OrderItems` tables, including primary keys, foreign keys, indexes, and the decimal fields used for prices and totals.

To apply the migration to the configured LocalDB database:

```bash
dotnet ef database update
```

To create a future migration after changing the models:

```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

Generated migration files belong in the `Migrations` folder and should be reviewed rather than manually rewritten.

## Repository and Unit of Work usage

`IRepository<T>` defines reusable asynchronous reads and entity state operations for any class entity. `Repository<T>` uses the `DbSet<T>` supplied by `ApplicationDbContext` and never calls `SaveChanges`.

`IUnitOfWork` exposes repositories for `Product`, `Order`, and `OrderItem`. `UnitOfWork` creates those repositories with the same context instance and provides `SaveAsync()` as the single commit point.

## Development notes

- Keep controllers dependent on `IUnitOfWork` or repository abstractions rather than `ApplicationDbContext`.
- Keep database writes inside the repository and Unit of Work flow.
- Keep connection strings in configuration instead of hardcoding them in the context.
- The current order form records the order header and total amount. The `OrderItem` model and relationships are ready for a future line-item workflow.
