-- startcode ReceiverSQLAssets

if not exists (select  *
               from    sys.schemas
               where   name = N'receiver' )
    exec('create schema receiver');


if object_id('receiver.SubmittedOrder', 'U') is null
    create table receiver.SubmittedOrder (
        Id varchar(50) not null primary key,
        Value int not null
    )

-- endcode