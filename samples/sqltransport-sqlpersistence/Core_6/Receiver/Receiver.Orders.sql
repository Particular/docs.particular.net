-- startcode OrdersTableSQL
create table receiver.OrdersRaw (
    OrderId varchar(5) not null primary key,
    Value decimal(19, 5) null
)
create table receiver.OrdersDapper (
    OrderId varchar(5) not null primary key,
    Value decimal(19, 5) null
)
create table receiver.OrdersEf (
    OrderId varchar(5) not null primary key,
    Value decimal(19, 5) null
)
-- endcode