startcode PostgreSql_SagaCreateSql

/* TableNameVariable */

/* Initialize */

/* CreateTable */

create or replace function pg_temp.create_saga_table_OrderSaga(tablePrefix varchar, schema varchar)
    returns integer as
    $body$
    declare
        tableNameNonQuoted varchar;
        script text;
        count int;
        columnType varchar;
        columnToDelete text;
    begin
        tableNameNonQuoted := tablePrefix || 'OrderSaga';
        script = 'create table if not exists "' || schema || '"."' || tableNameNonQuoted || '"
(
    "Id" uuid not null,
    "Metadata" text not null,
    "Data" jsonb not null,
    "PersistenceVersion" character varying(23),
    "SagaTypeVersion" character varying(23),
    "Concurrency" int not null,
    primary key("Id")
);';
        execute script;

/* AddProperty OrderNumber */

        script = 'alter table "' || schema || '"."' || tableNameNonQuoted || '" add column if not exists "Correlation_OrderNumber" integer';
        execute script;

/* VerifyColumnType Int */

        columnType := (
            select data_type
            from information_schema.columns
            where
            table_schema = schema and
            table_name = tableNameNonQuoted and
            column_name = 'Correlation_OrderNumber'
        );
        if columnType <> 'integer' then
            raise exception 'Incorrect data type for Correlation_OrderNumber. Expected "integer" got "%"', columnType;
        end if;

/* WriteCreateIndex OrderNumber */

        script = 'create unique index if not exists "' || tablePrefix || '_i_919CC583A044D760C1FA433AD451B9E67BFD7C07" on "' || schema || '"."' || tableNameNonQuoted || '" using btree ("Correlation_OrderNumber" asc);';
        execute script;
/* AddProperty OrderId */

        script = 'alter table "' || schema || '"."' || tableNameNonQuoted || '" add column if not exists "Correlation_OrderId" uuid';
        execute script;

/* VerifyColumnType Guid */

        columnType := (
            select data_type
            from information_schema.columns
            where
            table_schema = schema and
            table_name = tableNameNonQuoted and
            column_name = 'Correlation_OrderId'
        );
        if columnType <> 'uuid' then
            raise exception 'Incorrect data type for Correlation_OrderId. Expected "uuid" got "%"', columnType;
        end if;

/* CreateIndex OrderId */

        script = 'create unique index if not exists "' || tablePrefix || '_i_9ACA1354611B1EEE42F739F02F62E0AB88D036F3" on "' || schema || '"."' || tableNameNonQuoted || '" using btree ("Correlation_OrderId" asc);';
        execute script;
/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

for columnToDelete in
(
    select column_name
    from information_schema.columns
    where
        table_name = tableNameNonQuoted and
        column_name LIKE 'Correlation_%' and
        column_name <> 'Correlation_OrderNumber' and
        column_name <> 'Correlation_OrderId'
)
loop
	script = '
alter table "' || schema || '"."' || tableNameNonQuoted || '"
drop column "' || columnToDelete || '"';
    execute script;
end loop;

/* CompleteSagaScript */

        return 0;
    end;
    $body$
language 'plpgsql';

select pg_temp.create_saga_table_OrderSaga(@tablePrefix, @schema);

endcode
