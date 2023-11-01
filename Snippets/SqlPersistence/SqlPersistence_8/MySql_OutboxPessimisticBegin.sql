startcode MySql_OutboxPessimisticBeginSql

insert into `EndpointNameOutboxData`
(
    MessageId,
    Operations,
    PersistenceVersion
)
values
(
    @MessageId,
    '[]',
    @PersistenceVersion
)
endcode
