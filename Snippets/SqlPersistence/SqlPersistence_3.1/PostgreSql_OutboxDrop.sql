startcode PostgreSql_OutboxDropSql
create or replace function pg_temp.drop_outbox_table(tablePrefix varchar)
  returns integer as
  $body$
    declare
      tableNameNonQuoted varchar;
      dropTable text;
    begin
        tableNameNonQuoted := tablePrefix || 'OutboxData';
        dropTable = 'drop table if exists public."' || tableNameNonQuoted || '";';
        execute dropTable;
        return 0;
    end;
  $body$
  language 'plpgsql';

select pg_temp.drop_outbox_table(@tablePrefix);
endcode
