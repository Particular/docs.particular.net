if object_id('receiver.SubmittedOrder', 'U') is null
    create table receiver.SubmittedOrder (
        Id varchar(50) not null primary key,
        Value int not null
    )