startcode MySql_SagaDropSql

/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'OrderSaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'OrderSaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableNameQuoted);
prepare script from @dropTable;
execute script;
deallocate prepare script;

endcode
