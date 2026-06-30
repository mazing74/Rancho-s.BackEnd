# 🍔 Rancho's Restaurant — Backend API

A production-ready RESTful API for **Rancho's Restaurant** — a real client project. Built with ASP.NET Core 8 following Onion Architecture principles. Powers the full restaurant ordering system — from menu browsing to kitchen order management.

---

## 🚀 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 Web API |
| Database | SQL Server |
| ORM | Entity Framework Core 8 (Fluent API) |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Architecture | Onion Architecture (4 layers) |
| Mapping | AutoMapper |
| API Docs | Swagger / OpenAPI |

---

## 🏗️ Architecture

This project follows **Onion Architecture** — a clean separation of concerns across 4 layers:

```
┌──────────────────────────────────────────────────────┐
│                   API / PL Layer                     │
│         Controllers, Middleware, Program.cs          │
├──────────────────────────────────────────────────────┤
│                   Service Layer                      │
│          Business Logic, DTOs, AutoMapper            │
├──────────────────────────────────────────────────────┤
│                 Repository Layer                     │
│      EF Core, DbContext, Specifications, Repos       │
├──────────────────────────────────────────────────────┤
│                    Core Layer                        │
│           Entities, Interfaces, Enums                │
└──────────────────────────────────────────────────────┘
```

**Dependency Rule:** Every layer depends only on the layer below it. The **Core layer has zero external dependencies** — it knows nothing about EF Core, ASP.NET, or any framework.

---

## 🎯 Design Patterns Used

| Pattern | Where | Why |
|---------|-------|-----|
| **Onion Architecture** | Entire solution | Zero coupling between layers |
| **Generic Repository** | `GenericRepository<T>` | Reusable data access for all entities |
| **Specification Pattern** | Repository layer | Composable, testable query logic |
| **DTO Pattern** | Service layer | Never expose domain entities to clients |
| **AutoMapper Profiles** | Service layer | Clean mapping between entities and DTOs |
| **Soft Delete** | All entities | Preserve order history integrity |

---

## 📁 Project Structure

```
RanchosAPI/
│
├── Core/                              # Domain layer — zero dependencies
│   ├── Entities/
│   │   ├── AppUser.cs                 # Extended IdentityUser<int>
│   │   ├── AppRole.cs                 # Extended IdentityRole<int>
│   │   ├── Category.cs
│   │   ├── Product.cs
│   │   ├── OptionGroup.cs             # e.g. "Size", "Single or Double"
│   │   ├── Option.cs                  # e.g. "150g +0LE", "200g +30LE"
│   │   ├── Order.cs
│   │   ├── OrderItem.cs               # Price snapshot at order time
│   │   ├── OrderItemOption.cs         # Selected option snapshot
│   │   ├── OrderStatusHistory.cs      # Full audit trail
│   │   ├── BaseEntity.cs              # Id, CreatedAt, UpdatedAt
│   │   └── Enums.cs                   # OrderStatus, OrderType, PaymentMethod
│   └── Interfaces/
│       ├── IGenericRepository.cs
│       ├── IProductRepository.cs
│       ├── ICategoryRepository.cs
│       ├── IOptionGroupRepository.cs
│       ├── IOrderRepository.cs
│       └── IAuthRepository.cs
│
├── Repository/                        # Data access layer
│   ├── Data/
│   │   ├── AppDbContext.cs            # IdentityDbContext<AppUser, AppRole, int>
│   │   ├── DataSeed/
│   │   │   └── RanchosSeed.cs         # Menu seed data
│   │   └── Configurations/            # EF Core Fluent API per entity
│   │       ├── ProductConfiguration.cs
│   │       ├── CategoryConfiguration.cs
│   │       ├── OrderConfiguration.cs
│   │       ├── OrderItemConfiguration.cs
│   │       └── ...
│   └── Repositories/
│       ├── GenericRepository.cs       # Generic Repo + Specification support
│       ├── ProductRepository.cs
│       ├── CategoryRepository.cs
│       ├── OptionGroupRepository.cs
│       ├── OrderRepository.cs
│       └── AuthRepository.cs
│
├── Service/                           # Business logic layer
│   ├── DTOs/
│   │   ├── AuthDto.cs
│   │   ├── ProductDto.cs
│   │   ├── CategoryDto.cs
│   │   ├── OptionDto.cs
│   │   └── OrderDto.cs
│   ├── Mapping/                       # AutoMapper profiles
│   │   ├── ProductMappingProfile.cs
│   │   ├── OrderMappingProfile.cs
│   │   └── OptionMappingProfile.cs
│   └── Services/
│       ├── AuthService.cs
│       ├── ProductService.cs
│       ├── CategoryService.cs
│       ├── OptionService.cs
│       └── OrderService.cs
│
└── API/                               # Presentation layer
    ├── Controllers/
    │   ├── APiBaseController.cs        # Shared [ApiController] + [Route]
    │   ├── AuthController.cs
    │   ├── ProductsController.cs
    │   ├── CategoriesController.cs
    │   ├── OptionsController.cs
    │   ├── OrdersController.cs
    │   └── UploadsController.cs
    ├── Middleware/
    │   └── ExceptionMiddleware.cs      # Global JSON error handling
    ├── Services/
    │   └── ImageService.cs            # Local image storage
    ├── Errors/
    │   └── AppException.cs
    └── Program.cs
```

