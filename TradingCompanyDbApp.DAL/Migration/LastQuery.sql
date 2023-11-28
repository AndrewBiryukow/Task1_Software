-- Таблиця користувачів
CREATE TABLE users (
    Id INT IDENTITY PRIMARY KEY ,
    Nickname NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100) UNIQUE,
    Phone NVARCHAR(20) UNIQUE,
    Address NVARCHAR(255),
    Gender NVARCHAR(10),
    BankCardNumber VARCHAR(32),
    RecoveryKeyword NVARCHAR(50),
    Balance INT,
    CreatedAt DATETIME2 DEFAULT GETDATE(),  
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);


-- Таблиця товарів
CREATE TABLE products (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Price INT,
    Quantity INT,
    Description TEXT,
    CreatedAt DATETIME2 DEFAULT GETDATE(),  
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);


-- Таблиця замовлень
CREATE TABLE orders (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    OrderDate DATETIME2 DEFAULT GETDATE(),
    Amount INT,
    Address TEXT,
    Status VARCHAR(20),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_orders_users FOREIGN KEY (UserId) REFERENCES users (Id),
    CONSTRAINT FK_orders_products FOREIGN KEY (ProductId) REFERENCES products (Id)
);
