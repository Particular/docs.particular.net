if object_id('receiver.SubmittedOrder', 'U') is null
    create table receiver.SubmittedOrder (
        Id uniqueidentifier not null primary key,
        Value int not null
    )