---

## ✅ Features Implemented

### 🔐 Authentication & Authorization
- Register / Login with JWT Bearer tokens
- ASP.NET Core Identity — password hashing, account lockout (5 attempts → 5 min lock)
- Role-based authorization: **Admin**, **Customer**, **KitchenStaff**
- `[Authorize(Roles = "...")]` on all protected endpoints
- JWT scheme explicitly set as default to override Identity's cookie scheme

### 🍽️ Menu System
- Full **Category** CRUD — Beef Burger, Fried Chicken, Pasta, Rice, Sides, Drinks...
- Full **Product** CRUD — Arabic & English name support
- **Product Options & Sizes** — weight-based (150g/200g/300g/400g) and Single/Double
- Option groups with `single` / `multiple` selection types and `IsRequired` flag
- Soft delete on all menu items — order history always stays intact
- Featured items flag, real-time availability toggle

### 📦 Order System
- Place orders: **Delivery**, **Pickup**, **Dine-in**
- **Server-side price calculation** — client prices never trusted
- **Price snapshots** — ProductName, UnitPrice, OptionName copied at order time
- Option validation — selected options must belong to the ordered product
- Full status workflow with transition validation:
  ```
  Pending → Confirmed → Preparing → Ready → OutForDelivery → Delivered
                    ↘ Rejected          ↘ Cancelled
  ```
- Complete `OrderStatusHistory` audit trail — who changed what and when
- Customer order history, order detail, and cancellation (Pending only)

### 🖼️ Image Upload
- Upload product images → saved to `wwwroot/images/products/`
- File type validation (jpg, jpeg, png, webp only)
- File size limit: 5MB max
- GUID-based unique filenames — no collisions
- Auto-delete old image file when product image is replaced

### 🛡️ Security & Quality
- Global exception middleware — consistent JSON error shape across all endpoints
- CORS policy configured for Angular (`localhost:4200`)
- Enums stored as strings in DB — human-readable in SSMS
- Database indexes on `CategoryId`, `CustomerId`, `Status`, `BranchId`, `CreatedAt`
- `DeleteBehavior.Restrict` on Customer→Orders — prevents accidental data loss
- `DeleteBehavior.Cascade` on Order→OrderItems — clean order deletion

---

## 🗄️ Database Schema

```
AspNetUsers (AppUser)
  └── Orders (CustomerId FK, Restrict)
        ├── OrderItems
        │     └── OrderItemOptions    ← snapshots of selected options
        └── OrderStatusHistory        ← full audit trail

AspNetRoles (AppRole)
AspNetUserRoles

Categories
  └── Products (CategoryId FK, Restrict)
        └── OptionGroups
              └── Options
```

**14+ tables total.** All relationships explicitly configured via EF Core Fluent API.

---

## 🔌 API Endpoints

### Auth
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/api/auth/register` | Public | Create customer account |
| POST | `/api/auth/login` | Public | Login, receive JWT token |

### Categories
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/categories` | Public | All active categories |
| GET | `/api/categories/{id}` | Public | Single category |
| GET | `/api/categories/{id}/products` | Public | Category with all products |
| POST | `/api/categories` | Admin | Create category |
| PUT | `/api/categories/{id}` | Admin | Update category |
| DELETE | `/api/categories/{id}` | Admin | Soft delete |

