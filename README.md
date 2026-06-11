# Expense Tracker Application

A console-based expense tracking application built with C# and Entity Framework Core.

## Features

- **Add Expenses**: Record new expenses with description, amount, and category
- **View All Expenses**: Display all recorded expenses sorted by date
- **Filter by Category**: View expenses for a specific category
- **Calculate Totals**: Get total expenses across all records
- **Delete Expenses**: Remove specific expenses by ID
- **Generate Reports**: View expenses by category, monthly totals, and averages

## Project Structure

```
ExpenseTracker/
├── Models/
│   └── Expense.cs              # Expense model entity
├── Data/
│   └── ExpenseDbContext.cs     # Entity Framework DbContext
├── Services/
│   ├── ExpenseService.cs       # Core expense operations
│   └── ReportService.cs        # Reporting and analytics
├── Program.cs                  # Main application entry point
├── ExpenseTracker.csproj       # Project configuration
└── README.md                   # This file
```

## Namespaces

- `ExpenseTracker` - Main namespace
- `ExpenseTracker.Models` - Data models
- `ExpenseTracker.Data` - Database context
- `ExpenseTracker.Services` - Business logic services

## NuGet Packages

- **Microsoft.EntityFrameworkCore** (v8.0.0) - ORM framework
- **Microsoft.EntityFrameworkCore.SqlServer** (v8.0.0) - SQL Server provider
- **Microsoft.EntityFrameworkCore.Tools** (v8.0.0) - Database tools
- **Microsoft.Extensions.Logging** (v8.0.0) - Logging framework
- **Microsoft.Extensions.Logging.Console** (v8.0.0) - Console logger
- **Microsoft.Extensions.DependencyInjection** (v8.0.0) - Dependency injection
- **Microsoft.Extensions.Configuration** (v8.0.0) - Configuration management
- **Microsoft.Extensions.Configuration.Json** (v8.0.0) - JSON configuration
- **System.ComponentModel.DataAnnotations** (v4.7.0) - Data validation

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server LocalDB or SQL Server

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Sadiq2605/ExpanseTransaction_sadiq.git
   cd ExpanseTransaction_sadiq
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

### Running the Application

```bash
dotnet run
```

### Database Setup

The application uses Entity Framework Code-First approach with LocalDB. The database will be created automatically on first run.

To manually create the database:
```bash
dotnet ef database update
```

## Usage

Once running, follow the interactive menu:

1. **Add Expense** - Enter description, amount, and category
2. **View All Expenses** - See all recorded expenses
3. **View Expenses by Category** - Filter expenses by category
4. **Get Total Expenses** - Calculate sum of all expenses
5. **Delete Expense** - Remove an expense by ID
6. **Exit** - Close the application

## Example

```
=== Expense Tracker Menu ===
1. Add Expense
2. View All Expenses
3. View Expenses by Category
4. Get Total Expenses
5. Delete Expense
6. Exit

Select an option: 1
Enter description: Grocery Shopping
Enter amount: 45.50
Enter category: Food
Expense added successfully!
```

## Future Enhancements

- [ ] Monthly expense reports
- [ ] Budget limits and alerts
- [ ] Export to CSV/PDF
- [ ] Web UI interface
- [ ] Mobile app
- [ ] Advanced filtering and sorting

## License

This project is open source and available under the MIT License.

## Author

Created by Sadiq2605
