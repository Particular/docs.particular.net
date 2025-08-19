startcode MsSqlServer_OutboxPessimisticBeginSql

insert into [dbo].[EndpointNameOutboxData]
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
