-- startcode OrdersTableSQL
create table [receiver].[Orders] (
    [OrderId] varchar(5) not null primary key clustered,
    [Value] decimal(19, 5) null
)
-- endcode