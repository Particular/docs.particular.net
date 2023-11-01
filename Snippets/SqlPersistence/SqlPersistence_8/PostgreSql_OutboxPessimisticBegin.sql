startcode PostgreSql_OutboxPessimisticBeginSql

insert into "public"."EndpointNameOutboxData"
(
    "MessageId",
    "Operations",
    "PersistenceVersion"
)
values
(
    @MessageId,
    '[]',
    @PersistenceVersion
)
endcode
