-- This scripts shows the data from remote and local server before and after
-- executing stored procedure which uses distributed transaction,
-- to show how the data is changed by the stored procedure.

select * from [sqls-products].ProductsDB.dbo.products
select * from CustomersDB.dbo.orderLists

declare @result int
exec CustomersDB.dbo.sp_AddProductToOrderList @orderId = 8801, @productId = 7701, @productAmount = 1, @result = @result OUTPUT
select case when @result = 0 then 'SUCCESS' else 'FAIL' end as transaction_result

select * from [sqls-products].ProductsDB.dbo.products
select * from CustomersDB.dbo.orderLists
