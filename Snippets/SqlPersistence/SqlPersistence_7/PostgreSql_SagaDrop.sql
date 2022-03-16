startcode PostgreSql_SagaDropSql

/* TableNameVariable */

/* DropTable */
create or replace function pg_temp.drop_saga_table_OrderSaga(tablePrefix varchar, schema varchar)
    returns integer as
    $body$
    declare
        tableNameNonQuoted varchar;
        dropTable text;
    begin
        tableNameNonQuoted := tablePrefix || 'OrderSaga';
        dropTable = 'drop table if exists "' || schema || '"."' || tableNameNonQuoted || '";';
        execute dropTable;
        return 0;
    end;
    $body$
    language 'plpgsql';

select pg_temp.drop_saga_table_OrderSaga(@tablePrefix, @schema);

endcode
