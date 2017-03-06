-- startcode OrdersTableSQL
CREATE TABLE [receiver].[Orders] (
	[OrderId] varchar(5) NOT NULL PRIMARY KEY CLUSTERED,
	[Value] decimal(19, 5) NULL
)
-- endcode