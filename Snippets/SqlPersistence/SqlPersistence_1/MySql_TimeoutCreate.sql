startcode MySql_TimeoutCreateSql
set @tableName = concat(@tablePrefix, 'TimeoutData');
set @createTable = concat('
    create table if not exists ', @tableName, '(
        Id varchar(38) not null,
        Destination varchar(1024),
        SagaId varchar(38),
        State longblob,
        Time datetime,
        Headers json not null,
        PersistenceVersion varchar(23) not null,
        primary key (Id)
    ) default charset=utf8;
');
prepare statment from @createTable;
execute statment;
deallocate prepare statment;
endcode
