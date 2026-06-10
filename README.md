# 🍔 Rancho's Restaurant — Backend API

A production-ready RESTful API for **Rancho's Restaurant** built with ASP.NET Core, following Clean Architecture (Onion Architecture) principles. This backend powers the full restaurant ordering system — from menu browsing to order tracking.

---

## 🚀 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 Web API |
| Database | SQL Server |
| ORM | Entity Framework Core 8 |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Architecture | Onion Architecture (4 layers) |
| API Docs | Swagger / OpenAPI |

---

## 🏗️ Architecture

This project follows **Onion Architecture** — a clean separation of concerns across 4 layers:

```
┌─────────────────────────────────────┐
│           API / PL Layer            │  ← Controllers, Middleware, Program.cs
├─────────────────────────────────────┤
│          Service Layer              │  ← Business Logic, DTOs, Services
├─────────────────────────────────────┤
│        Repository Layer             │  ← EF Core, DbContext, Repositories
├─────────────────────────────────────┤
│           Core Layer                │  ← Entities, Interfaces, Enums
└─────────────────────────────────────┘
```

**Dependency Rule:** Every layer depends only on the layer below it. The Core layer has zero external dependencies.

---

## 📁 Project Structure

```
RanchosAPI/
│
├── Core/                          # Domain layer — no dependencies
│   ├── Entities/
│   │   ├── AppUser.cs             # Identity user (extended)
│   │   ├── AppRole.cs             # Identity role (extended)
│   │   ├── Category.cs
│   │   ├── Product.cs
│   │   ├── OptionGroup.cs         # e.g. "Size", "Single or Double"
│   │   ├── Option.cs              # e.g. "150g", "200g", "Single"
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── OrderItemOption.cs
│   │   ├── OrderStatusHistory.cs
│   │   ├── BaseEntity.cs
│   │   └── Enums.cs               # OrderStatus, OrderType, PaymentMethod
│   └── Interfaces/
│       ├── IGenericRepository.cs
│       ├── IProductRepository.cs
│       ├── ICategoryRepository.cs
│       ├── IOptionGroupRepository.cs
│       ├── IOrderRepository.cs
│       └── IAuthRepository.cs
│
├── Repository/                    # Data access layer
│   ├── Data/
│   │   ├── AppDbContext.cs        # IdentityDbContext
│   │   └── Configurations/        # EF Core Fluent API configs
│   └── Repositories/
│       ├── GenericRepository.cs
│       ├── ProductRepository.cs
│       ├── CategoryRepository.cs
│       ├── OptionGroupRepository.cs
│       ├── OrderRepository.cs
│       └── AuthRepository.cs
│
├── Service/                       # Business logic layer
│   ├── DTOs/
│   │   ├── AuthDto.cs
│   │   ├── ProductDto.cs
│   │   ├── CategoryDto.cs
│   │   ├── OptionDto.cs
│   │   └── OrderDto.cs
│   └── Services/
│       ├── AuthService.cs
│       ├── ProductService.cs
│       ├── CategoryService.cs
│       ├── OptionService.cs
│       └── OrderService.cs
│
└── API/                           # Presentation layer
    ├── Controllers/
    │   ├── AuthController.cs
    │   ├── ProductsController.cs
    │   ├── CategoriesController.cs
    │   ├── OptionsController.cs
    │   ├── OrdersController.cs
    │   └── UploadsController.cs
    ├── Middleware/
    │   └── ExceptionMiddleware.cs
    ├── Services/
    │   └── ImageService.cs
    ├── Errors/
    │   └── AppException.cs
    └── Program.cs
```

---

## ✅ Features Implemented

### 🔐 Authentication & Authorization
- Register / Login with JWT tokens
- ASP.NET Core Identity (password hashing, lockout, validation)
- Role-based access control: **Admin**, **Customer**, **KitchenStaff**
- Protected endpoints with `[Authorize(Roles = "...")]`

### 🍽️ Menu System
- Full **Category** CRUD (Beef Burger, Fried Chicken, Pasta, Rice, Sides...)
- Full **Product** CRUD with Arabic name support
- **Product Options & Sizes** — Single/Double, 150g/200g/300g/400g
- Option groups with required/optional selection types
- Soft delete (items never hard deleted — orders stay intact)
- Featured items, availability toggle

### 📦 Order System
- Place orders: **Delivery**, **Pickup**, **Dine-in**
- Server-side price calculation — client prices never trusted
- Price snapshots on order items (historical accuracy)
- Full order status workflow:
  ```
  Pending → Confirmed → Preparing → Ready → OutForDelivery → Delivered
                                          ↘ Cancelled / Rejected
  ```
- Complete status change history with timestamps
- Customer order history & cancellation (Pending only)

