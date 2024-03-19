CREATE DATABASE RestaurantAppDb;
GO

USE RestaurantAppDb;
GO

CREATE SCHEMA RestaurantAppSCHEMA;
GO

CREATE TABLE RestaurantAppSCHEMA.Auth (
    Email NVARCHAR(100),
    PasswordSalt VARBINARY(MAX),
    PasswordHash VARBINARY(MAX)
);

CREATE TABLE RestaurantAppSCHEMA.[User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email VARCHAR(100),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    PhoneNumber NVARCHAR(30) -- +40 733 155 544
);

CREATE TABLE RestaurantAppSCHEMA.Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES RestaurantAppSCHEMA.[User](UserId),
    TotalAmount DECIMAL,
    [Status] VARCHAR(30),
    OrderDate DATETIME,
    DeliveryDate DATETIME,
    PaymentId INT FOREIGN KEY REFERENCES RestaurantAppSCHEMA.Payments(PaymentId)
);


CREATE TABLE RestaurantAppSCHEMA.Menu (
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(30),
    [Description] NVARCHAR(MAX),
    Price DECIMAL
);

CREATE TABLE RestaurantAppSCHEMA.Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES RestaurantAppSCHEMA.Orders(OrderId),
    TotalAmount DECIMAL,
    PaymentDate DATETIME,
    PaymentStatus VARCHAR(30)
);
