DROP TABLE Inventories;
DROP TABLE Products;
DROP TABLE Categories;
DROP TABLE Manufacturers;

CREATE TABLE Manufacturers (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    Price MONEY NOT NULL,
    ManufacturerId INT NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE Inventories (
    Id INT PRIMARY KEY IDENTITY,
    Quantity INT NOT NULL,
    ProductId INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);