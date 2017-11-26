startcode PostgreSql_OutboxCreateSql
create or replace function pg_temp.create_outbox_table(tablePrefix varchar, schema varchar)
  returns integer as
  $body$
    declare
      tableNameNonQuoted varchar;
      createTable text;
    begin
        tableNameNonQuoted := tablePrefix || 'OutboxData';
        createTable = 'create table if not exists "' || schema || '"."' || tableNameNonQuoted || '"
    (
        "MessageId" character varying(200),
        "Dispatched" boolean not null default false,
        "DispatchedAt" timestamp,
        "PersistenceVersion" character varying(23),
        "Operations" jsonb not null,
        primary key ("MessageId")
    );
    create index if not exists "Index_DispatchedAt" on "' || schema || '"."' || tableNameNonQuoted || '" using btree ("DispatchedAt" asc nulls last);
    create index if not exists "Index_Dispatched" on "' || schema || '"."' || tableNameNonQuoted || '" using btree ("Dispatched" asc nulls last);
';
        execute createTable;
        return 0;
    end;
  $body$
  language 'plpgsql';

select pg_temp.create_outbox_table(@tablePrefix, @schema);
endcode
