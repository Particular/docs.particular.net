startcode PostgreSql_OutboxOptimisticStoreSql

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
