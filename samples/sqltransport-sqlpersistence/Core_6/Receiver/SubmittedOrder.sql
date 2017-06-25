-- startcode OrdersTableSQL

if object_id('receiver.SubmittedOrder', 'U') is not null
    drop table receiver.SubmittedOrder
create table receiver.SubmittedOrder (
    Id varchar(50) not null primary key,
    Value int not null
)

-- endcode