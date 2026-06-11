# Expense Tracker Application

A comprehensive console-based expense tracking application built with C# and Entity Framework Core.

## 🎯 Features

- **Add Expenses**: Record new expenses with description, amount, and category
- **View All Expenses**: Display all recorded expenses with sorting and totals
- **Filter by Category**: View and analyze expenses by category
- **Calculate Totals**: Get total expenses across all records
- **Advanced Reporting**:
  - Expenses by category with percentages
  - Monthly expense reports
  - Average expense calculations
  - Highest expense tracking
  - Date range filtering
- **Delete Expenses**: Remove specific expenses by ID
- **Update Expenses**: Modify existing expense records
- **Dependency Injection**: Professional service architecture
- **Logging**: Built-in logging for debugging and monitoring
- **Beautiful UI**: Enhanced console interface with emoji indicators

## 📁 Project Structure

```
ExpenseTracker/
├── Data/
│   └── ExpenseDbContext.cs              # Entity Framework DbContext
├── Models/
│   └── Expense.cs                       # Expense entity model
├── Services/
│   ├── ExpenseService.cs                # Core expense operations
│   └── ReportService.cs                 # Reporting and analytics
├── Program.cs                           # Main application entry point
├── ExpenseTracker.csproj                # Project configuration
├── README.md                            # This file
└── .gitignore                           # Git ignore rules
```

## 📦 Namespaces

```csharp
namespace ExpenseTracker;                   // Main namespace
namespace ExpenseTracker.Data;              // Database context
namespace ExpenseTracker.Models;            // Data models
namespace ExpenseTracker.Services;          // Business logic services
```

## 📦 NuGet Packages

| Package | Version | Purpose |
|---------|---------|----------|
| Microsoft.EntityFrameworkCore | 8.0.0 | ORM framework |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.0 | SQL Server database provider |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | Database migration tools |
| Microsoft.Extensions.Logging | 8.0.0 | Logging framework |
| Microsoft.Extensions.Logging.Console | 8.0.0 | Console logger implementation |
| Microsoft.Extensions.DependencyInjection | 8.0.0 | Dependency injection container |
| Microsoft.Extensions.Configuration | 8.0.0 | Configuration management |
| Microsoft.Extensions.Configuration.Json | 8.0.0 | JSON configuration provider |
| System.ComponentModel.DataAnnotations | 4.7.0 | Data validation attributes |

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server LocalDB (or SQL Server)
- Git

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Sadiq2605/ExpanseTransaction_sadiq.git
   cd ExpanseTransaction_sadiq
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Create the database**:
   ```bash
   dotnet ef database update
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

## 💻 Usage Guide

### Main Menu Options

```
╔════════════════════════════════════════╗
║     EXPENSE TRACKER APPLICATION        ║
╚════════════════════════════════════════╝

📊 MAIN MENU:
1. ➕ Add Expense
2. 📋 View All Expenses
3. 🏷️  View Expenses by Category
4. 💰 Get Total Expenses
5. 📈 View Reports
6. ❌ Delete Expense
7. 🚪 Exit
```

### Example Workflow

#### 1. Add an Expense
```
Select an option (1-7): 1

📝 Enter description: Grocery Shopping
💵 Enter amount: 50.00
🏷️  Enter category: Food

✅ Expense added successfully!
   Description: Grocery Shopping
   Amount: $50.00
   Category: Food
```

#### 2. View All Expenses
```
Select an option (1-7): 2

ID    Description          Amount       Category        Date
─────────────────────────────────────────────────────────────
1     Grocery Shopping     $50.00       Food            2024-06-11
2     Taxi Ride           $15.50       Transport       2024-06-11
                                        Total:          $65.50
```

#### 3. View Reports
```
Select an option (1-7): 5

1. 📊 Expenses by Category
2. 📅 Monthly Expenses
3. 📈 Average Expense
4. 🔝 Highest Expense
5. 📆 Expenses in Date Range
6. ⬅️  Back to Main Menu
```

## 🏗️ Architecture

### Service Layer

The application follows a clean architecture pattern with dependency injection:

- **ExpenseService**: Handles all CRUD operations for expenses
- **ReportService**: Generates various reports and analytics
- **ExpenseDbContext**: Entity Framework configuration and database access

### Data Model

```csharp
public class Expense
{
    public int Id { get; set; }                    // Primary key
    public string Description { get; set; }        // Expense description (max 200 chars)
    public decimal Amount { get; set; }            // Expense amount
    public string Category { get; set; }           // Category (max 50 chars)
    public DateTime Date { get; set; }             // Transaction date
}
```

## 🔧 Configuration

### Database Connection

Edit `Data/ExpenseDbContext.cs` to change the connection string:

```csharp
// Default: LocalDB
optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ExpenseTrackerDb;Trusted_Connection=true;");

// For SQL Server:
optionsBuilder.UseSqlServer(@"Server=YOUR_SERVER;Database=ExpenseTrackerDb;User Id=sa;Password=YOUR_PASSWORD;");
```

## 📊 Common Categories

- **Food**: Groceries, restaurants, dining
- **Transport**: Taxi, bus, fuel, parking
- **Entertainment**: Movies, games, events
- **Utilities**: Electricity, water, internet
- **Healthcare**: Medicine, doctor visits
- **Shopping**: Clothing, household items
- **Education**: Books, courses, tuition
- **Other**: Miscellaneous expenses

## 🐛 Troubleshooting

### Database Connection Error

```
Microsoft.Data.SqlClient.SqlException: Login failed for user
```

**Solution**: Ensure SQL Server LocalDB is running:
```bash
sqllocaldb start mssqllocaldb
```

### Entity Framework Errors

```
The database already exists. Choosing overwrite
```

**Solution**: Delete the existing database or create migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🚀 Future Enhancements

- [ ] Web UI using ASP.NET Core
- [ ] Budget alerts and limits
- [ ] Export to CSV/PDF/Excel
- [ ] Recurring expenses
- [ ] Budget forecasting
- [ ] Mobile app
- [ ] Cloud database integration
- [ ] Multi-user support
- [ ] Advanced filtering and search
- [ ] Data visualization/charts

## 📝 License

This project is open source and available under the MIT License.

## 👤 Author

Created by **Sadiq2605**

- GitHub: [@Sadiq2605](https://github.com/Sadiq2605)
- Repository: [ExpanseTransaction_sadiq](https://github.com/Sadiq2605/ExpanseTransaction_sadiq)

## 💬 Support

For issues, questions, or suggestions, please open an issue on GitHub.

---

**Made with ❤️ using C# and .NET 8.0**
