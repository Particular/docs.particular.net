startcode MySql_SagaDropSql

/* TableNameVariable */

set @tableName = concat(@tablePrefix, 'OrderSaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableName);
prepare statment from @dropTable;
execute statment;
deallocate prepare statment;

endcode