### Products
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/products` | Public | All active products |
| GET | `/api/products/{id}` | Public | Product with option groups |
| GET | `/api/products/category/{id}` | Public | Products by category |
| POST | `/api/products` | Admin | Create product |
| PUT | `/api/products/{id}` | Admin | Update product |
| DELETE | `/api/products/{id}` | Admin | Soft delete |
| PATCH | `/api/products/{id}/toggle-availability` | Admin/Kitchen | Mark sold out |

### Options
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/options/product/{id}` | Public | All option groups for a product |
| POST | `/api/options/groups` | Admin | Create option group + options |
| DELETE | `/api/options/groups/{id}` | Admin | Delete option group |

### Orders
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/api/orders` | Customer | Place an order |
| GET | `/api/orders/my-orders` | Customer | My order history |
| GET | `/api/orders/{id}` | Customer/Admin | Full order details |
| POST | `/api/orders/{id}/cancel` | Customer | Cancel pending order |
| PATCH | `/api/orders/{id}/status` | Admin/Kitchen | Update order status |
| GET | `/api/orders/kitchen` | Admin/Kitchen | Kitchen live dashboard |
| GET | `/api/orders/admin` | Admin | All orders with filters |

### Uploads
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/api/uploads/image` | Admin | Upload product image |
| DELETE | `/api/uploads/image` | Admin | Delete image by URL |

---

## ⚙️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ranchos-api.git
cd ranchos-api
```

### 2. Configure appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RanchosDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "RanchosAPI",
    "Audience": "RanchosApp",
    "ExpiryDays": "7"
  }
}
```

> ⚠️ Never commit real secrets. Use `appsettings.Development.json` locally and environment variables in production.

### 3. Apply Migrations

Open **Package Manager Console**, set Default Project to `Repository`:

```powershell
Add-Migration InitialCreate -StartupProject API
Update-Database -StartupProject API
```

This creates the database, all tables, and seeds the 3 default roles automatically.

### 4. Run

Press **F5** in Visual Studio or:
```bash
dotnet run --project API
```

### 5. Open Swagger
```
https://localhost:{port}/swagger
```

---

## 🔑 Testing Protected Endpoints in Swagger

```
1. POST /api/auth/register  → create an account
2. POST /api/auth/login     → copy the token from the response
3. Click Authorize 🔒       → enter:  Bearer eyJhbGci...
4. All protected endpoints are now unlocked
```

---

## 🌱 Seed Data

Roles are automatically seeded on first migration:

| Id | Role | Access |
|----|------|--------|
| 1 | Admin | Full system access |
| 2 | Customer | Browse menu, place orders |
| 3 | KitchenStaff | View & update order status |

> To create your first Admin: register via API → open SSMS → insert into `AspNetUserRoles` with `RoleId = 1`.

---

## 🗺️ Roadmap

- [x] Onion Architecture — 4 layers
- [x] Generic Repository + Specification Pattern
- [x] AutoMapper profiles (Product, Order, Option)
- [x] ASP.NET Core Identity + JWT authentication
- [x] Role-based authorization
- [x] Menu system — Categories, Products, Options & Sizes
- [x] Customer ↔ Order relationship (Restrict delete)
- [x] Complete order lifecycle — Delivery / Pickup / DineIn
- [x] Server-side price calculation with option pricing
- [x] Price & option snapshots on order items
- [x] Order status workflow with audit history
- [x] Image upload with validation
- [x] Global exception middleware
- [x] CORS for Angular
- [ ] Coupons & Discounts
- [ ] FluentValidation
- [ ] Pagination
- [ ] Admin sales reports
- [ ] SignalR real-time kitchen notifications
- [ ] Angular frontend

---

## 👨‍💻 About This Project

This is a **real client project** for Rancho's Restaurant — not a tutorial clone.

Built entirely from scratch as a junior-to-mid level portfolio project, covering real-world concerns: security, data integrity, price calculation, order workflows, and clean architecture.

**Restaurant:** Rancho's — *Feel The Taste* 🍔
**Location:** Mit Ghamr, Egypt
**Social:** [@RanchosEG](https://instagram.com/RanchosEG)

---

## 📄 License

Built for educational and portfolio purposes.
ENDOFFILE
