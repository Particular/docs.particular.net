startcode PostgreSql_TimeoutCreateSql
create or replace function pg_temp.create_timeouts_table(tablePrefix varchar, schema varchar)
  returns integer as
  $body$
    declare
      tableName varchar;
      timeIndexName varchar;
      sagaIndexName varchar;
      createTable text;
    begin
        tableName := tablePrefix || 'TimeoutData';
        timeIndexName := tableName || '_TimeIdx';
        sagaIndexName := tableName || '_SagaIdx';
        createTable = 'create table if not exists "' || schema || '"."' || tableName || '"
    (
        "Id" uuid not null,
        "Destination" character varying(200),
        "SagaId" uuid,
        "State" bytea,
        "Time" timestamp,
        "Headers" text,
        "PersistenceVersion" character varying(23),
        primary key ("Id")
    );
    create index if not exists "' || timeIndexName || '" on "' || schema || '"."' || tableName || '" using btree ("Time" asc nulls last);
    create index if not exists "' || sagaIndexName || '" on "' || schema || '"."' || tableName || '" using btree ("SagaId" asc nulls last);
';
        execute createTable;
        return 0;
    end;
  $body$
  language 'plpgsql';

select pg_temp.create_timeouts_table(@tablePrefix, @schema);
endcode
