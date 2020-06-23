startcode MsSqlServer_OutboxOptimisticStoreSql

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
