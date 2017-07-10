-- startcode ReceiverSQLAssets

if not exists (select  *
               from    sys.schemas
               where   name = N'receiver' )
    exec('create schema receiver');

-- endcode