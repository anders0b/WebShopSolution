CREATE TABLE [dbo].[Products] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NULL,
	[Description] NVARCHAR(MAX) NULL,
	[Price] DECIMAL(18, 2) NULL,
	[Stock] INT NULL
);
CREATE TABLE [dbo].[Customers] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FirstName] NVARCHAR(100) NULL,
	[LastName] NVARCHAR(100) NULL,
	[Address] NVARCHAR(100) NULL,
	[PostalCode] INT NULL,
	[City] NVARCHAR(100) NULL,
	[Email] NVARCHAR(100) NULL,
	[Phone] NVARCHAR(100) NULL,
);
CREATE TABLE [dbo].[Orders] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[OrderDate] DATETIME NULL,
	[CustomerId] INT NULL,
	[IsShipped] BIT NULL
	FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
CREATE TABLE [dbo].[OrderItems] (
	[OrderId] INT NOT NULL,
	[ProductId] INT NOT NULL,
	PRIMARY KEY ([OrderId], [ProductId]),
	FOREIGN KEY (OrderId) REFERENCES Orders(Id),
	FOREIGN KEY (ProductId) REFERENCES Products(Id)
);