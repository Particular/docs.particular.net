startcode MySql_SagaDropSql

/* TableNameVariable */

set @tableName = concat(@tablePrefix, 'OrderSaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableName);
prepare script from @dropTable;
execute script;
deallocate prepare script;

endcode
