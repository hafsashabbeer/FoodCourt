--CREATE DATABASE FoodCourt
USE FoodCourt

CREATE TABLE ItemMaster
(
	ItemTypeNo INT NOT NULL IDENTITY(101,1) PRIMARY KEY,
	ItemType NVARCHAR(30) NOT NULL
)

CREATE TABLE Items
(
	ItemNo INT NOT NULL IDENTITY(201,1) PRIMARY KEY,
	ItemName NVARCHAR(30) NOT NULL,
	ItemTypeNo INT NOT NULL FOREIGN KEY REFERENCES ItemMaster(ItemTypeNo),
	ItemQuantity INT NOT NULL,
	ItemPrice INT NOT NULL 
)

ALTER TABLE StockTrans
ADD [Date] Date NOT NULL

CREATE TABLE StockTrans
(
	TransId INT NOT NULL IDENTITY(301,1) PRIMARY KEY,
	ItemNo INT NOT NULL FOREIGN KEY REFERENCES Items(ItemNo),
	StockQuantity INT NOT NULL,
	StockAmount INT NOT NULL,
	[Date] Date NOT NULL
)

CREATE TABLE BillMaster
(
	BMId INT NOT NULL IDENTITY(401,1) PRIMARY KEY,
	TotalSales INT NOT NULL,
	TotalAmount INT NOT NULL,
	BDate DATE NOT NULL,
	EId INT NOT NULL FOREIGN KEY REFERENCES Employee(EId)
)

CREATE TABLE BillTrans
(
	BillId INT NOT NULL IDENTITY(501,1) PRIMARY KEY,
	BMId INT NOT NULL FOREIGN KEY REFERENCES BillMaster(BMId),
	ItemNo INT NOT NULL FOREIGN KEY REFERENCES Items(ItemNo),
	Quantity INT NOT NULL,
	Amount INT NOT NULL
)


CREATE TABLE Employee
(
	EId INT NOT NULL IDENTITY(601,1) PRIMARY KEY,
	EmpName NVARCHAR(50) NOT NULL,
	UserName NVARCHAR(50) NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	[Address] NVARCHAR(50) NOT NULL,
	PhoneNumber NVARCHAR(10) NOT NULL,
	Hint NVARCHAR(50),
	IsAdmin INT NOT NULL
)

INSERT INTO Employee VALUES
(
	'Admin',
	'Admin',
	'Admin@123',
	'Warangal',
	'9087654321',
	'I am Admin',
	0
)
 
--DROP TABLE Items
--DROP TABLE BillMaster
--DROP TABLE BillTrans
--DROP TABLE ItemMaster
--DROP TABLE StockTrans
--DROP TABLE Employee

--SELECT * FROM Employee
--DROP Procedure spGetTypeNo
CREATE PROCEDURE SpGetTypeNo
AS
BEGIN
	IF (NOT EXISTS (SELECT 1 FROM ItemMaster))
	BEGIN
		SELECT CAST(IDENT_CURRENT('ItemMaster') AS INT) AS 'ItemTypeNo' ,'ABC' AS 'ItemType'
	END
	ELSE
	BEGIN
		SELECT CAST(IDENT_CURRENT('ItemMaster') + IDENT_INCR('ItemMaster') AS INT) AS 'ItemTypeNo','ABC' AS 'ItemType'
	END
END

EXEC SpGetTypeNo

SELECT * FROM Items

ALTER TABLE StockTrans
ADD [Date] DATE NOT NULL

DROP TABLE SpGetItemNo
CREATE PROCEDURE SpGetItemNo
AS
BEGIN
	IF (NOT EXISTS (SELECT 1 FROM Items))
	BEGIN
		SELECT CAST(IDENT_CURRENT('Items') AS INT) AS 'ItemNo' ,Null AS 'ItemName', CAST(0 AS INT) AS 'ItemTypeNo', CAST(0 AS INT) AS 'ItemQuantity', CAST(0 AS INT) AS 'ItemPrice'
	END
	ELSE
	BEGIN
		SELECT CAST(IDENT_CURRENT('Items') + IDENT_INCR('Items') AS INT) AS 'ItemNo', Null AS 'ItemName', CAST(0 AS INT) AS 'ItemTypeNo', CAST(0 AS INT) AS 'ItemQuantity', CAST(0 AS INT) AS 'ItemPrice'
	END
END

DROP PROC SpGetTransId
CREATE PROCEDURE SpGetTransId
AS
BEGIN
	IF (NOT EXISTS (SELECT 1 FROM StockTrans))
	BEGIN
		SELECT CAST(IDENT_CURRENT('StockTrans') AS INT) AS 'TransId' , CAST(0 AS INT) AS 'ItemNo', CAST(0 AS INT) AS 'StockQuantity', CAST(0 AS INT) AS 'StockAmount', GETDATE() AS 'Date' 
	END
	ELSE
	BEGIN
		SELECT CAST(IDENT_CURRENT('StockTrans') + IDENT_INCR('StockTrans') AS INT) AS 'TransId', CAST(0 AS INT) AS 'ItemNo', CAST(0 AS INT) AS 'StockQuantity', CAST(0 AS INT) AS 'StockAmount', GETDATE() AS 'Date'
	END
END

SELECT * FROM Employee

SELECT * FROM ItemMaster

SELECT * FROM Items

SELECT * FROM StockTrans


