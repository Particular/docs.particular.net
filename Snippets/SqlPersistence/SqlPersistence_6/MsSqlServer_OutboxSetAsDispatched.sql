startcode MsSqlServer_OutboxSetAsDispatchedSql

update [dbo].[EndpointNameOutboxData]
set
    Dispatched = 1,
    DispatchedAt = @DispatchedAt,
    Operations = '[]'
where MessageId = @MessageId
endcode
