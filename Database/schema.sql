-- Expense Tracker Database Schema
-- This file contains the database structure for the expense tracking application

-- Transactions Table
-- Stores all income, expense, and savings transactions
CREATE TABLE Transactions
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    Type TEXT NOT NULL,         -- Income, Expense, Savings
    Category TEXT NOT NULL,
    Amount REAL NOT NULL,
    Notes TEXT
);

-- Goals Table
-- Stores financial goals and savings progress
CREATE TABLE Goals
(
    Id BIGSERIAL PRIMARY KEY,
    GoalName VARCHAR(100) NOT NULL,
    TargetAmount DECIMAL(18,2) NOT NULL,
    SavedAmount DECIMAL(18,2) DEFAULT 0
);

-- Categories Table
-- Stores transaction categories for organization
CREATE TABLE Categories
(
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Type VARCHAR(20) NOT NULL -- Income, Expense, Savings
);
