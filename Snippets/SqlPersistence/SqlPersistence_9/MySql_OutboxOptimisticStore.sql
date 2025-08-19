startcode MySql_OutboxOptimisticStoreSql

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
