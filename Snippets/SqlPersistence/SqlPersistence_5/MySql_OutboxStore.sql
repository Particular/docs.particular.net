startcode MySql_OutboxStoreSql

insert into `EndpointNameOutboxData`
(
    MessageId,
    Operations,
    PersistenceVersion
)
values
(
    @MessageId,
    @Operations,
    @PersistenceVersion
)
endcode
