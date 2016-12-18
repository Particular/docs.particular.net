startcode MySql_OutboxCreateSql
set @tableName = concat(@tablePrefix, 'OutboxData');
set @createTable =  concat('
    create table if not exists ', @tableName, '(
        MessageId varchar(1024) not null,
        Dispatched bit not null default 0,
        DispatchedAt datetime,
        PersistenceVersion varchar(23) not null,
        Operations json not null,
        primary key (`MessageId`)
    ) default charset=utf8;
');
prepare statment from @createTable;
execute statment;
deallocate prepare statment;
endcode
