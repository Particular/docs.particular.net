startcode PostgreSql_OutboxStoreSql

insert into "public"."EndpointNameOutboxData"
(
    "MessageId",
    "Operations",
    "PersistenceVersion"
)
values
(
    @MessageId,
    @Operations,
    @PersistenceVersion
)
endcode
