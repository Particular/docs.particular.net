startcode PostgreSql_SubscriptionCreateSql
create or replace function pg_temp.create_subscription_table(tablePrefix varchar, schema varchar)
  returns integer as
  $body$
    declare
      tableNameNonQuoted varchar;
      createTable text;
    begin
        tableNameNonQuoted := tablePrefix || 'SubscriptionData';
        createTable = 'create table if not exists "' || schema || '"."' || tableNameNonQuoted || '"
    (
        "Id" character varying(400) not null,
        "Subscriber" character varying(200) not null,
        "Endpoint" character varying(200),
        "MessageType" character varying(200) not null,
        "PersistenceVersion" character varying(200) not null,
        primary key ("Id")
    );
';
        execute createTable;
        return 0;
    end;
  $body$
  language 'plpgsql';

select pg_temp.create_subscription_table(@tablePrefix, @schema);
endcode
