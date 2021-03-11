-- This script creates database, its schema and one stored procedure that uses distributed transactions from T-SQL.
--

USE master
GO

CREATE DATABASE CustomersDB
GO

use CustomersDB
GO

CREATE TABLE [dbo].[customers](
	[id] [int] NOT NULL,
	[name] [nvarchar](max) NULL,
	[address] [nvarchar](max) NULL,
	[createTimeUtc] [datetime] NULL,
 CONSTRAINT [customers_pk_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[orders](
	[id] [int] NOT NULL,
	[createTimeUtc] [datetime] NULL,
	[customerId] [int] NULL,
	[state] [nvarchar](20) NULL,
	[deliveryDateUtc] [datetime] NULL,
	[lastUpdateTimeUtc] [datetime] NULL,
 CONSTRAINT [orders_pk_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[orderLists](
	[id] [int] NOT NULL,
	[orderId] [int] NOT NULL,
	[productId] [int] NULL,
	[amount] [int] NULL
) ON [PRIMARY]
GO


INSERT INTO customers VALUES
(101, 'Vance P. Johns', 'Lewiston Mall Lewiston Idaho United States', getutcdate()),
(102, 'Richard M. Bentley', '25472 Marlay Ave Fontana California United States', getutcdate()),
(103, 'Hattie J. Haemon', 'Po Box 8259024 Dallas Texas United States', getutcdate()),
(104, 'Eric A. Jacobsen', '2681 Eagle Peak Bellevue Washington United States', getutcdate())
GO

INSERT INTO orders VALUES 
(8801, GETUTCDATE(), 102, 'preparing', null, GETUTCDATE()),
(8802, GETUTCDATE(), 103, 'preparing', null, GETUTCDATE())
GO


INSERT INTO customers VALUES 
(201, 'Paulo H. Lisboa', '25 Hartfield Road, Wimbledon London England United Kingdom', getutcdate()),
(202, 'Jared L. Bustamante', 'Burgess Hill West Sussex England United Kingdom', getutcdate()),
(203, 'Frances J. Giglio', 'Internet House, 3399 Science Park Cambridge England United Kingdom', getutcdate()),
(204, 'Matthew J. Cavallari', 'Garamonde Drive, Wymbush Milton Keynes England United Kingdom', getutcdate())
GO

INSERT INTO orders VALUES 
(9901, GETUTCDATE(), 203, 'preparing', null, GETUTCDATE()),
(9902, GETUTCDATE(), 204, 'preparing', null, GETUTCDATE())
GO


CREATE PROCEDURE sp_AddProductToOrderList 
	@orderId int, @productId int, @productAmount int,
	@result int OUTPUT

AS
BEGIN
	SET @result = -1;

	SET XACT_ABORT ON;
	BEGIN DISTRIBUTED TRANSACTION

	UPDATE [sqls-products].ProductsDB.dbo.products
	SET amount = amount-@productAmount, lastUpdateTimeUtc = getutcdate()
	WHERE id = @productId

	DECLARE @cnt int

	SELECT @cnt = count(1)
	FROM [sqls-products].ProductsDB.dbo.products
	WHERE id = @productId and amount < 0

	IF (@cnt > 0) 
	BEGIN
		ROLLBACK
	END
	else
	BEGIN
		DECLARE @id int = (SELECT max(id)+1 FROM orderLists)
		insert into orderLists values (@id, @orderId, @productId, @productAmount)
		SET @result = 0
		
		COMMIT
	END
END

-- DECLARE @res int
-- EXEC sp_AddProductToOrderList 8801, 7701, 1, @res OUTPUT
-- SELECT @res
