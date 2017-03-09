startcode MsSqlServer_OutboxStoreSql

insert into [dbo].[EndpointNameOutboxData]
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
