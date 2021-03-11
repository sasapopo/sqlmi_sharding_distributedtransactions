USE master
GO

CREATE DATABASE ProductsDB
GO

USE ProductsDB
GO


CREATE TABLE [dbo].[products](
	[id] [int] NOT NULL,
	[name] [nvarchar](max) NULL,
	[createTimeUtc] [datetime] NULL,
	[amount] int null,
	[lastUpdateTimeUtc] [datetime] NULL,
 CONSTRAINT [products_pk_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


INSERT INTO products VALUES
(7701, 'Long-Sleeve Logo Jersey, L', '2020-10-22 22:46:42', 10, GETUTCDATE()),
(7702, 'Sport-100 Helmet, Blue', '2020-10-22 22:46:42', 10, GETUTCDATE()),
(7703, 'Mountain Bike Socks, L', '2020-10-22 22:46:42', 0, GETUTCDATE()),
(7704, 'ML Road Frame - Red, 44', '2020-10-22 22:46:42', 1, GETUTCDATE()),
(7705, 'LL Road Frame - Red, 62', '2020-10-22 22:46:42', 5, GETUTCDATE())
GO