### 🖼️ Image Upload
- Upload product images to server (`wwwroot/images/products/`)
- File type validation (jpg, jpeg, png, webp)
- File size limit (5MB)
- Unique filename generation (GUID-based)
- Auto-delete old image when product image is updated

### 🛡️ Security & Quality
- Global exception handling middleware (clean JSON errors always)
- CORS configured for Angular frontend (`localhost:4200`)
- Enum values stored as strings in DB (human-readable in SSMS)
- Database indexes on frequently queried columns
- `DeleteBehavior.Restrict` on critical relationships

---

## 🗄️ Database Schema

```
AspNetUsers ──────────────────────────────┐
AspNetRoles                               │
AspNetUserRoles                           │
                                          │
Categories                                │
  └── Products                            │
        ├── OptionGroups                  │
        │     └── Options                 │
        └── ProductImages                 │
                                          │
Orders ◄──────────────────────────────────┘
  ├── OrderItems
  │     └── OrderItemOptions
  └── OrderStatusHistory
```

---

## 🔌 API Endpoints

### Auth
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/api/auth/register` | Public | Create customer account |
| POST | `/api/auth/login` | Public | Login, get JWT token |

### Menu
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/categories` | Public | All active categories |
| GET | `/api/categories/{id}/products` | Public | Category with products |
| POST | `/api/categories` | Admin | Create category |
| PUT | `/api/categories/{id}` | Admin | Update category |
| DELETE | `/api/categories/{id}` | Admin | Soft delete category |
| GET | `/api/products` | Public | All active products |
| GET | `/api/products/{id}` | Public | Product with options |
| GET | `/api/products/category/{id}` | Public | Products by category |
| POST | `/api/products` | Admin | Create product |
| PUT | `/api/products/{id}` | Admin | Update product |
| DELETE | `/api/products/{id}` | Admin | Soft delete product |
| PATCH | `/api/products/{id}/toggle-availability` | Admin/Kitchen | Mark sold out |

### Options
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/options/product/{id}` | Public | Get product options |
| POST | `/api/options/groups` | Admin | Create option group |
| DELETE | `/api/options/groups/{id}` | Admin | Delete option group |

### Orders
| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| POST | `/api/orders` | Customer | Place an order |
| GET | `/api/orders/my-orders` | Customer | Order history |
| GET | `/api/orders/{id}` | Customer/Admin | Order details |
| POST | `/api/orders/{id}/cancel` | Customer | Cancel pending order |
| PATCH | `/api/orders/{id}/status` | Admin/Kitchen | Update status |
| GET | `/api/orders/kitchen` | Admin/Kitchen | Kitchen dashboard |
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
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ranchos-api.git
cd ranchos-api
```

### 2. Configure the Connection String

Open `API/appsettings.json` and update:

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

> ⚠️ **Never commit real secrets to GitHub.** Use environment variables or `appsettings.Development.json` (add to `.gitignore`) for sensitive values.

### 3. Run Migrations

Open **Package Manager Console** in Visual Studio, set Default Project to `Repository`, then run:

```powershell
Add-Migration InitialCreate -StartupProject API
Update-Database -StartupProject API
```

This will:
- Create the database
- Create all tables
- Seed the 3 default roles (Admin, Customer, KitchenStaff)

### 4. Run the Project

```bash
cd API
dotnet run
```

Or press **F5** in Visual Studio.

### 5. Open Swagger

Navigate to:
```
https://localhost:{port}/swagger
```

---

## 🔑 How to Test Protected Endpoints in Swagger

1. Call `POST /api/auth/register` to create an account
2. Call `POST /api/auth/login` to get your token
3. Click the **Authorize 🔒** button at the top of Swagger
4. Enter: `Bearer eyJhbGci...` (your token)
5. Now all protected endpoints are unlocked

---

## 🌱 Seed Data

After running migrations, the following roles are automatically seeded:

| Id | Role | Description |
|----|------|-------------|
| 1 | Admin | Full system access |
| 2 | Customer | Can browse menu and place orders |
| 3 | KitchenStaff | Can view and update order status |

To create your first **Admin** user, register normally via the API then manually update the `AspNetUserRoles` table in SSMS to assign RoleId = 1.

---

## 🗺️ Roadmap

- [x] Onion Architecture setup
- [x] Microsoft Identity authentication
- [x] Menu system (Categories + Products)
- [x] Product Options & Sizes
- [x] Order system (Delivery/Pickup/DineIn)
- [x] Image upload
- [x] Kitchen dashboard endpoint
- [ ] Coupons & Discounts
- [ ] Input validation (FluentValidation)
- [ ] Pagination on list endpoints
- [ ] Admin sales reports
- [ ] SignalR real-time order notifications
- [ ] Angular frontend

---

## 👨‍💻 Author

Built step by step as a real-world learning project.

Restaurant: **Rancho's** — Feel The Taste 🍔

---

## 📄 License

This project is for educational and portfolio purposes.
