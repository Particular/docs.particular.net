startcode MySql_SubscriptionCreateSql
set @tableName = concat('`', @tablePrefix, 'SubscriptionData`');

set @createTable = concat('
    create table if not exists ', @tableName, '(
        Subscriber nvarchar(200) not null,
        Endpoint nvarchar(200),
        MessageType nvarchar(200) not null,
        PersistenceVersion varchar(23) not null,
        primary key clustered (Subscriber, MessageType)
    ) default charset=ascii;
');
prepare script from @createTable;
execute script;
deallocate prepare script;
endcode
