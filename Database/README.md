# Database Schema

This directory contains the SQL database schema for the Expense Tracker application.

## Files

- **schema.sql** - Main database schema with all table definitions

## Tables

### Transactions
Stores all financial transactions (income, expenses, savings)
- `Id` - Unique transaction identifier
- `Date` - Transaction date
- `Type` - Transaction type (Income, Expense, Savings)
- `Category` - Transaction category
- `Amount` - Transaction amount
- `Notes` - Additional transaction notes

### Goals
Stores financial goals and tracks savings progress
- `Id` - Unique goal identifier
- `GoalName` - Name of the goal
- `TargetAmount` - Target amount to save
- `SavedAmount` - Current amount saved (default: 0)

### Categories
Stores available transaction categories
- `Id` - Unique category identifier
- `Name` - Category name
- `Type` - Category type (Income, Expense, Savings)

## Usage

To initialize the database, run:
```sql
sqlite3 expense_tracker.db < Database/schema.sql
```

Or if using SQL Server/PostgreSQL, adapt the schema accordingly.
