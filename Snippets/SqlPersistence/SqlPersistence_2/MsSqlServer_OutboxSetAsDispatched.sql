startcode MsSqlServer_OutboxSetAsDispatchedSql

update EndpointNameOutboxData
set
    Dispatched = 1,
    DispatchedAt = @DispatchedAt
where MessageId = @MessageId
endcode
